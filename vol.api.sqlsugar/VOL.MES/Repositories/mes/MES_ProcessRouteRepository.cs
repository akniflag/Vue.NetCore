/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *Repository提供数据库操作，如果要增加数据库操作请在当前目录下Partial文件夹MES_ProcessRouteRepository编写代码
 */
using VOL.Core.BaseProvider;
using VOL.Core.DbContext;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;
using VOL.MES.IRepositories;

namespace VOL.MES.Repositories
{
    public partial class MES_ProcessRouteRepository
        : RepositoryBase<MES_ProcessRoute>,
            IMES_ProcessRouteRepository
    {
        public MES_ProcessRouteRepository(VOLContext dbContext)
            : base(dbContext) { }

        public static IMES_ProcessRouteRepository Instance
        {
            get { return AutofacContainerModule.GetService<IMES_ProcessRouteRepository>(); }
        }
    }
}
