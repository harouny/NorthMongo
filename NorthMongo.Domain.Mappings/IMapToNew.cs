using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthMongo.Domain.Mappings
{
    public interface IMapToNew<in TSource, out TTarget>
    {
        TTarget Map(TSource source);
    }
}
