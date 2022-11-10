namespace GitInsight.Core
{
    public interface ICommitRepository
    {
        void Create(CommitCreateDTO commit);
        CommitDTO? Find(string hash);
        IReadOnlyCollection<CommitDTO> ReadAllInRepo(string repo);
        IReadOnlyCollection<System.DateTime> ReadAllUniqueDatesInRepo(string repo);
        IReadOnlyCollection<string> ReadAllUniqueAuthorsInRepo(string repo);
        IReadOnlyCollection<CommitDTO> ReadAllInRepoOnDate(string repo, System.DateTime date);
        IReadOnlyCollection<CommitDTO> ReadAllInRepoOnDateByAuthor(string repo, System.DateTime date, string author);
    }

}