using System;

namespace PromotionViabilityWpf.Model
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