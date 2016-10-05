using System.Collections.Generic;
using System.Diagnostics;
using CostEffectiveCode.Components;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Cqrs.Commands;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Pagination;
using CostEffectiveCode.Ddd.Specifications;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class AutoRegistrationTests
    {
        [Fact]
        public void AutoRegistration_MainComponentsSetUp()
        {
            var sw = new Stopwatch();
            sw.Start();
            var assembly = GetType().Assembly;
            var res = AutoRegistration.GetComponentMap(assembly, t => t == typeof(TestDependentObject), assembly, t => true);
            sw.Stop();

            Assert.Equal(
                typeof(ProjectionQuery<object, Product, ProductDto>),
                res[typeof(IQuery<object, IEnumerable<ProductDto>>)]);

            Assert.Equal(
                typeof(PagedQuery<int, IdPaging<ProductDto>, Product, ProductDto>),
                res[typeof(IQuery<IdPaging<ProductDto>, IPagedEnumerable<ProductDto>>)]);

            Assert.Equal(
                typeof(GetByIdQuery<int, Product, ProductDto>),
                res[typeof(IQuery<int, ProductDto>)]);

            Assert.Equal(
                typeof(CreateOrUpdateHandler<int, ProductDto, Product>),
                res[typeof(ICommandHandler<ProductDto, int>)]);

            Assert.True(sw.ElapsedMilliseconds < 50);
        }
    }
}
