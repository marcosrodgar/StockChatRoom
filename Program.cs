using Microsoft.EntityFrameworkCore;
using StockBotChatRoom.Data;
using StockBotChatRoom.Data.Repositories;
using StockBotChatRoom.Hubs;
using Microsoft.AspNetCore.Identity;
using StockBotChatRoom.Data.Entities;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ChatContextDb") ?? throw new InvalidOperationException("Connection string 'ChatContextConnection' not found.");

// Add services to the container.
builder.Services.AddDbContext<ChatContext>(cfg =>
{
    cfg.UseSqlServer();
});

builder.Services.AddDefaultIdentity<ChatUser>()
    .AddEntityFrameworkStores<ChatContext>();

builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddControllers().AddRazorRuntimeCompilation();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

app.Run();
