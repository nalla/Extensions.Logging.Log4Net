using System;
using Microsoft.Extensions.DependencyInjection;
using Extensions.Logging.Log4Net;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging
{
	/// <summary>
	///     Extensions to the <see cref="ILoggingBuilder" />
	/// </summary>
	public static class LoggingBuilderExtensions
	{
		/// <summary>
		///     Adds the log4net logging provider.
		/// </summary>
		/// <param name="builder">The logging builder instance.</param>
		/// <param name="repositoryName">The name to be used when creating the log4Net repository.</param>
		/// <param name="configure">
		///     An optional log4net configuration action invoked before the provider is added to the
		///     <see cref="ILoggingBuilder" />.
		/// </param>
		/// <returns>The <see ref="ILoggingBuilder" /> passed as parameter with the new provider registered.</returns>
		public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, string repositoryName, Action configure = null)
		{
			configure?.Invoke();
			builder.Services.AddSingleton<ILoggerProvider>(new Log4NetProvider(repositoryName));

			return builder;
		}
	}
}
