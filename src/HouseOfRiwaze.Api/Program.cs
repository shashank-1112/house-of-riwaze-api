using Modules.Catalog;
using Modules.Store;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173",
                "https://house-of-riwaze-web.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddStoreModule(builder.Configuration);

var app = builder.Build();

app.UseCors("Frontend");


app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => Results.Ok(new
{
    app = "House of Riwaze API",
    status = "running"
}));

app.MapCatalogEndpoints();
app.MapStoreEndpoints();

app.Run();