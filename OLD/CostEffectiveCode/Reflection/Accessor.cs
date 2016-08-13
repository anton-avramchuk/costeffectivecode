using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace CostEffectiveCode.Reflection
{
    internal class Accessor<TObject, TProperty> : IAccessor<TObject, TProperty>
    {
        private readonly Func<TObject, TProperty> _getter;
        private readonly Action<TObject, TProperty> _setter;

        public Accessor(Func<TObject, TProperty> getter, Action<TObject, TProperty> setter)
        {
            this._getter = getter;
            this._setter = setter;
        }

        public void Set(TObject @object, TProperty value)
        {
            _setter(@object, value);
        }

        public TProperty Get(TObject @object)
        {
            return _getter(@object);
        }

        public void Set(object @object, object value)
        {
            _setter((TObject)@object, (TProperty)value);
        }

        public object Get(object @object)
        {
            return _getter((TObject)@object);
        }
    }

    [PublicAPI]
    public static class Accessor
    {
        public static AccessorBuilder<TObject> For<TObject>()
        {
            return new AccessorBuilder<TObject>();
        }

        public class AccessorBuilder<TObject>
        {
            public IAccessor<TObject, TProperty> From<TProperty>(Expression<Func<TObject, TProperty>> expression)
            {
                var memberExpression = expression.Body as MemberExpression;
                if (memberExpression == null)
                {
                    throw new NotSupportedException();
                }

                var param1 = Expression.Parameter(typeof(TObject), "object");
                var param2 = Expression.Parameter(typeof(TProperty), "value");

                var setter = MakeSetter<TProperty>(param1, param2, memberExpression.Member);

                return new Accessor<TObject, TProperty>(expression.Compile(), setter);
            }

            private static Action<TObject, TProperty> MakeSetter<TProperty>(ParameterExpression param1,
                                                                            ParameterExpression param2,
                                                                            MemberInfo memberInfo)
            {
                var exp = Expression.Lambda<Action<TObject, TProperty>>(
                    Expression.Assign(Expression.MakeMemberAccess(param1, memberInfo), param2),
                    param1,
                    param2);

                return exp.Compile();
            }

            public IAccessor<TObject, TProperty> From<TProperty>(string memberName)
            {
                var member = typeof(TObject).GetMember(memberName,
                                                                 BindingFlags.Instance |
                                                                 BindingFlags.Public |
                                                                 BindingFlags.NonPublic);

                var memberInfo = member.FirstOrDefault();
                if (memberInfo == null)
                {
                    throw new NotSupportedException();
                }

                var param1 = Expression.Parameter(typeof(TObject), "object");
                var param2 = Expression.Parameter(typeof(TProperty), "value");

                var getter = MakeGetter<TProperty>(memberInfo, param1);

                var setter = MakeSetter<TProperty>(param1, param2, memberInfo);

                return new Accessor<TObject, TProperty>(getter, setter);
            }

            private static Func<TObject, TProperty> MakeGetter<TProperty>(MemberInfo memberInfo,
                                                                          ParameterExpression param1)
            {
                var expression = Expression.Lambda<Func<TObject, TProperty>>(
                    Expression.MakeMemberAccess(param1, memberInfo), param1);

                return expression.Compile();
            }
        }
    }
}
