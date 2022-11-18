namespace GitInsight.Core;

public record CommitDTO(string Sha, string Message, System.DateTime Date, string Author, string RepoPath);

public record CommitCreateDTO(string Sha, string Message, System.DateTime Date, string Author, string RepoPath);