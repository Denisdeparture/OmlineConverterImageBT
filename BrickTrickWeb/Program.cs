using BrickTrickWeb;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Diagnostics;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddTransient<ServiceImage>();
var app = builder.Build();
app.MapRazorPages();

app.UseStaticFiles();
app.UseDefaultFiles();

string pathFromSaveDirectory = "wwwroot/ConvertionImg";
string pathFromChangeDirectory = "wwwroot/NewImages";

var options = new RewriteOptions()
    .AddRedirect("(.*)[.html]", "$1")
    .AddRedirect("(.*)[.php]", "$1");
app.UseRewriter(options);

app.Environment.EnvironmentName = "Production";
// обработка любых ошибок
app.UseStatusCodePages(async statusCodeContext =>
{
    var response = statusCodeContext.HttpContext.Response;
    var path = statusCodeContext.HttpContext.Request.Path;
    response.ContentType = "text/html; charset=UTF-8";
    await response.SendFileAsync("wwwroot/htmlPages/Error.html");

});

app.Map("/Main",async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    string? id = null;
    string? name = null;
    if (context.Request.Method == "POST")
    {
        name = context.Request.Form["name"];
        id = context.Request.Form["pass"];
        if (id != null & name != null) { context.Response.Redirect($"/Main/{id}/{name}"); }
    }
    else
    {
        await context.Response.SendFileAsync("wwwroot/htmlPages/RegistrationPages.html");
    }
});
app.Map("/Main/{id}/{name}", (string id, string name) => Results.Redirect("/Main/Converter"));
app.Map("/Main/Conver", async context =>
{
var response = context.Response;
var request = context.Request;
response.ContentType = "text/html; charset=utf-8";
var Converter = app.Services.GetService<ServiceImage>();
   
    if (request.Method == "POST")
    {
        IFormFile? file = request.Form.Files.GetFile("UserPhoto");
        if (file != null)
        {
            Directory.CreateDirectory(pathFromSaveDirectory);
            Directory.CreateDirectory(pathFromChangeDirectory);
            var format = request.Form["format"];
            string fullPath = $"{pathFromSaveDirectory}/{file.FileName}";
            var fileStream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            Converter.pathDirectory = $"wwwroot/ConvertionImg/";
            fileStream.Close();
            Converter.ChangeImage(fullPath, format, file.FileName);
            response.Headers.ContentDisposition = $"attachment; filename= YourImg{format}";
            
            await response.SendFileAsync(@$"wwwroot/ConvertionImg/{file.FileName}{format}");
        }
        else
        {
            context.Response.Redirect("/Main/Conver");
        }
    }
    else
    {
        await response.SendFileAsync("wwwroot/htmlPages/MainPage.html");
    }
   
});
app.Map("/Main/abou", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/htmlPages/about.html");
});
app.Map("/Main/con", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/htmlPages/Contacts.html");
});
app.Run();

