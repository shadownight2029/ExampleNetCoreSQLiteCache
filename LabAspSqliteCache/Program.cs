using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NeoSmart.Caching.Sqlite;
using NUnit.Framework;
using System;
using System.Text;

namespace LabAspSqliteCache
{
    class Program
    {
        IDistributedCache cc;

        static void Main(string[] args)
        {
            string helloText = string.Empty;
            string cachePath = System.IO.Path.Combine(Environment.CurrentDirectory, "cache.db");
            SqliteCacheOptions Configuration = new SqliteCacheOptions()
            {
                MemoryOnly = false,
                CachePath = cachePath,
            };

            var logger = new Logger<SqliteCache>(new LoggerFactory());
            var cache = new SqliteCache(Configuration, logger);
            var bytes = cache.Get("hello");
            if (bytes == null)
            {
                cache.Set("hello", Encoding.ASCII.GetBytes("hello"), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(1)
                });

                helloText = Encoding.ASCII.GetString(cache.Get("hello")) + " from set";

            }
            else
            {
                helloText = Encoding.ASCII.GetString(cache.Get("hello")) + " from cache";
            }

            Console.WriteLine(helloText);
            Console.ReadLine();
        }
    }
}
