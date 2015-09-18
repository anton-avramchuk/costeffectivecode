using Newtonsoft.Json;

namespace CostEffectiveCode.Extensions
{
	public static  class SerializationHelper
	{
		public static string ToJson(this object obj)
		{
			return JsonConvert.SerializeObject(obj);
		}
	}
}