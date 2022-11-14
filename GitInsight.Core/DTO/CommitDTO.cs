namespace GitInsight.Core;

public record CommitDTO(int Hash, string Message, System.DateTime Date, string Author, int RepoHash);

public record CommitCreateDTO(int Hash, string Message, System.DateTime Date, string Author, int RepoHash);