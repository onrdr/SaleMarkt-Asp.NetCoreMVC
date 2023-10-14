using DataAccess;

namespace WebUI.ExtensionMethods;

public static class ApplicationBuilderExtensionMethods
{
    public static async Task EnsureDatabaseConnected(this IApplicationBuilder app, CancellationToken cancellationToken = default)
    {
        using var scope = app.ApplicationServices.CreateScope(); 
        var salesContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        while (true)
        {
            try
            {
                var result = await salesContext.Database.EnsureCreatedAsync(cancellationToken);
                if (result)
                {
                    Console.WriteLine("Connected...");
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }

            Thread.Sleep(3000);
        } 
    }
}
