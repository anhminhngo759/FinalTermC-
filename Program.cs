using FinalTerm.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//lấy chuỗi kết nối từ tệp cấu hình
var connectionString = builder.Configuration.GetConnectionString("Default") ?? "";
// đăng ký một dịch vụ với chuỗi kết nối đã lấy từ tệp cấu hình
builder.Services.AddScoped<IDatabaseService>(x => new DatabaseService(connectionString));
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
