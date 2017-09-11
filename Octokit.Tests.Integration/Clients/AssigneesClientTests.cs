using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using Octokit.Tests.Integration;
using Octokit.Tests.Integration.Helpers;
using Xunit;

[Collection(VCRFixture.Key)]
public class AssigneesClientTests
{
    RepositoryContext _context;
    readonly VCRFixture fixture;

    public AssigneesClientTests(VCRFixture fixture)
    {
        this.fixture = fixture;
    }

    private async Task<IGitHubClient> Setup(string session)
    {
        var github = fixture.GetAuthenticatedClient(session);
        var repoName = Helper.MakeNameWithTimestamp("public-repo");
        _context = await github.CreateRepositoryContext(new NewRepository(repoName));
        return github;
    }

    [IntegrationTest]
    public async Task CanCheckAssignees()
    {
        var client = await Setup("issue\\assigness\\can-check");

        var isAssigned = await client.Issue.Assignee.CheckAssignee(_context.RepositoryOwner, _context.RepositoryName, "FakeHaacked");
        Assert.False(isAssigned);

        // Repository owner is always an assignee
        isAssigned = await client.Issue.Assignee.CheckAssignee(_context.RepositoryOwner, _context.RepositoryName, _context.RepositoryOwner);
        Assert.True(isAssigned);
    }

    [IntegrationTest]
    public async Task CanCheckAssigneesWithRepositoryId()
    {
        var client = await Setup("issue\\assigness\\can-check-with-id");

        var isAssigned = await client.Issue.Assignee.CheckAssignee(_context.Repository.Id, "FakeHaacked");
        Assert.False(isAssigned);

        // Repository owner is always an assignee
        isAssigned = await client.Issue.Assignee.CheckAssignee(_context.Repository.Id, _context.RepositoryOwner);
        Assert.True(isAssigned);
    }

    [IntegrationTest]
    public async Task CanListAssignees()
    {
        var client = await Setup("issue\\assigness\\can-list");

        // Repository owner is always an assignee
        var assignees = await client.Issue.Assignee.GetAllForRepository(_context.RepositoryOwner, _context.RepositoryName);
        Assert.True(assignees.Any(u => u.Login == Helper.UserName));
    }

    [IntegrationTest]
    public async Task CanAddAndRemoveAssignees()
    {
        var client = await Setup("issue\\assigness\\can-add-and-remove");

        var newAssignees = new AssigneesUpdate(new List<string>() { _context.RepositoryOwner });
        var newIssue = new NewIssue("a test issue") { Body = "A new unassigned issue" };
        var issuesClient = client.Issue;

        var issue = await issuesClient.Create(_context.RepositoryOwner, _context.RepositoryName, newIssue);

        Assert.NotNull(issue);

        var addAssignees = await client.Issue.Assignee.AddAssignees(_context.RepositoryOwner, _context.RepositoryName, issue.Number, newAssignees);

        Assert.IsType<Issue>(addAssignees);

        //Check if assignee was added to issue
        Assert.True(addAssignees.Assignees.Any(x => x.Login == _context.RepositoryOwner));

        //Test to remove assignees
        var removeAssignees = await client.Issue.Assignee.RemoveAssignees(_context.RepositoryOwner, _context.RepositoryName, issue.Number, newAssignees);

        //Check if assignee was removed
        Assert.False(removeAssignees.Assignees.Any(x => x.Login == _context.RepositoryOwner));
    }

    public async Task CanListAssigneesWithRepositoryId()
    {
        var client = await Setup("issue\\assigness\\can-add-and-remove-with-id");

        // Repository owner is always an assignee
        var assignees = await client.Issue.Assignee.GetAllForRepository(_context.Repository.Id);
        Assert.True(assignees.Any(u => u.Login == Helper.UserName));
    }
}