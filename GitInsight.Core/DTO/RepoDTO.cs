namespace GitInsight.Core;

public record RepoDTO(string Hash, string Name);

public record RepoCreateDTO(string Hash, string Name);