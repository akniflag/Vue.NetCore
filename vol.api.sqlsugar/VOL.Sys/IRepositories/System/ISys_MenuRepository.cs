using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Sys.IRepositories
{
    public partial interface ISys_MenuRepository : IDependency, IRepository<Sys_Menu> { }
}
