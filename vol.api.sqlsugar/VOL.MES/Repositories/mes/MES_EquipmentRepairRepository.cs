/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *Repository提供数据库操作，如果要增加数据库操作请在当前目录下Partial文件夹MES_EquipmentRepairRepository编写代码
 */
using VOL.Core.BaseProvider;
using VOL.Core.DbContext;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;
using VOL.MES.IRepositories;

namespace VOL.MES.Repositories
{
    public partial class MES_EquipmentRepairRepository : RepositoryBase<MES_EquipmentRepair>, IMES_EquipmentRepairRepository
    {
        public MES_EquipmentRepairRepository(VOLContext dbContext)
            : base(dbContext) { }

        public static IMES_EquipmentRepairRepository Instance
        {
            get { return AutofacContainerModule.GetService<IMES_EquipmentRepairRepository>(); }
        }
    }
}
