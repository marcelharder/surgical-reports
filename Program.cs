using System.Reflection;
using surgical_reports.helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(builder.Configuration["ConnectionStrings:SQLConnection"]));
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IFinalReportRepo,FinalReportRepo>();
builder.Services.AddScoped<IPreviewReport, PreviewReport>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));




builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
