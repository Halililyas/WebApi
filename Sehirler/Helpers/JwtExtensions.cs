namespace Sehirler.Helpers
{
	public static class JwtExtensions
	{
		public static void AddApplicationError(this HttpResponse response,string message)
		{
			response.Headers.Add("Application-Error", message);
			response.Headers.Add("Access-Control-Allow-Orgin", "*");
			response.Headers.Add("Access-Control-Expose-Header", "Application-Error");
		}
	}
}
