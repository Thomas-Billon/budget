namespace Budget.Server.Middleware.Startup
{
	public interface IDbInitializerService
	{
		public Task Init(IServiceProvider services);
	}
}