/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("MES_MaterialCatalog",Enums.ActionPermissionOptions.Search)]
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VOL.Core.BaseProvider;
using VOL.Entity.DomainModels;
using VOL.MES.IRepositories;
using VOL.MES.IServices;

namespace VOL.MES.Controllers
{
    public partial class MES_MaterialCatalogController
    {
        private readonly IMES_MaterialCatalogService _service; //访问业务代码
        private readonly IMES_MaterialCatalogRepository _repository; //访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public MES_MaterialCatalogController(
            IMES_MaterialCatalogService service,
            IMES_MaterialCatalogRepository repository,
            IHttpContextAccessor httpContextAccessor
        )
            : base(service)
        {
            _service = service;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 商品信息tree页面获取左边的tree的所有商品分类
        /// </summary>
        /// <returns></returns>
        [Route("getList"), HttpGet]
        public async Task<IActionResult> GetList()
        {
            var data = await _repository
                .FindAsIQueryable(x => true)
                .Select(s => new
                {
                    id = s.CatalogID,
                    s.ParentId,
                    name = s.CatalogName,
                })
                .ToListAsync();
            return Json(data);
        }
    }
}
