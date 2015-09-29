using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostEffectiveCode.Common
{
    public interface IDiContainer
    {
        T Resolve<T>();
    }
}
