/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *Repository提供数据库操作，如果要增加数据库操作请在当前目录下Partial文件夹FormDesignOptionsRepository编写代码
 */
using VOL.Core.BaseProvider;
using VOL.Core.DbContext;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;
using VOL.Sys.IRepositories;

namespace VOL.Sys.Repositories
{
    public partial class FormDesignOptionsRepository
        : RepositoryBase<FormDesignOptions>,
            IFormDesignOptionsRepository
    {
        public FormDesignOptionsRepository(VOLContext dbContext)
            : base(dbContext) { }

        public static IFormDesignOptionsRepository Instance
        {
            get { return AutofacContainerModule.GetService<IFormDesignOptionsRepository>(); }
        }
    }
}
