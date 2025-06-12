using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GHLearning.EasyApiKey.WebApiTests;

internal class CustomWebApplicationFactory(Action<IWebHostBuilder>? webHostConfigure = null) : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		_ = builder.ConfigureAppConfiguration(c => c.Sources.Clear());
		webHostConfigure?.Invoke(builder);
	}
}
