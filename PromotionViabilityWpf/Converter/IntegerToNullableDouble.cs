using System;
using ReactiveUI;

namespace PromotionViabilityWpf.Converter
{
    public class IntegerToNullableDouble : IBindingTypeConverter
    {
        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            if ((fromType == typeof (int) && toType == typeof (double?))
                || (fromType == typeof (double?) && toType == typeof (int)))
            {
                return 2;
            }
            return -1;
        }

        public bool TryConvert(object obj, Type toType, object conversionHint, out object result)
        {
            if (toType == typeof (int))
            {
                var nullableDouble = obj as double?;
                result = nullableDouble == null ? 0 : Convert.ToInt32(nullableDouble.Value);
            }
            else if (toType == typeof (double?))
            {
                result = Convert.ToDouble(obj);
            }
            else
            {
                throw new InvalidOperationException("Unsupported conversion");
            }
           
            return true;
        }
    }
}
