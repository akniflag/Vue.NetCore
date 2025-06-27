using System.Collections.Generic;
using System.Linq;
using VOL.Core.BaseProvider;
using VOL.Core.Enums;
using VOL.Core.Extensions;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Sys.Services
{
    public partial class Sys_DictionaryListService
    {
        public override PageGridData<Sys_DictionaryList> GetPageData(PageDataOptions pageData)
        {
            base.OrderByExpression = x => new Dictionary<object, QueryOrderBy>() { { x.OrderNo, QueryOrderBy.Desc }, { x.DicList_ID, QueryOrderBy.Asc } };
            return base.GetPageData(pageData);
        }
    }
}
