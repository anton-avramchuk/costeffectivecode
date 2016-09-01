using System.Web.Http;
using CostEffectiveCode.Cqrs.Commands;
using CostEffectiveCode.Cqrs.Queries;

namespace CostEffectiveCode.WebApi2
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
