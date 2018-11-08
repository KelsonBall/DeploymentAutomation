using System;

namespace GitHubModel
{
    public class EventModel
    {
        public string Ref { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public CommitModel[] Commits { get; set; }
        public CommitModel HeadCommit { get; set; }
        public RepositoryModel Repository { get; set; }
    }

    public class CommitModel
    {
        public string Id { get; set; }
        public string Tree_Id { get; set; }
        public bool Distinct { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Url { get; set; }
        public GitUserModel Author { get; set; }
        public GitUserModel Committer { get; set; }    
        public string[] Added { get; set; }
        public string[] Removed { get; set; }
        public string[] Modified { get; set; }    
    }

    public class GitUserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }

    public class GitHubUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Node_Id { get; set; }
        public string Avatar_Url { get; set; }
        public string GravatarId { get; set; }
        public string Url { get; set; }
        public string Followers_Url { get; set; }
        public string Following_Url { get; set; }
        public string Gists_Url { get; set; }
        public string Starred_Url { get; set; }
        public string Subscriptions_Url { get; set; }
        public string Organizations_Url { get; set; }
        public string Repos_Url { get; set; }
        public string Events_Url { get; set; }
        public string RecievedEvents_Url { get; set; }
        public string Type { get; set; }
        public bool SiteAdmin { get; set; }
    }

    public class RepositoryModel 
    {
        public int Id { get; set; }
        public string Node_Id { get; set; }
        public string Name  { get; set; }
        public string FullName  { get; set; }
        public bool Private  { get; set; }
        public GitHubUserModel Owner { get; set; }
        public string Html_Url { get; set; }
        public string Description { get; set; }
        public bool Fork { get; set; }       
    }

    // wow    
    public class Repository_Urls
    {
        public string Url { get; set; }
        public string Forks_Url { get; set; }
        public string Keys_Url { get; set; }
        public string Collaborators_Url { get; set; }
        public string Teams_Url { get; set; }
        public string Hooks_Url { get; set; }
        public string IssueEvents_Url { get; set; }
        public string Events_Url { get; set; }
        public string Assignees_Url { get; set; }
        public string Branches_Url { get; set; }
        public string Tags_Url { get; set; }
        public string Blobs_Url { get; set; }
        public string GitTags_Url { get; set; }
        public string GitRefs_Url { get; set; }
        public string Trees_Url { get; set; }
        public string Statuses_Url { get; set; }
        public string Languages_Url { get; set; }
        // and more!
    }
}