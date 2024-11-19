using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehub.repository
{
    public static class ObjectMapper
    {
        public static TTarget Map<TSource, TTarget>(TSource source) where TTarget : new()
        {
            TTarget target = new TTarget();
            var sourceProperties = typeof(TSource).GetProperties();
            var targetProperties = typeof(TTarget).GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var targetProperty = targetProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);
                if (targetProperty != null && targetProperty.CanWrite)
                {
                    var value = sourceProperty.GetValue(source);
                    targetProperty.SetValue(target, value);
                }
            }

            return target;
        }
    }
}
