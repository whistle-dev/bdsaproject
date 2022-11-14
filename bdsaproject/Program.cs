﻿namespace app;
using static Connection;

public class Program
{
    public class Options
    {
        [Option("path", Required = true, HelpText = "Sets path to the git repository")]
        public string Path { get; set; } = default!;

        [Option('m', "mode", Required = true, HelpText = "Mode 'f' for frequency mode, mode 'a' for author mode")]
        public char Mode { get; set; }
    }
    public static void Main(String[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                if (o.Mode == 'a' || o.Mode == 'f')
                {
                    GitInsight git = new GitInsight(o.Path, o.Mode);
                    git.printCommits();
                    // GitInsight git = new GitInsight(o.Path, 'a'); // Only author mode implemented for db
                    // connection.createDB();
                    // connection.createTable();
                    // connection.insertCommits(git.getCommits());
                    //connection.selectCommits();
                }
                else
                {
                    Console.WriteLine("Invalid mode. Please use either 'f' for frequency mode or 'a' for author mode.");
                }
            });
    }
}
