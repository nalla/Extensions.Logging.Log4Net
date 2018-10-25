# Extensions.Logging.Log4Net

Allows to configure Log4net as Microsoft Extensions Logging handler on any .NET Core application.

Thanks to [@anuraj](https://github.com/anuraj) for this [original blog post](https://dotnetthoughts.net/how-to-use-log4net-with-aspnetcore-for-logging/).


[![NuGet](https://img.shields.io/nuget/dt/Extensions.Logging.Log4net.svg)](https://www.nuget.org/packages/Extensions.Logging.Log4Net/) [![Build status](https://ci.appveyor.com/api/projects/status/g481qsmsvindvo96/branch/master?svg=true)](https://ci.appveyor.com/project/nalla/extensions-logging-log4net/branch/master)

## Example of use

* Install the package or reference the project into your .net core application.

* Add the `AddLog4Net()` call into your `ILoggerFactory` configuration method.

```csharp
serviceCollection.AddLogging(builder => builder
	.AddLog4Net()
	.SetMinimumLevel(LogLevel.Debug));
```

* Add a `log4net.config` file with the content:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <appender name="DebugAppender" type="log4net.Appender.DebugAppender" >
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
    </layout>
  </appender>
  <root>
    <level value="ALL"/>
    <appender-ref ref="DebugAppender" />
  </root>
</log4net>
```

You can found more configuration examples on [configuration documentation](/doc/CONFIG.md).

## Special thanks

Thank you very much to all contributors & users by its collaboration, and specially to:

* [@huorswords](https://github.com/huorswords) by his great job on the aspnetcore variant which was the base of this package.
