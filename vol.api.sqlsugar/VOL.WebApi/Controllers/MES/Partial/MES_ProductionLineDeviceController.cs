/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("MES_ProductionLineDevice",Enums.ActionPermissionOptions.Search)]
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using VOL.Entity.DomainModels;
using VOL.MES.IServices;

namespace VOL.MES.Controllers
{
    public partial class MES_ProductionLineDeviceController
    {
        private readonly IMES_ProductionLineDeviceService _service; //访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public MES_ProductionLineDeviceController(IMES_ProductionLineDeviceService service, IHttpContextAccessor httpContextAccessor)
            : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
