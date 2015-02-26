using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace gw2api.Object
{
    // Adapted from http://stackoverflow.com/questions/13108693/serializing-heavily-linked-data-in-net-customizing-json-net-references
    public class ItemIdsContext
    {
        private readonly Dictionary<string, int> referenceToIdMap;
        private readonly ILookup<int, string> idToReferenceMap; 
        public ItemIdsContext(Dictionary<string, int> referenceToIdMap)
        {
            this.referenceToIdMap = referenceToIdMap;
            idToReferenceMap = this.referenceToIdMap.ToLookup(pair => pair.Value, pair => pair.Key);
        }

        public static object GetLinkedValue(JsonSerializer serializer, Type type, string reference)
        {
            var context = serializer.Context.Context as ItemIdsContext;
            if (context == null)
                throw new InvalidOperationException("Serializer does not have the context with the Item IDs");
            return GetLinkedValue(context, type, reference);
        }


        public static object GetLinkedValue(ItemIdsContext context, Type type, string reference)
        {
            if (type == typeof(int))
            {
                return context.referenceToIdMap[reference];
            }
            if (type == typeof(string))
            {
                return context.idToReferenceMap[Convert.ToInt32(reference)].Single();
            }
            throw new InvalidOperationException(String.Format("Unsupported for type {0}", type.FullName));
        }
    }
}
