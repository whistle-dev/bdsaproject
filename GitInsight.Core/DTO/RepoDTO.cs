namespace GitInsight.Core;

public record RepoDTO(int Hash, string Name);

public record RepoCreateDTO(int Hash, string Name);