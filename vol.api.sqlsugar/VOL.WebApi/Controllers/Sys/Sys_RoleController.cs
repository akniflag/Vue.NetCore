﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Core.Enums;
using VOL.Core.Filters;
using VOL.Entity.AttributeManager;
using VOL.Entity.DomainModels;
using VOL.Sys.IServices;

namespace VOL.Sys.Controllers
{
    [Route("api/Sys_Role")]
    [PermissionTable(Name = "Sys_Role")]
    public partial class Sys_RoleController : ApiBaseController<ISys_RoleService>
    {
        public Sys_RoleController(ISys_RoleService service)
            : base("System", "System", "Sys_Role", service) { }
    }
}
