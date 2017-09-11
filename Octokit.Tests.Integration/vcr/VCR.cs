using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace VCRSharp
{
    public class VCR
    {
        public static string GetFixturePath(string session)
        {
            if (string.IsNullOrWhiteSpace(FixtureDirectory))
            {
                FixtureDirectory = GetDefaultFixturePath();
            }

            return Path.Combine(FixtureDirectory, session + ".json");
        }

        static string GetDefaultFixturePath()
        {
            var codeBase = Assembly.GetEntryAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public static string FixtureDirectory { get; set; }

        public static HttpClient WithCassette(string session)
        {
            if (string.IsNullOrWhiteSpace(FixtureDirectory))
            {
                FixtureDirectory = GetDefaultFixturePath();
            }

            var testCassettePath = GetFixturePath(session);
            var handler = new ReplayingHandler(testCassettePath);
            var httpClient = new HttpClient(handler);
            return httpClient;
        }
    }
}
