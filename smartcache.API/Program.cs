using Orleans.Runtime;
using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Host.UseOrleans(siloBuilder =>
{
    var connectionString = builder.Configuration.GetSection("ConnectionString").Value;

    siloBuilder
        .UseAzureStorageClustering(o => o.ConfigureTableServiceClient(connectionString))
        .AddAzureBlobGrainStorage("emailsmartcache", o => o.ConfigureBlobServiceClient(connectionString));

    siloBuilder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "emailsm1";
        options.ServiceId = "emailsmartcache";
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
