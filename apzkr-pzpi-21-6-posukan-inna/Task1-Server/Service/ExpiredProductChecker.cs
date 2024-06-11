using InventoryAPI.Data;
using InventoryAPI.Models;


public class ExpiredProductChecker : BackgroundService
{
    private readonly IServiceProvider _services;

    public ExpiredProductChecker(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<InventoryContext>();

                try
                {
                    var expiredProducts = context.Products
                        .Where(p => p.ExpiryDate.HasValue && p.ExpiryDate < DateTime.Now)
                        .ToList();

                    foreach (var product in expiredProducts)
                    {
                        var existingDefectiveProduct = context.DefectiveProducts
                            .FirstOrDefault(dp => dp.ProductId == product.ProductId && dp.Reason == "Expired");

                        if (existingDefectiveProduct == null)
                        {
                            var defectiveProduct = new DefectiveProduct
                            {
                                ProductId = product.ProductId,
                                Reason = "Expired",
                                DateDetected = DateTime.Now
                            };
                            context.DefectiveProducts.Add(defectiveProduct);
                        }
                    }

                    await context.SaveChangesAsync(); 

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while checking expired products: {ex.Message}");
                }
            }
        }
    }
}
 