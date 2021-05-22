using Newtonsoft.Json;

namespace WebApp.Server
{
	public class JsonConvert : IJsonConvert
	{
		public string Serialize(object data)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(data, JsonOptions);
		}

		public T Deserialize<T>(string json)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, JsonOptions);
		}

		private static JsonSerializerSettings JsonOptions => new()
		{
			NullValueHandling = NullValueHandling.Ignore
		};
	}
}
