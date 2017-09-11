using System.Linq;
using System.Threading.Tasks;
using Octokit.Tests.Integration.Helpers;
using Xunit;
using Octokit.Tests.Integration;
using Octokit;

public class WatchedClientTests
{
    [Collection(VCRFixture.Key)]
    public class TheGetAllForCurrentMethod
    {
        readonly VCRFixture fixture;

        public TheGetAllForCurrentMethod(VCRFixture fixture)
        {
            this.fixture = fixture;
        }

        [IntegrationTest]
        public async Task CanRetrieveResults()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-for-current\\default");

            var repositories = await github.Activity.Watching.GetAllForCurrent();

            Assert.NotEmpty(repositories);
        }

        [IntegrationTest]
        public async Task ReturnsCorrectCountOfRepositoriesWithoutStart()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-for-current\\no-start");

            var options = new ApiOptions
            {
                PageSize = 3,
                PageCount = 1
            };

            var repositories = await github.Activity.Watching.GetAllForCurrent(options);

            Assert.Equal(3, repositories.Count);
        }

        [IntegrationTest]
        public async Task ReturnsCorrectCountOfRepositoriesWithStart()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-for-current\\with-start");

            var options = new ApiOptions
            {
                PageSize = 3,
                PageCount = 1,
                StartPage = 2
            };

            var repositories = await github.Activity.Watching.GetAllForCurrent(options);

            Assert.Equal(3, repositories.Count);
        }

        [IntegrationTest]
        public async Task ReturnsDistinctWatchersBasedOnStartPage()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-for-current\\with-skip");

            var startOptions = new ApiOptions
            {
                PageSize = 1,
                PageCount = 1
            };

            var firstPage = await github.Activity.Watching.GetAllForCurrent(startOptions);

            var skipStartOptions = new ApiOptions
            {
                PageSize = 1,
                PageCount = 1,
                StartPage = 2
            };

            var secondPage = await github.Activity.Watching.GetAllForCurrent(skipStartOptions);

            Assert.NotEqual(firstPage[0].Id, secondPage[0].Id);
        }
    }

    [Collection(VCRFixture.Key)]
    public class TheGetAllForUserMethod
    {
        readonly VCRFixture fixture;

        public TheGetAllForUserMethod(VCRFixture fixture)
        {
            this.fixture = fixture;
        }

        [IntegrationTest]
        public async Task CanRetrieveResults()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-for-user\\default");

            var repositories = await github.Activity.Watching.GetAllForUser(Helper.UserName);

            Assert.NotEmpty(repositories);
        }

        [IntegrationTest]
        public async Task ReturnsCorrectCountOfRepositoriesWithoutStart()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-for-user\\no-start");

            var options = new ApiOptions
            {
                PageSize = 3,
                PageCount = 1
            };

            var repositories = await github.Activity.Watching.GetAllForUser(Helper.UserName, options);

            Assert.Equal(3, repositories.Count);
        }

        [IntegrationTest]
        public async Task ReturnsCorrectCountOfRepositoriesWithStart()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-for-current\\with-start");

            var options = new ApiOptions
            {
                PageSize = 3,
                PageCount = 1,
                StartPage = 2
            };

            var repositories = await github.Activity.Watching.GetAllForUser(Helper.UserName, options);

            Assert.Equal(3, repositories.Count);
        }

        [IntegrationTest]
        public async Task ReturnsDistinctWatchersBasedOnStartPage()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-for-current\\with-skip");

            var startOptions = new ApiOptions
            {
                PageSize = 1,
                PageCount = 1
            };

            var firstPage = await github.Activity.Watching.GetAllForUser(Helper.UserName, startOptions);

            var skipStartOptions = new ApiOptions
            {
                PageSize = 1,
                PageCount = 1,
                StartPage = 2
            };

            var secondPage = await github.Activity.Watching.GetAllForUser(Helper.UserName, skipStartOptions);

            Assert.NotEqual(firstPage[0].Id, secondPage[0].Id);
        }
    }

    [Collection(VCRFixture.Key)]
    public class TheGetAllWatchersMethod
    {
        readonly VCRFixture fixture;

        public TheGetAllWatchersMethod(VCRFixture fixture)
        {
            this.fixture = fixture;
        }

        [IntegrationTest]
        public async Task CanRetrieveResults()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-watchers\\default");

            var repositories = await github.Activity.Watching.GetAllWatchers("octokit", "octokit.net");

            Assert.NotEmpty(repositories);
        }

        [IntegrationTest]
        public async Task CanRetrieveResultsWithRepositoryId()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-watchers\\with-id");

            var repositories = await github.Activity.Watching.GetAllWatchers(7528679);

            Assert.NotEmpty(repositories);
        }

        [IntegrationTest]
        public async Task ReturnsCorrectCountOfRepositoriesWithoutStart()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-watchers\\no-start");

            var options = new ApiOptions
            {
                PageSize = 3,
                PageCount = 1
            };

            var repositories = await github.Activity.Watching.GetAllWatchers("octokit", "octokit.net", options);

            Assert.Equal(3, repositories.Count);
        }

        [IntegrationTest]
        public async Task ReturnsCorrectCountOfRepositoriesWithoutStartWithRepositoryId()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-watchers\\no-start-with-id");

            var options = new ApiOptions
            {
                PageSize = 3,
                PageCount = 1
            };

            var repositories = await github.Activity.Watching.GetAllWatchers(7528679, options);

            Assert.Equal(3, repositories.Count);
        }

        [IntegrationTest]
        public async Task ReturnsCorrectCountOfRepositoriesWithStart()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-watchers\\with-start");

            var options = new ApiOptions
            {
                PageSize = 3,
                PageCount = 1,
                StartPage = 2
            };

            var repositories = await github.Activity.Watching.GetAllWatchers("octokit", "octokit.net", options);

            Assert.Equal(3, repositories.Count);
        }

        [IntegrationTest]
        public async Task ReturnsCorrectCountOfRepositoriesWithStartWithRepositoryId()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-watchers\\with-start-with-id");

            var options = new ApiOptions
            {
                PageSize = 3,
                PageCount = 1,
                StartPage = 2
            };

            var repositories = await github.Activity.Watching.GetAllWatchers(7528679, options);

            Assert.Equal(3, repositories.Count);
        }

        [IntegrationTest]
        public async Task ReturnsDistinctWatchersBasedOnStartPage()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-watchers\\with-start");

            var startOptions = new ApiOptions
            {
                PageSize = 1,
                PageCount = 1
            };

            var firstPage = await github.Activity.Watching.GetAllWatchers("octokit", "octokit.net", startOptions);

            var skipStartOptions = new ApiOptions
            {
                PageSize = 1,
                PageCount = 1,
                StartPage = 2
            };

            var secondPage = await github.Activity.Watching.GetAllWatchers("octokit", "octokit.net", skipStartOptions);

            Assert.NotEqual(firstPage[0].Id, secondPage[0].Id);
        }

        [IntegrationTest]
        public async Task ReturnsDistinctWatchersBasedOnStartPageWithRepositoryId()
        {
            var github = fixture.GetAuthenticatedClient("watched\\get-all-watchers\\different-pages");

            var startOptions = new ApiOptions
            {
                PageSize = 1,
                PageCount = 1
            };

            var firstPage = await github.Activity.Watching.GetAllWatchers(7528679, startOptions);

            var skipStartOptions = new ApiOptions
            {
                PageSize = 1,
                PageCount = 1,
                StartPage = 2
            };

            var secondPage = await github.Activity.Watching.GetAllWatchers(7528679, skipStartOptions);

            Assert.NotEqual(firstPage[0].Id, secondPage[0].Id);
        }
    }

    [Collection(VCRFixture.Key)]
    public class TheCheckWatchedMethod
    {
        readonly VCRFixture fixture;

        RepositoryContext _context;

        public TheCheckWatchedMethod(VCRFixture fixture)
        {
            this.fixture = fixture;
        }

        private async Task<IWatchedClient> Setup(string session)
        {
            var github = fixture.GetAuthenticatedClient(session);
            _context = await github.CreateRepositoryContext("public-repo");
            return github.Activity.Watching;
        }

        [IntegrationTest]
        public async Task CheckWatched()
        {
            var client = await Setup("watched\\check-watched\\default");

            var check = await client.CheckWatched(_context.RepositoryOwner, _context.RepositoryName);

            Assert.True(check);
        }

        [IntegrationTest]
        public async Task CheckWatchedWithRepositoryId()
        {
            var client = await Setup("watched\\check-watched\\with-id");

            var check = await client.CheckWatched(_context.Repository.Id);

            Assert.True(check);
        }
    }

    [Collection(VCRFixture.Key)]
    public class TheWatchRepoMethod
    {
        readonly VCRFixture fixture;

        RepositoryContext _context;

        public TheWatchRepoMethod(VCRFixture fixture)
        {
            this.fixture = fixture;
        }

        private async Task<IWatchedClient> Setup(string session)
        {
            var github = fixture.GetAuthenticatedClient(session);
            _context = await github.CreateRepositoryContext("public-repo");
            return github.Activity.Watching;
        }

        [IntegrationTest]
        public async Task WatchRepo()
        {
            var client = await Setup("watchers\\watch-repo\\default");

            var newSubscription = new NewSubscription
            {
                Subscribed = true
            };

            await client.UnwatchRepo("octocat", "hello-worId");

            var subscription = await client.WatchRepo("octocat", "hello-worId", newSubscription);
            Assert.NotNull(subscription);

            var newWatchers = await client.GetAllWatchers("octocat", "hello-worId");
            var @default = newWatchers.FirstOrDefault(user => user.Login == Helper.UserName);
            Assert.NotNull(@default);
        }

        [IntegrationTest]
        public async Task WatchRepoWithRepositoryId()
        {
            var client = await Setup("watchers\\watch-repo\\with-id");

            var newSubscription = new NewSubscription();

            await client.UnwatchRepo(18221276);

            var subscription = await client.WatchRepo(18221276, newSubscription);
            Assert.NotNull(subscription);

            var newWatchers = await client.GetAllWatchers(18221276);
            var @default = newWatchers.FirstOrDefault(user => user.Login == Helper.UserName);
            Assert.NotNull(@default);
        }
    }

    [Collection(VCRFixture.Key)]
    public class TheUnwatchRepoMethod
    {
        readonly VCRFixture fixture;

        RepositoryContext _context;

        public TheUnwatchRepoMethod(VCRFixture fixture)
        {
            this.fixture = fixture;
        }

        private async Task<IWatchedClient> Setup(string session)
        {
            var github = fixture.GetAuthenticatedClient(session);
            _context = await github.CreateRepositoryContext("public-repo");
            return github.Activity.Watching;
        }

        [IntegrationTest]
        public async Task UnwatchRepo()
        {
            var client = await Setup("watchers\\unwatch\\default");

            var newSubscription = new NewSubscription
            {
                Subscribed = true
            };

            await client.UnwatchRepo("octocat", "hello-worId");

            var subscription = await client.WatchRepo("octocat", "hello-worId", newSubscription);
            Assert.NotNull(subscription);

            await client.UnwatchRepo("octocat", "hello-worId");

            var newWatchers = await client.GetAllWatchers("octocat", "hello-worId");
            var @default = newWatchers.FirstOrDefault(user => user.Login == Helper.UserName);
            Assert.Null(@default);
        }

        [IntegrationTest]
        public async Task UnwatchRepoWithRepositoryId()
        {
            var client = await Setup("watchers\\unwatch\\with-id");

            var newSubscription = new NewSubscription();

            await client.UnwatchRepo(18221276);

            var subscription = await client.WatchRepo(18221276, newSubscription);
            Assert.NotNull(subscription);

            await client.UnwatchRepo(18221276);

            var newWatchers = await client.GetAllWatchers(18221276);
            var @default = newWatchers.FirstOrDefault(user => user.Login == Helper.UserName);
            Assert.Null(@default);
        }
    }
}
