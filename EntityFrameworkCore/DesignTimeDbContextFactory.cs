using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EntityFrameworkCore
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PostgreDbContext>
    {
        internal const string ConnectionStringName = "Default"; // 连接字符串名称

        public PostgreDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<PostgreDbContext> builder = new DbContextOptionsBuilder<PostgreDbContext>();
            IConfigurationRoot configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(ConnectionStringName)
            );

            return new PostgreDbContext(builder.Options);
        }
    }

    public static class AppConfigurations
    {
        private static readonly ConcurrentDictionary<string, IConfigurationRoot> ConfigurationCache;

        static AppConfigurations()
        {
            ConfigurationCache = new ConcurrentDictionary<string, IConfigurationRoot>();
        }

        public static IConfigurationRoot Get(string path, string environmentName = null)
        {
            var cacheKey = path + "#" + environmentName;
            return ConfigurationCache.GetOrAdd(
                cacheKey,
                _ => BuildConfiguration(path, environmentName)
            );
        }

        private static IConfigurationRoot BuildConfiguration(string path, string environmentName = null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); // 设置基础配置文件

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                builder = builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true); // 设置对应环境的配置文件
            }

            builder = builder.AddEnvironmentVariables(); // 添加环境变量

            return builder.Build();
        }
    }

    public static class WebContentDirectoryFinder
    {
        public static string CalculateContentRootFolder()
        {
            var coreAssemblyDirectoryPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            if (coreAssemblyDirectoryPath == null)
            {
                throw new Exception("Could not find location of AuthenticationService assembly!");
            }

            var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
            while (!DirectoryContains(directoryInfo.FullName, "EFCoreWithPostgreSqlSamples.sln")) // 项目文件
            {
                if (directoryInfo.Parent == null)
                {
                    throw new Exception("Could not find content root folder!");
                }

                directoryInfo = directoryInfo.Parent;
            }

            return Path.Combine(directoryInfo.FullName, $"Api"); // 存放配置文件的目录
        }

        private static bool DirectoryContains(string directory, string fileName)
        {
            return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
        }
    }

    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<PostgreDbContext> dbContextOptions,
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for IdentityContext */
            dbContextOptions
                .UseNpgsql(connectionString,
                    options =>
                    {
                        //options.EnableRetryOnFailure(3);
                    });
        }
    }
}
