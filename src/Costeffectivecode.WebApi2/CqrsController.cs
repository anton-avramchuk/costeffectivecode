using System.Web.Http;
using CosteffectiveCode.Cqrs.Commands;
using CosteffectiveCode.Cqrs.Queries;

namespace Costeffectivecode.WebApi2
{
    public abstract class CqrsController : ApiController
    {
        public readonly ICommandFactory CommandFactory;

        public readonly IQueryFactory QueryFactory;

        protected CqrsController(ICommandFactory commandFactory, IQueryFactory queryFactory)
        {
            CommandFactory = commandFactory;
            QueryFactory = queryFactory;
        }
    }
}
