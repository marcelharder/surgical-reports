var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(builder.Configuration["ConnectionStrings:SQLConnection"]));
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<reportMapper>();
builder.Services.AddScoped<ICPBRepo,CPBRepo>();
builder.Services.AddScoped<ICABGRepo,CABGRepo>();
builder.Services.AddScoped<IProcedureRepository, ProcedureRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IHospitalRepository, HospitalRepository>();
builder.Services.AddScoped<IComposeFinalReport,ComposeFinalReport>();
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
