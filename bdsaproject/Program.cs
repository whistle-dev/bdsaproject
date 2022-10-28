namespace app;

public class Program
{

    public static void Main(String[] args)
    {
        String path = null;
        String mode = null;

        foreach (string arg in args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-path"))
                {
                    path = args[i + 1];
                }
                if (args[i].StartsWith("-mode"))
                {
                    mode = args[i + 1];
                }
            }
        }

        if(!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(mode)) {
          GitInsight git = new GitInsight(path, mode);
        } else {
          Console.WriteLine("Missing arguments");
        }
    }
}
