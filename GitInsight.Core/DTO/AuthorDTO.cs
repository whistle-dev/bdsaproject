namespace GitInsight.Core;

public record AuthorDTO(string Hash, string Name, string Email);

public record AuthorCreateDTO(string Hash, string Name, string Email);