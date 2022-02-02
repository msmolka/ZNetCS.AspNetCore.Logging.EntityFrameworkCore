# ZNetCS.AspNetCore.Logging.EntityFrameworkCore

[![NuGet](https://img.shields.io/nuget/v/ZNetCS.AspNetCore.Logging.EntityFrameworkCore.svg)](https://www.nuget.org/packages/ZNetCS.AspNetCore.Logging.EntityFrameworkCore)
[![Build](https://github.com/msmolka/ZNetCS.AspNetCore.Logging.EntityFrameworkCore/workflows/build/badge.svg)](https://github.com/msmolka/ZNetCS.AspNetCore.Logging.EntityFrameworkCore/actions)

This is Entity Framework Core logger and logger provider. A small package to allow store logs in any data store using Entity Framework Core. It was prepared to use in ASP
NET Core application, but it does not contain any references that prevents to use it in plain .NET Core application.

As from version 2.0.2 there is silent error handling on logger SaveChanges(). To avoid Db error having impact on application.

## Installing

Install using the [ZNetCS.AspNetCore.Logging.EntityFrameworkCore NuGet package](https://www.nuget.org/packages/ZNetCS.AspNetCore.Logging.EntityFrameworkCore)

```
PM> Install-Package ZNetCS.AspNetCore.Logging.EntityFrameworkCore
```

## Usage

When you install the package, it should be added to your `.csproj`. Alternatively, you can add it directly by adding:

```xml
<ItemGroup>
    <PackageReference Include="ZNetCS.AspNetCore.Logging.EntityFrameworkCore" Version="6.0.0" />
</ItemGroup>
```

### .NET 6
In order to use the IP filtering middleware, you must configure the services in the `Program.cs` file.

```c#
// Add services to the container.
builder.Logging.AddEntityFramework<MyDbContext>();
```


### .NET 5 and Below

```c#
public static void Main(string[] args)
{
    var webHost = new WebHostBuilder()
        // other code ommited to focus on logging settings
        .ConfigureLogging((hostingContext, logging) =>
        {
            // other log providers
            // ...
            //
            logging.AddEntityFramework<MyDbContext>();

        })
        .UseStartup<Startup>()
        .Build();

    webHost.Run();
}

```

### Important Notes

In most case scenario you would not like add all logs from application to database. A lot of of them is jut debug/trace ones. In that case is better use filter before
add `Logger`. This will also prevent some `StackOverflowException` when using this logger to log `EntityFrameworkCore` logs.

### .NET 6

```c#
builder.Logging..AddFilter<EntityFrameworkLoggerProvider<MyDbContent>>("Microsoft", LogLevel.None);
builder.Logging..AddFilter<EntityFrameworkLoggerProvider<MyDbContent>>("System", LogLevel.None);
builder.Logging..AddEntityFramework<MyDbContext>();
```

### .NET 5 and Below

```c#
public static void Main(string[] args)
{
    var webHost = new WebHostBuilder()
        // other code ommited to focus on logging settings
        .ConfigureLogging((hostingContext, logging) =>
        {
            // other log providers
            // ...
            //

            // because setting up filter inside code requires exact provider class, and EntityFrameworkLoggerProvider is generic class with multiple overrides
            // filters needs to applied properly to chosen provider
            logging.AddFilter<EntityFrameworkLoggerProvider<MyDbContent>>("Microsoft", LogLevel.None);
            logging.AddFilter<EntityFrameworkLoggerProvider<MyDbContent>>("System", LogLevel.None);
            logging.AddEntityFramework<MyDbContext>();

        })
        .UseStartup<Startup>()
        .Build();

    webHost.Run();
}
```

It is also possible to setting filters inside `appsettings.json` file. This provider is using `EntityFramework` alias. This way is recommended because there is no need to
care about proper provider definition.

```json
{
  "Logging": {
    "EntityFramework": {
      "LogLevel": {
        "Microsoft": "None",
        "System": "None"
      }
    }
  }
}
```

```c#
public static void Main(string[] args)
{
    var webHost = new WebHostBuilder()
        // other code ommited to focus on logging settings
        .ConfigureLogging((hostingContext, logging) =>
        {
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

            // other log providers
            // ...
            //

            logging.AddEntityFramework<MyDbContext>();

        })
        .UseStartup<Startup>()
        .Build();

    webHost.Run();
}
```

Then you need to setup your context to have access to log table e.g.

```c#
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

There is also possibility to extend base `Log` class.

```c#

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

Change `MyDbContext` to use new extended log model

```c#
public DbSet<ExtendedLog> Logs => this.Set<ExtendedLog>;
```

You can extend `ModelBuilder` as well:

```c#

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

To use `IHttpContextAccessor` there is need to register it inside `ConfigureServices` call of `Startup`:

```c#

public void ConfigureServices(IServiceCollection services)
{
    // requires for http context access.
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
}

```

Add extended log registration

```c#
public static void Main(string[] args)
{
    var webHost = new WebHostBuilder()
        // other code ommited to focus on logging settings
        .ConfigureLogging((hostingContext, logging) =>
        {
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

            // other log providers
            // ...
            //

            logging.AddEntityFramework<MyDbContext, ExtendedLog>();

        })
        .UseStartup<Startup>()
        .Build();

    webHost.Run();
}
```

There is also possibility to create new log model using custom creator method. This can be done by providing options during configuration.

```c#
public static void Main(string[] args)
{
    var webHost = new WebHostBuilder()
        // other code ommited to focus on logging settings
        .ConfigureLogging((hostingContext, logging) =>
        {
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

            // other log providers
            // ...
            //

            logging.AddEntityFramework<MyDbContext>(
                opts =>
                {
                    opts.Creator = (logLevel, eventId, name, message) => new Log
                    {
                        TimeStamp = DateTimeOffset.Now,
                        Level = logLevel,
                        EventId = eventId,
                        Name = "This is my custom log",
                        Message = message
                    };
                });

        })
        .UseStartup<Startup>()
        .Build();

    webHost.Run();
}
```


