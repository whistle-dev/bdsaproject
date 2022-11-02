namespace app;

public class Program
{
    public class Options
    {
        [Option("path", Required = true, HelpText = "Sets path to the git repository")]
        public string Path { get; set; } = default!;

        [Option('m', "mode", Required = true, HelpText = "Mode 'f' for frequency mode, mode 'a' for author mode")]
        public char Mode { get; set; }
    }

    //put this in cmd
    //docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SMBbdsaproject1" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
    public static void Main(String[] args)
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = "localhost";
        builder.UserID = "sa";
        builder.Password = "SMBbdsaproject1";
        builder.InitialCatalog = "master";

        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
        {
            Console.WriteLine("Connecting to SQL Server ...");
            connection.Open();
            Console.WriteLine("Done.");
        }

        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                if (o.Mode == 'a' || o.Mode == 'f')
                {
                    GitInsight git = new GitInsight(o.Path, o.Mode);
                }
                else
                {
                    Console.WriteLine("Invalid mode. Please use either 'f' for frequency mode or 'a' for author mode.");
                }
            });

        //creating table if it doesn't exist
        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
        {
            connection.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("DROP TABLE IF EXISTS [dbo].[Commits];");
            sb.Append("CREATE TABLE [dbo].[Commits] (");
            sb.Append("[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,");
            sb.Append("[Author] VARCHAR(50) NOT NULL,");
            sb.Append("[Date] DATETIME NOT NULL,");
            sb.Append(");");
            String sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        //inserting data into table
        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
        {
            connection.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [dbo].[Commits] ([Author], [Date]) VALUES ('");
            sb.Append("Random TestAuthor1");
            sb.Append("', '");
            sb.Append("2022-11-02");
            sb.Append("');");
            String sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
        {
            connection.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [dbo].[Commits] ([Author], [Date]) VALUES ('");
            sb.Append("Random TestAuthor2");
            sb.Append("', '");
            sb.Append("2022-11-03");
            sb.Append("');");
            String sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }



        //reading data from table
        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
        {
            connection.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM [dbo].[Commits];");
            String sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2).ToShortDateString());
                    }
                }
            }
        }

    }
}
