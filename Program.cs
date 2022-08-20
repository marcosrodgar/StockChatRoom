using Microsoft.EntityFrameworkCore;
using StockBotChatRoom.Data;
using StockBotChatRoom.Data.Repositories;
using StockBotChatRoom.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ChatContext>(cfg =>
{
    cfg.UseSqlServer();
});

builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddControllers();

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
