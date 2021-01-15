using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BooksStore2021.Mvc.Utility
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            var str = JsonConvert.SerializeObject(value);
            session.SetString(key, str);
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if(value == null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
