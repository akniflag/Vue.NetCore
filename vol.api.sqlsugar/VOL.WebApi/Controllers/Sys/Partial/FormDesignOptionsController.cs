/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("FormDesignOptions",Enums.ActionPermissionOptions.Search)]
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using VOL.Core.DBManager;
using VOL.Entity.DomainModels;
using VOL.Sys.IRepositories;
using VOL.Sys.IServices;
using VOL.Sys.Services;

namespace VOL.Sys.Controllers
{
    public partial class FormDesignOptionsController
    {
        private readonly IFormDesignOptionsService _service; //访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFormCollectionObjectRepository _formCollectionRepository;
        private readonly IFormDesignOptionsRepository _formDesignOptionsRepository;

        [ActivatorUtilitiesConstructor]
        public FormDesignOptionsController(
            IFormDesignOptionsService service,
            IHttpContextAccessor httpContextAccessor,
            IFormCollectionObjectRepository formCollectionRepository,
            IFormDesignOptionsRepository formDesignOptionsRepository
        )
            : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _formCollectionRepository = formCollectionRepository;
            _formDesignOptionsRepository = formDesignOptionsRepository;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("getFormOptions"), HttpGet]
        public async Task<IActionResult> GetFormOptions(Guid id)
        {
            var options = await _formDesignOptionsRepository.FindAsIQueryable(x => x.FormId == id).Select(s => new { s.Title, s.FormOptions }).FirstOrDefaultAsync();
            return Json(new { data = options });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        [Route("submit"), HttpPost]
        public IActionResult Submit([FromBody] SaveModel saveModel)
        {
            var result = FormCollectionObjectService.Instance.Add(saveModel);
            return Json(result);
        }

        /// <summary>
        ///获取有数据的设计器
        /// </summary>
        /// <returns></returns>
        [Route("getList"), HttpGet]
        public IActionResult GetList()
        {
            var query = _formCollectionRepository.FindAsIQueryable(x => true);
            var data = _formDesignOptionsRepository
                .FindAsIQueryable(x => SqlFunc.Subqueryable<FormCollectionObject>().Where(c => c.FormId == x.FormId).Any())
                .Select(s => new
                {
                    s.FormId,
                    s.Title,
                    s.FormOptions,
                })
                .ToList();
            return Json(data);
        }
    }
}
