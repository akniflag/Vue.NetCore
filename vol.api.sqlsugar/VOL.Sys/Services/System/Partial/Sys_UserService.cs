﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;
using VOL.Core.Configuration;
using VOL.Core.DBManager;
using VOL.Core.Enums;
using VOL.Core.Extensions;
using VOL.Core.ManageUser;
using VOL.Core.Services;
using VOL.Core.Utilities;
using VOL.Entity.DomainModels;
using VOL.Sys.IRepositories;

namespace VOL.Sys.Services
{
    public partial class Sys_UserService
    {
        private Microsoft.AspNetCore.Http.HttpContext _context;
        private ISys_UserRepository _repository;

        [ActivatorUtilitiesConstructor]
        public Sys_UserService(IHttpContextAccessor httpContextAccessor, ISys_UserRepository repository)
            : base(repository)
        {
            _context = httpContextAccessor.HttpContext;
            _repository = repository;
        }

        WebResponseContent webResponse = new WebResponseContent();

        /// <summary>
        /// WebApi登陆
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> Login(LoginInfo loginInfo, bool verificationCode = true)
        {
            string msg = string.Empty;
            //   2020.06.12增加验证码
            IMemoryCache memoryCache = _context.GetService<IMemoryCache>();
            string cacheCode = (memoryCache.Get(loginInfo.UUID) ?? "").ToString();
            if (string.IsNullOrEmpty(cacheCode))
            {
                return webResponse.Error("验证码已失效");
            }
            if (cacheCode.ToLower() != loginInfo.VerificationCode.ToLower())
            {
                memoryCache.Remove(loginInfo.UUID);
                return webResponse.Error("验证码不正确");
            }
            try
            {
                Sys_User user = await repository.FindAsIQueryable(x => x.UserName == loginInfo.UserName).FirstOrDefaultAsync();

                if (user == null || loginInfo.Password.Trim().EncryptDES(AppSetting.Secret.User) != (user.UserPwd ?? ""))
                    return webResponse.Error(ResponseType.LoginError);

                string token = JwtHelper.IssueJwt(
                    new UserInfo()
                    {
                        User_Id = user.User_Id,
                        UserName = user.UserName,
                        Role_Id = user.Role_Id,
                    }
                );
                user.Token = token;
                webResponse.Data = new
                {
                    token,
                    userName = user.UserTrueName,
                    img = user.HeadImageUrl,
                };
                repository.Update(user, x => x.Token, true);
                UserContext.Current.LogOut(user.User_Id);

                loginInfo.Password = string.Empty;

                return webResponse.OK(ResponseType.LoginSuccess);
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                if (_context.GetService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>().IsDevelopment())
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                return webResponse.Error(ResponseType.ServerError);
            }
            finally
            {
                memoryCache.Remove(loginInfo.UUID);
                Logger.Info(LoggerType.Login, loginInfo.Serialize(), webResponse.Message, msg);
            }
        }

        /// <summary>
        ///当token将要过期时，提前置换一个新的token
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> ReplaceToken()
        {
            string error = "";
            UserInfo userInfo = null;
            try
            {
                string requestToken = _context.Request.Headers[AppSetting.TokenHeaderName];
                requestToken = requestToken?.Replace("Bearer ", "");
                if (UserContext.Current.Token != requestToken)
                    return webResponse.Error("Token已失效!");

                if (JwtHelper.IsExp(requestToken))
                    return webResponse.Error("Token已过期!");

                int userId = UserContext.Current.UserId;
                userInfo = await repository.FindFirstAsync(
                    x => x.User_Id == userId,
                    s => new UserInfo()
                    {
                        User_Id = userId,
                        UserName = s.UserName,
                        UserTrueName = s.UserTrueName,
                        Role_Id = s.Role_Id,
                        RoleName = s.RoleName,
                    }
                );

                if (userInfo == null)
                    return webResponse.Error("未查到用户信息!");

                string token = JwtHelper.IssueJwt(userInfo);
                //移除当前缓存
                base.CacheContext.Remove(userId.GetUserIdKey());
                //只更新的token字段
                repository.Update(new Sys_User() { User_Id = userId, Token = token }, x => x.Token, true);
                webResponse.OK(null, token);
            }
            catch (Exception ex)
            {
                error = ex.Message + ex.StackTrace + ex.Source;
                webResponse.Error("token替换出错了..");
            }
            finally
            {
                Logger.Info(
                    LoggerType.ReplaceToeken,
                    ($"用户Id:{userInfo?.User_Id},用户{userInfo?.UserTrueName}") + (webResponse.Status ? "token替换成功" : "token替换失败"),
                    null,
                    error
                );
            }
            return webResponse;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> ModifyPwd(string oldPwd, string newPwd)
        {
            oldPwd = oldPwd?.Trim();
            newPwd = newPwd?.Trim();
            string message = "";
            try
            {
                if (string.IsNullOrEmpty(oldPwd))
                    return webResponse.Error("旧密码不能为空");
                if (string.IsNullOrEmpty(newPwd))
                    return webResponse.Error("新密码不能为空");
                if (newPwd.Length < 6)
                    return webResponse.Error("密码不能少于6位");

                int userId = UserContext.Current.UserId;
                string userCurrentPwd = await base.repository.FindFirstAsync(x => x.User_Id == userId, s => s.UserPwd);

                string _oldPwd = oldPwd.EncryptDES(AppSetting.Secret.User);
                if (_oldPwd != userCurrentPwd)
                    return webResponse.Error("旧密码不正确");

                string _newPwd = newPwd.EncryptDES(AppSetting.Secret.User);
                if (userCurrentPwd == _newPwd)
                    return webResponse.Error("新密码不能与旧密码相同");

                repository.Update(
                    new Sys_User
                    {
                        User_Id = userId,
                        UserPwd = _newPwd,
                        LastModifyPwdDate = DateTime.Now,
                    },
                    x => new { x.UserPwd, x.LastModifyPwdDate },
                    true
                );

                webResponse.OK("密码修改成功");
            }
            catch (Exception ex)
            {
                message = ex.Message;
                webResponse.Error("服务器了点问题,请稍后再试");
            }
            finally
            {
                if (message == "")
                {
                    Logger.OK(LoggerType.ApiModifyPwd, "密码修改成功");
                }
                else
                {
                    Logger.Error(LoggerType.ApiModifyPwd, message);
                }
            }
            return webResponse;
        }

        /// <summary>
        /// 个人中心获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCurrentUserInfo()
        {
            var data = await base
                .repository.FindAsIQueryable(x => x.User_Id == UserContext.Current.UserId)
                .Select(s => new
                {
                    s.UserName,
                    s.UserTrueName,
                    s.Address,
                    s.PhoneNo,
                    s.Email,
                    s.Remark,
                    s.Gender,
                    s.RoleName,
                    s.HeadImageUrl,
                    s.CreateDate,
                })
                .FirstOrDefaultAsync();
            return webResponse.OK(null, data);
        }

        /// <summary>
        /// 设置固定排序方式及显示用户过滤
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public override PageGridData<Sys_User> GetPageData(PageDataOptions pageData)
        {
            int roleId = -1;
            //树形菜单传查询角色下所有用户
            if (pageData.Value != null)
            {
                roleId = pageData.Value.ToString().GetInt();
            }
            QueryRelativeExpression = (ISugarQueryable<Sys_User> queryable) =>
            {
                if (roleId <= 0)
                {
                    if (UserContext.Current.IsSuperAdmin)
                        return queryable;
                    roleId = UserContext.Current.RoleId;
                }

                //查看用户时，只能看下自己角色下的所有用户
                List<int> roleIds = Sys_RoleService.Instance.GetAllChildrenRoleId(roleId);
                roleIds.Add(roleId);
                //判断查询的角色是否越权
                if (roleId != UserContext.Current.RoleId && !roleIds.Contains(roleId))
                {
                    roleId = -999;
                }
                return queryable.Where(x => roleIds.Contains(x.Role_Id));
            };
            var gridData = base.GetPageData(pageData);
            gridData.rows.ForEach(x =>
            {
                x.Token = null;
            });
            return gridData;
        }

        /// <summary>
        /// 新建用户，根据实际情况自行处理
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        public override WebResponseContent Add(SaveModel saveModel)
        {
            saveModel.MainData["RoleName"] = "无";
            base.AddOnExecute = (SaveModel userModel) =>
            {
                int roleId = userModel?.MainData?["Role_Id"].GetInt() ?? 0;
                if (roleId > 0 && !UserContext.Current.IsSuperAdmin)
                {
                    string roleName = GetChildrenName(roleId);
                    if ((roleId == 1) || string.IsNullOrEmpty(roleName))
                        return webResponse.Error("不能选择此角色");
                }
                return webResponse.OK();
            };

            ///生成6位数随机密码
            string pwd = 6.GenerateRandomNumber();
            //在AddOnExecuting之前已经对提交的数据做过验证是否为空
            base.AddOnExecuting = (Sys_User user, object obj) =>
            {
                user.UserName = user.UserName.Trim();
                if (repository.Exists(x => x.UserName == user.UserName))
                    return webResponse.Error("用户名已经被注册");
                user.UserPwd = pwd.EncryptDES(AppSetting.Secret.User);
                //设置默认头像
                return webResponse.OK();
            };

            base.AddOnExecuted = (Sys_User user, object list) =>
            {
                var deptIds = user.DeptIds?.Split(",").Select(s => s.GetGuid()).Where(x => x != null).Select(s => (Guid)s).ToArray();
                SaveDepartment(deptIds, user.User_Id);
                return webResponse.OK($"用户新建成功.帐号{user.UserName}密码{pwd}");
            };
            return base.Add(saveModel);
            ;
        }

        /// <summary>
        /// 删除用户拦截过滤
        /// 用户被删除后同时清空对应缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="delList"></param>
        /// <returns></returns>
        public override WebResponseContent Del(object[] keys, bool delList = false)
        {
            base.DelOnExecuting = (object[] ids) =>
            {
                if (!UserContext.Current.IsSuperAdmin)
                {
                    int[] userIds = ids.Select(x => Convert.ToInt32(x)).ToArray();
                    //校验只能删除当前角色下能看到的用户
                    var xxx = repository.Find(x => userIds.Contains(x.User_Id));
                    var delUserIds = repository.Find(
                        x => userIds.Contains(x.User_Id),
                        s => new
                        {
                            s.User_Id,
                            s.Role_Id,
                            s.UserTrueName,
                        }
                    );
                    List<int> roleIds = Sys_RoleService.Instance.GetAllChildrenRoleId(UserContext.Current.RoleId);

                    string[] userNames = delUserIds.Where(x => !roleIds.Contains(x.Role_Id)).Select(s => s.UserTrueName).ToArray();
                    if (userNames.Count() > 0)
                    {
                        return webResponse.Error($"没有权限删除用户：{string.Join(',', userNames)}");
                    }
                }

                return webResponse.OK();
            };
            base.DelOnExecuted = (object[] userIds) =>
            {
                var objKeys = userIds.Select(x => x.GetInt().GetUserIdKey());
                base.CacheContext.RemoveAll(objKeys);
                return new WebResponseContent() { Status = true };
            };
            return base.Del(keys, delList);
        }

        private string GetChildrenName(int roleId)
        {
            //只能修改当前角色能看到的用户
            string roleName = Sys_RoleService.Instance.GetAllChildren(UserContext.Current.UserInfo.Role_Id).Where(x => x.Id == roleId).Select(s => s.RoleName).FirstOrDefault();
            return roleName;
        }

        /// <summary>
        /// 修改用户拦截过滤
        ///
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        public override WebResponseContent Update(SaveModel saveModel)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            saveModel.MainData["RoleName"] = "无";
            //禁止修改用户名
            base.UpdateOnExecute = (SaveModel saveInfo) =>
            {
                if (!UserContext.Current.IsSuperAdmin)
                {
                    int roleId = saveModel.MainData["Role_Id"].GetInt();
                    string roleName = GetChildrenName(roleId);
                    saveInfo.MainData.TryAdd("RoleName", roleName);
                    if (UserContext.IsRoleIdSuperAdmin(userInfo.Role_Id))
                    {
                        return webResponse.OK();
                    }
                    if (string.IsNullOrEmpty(roleName))
                        return webResponse.Error("不能选择此角色");
                }

                return webResponse.OK();
            };
            base.UpdateOnExecuting = (Sys_User user, object obj1, object obj2, List<object> list) =>
            {
                if (user.User_Id == userInfo.User_Id && user.Role_Id != userInfo.Role_Id)
                    return webResponse.Error("不能修改自己的角色");

                var _user = repository.Find(x => x.User_Id == user.User_Id, s => new { s.UserName, s.UserPwd }).FirstOrDefault();
                user.UserName = _user.UserName;
                //Sys_User实体的UserPwd用户密码字段的属性不是编辑，此处不会修改密码。但防止代码生成器将密码字段的修改成了可编辑造成密码被修改
                user.UserPwd = _user.UserPwd;
                return webResponse.OK();
            };
            //用户信息被修改后，将用户的缓存信息清除
            base.UpdateOnExecuted = (Sys_User user, object obj1, object obj2, List<object> List) =>
            {
                base.CacheContext.Remove(user.User_Id.GetUserIdKey());
                var deptIds = user.DeptIds?.Split(",").Select(s => s.GetGuid()).Where(x => x != null).Select(s => (Guid)s).ToArray();
                SaveDepartment(deptIds, user.User_Id);
                return new WebResponseContent(true);
            };
            return base.Update(saveModel);
        }

        /// <summary>
        /// 保存部门
        /// </summary>
        /// <param name="deptIds"></param>
        /// <param name="userId"></param>
        public void SaveDepartment(Guid[] deptIds, int userId)
        {
            if (userId <= 0)
            {
                return;
            }
            if (deptIds == null)
            {
                deptIds = new Guid[] { };
            }

            //如果需要判断当前角色是否越权，再调用一下获取当前部门下的所有子角色判断即可

            var roles = repository
                .DbContext.Set<Sys_UserDepartment>()
                .Where(x => x.UserId == userId)
                .Select(s => new
                {
                    s.DepartmentId,
                    s.Enable,
                    s.Id,
                })
                .ToList();
            //没有设置部门
            if (deptIds.Length == 0 && !roles.Exists(x => x.Enable == 1))
            {
                return;
            }

            UserInfo user = UserContext.Current.UserInfo;
            //新设置的部门
            var add = deptIds
                .Where(x => !roles.Exists(r => r.DepartmentId == x))
                .Select(s => new Sys_UserDepartment()
                {
                    Id = Guid.NewGuid(),
                    DepartmentId = s,
                    UserId = userId,
                    Enable = 1,
                    CreateDate = DateTime.Now,
                    Creator = user.UserTrueName,
                    CreateID = user.User_Id,
                })
                .ToList();

            //删除的部门
            var update = roles
                .Where(x => !deptIds.Contains(x.DepartmentId) && x.Enable == 1)
                .Select(s => new Sys_UserDepartment()
                {
                    Id = s.Id,
                    Enable = 0,
                    ModifyDate = DateTime.Now,
                    Modifier = user.UserTrueName,
                    ModifyID = user.User_Id,
                })
                .ToList();

            //之前设置过的部门重新分配
            update.AddRange(
                roles
                    .Where(x => deptIds.Contains(x.DepartmentId) && x.Enable != 1)
                    .Select(s => new Sys_UserDepartment()
                    {
                        Id = s.Id,
                        Enable = 1,
                        ModifyDate = DateTime.Now,
                        Modifier = user.UserTrueName,
                        ModifyID = user.User_Id,
                    })
                    .ToList()
            );
            repository.AddRange(add);

            repository.UpdateRange(
                update,
                x => new
                {
                    x.Enable,
                    x.ModifyDate,
                    x.Modifier,
                    x.ModifyID,
                }
            );
            repository.SaveChanges();
        }

        /// <summary>
        /// 导出处理
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public override WebResponseContent Export(PageDataOptions pageData)
        {
            //限定只能导出当前角色能看到的所有用户
            QueryRelativeExpression = (ISugarQueryable<Sys_User> queryable) =>
            {
                if (UserContext.Current.IsSuperAdmin)
                    return queryable;
                List<int> roleIds = Sys_RoleService.Instance.GetAllChildrenRoleId(UserContext.Current.RoleId);
                return queryable.Where(x => roleIds.Contains(x.Role_Id) || x.User_Id == UserContext.Current.UserId);
            };

            base.ExportOnExecuting = (List<Sys_User> list, List<string> ignoreColumn) =>
            {
                if (!ignoreColumn.Contains("Role_Id"))
                {
                    ignoreColumn.Add("Role_Id");
                }
                if (!ignoreColumn.Contains("RoleName"))
                {
                    ignoreColumn.Remove("RoleName");
                }
                WebResponseContent responseData = new WebResponseContent(true);
                return responseData;
            };
            return base.Export(pageData);
        }
    }
}
