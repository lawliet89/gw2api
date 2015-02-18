using System;

namespace PromotionViabilityWpf.Data
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MetadataAttribute : Attribute
    {
        public Type MetadataType { get; private set; }

        public MetadataAttribute(Type metadataType)
        {
            MetadataType = metadataType;
        }
    }
}