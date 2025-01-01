using ConsoleApp;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;

//menus.MainMenu.SetMenuItemAction("N", GameController.MainLoop);

var connectionString = $"Data Source={FileHelper.BasePath}app.db";
Console.WriteLine(connectionString);

//var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
    //.UseSqlite(connectionString)
    //.EnableDetailedErrors()
    //.EnableSensitiveDataLogging()
    //.Options;

//using var ctx = new AppDbContext(contextOptions);

//ctx.Database.Migrate();

menus.MainMenu.Run();