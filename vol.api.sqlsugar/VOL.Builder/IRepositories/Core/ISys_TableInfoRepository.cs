using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Builder.IRepositories
{
    public partial interface ISys_TableInfoRepository : IDependency, IRepository<Sys_TableInfo> { }
}
