using AfpCompanyApi.Data;
using AfpCompanyApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(config =>
{
    config.AddPolicy("AfpPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped(sp =>
{
    return new AppDbContext(options =>
    {
        options.ConnectionString = builder.Configuration.GetConnectionString("Default");
        options.AllowedSqlDbTypes = new SqlTypesDictionary().AddDefaultTypes().GetSqlTypes();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AfpPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
