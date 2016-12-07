# ZNetCS.AspNetCore.Logging.EntityFrameworkCore

[![NuGet](https://img.shields.io/nuget/v/ZNetCS.AspNetCore.Logging.EntityFrameworkCore.svg)](https://www.nuget.org/packages/ZNetCS.AspNetCore.Logging.EntityFrameworkCore)

This is Entity Framework Core logger and logger provider. A small package to allow store logs in any data store using Entity Framework Core. It was prepared to use in ASP NET Core application, but it does not contain any 
references that prevents to use it in plain .NET Core application.

## Installing 

Install using the [ZNetCS.AspNetCore.Logging.EntityFrameworkCore NuGet package](https://www.nuget.org/packages/ZNetCS.AspNetCore.Logging.EntityFrameworkCore)

```
PM> Install-Package ZNetCS.AspNetCore.Logging.EntityFrameworkCore
```

##Usage 

When you install the package, it should be added to your `package.json`. Alternatively, you can add it directly by adding:


```json
{
  "dependencies" : {
    "ZNetCS.AspNetCore.Logging.EntityFrameworkCore": "1.0.0"
  }
}
```

In order to use the Entity Framework Logger Provider, you must configure the logger factory in `Configure` call of `Startup`: 

```csharp
using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;
```

```
...
```

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
{
    // MyDbContext is registered in ConfigureServices Entity Framework Core application context
	loggerFactory.AddEntityFramework<MyDbContent>(serviceProvider);

	// other middleware e.g. MVC etc
}
```
Then you need to setup your context to have access to log table e.g.

```csharp
using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions options) : base(options)
    {
    }
   
    public DbSet<Log> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // build default model.
        LogModelBuilderHelper.Build(modelBuilder.Entity<Log>());

        // real relation database can map table:
        modelBuilder.Entity<Log>().ToTable("Log");
    }   
}
```

There is also possibilty to extend base `Log` class.
```csharp

public class ExtendedLog : Log
{
    public ExtendedLog(IHttpContextAccessor accessor)
    {          
        string browser = accessor.HttpContext.Request.Headers["User-Agent"];
        if (!string.IsNullOrEmpty(browser) && (browser.Length > 255))
        {
            browser = browser.Substring(0, 255);
        }

        this.Browser = browser;
        this.Host = accessor.HttpContext.Connection?.RemoteIpAddress?.ToString();
        this.User = accessor.HttpContext.User?.Identity?.Name;
        this.Path = accessor.HttpContext.Request.Path;
    }

    protected ExtendedLog()
    {
    }
      
    public string Browser { get; set; }
    public string Host { get; set; }   
    public string Path { get; set; }    
    public string User { get; set; }
}
```

Then change registration in `Configure` call of `Startup`: 

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
{
    // MyDbContext is registered in ConfigureServices Entity Framework Core application context
	loggerFactory.AddEntityFramework<MyDbContext, ExtendedLog>(serviceProvider);

	// other middleware e.g. MVC etc
}

public void ConfigureServices(IServiceCollection services)
{
    // requires for http context access.
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
}

```

Change `MyDbContext` to use new extended log model

```csharp
public DbSet<ExtendedLog> Logs { get; set; }
```

You can extend `ModelBuilder` as well:

```csharp

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // build default model.
    LogModelBuilderHelper.Build(modelBuilder.Entity<ExtendedLog>());

    // real relation database can map table:
    modelBuilder.Entity<ExtendedLog>().ToTable("Log");

    modelBuilder.Entity<ExtendedLog>().Property(r => r.Id).ValueGeneratedOnAdd();

    modelBuilder.Entity<ExtendedLog>().HasIndex(r => r.TimeStamp).HasName("IX_Log_TimeStamp");
    modelBuilder.Entity<ExtendedLog>().HasIndex(r => r.EventId).HasName("IX_Log_EventId");
    modelBuilder.Entity<ExtendedLog>().HasIndex(r => r.Level).HasName("IX_Log_Level");

    modelBuilder.Entity<ExtendedLog>().Property(u => u.Name).HasMaxLength(255);
    modelBuilder.Entity<ExtendedLog>().Property(u => u.Browser).HasMaxLength(255);
    modelBuilder.Entity<ExtendedLog>().Property(u => u.User).HasMaxLength(255);
    modelBuilder.Entity<ExtendedLog>().Property(u => u.Host).HasMaxLength(255);
    modelBuilder.Entity<ExtendedLog>().Property(u => u.Path).HasMaxLength(255);
}   

```

There is also possibility to create new log model using custom creator method (without resolving dependencies). This can be done by extending `loggerFactory.AddEntityFramework` call.

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
{
    // MyDbContext is registered in ConfigureServices Entity Framework Core application context
	loggerFactory 
        .AddEntityFramework<MyDbContext>(
            serviceProvider,
            creator: (logLevel, eventId, name, message) => new Log
            {
                TimeStamp = DateTimeOffset.Now,
                Level = logLevel,
                EventId = eventId,
                Name = "This is my custom log",
                Message = message
            });

	// other middleware e.g. MVC etc
}
```