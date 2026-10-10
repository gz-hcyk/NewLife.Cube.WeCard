using NewLife.Cube;
using NewLife.Cube.WeCard;

var builder = WebApplication.CreateBuilder(args);

// Web SDK 会把引用程序集写入 ApplicationPart。不要再手工 AddApplicationPart。
builder.Services.AddControllers();
builder.Services.Configure<WeCardOptions>(builder.Configuration.GetSection("WeCard"));
builder.Services.AddWeCard();

var app = builder.Build();
app.UseWeCard();
app.MapControllers();
app.Run();

/// <summary>供测试宿主工厂引用。</summary>
public partial class Program
{
}
