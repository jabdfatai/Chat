using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SecureCommSvc.Application.Services;
using SecureCommSvc.Application.Services.Interface;
using SecureCommSvc.Core.Repo.Interface;
using SecureCommSvc.Infrastructure.Context;
using SecureCommSvc.Infrastructure.Providers;
using SecureCommSvc.Infrastructure.Repository;
using UserIdentitySvc.Core.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SecureConnDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SecureConnDBConnString"), options => options.EnableRetryOnFailure()));
//builder.Services.AddSingleton<IHubContext<ChatHub>>();


builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IChatCommandRepo, ChatCommandRepo>();
builder.Services.AddTransient<IChatQueryRepo, ChatQueryRepo>();

builder.Services.AddTransient<IUserCommandRepo, UserCommandRepo>();
builder.Services.AddTransient<IUserQueryRepo, UserQueryRepo>();

builder.Services.AddTransient<IEventStreamProvider, EventStreamProvider>();


//var corshost = builder.Configuration.GetValue<string[]>("Parameter:CorsAllowedHosts");

var corshost = builder.Configuration.GetSection("Parameter:CorsAllowedHosts").Get<List<string>>();

//builder.Services.AddCors(options => options.AddPolicy("AllowLocalhost7121",
//              builder =>
//              {
//                  builder.AllowAnyHeader()
//                         .AllowAnyMethod()
//                         .SetIsOriginAllowed((host) => true)
//                         .AllowCredentials();
//              }));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost7121",
        builder =>
        {
            builder.WithOrigins(corshost.ToArray())

                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); ;
            // Do not use .AllowCredentials() here
        });
});


builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors("AllowLocalhost7121");

//app.UseCors("AllowAll");


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub").RequireCors("AllowLocalhost7121");
//app.MapHub<ChatHub>("/chatHub");//.RequireCors("AllowAll");
app.Run();
