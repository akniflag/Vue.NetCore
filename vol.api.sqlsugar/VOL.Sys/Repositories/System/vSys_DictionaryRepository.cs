/*
 *Author：jxx
 *Contact：283591387@qq.com
 *Date：2018-07-01
 * 此代码由框架生成，请勿随意更改
 */
using VOL.Core.BaseProvider;
using VOL.Core.DbContext;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;
using VOL.Sys.IRepositories;

namespace VOL.Sys.Repositories
{
    public partial class vSys_DictionaryRepository
        : RepositoryBase<vSys_Dictionary>,
            IvSys_DictionaryRepository
    {
        public vSys_DictionaryRepository(VOLContext dbContext)
            : base(dbContext) { }

        public static IvSys_DictionaryRepository Instance
        {
            get { return AutofacContainerModule.GetService<IvSys_DictionaryRepository>(); }
        }
    }
}
