namespace WebApp.Server
{
	public interface IJsonConvert
	{
		string Serialize(object data);
		T Deserialize<T>(string json);
	}
}
