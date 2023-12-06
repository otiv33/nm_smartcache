using Orleans.Configuration;
using Orleans.Runtime;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetSection("ConnectionString").Value;

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseAzureStorageClustering(o => o.ConfigureTableServiceClient(connectionString));
    siloBuilder.AddAzureBlobGrainStorage(
        name: "nomniotest",
        configureOptions: options =>
        {
            options.ConfigureBlobServiceClient(connectionString);
        }
    );
    //siloBuilder.AddAzureBlobGrainStorageAsDefault("nomniotest");
    siloBuilder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "nomniotest-cluster";
        options.ServiceId = "nomniotest";
    });
});

builder.Services.AddControllers();

var app = builder.Build();


app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
