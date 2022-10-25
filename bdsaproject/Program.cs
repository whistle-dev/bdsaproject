namespace Gitinsight;

public class Program
{
    public static void Main(String[] args)
    {
        Console.WriteLine(@"GitInsights");
        Console.WriteLine("=================");
        Console.WriteLine("Enter path to git project:");

        string path = Console.ReadLine();
        Gitinsight GitInsight = new Gitinsight(path);
        // håndter path error
        // hvis fejl
        // while(fejl) Console.WriteLine("Error in path, please try again: ");

        Console.WriteLine("Successfully, connected to " + GitInsight.getRepoName());
        String mode = "1";
        while (mode != "3")
        {
            Console.WriteLine("[1] Commit Frequency");
            Console.WriteLine("[2] Commit Author");
            Console.WriteLine("[3] Exit");

            mode = Console.ReadLine();
            GitInsight.getCommits(mode);
        }

        // C:\Users\maxem\OneDrive - ITU\3. Semester\ADSA\Project\bdsaproject
    }
}