using MemberAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddSingleton(new MemberRepository(connString));

var app = builder.Build();
// Enable Swagger in development
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MemberAPI v1");
});

app.UseAuthorization();
app.MapControllers();

app.Run();