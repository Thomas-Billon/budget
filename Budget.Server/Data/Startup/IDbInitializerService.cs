namespace Budget.Server.Data.Startup
{
	public interface IDbInitializerService
	{
		public Task Init(IServiceProvider services);
	}
}