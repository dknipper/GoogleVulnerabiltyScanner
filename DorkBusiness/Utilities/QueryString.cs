using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace DorkBusiness.Utilities
{
    public class QueryString
    {
        public static NameValueCollection RemoveParameters(NameValueCollection queryStringValueCollection, List<string> parameters)
        {
            foreach (var param in parameters.Where(param => queryStringValueCollection[param] != null))
            {
                queryStringValueCollection.Remove(param);
            }
            return queryStringValueCollection;
        }

        public static NameValueCollection AddParameters(NameValueCollection queryStringValueCollection, Dictionary<string, string> parameters)
        {
            foreach (var param in parameters)
            {
                if (queryStringValueCollection[param.Key] != null)
                {
                    queryStringValueCollection.Remove(param.Key);
                }
                queryStringValueCollection.Add(new NameValueCollection { { param.Key, param.Value } });
            }
            return queryStringValueCollection;
        }
    }
}
