using Template.Domain.Interfaces;
using Template.Domain.Entities;
using Template.Infrastruture.Repositories;
using Template.Application.Services;
using Template.Infrastruture.configuration;

var builder = WebApplication.CreateBuilder(args);

// Load storage settings from configuration
var storageSettings = builder.Configuration.GetSection("StorageSettings").Get<StorageSettings>();

// Register services
builder.Services.AddScoped<IFileRepository<FileEntity>>(provider =>
    new AzureDataLakeFileRepository(storageSettings.ConnectionString, storageSettings.FileSystemName));

// Register FileService with its interface
builder.Services.AddScoped<IFileService<FileEntity>, FileService<FileEntity>>();

// Add controllers
builder.Services.AddControllers();

// Register the storage settings configuration section
builder.Services.Configure<StorageSettings>(builder.Configuration.GetSection("StorageSettings"));

// Configure Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
