using System;
using Costeffectivecode.WebApi2.Example.Models;
using CostEffectiveCode.AutoMapper;
using CostEffectiveCode.Cqrs.Commands;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Entities;

namespace Costeffectivecode.WebApi2.Example
{
    public class CommandQueryFactory : IQueryFactory, ICommandFactory
    {
        public IQuery<TSpecification, TResult> GetQuery<TSpecification, TResult>()
        {
            throw new NotImplementedException();
        }

        public TQuery GetQuery<TSpecification, TResult, TQuery>() where TQuery : IQuery<TSpecification, TResult>
        {
            if (typeof(TQuery) == typeof(GetQuery<int, Product, ProductDto>))
            {
                return (TQuery) (object)new GetQuery<int, Product, ProductDto>(new LinqProvider(new Product
                {
                    Id  = 1,
                    Info = new Info { Name = "1 Name"}
                },
                    new Product
                    {
                        Id = 2,
                        Info = new Info { Name = "2 Name"}
                    }), new AutoMapperWrapper());
            }

            throw new NotImplementedException();
        }

        public TCommand GetCommand<TInput, TCommand>() where TCommand : ICommand<TInput>
        {
            throw new NotImplementedException();
        }

        public TCommand GetCommand<TInput, TResult, TCommand>() where TResult : struct where TCommand : ICommand<TInput, TResult>
        {
            throw new NotImplementedException();
        }

        public TCommand GetCommand<TCommand>() where TCommand : ICommand
        {
            throw new NotImplementedException();
        }

        public SaveCommand<TEntity, TKey> GetCreateCommand<TKey, TEntity>() where TKey : struct where TEntity : EntityBase<TKey>
        {
            throw new NotImplementedException();
        }

        public DeleteCommand<T> GetDeleteCommand<T>() where T : class, IEntity
        {
            throw new NotImplementedException();
        }
    }
}
