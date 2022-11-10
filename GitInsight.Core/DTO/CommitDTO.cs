namespace GitInsight.Core;

public record CommitDTO(string Hash, string Message, System.DateTime Date, string AuthorHash, string RepoHash);

public record CommitCreateDTO(string Hash, string Message, System.DateTime Date, string AuthorHash, string RepoHash);