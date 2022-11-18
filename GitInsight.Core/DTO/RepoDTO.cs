namespace GitInsight.Core;

public record RepoDTO(string Path, string? LatestCommitSha);

public record RepoCreateDTO(string Path, string? LatestCommitSha);

public record RepoUpdateDTO(string Path, string? LatestCommitSha);