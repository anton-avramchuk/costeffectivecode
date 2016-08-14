using System.Linq;
using CosteffectiveCode.AutoMapper;
using CosteffectiveCode.Domain.Ddd.Entities;
using CosteffectiveCode.Domain.Ddd.Specifications;
using CosteffectiveCode.FunctionalProgramming;
using CosteffectiveCode.SimpleInjector;
using Xunit;
using SI = SimpleInjector;
namespace CosteffectiveCode.Tests
{
    public class SimpleListMorphism<TEntity, TDto> : EntityToDtoQuery<TEntity, TDto>,
        IMorphism<ExpressionSpecification<TEntity>, TDto[]>
        where TEntity : class, IEntity
    {
        public SimpleListMorphism() : base( new[] { new Foo() { Id = 1, Bar = new Bar() { Name = "Super Bar" }}}.Cast<TEntity>().AsQueryable())

        {
        }

        public TDto[] Execute(ExpressionSpecification<TEntity> input) => Where(input).Execute().ToArray();
    }

    public class IocTests
    {
        [Fact]
        public void Container_RegisterCategoryAndDefineSpec()
        {
            // IOC-контейнер
            var container = new SI.Container();

            // Объявляем "категорию"
            // @see https://habrahabr.ru/post/133277/
            var category = new Category("ExpressionSpecification<TEntity> -> TDto[]");

            // Объявляем морфизмы типа IMporphism<ExpressionSpecification<TEntity>, TDto[]>>
            // Читается как: дай мне функцию преобразования, которая по заданому через Expression условию
            // вернет подходящие сущности Foo и трансформирует их в FooDto
            // Работает это через Automapper и Linq. Automapper налету создает экспрешны для .Select
            // https://github.com/AutoMapper/AutoMapper/wiki/Queryable-Extensions
            category.DefineSpec<Foo, FooDto, SimpleListMorphism<Foo, FooDto>>();

            // Регистрируем категорию в контейнере вместе с еще одной пустой
            // Все категории будут зарегистрированы в контейнере и метаинформацию
            // можно будет получить через container.GetAllInstances<Category>()
            container.RegisterCategories(SI.Lifestyle.Transient, category, new Category("Empty"));

            // Проверяем корректность регистрации
            container.Verify();
            // Заставляем Automapper доопределить Mapping'и если не хватает
            // И проверить Mapper.AssertConfigurationIsValid()
            AutoMapperWrapper.Init();

            // Получаем морфизм типа EpressionSpecification<Foo> -> FooDto
            // Expression из парметров запроса можно получить так:
            // http://sergeyteplyakov.blogspot.ru/2010/12/dynamic-linq.html
            var opr = container.GetSpecMorphism<Foo, FooDto>();

            // Получаем Dto для всех Entity, у которых Id >= 1
            var res = opr.Execute(x => x.Id >= 1);

            Assert.Equal(1, res.Length);
            Assert.Equal(2, container.GetAllInstances<Category>().Count());
        }
    }
}
