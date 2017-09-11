using Octokit.Internal;
using System;
using VCRSharp;
using Xunit;

namespace Octokit.Tests.Integration
{
    public class VCRFixture : IDisposable
    {
        public const string Key = "VCR Tests";

        public VCRFixture()
        {
            Environment.SetEnvironmentVariable("VCR_MODE", "cache");
            // TODO: how to resolve this dynamically at runtime?
            VCR.FixtureDirectory = "c:\\Users\\shiftkey\\src\\octokit.net\\Octokit.Tests.Integration\\fixtures\\";
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable("VCR_MODE", "");
        }

        internal IGitHubClient GetAuthenticatedClient(string session)
        {
            var credentials = Helper.Credentials;
            var httpClient = VCR.WithCassette(session);
            var wrapper = new HttpClientWrapper(httpClient);
            var connection = new Connection(
                new ProductHeaderValue("OctokitTests"),
                Helper.TargetUrl,
                new InMemoryCredentialStore(credentials),
                wrapper,
                new SimpleJsonSerializer());
            var client = new GitHubClient(connection);
            return client;
        }

        internal IGitHubClient GetAnonymousClient(string session)
        {
            var httpClient = VCR.WithCassette(session);
            var wrapper = new HttpClientWrapper(httpClient);
            var connection = new Connection(
                new ProductHeaderValue("OctokitTests"),
                Helper.TargetUrl,
                new InMemoryCredentialStore(Credentials.Anonymous),
                wrapper,
                new SimpleJsonSerializer());
            var client = new GitHubClient(connection);
            return client;
        }

        internal IGitHubClient GetClientWithBadCredentials(string session)
        {
            var credentials = new Credentials(Guid.NewGuid().ToString(), "bad-password");
            var httpClient = VCR.WithCassette(session);
            var wrapper = new HttpClientWrapper(httpClient);
            var connection = new Connection(
                new ProductHeaderValue("OctokitTests"),
                Helper.TargetUrl,
                new InMemoryCredentialStore(credentials),
                wrapper,
                new SimpleJsonSerializer());
            var client = new GitHubClient(connection);
            return client;
        }
    }

    [CollectionDefinition("VCR Tests")]
    public class VCRCollection : ICollectionFixture<VCRFixture>
    {
    }
}
