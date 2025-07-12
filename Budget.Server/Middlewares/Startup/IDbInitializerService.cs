namespace Budget.Server.Middlewares.Startup
{
	public interface IDbInitializerService
	{
		public Task Init(IServiceProvider services);
	}
}