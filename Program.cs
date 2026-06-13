using Microsoft.EntityFrameworkCore;
using dotnet_task_manager_api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    db.Database.OpenConnection();

    using var command = db.Database.GetDbConnection().CreateCommand();
    command.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Tasks') WHERE name = 'CreatedAt';";

    var createdAtColumnExists = Convert.ToInt32(command.ExecuteScalar()) > 0;

    if (!createdAtColumnExists)
    {
        db.Database.ExecuteSqlRaw(
            "ALTER TABLE Tasks ADD COLUMN CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP;");
    }
}

app.Run();
