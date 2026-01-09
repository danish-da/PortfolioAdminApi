//using PortfolioAdminApi.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container
//builder.Services.AddControllers();
//builder.Services.AddSwaggerGen();
//builder.Services.AddEndpointsApiExplorer();

//// Define a named CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin", policy =>
//    {
//        policy.WithOrigins(
//            "http://localhost:4200",
//            "https://mac.mactech.net.in")
//        .AllowAnyHeader()
//        .AllowAnyMethod();
//    });
//});

//builder.Services.AddSingleton<OracleDbService>();



//var app = builder.Build();

//// Configure the HTTP request pipeline
//app.UseSwagger();
//app.UseSwaggerUI();

//app.UseHttpsRedirection();



//app.UseRouting(); // Required before CORS

//app.UseCors("AllowSpecificOrigin"); // Apply named CORS policy

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
using PortfolioAdminApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Define a named CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://mac.mactech.net.in")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Register Oracle service
builder.Services.AddSingleton<OracleDbService>();

// ✅ Register HttpClientFactory for ProxyController
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting(); // Required before CORS

app.UseCors("AllowSpecificOrigin"); // Apply named CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();
