using System.Collections.Generic;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using CostEffectiveCode.EntityFramework.Tests.Entities;
using CostEffectiveCode.Extensions;
using NUnit.Framework;

namespace CostEffectiveCode.EntityFramework.Tests.Tests
{
    public class CommandTests : DataTestsBase
    {
        private UpdateEntityCommand<Product> _updateCommand;

        public override void SetUp()
        {
            base.SetUp();

            _updateCommand = new UpdateEntityCommand<Product>(new Scope<IUnitOfWork>(DataContext));
        }

        [Test]
        public void UpdateCommand_Updated()
        {
            // arrange

            // act
            var product = new Product("newname", new Category("newcategoryname"), new decimal(10.0));

            _updateCommand.Execute(product);

            // assert
            Assert.True((DataContext.TestStorage[typeof(Product)] as List<Product>)
                .CheckNotNull()
                .Contains(product));
        }
    }
}
