/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Sys_WorkFlowStep",Enums.ActionPermissionOptions.Search)]
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using VOL.Entity.DomainModels;
using VOL.Sys.IServices;

namespace VOL.Sys.Controllers
{
    public partial class Sys_WorkFlowStepController
    {
        private readonly ISys_WorkFlowStepService _service; //访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_WorkFlowStepController(ISys_WorkFlowStepService service, IHttpContextAccessor httpContextAccessor)
            : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
