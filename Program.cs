using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using DownloadFiles;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Download files API", Description = "Downloading files deposit to local machine", Version = "v1" });
});
builder.Services.AddSingleton<Files>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Download files API v1"); });
}

app.MapPost("/files/upload", async (HttpRequest request, Files filesUploadService) => {
    if (!request.HasFormContentType)
    {
        return Results.BadRequest("The request doesn't contain form data");
    }
    var form = await request.ReadFormAsync();
    var file = form.Files.GetFile("file");
    var filePath = request.Query["path"].ToString();
    var targetDir = form["targetDir"].ToString();
    try
    {
        var fullPath = await filesUploadService.upload(file,filePath,targetDir);
        return Results.Ok(new { filepath = fullPath });
    }catch(ArgumentException ex)
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

app.Run();
