using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class ObjectExtensions
    {
        public static string ToJsonString(this object instance)
        {
            return JsonConvert.SerializeObject(instance);
        }
    }
}
