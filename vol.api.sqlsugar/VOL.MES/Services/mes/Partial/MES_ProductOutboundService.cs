/*
 *所有关于MES_ProductOutbound类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*MES_ProductOutboundService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
*/
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using VOL.Core.BaseProvider;
using VOL.Core.DBManager;
using VOL.Core.DbSqlSugar;
using VOL.Core.Extensions;
using VOL.Core.Extensions.AutofacManager;
using VOL.Core.Utilities;
using VOL.Entity.DomainModels;
using VOL.MES.IRepositories;

namespace VOL.MES.Services
{
    public partial class MES_ProductOutboundService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMES_ProductOutboundRepository _repository; //访问数据库

        [ActivatorUtilitiesConstructor]
        public MES_ProductOutboundService(IMES_ProductOutboundRepository dbRepository, IHttpContextAccessor httpContextAccessor)
            : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public override PageGridData<MES_ProductOutbound> GetPageData(PageDataOptions options)
        {
            SummaryExpress = (ISugarQueryable<MES_ProductOutbound> queryable) =>
            {
                return queryable.Select(x => new { OutboundQuantity = SqlFunc.AggregateSum(x.OutboundQuantity).ToString("f2") }).FirstOrDefault();
            };
            return base.GetPageData(options);
        }
    }
}
