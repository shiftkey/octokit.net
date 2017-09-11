using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Octokit;
using Octokit.Tests.Integration;
using Xunit;

public class UsersClientTests
{
    [Collection(VCRFixture.Key)]
    public class TheGetMethod
    {
        readonly VCRFixture fixture;

        public TheGetMethod(VCRFixture fixture)
        {
            this.fixture = fixture;
        }


        [IntegrationTest]
        public async Task ReturnsSpecifiedUser()
        {
            var github = fixture.GetAuthenticatedClient("user\\user");

            var user = await github.User.Get("tclem");

            Assert.Equal("GitHub", user.Company);
            Assert.Equal(AccountType.User, user.Type);
        }

        [IntegrationTest]
        public async Task ReturnsSpecifiedOrganization()
        {
            var github = fixture.GetAuthenticatedClient("user\\org");

            var user = await github.User.Get("octokit");

            Assert.Null(user.Company);
            Assert.Equal(AccountType.Organization, user.Type);
        }

        // TODO: what should i do here?
        [IntegrationTest]
        public async Task ReturnsSpecifiedUserUsingAwaitableCredentialProvider()
        {
            var github = new GitHubClient(new ProductHeaderValue("OctokitTests"),
                new ObservableCredentialProvider());

            var user = await github.User.Get("tclem");

            Assert.Equal("GitHub", user.Company);
        }

        class ObservableCredentialProvider : ICredentialStore
        {
            public async Task<Credentials> GetCredentials()
            {
                return await Observable.Return(Helper.Credentials);
            }
        }
    }

    [Collection(VCRFixture.Key)]
    public class TheCurrentMethod
    {
        readonly VCRFixture fixture;

        public TheCurrentMethod(VCRFixture fixture)
        {
            this.fixture = fixture;
        }

        [IntegrationTest]
        public async Task ReturnsSpecifiedUser()
        {
            var github = fixture.GetAuthenticatedClient("user\\current");

            var user = await github.User.Current();

            Assert.Equal(Helper.UserName, user.Login);
            Assert.Equal(AccountType.User, user.Type);
        }
    }

    [Collection(VCRFixture.Key)]
    public class TheUpdateMethod
    {
        readonly VCRFixture fixture;

        public TheUpdateMethod(VCRFixture fixture)
        {
            this.fixture = fixture;
        }

        [IntegrationTest]
        public async Task FailsWhenNotAuthenticated()
        {
            var github = fixture.GetAnonymousClient("user\\unauthenticated");

            var userUpdate = new UserUpdate
            {
                Name = Helper.Credentials.Login,
                Bio = "UPDATED BIO"
            };

            var e = await Assert.ThrowsAsync<AuthorizationException>(() => github.User.Update(userUpdate));
            Assert.Equal(HttpStatusCode.Unauthorized, e.StatusCode);
        }

        [IntegrationTest]
        public async Task FailsWhenAuthenticatedWithBadCredentials()
        {
            var github = fixture.GetClientWithBadCredentials("user\\error");

            var userUpdate = new UserUpdate
            {
                Name = Helper.Credentials.Login,
                Bio = "UPDATED BIO"
            };

            var e = await Assert.ThrowsAsync<AuthorizationException>(() => github.User.Update(userUpdate));
            Assert.Equal(HttpStatusCode.Unauthorized, e.StatusCode);
        }
    }
}
