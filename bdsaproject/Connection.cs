namespace app;

public class Connection : IDisposable
{
    private readonly SqlConnectionStringBuilder _builder;
    private readonly SqlConnection _connection;
    public Connection()
    {
        _builder = new SqlConnectionStringBuilder();
        _builder.DataSource = "localhost";
        _builder.UserID = "sa";
        _builder.Password = "SMBbdsaproject1";
        _connection = new SqlConnection(_builder.ConnectionString);
        _connection.Open();
    }

    public void createDB()
    {
        Console.WriteLine("Creating database ...");
        StringBuilder sb = new StringBuilder();
        sb.Append("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'bdsaproject')");
        sb.Append("CREATE DATABASE [bdsaproject];");
        String sql = sb.ToString();
        SqlCommand command = new SqlCommand(sql, _connection);
        command.ExecuteNonQuery();
        Console.WriteLine("Database created.");
    }

    public void createTable()
    {
        Console.WriteLine("Creating table ...");
        StringBuilder sb = new StringBuilder();
        sb.Append("USE [bdsaproject];");
        sb.Append("IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Commits]') AND type in (N'U'))");
        sb.Append("CREATE TABLE [dbo].[Commits] (");
        sb.Append("[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,");
        sb.Append("[Author] NVARCHAR(50) NOT NULL,");
        sb.Append("[Date] DATE NOT NULL,");
        sb.Append("[Commits] INT NOT NULL");
        sb.Append(");");
        String sql = sb.ToString();
        SqlCommand command = new SqlCommand(sql, _connection);
        command.ExecuteNonQuery();
        Console.WriteLine("Table created.");
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public void insertCommits(Dictionary<String, Dictionary<DateTime, int>> commits)
    {
        Console.WriteLine("Inserting data ...");
        StringBuilder sb = new StringBuilder();
        sb.Append("USE [bdsaproject];");
        sb.Append("DELETE FROM [dbo].[Commits];");
        String sql = sb.ToString();
        SqlCommand command = new SqlCommand(sql, _connection);
        command.ExecuteNonQuery();
        foreach (var author in commits)
        {
            foreach (var commit in author.Value)
            {
                sb = new StringBuilder();
                sb.Append("USE [bdsaproject];");
                sb.Append("INSERT INTO [dbo].[Commits] ([Author], [Date], [Commits]) VALUES (");
                sb.Append("'" + author.Key + "',");
                sb.Append("'" + commit.Key.ToString("yyyy-MM-dd") + "',");
                sb.Append(commit.Value);
                sb.Append(");");
                sql = sb.ToString();
                command = new SqlCommand(sql, _connection);
                command.ExecuteNonQuery();
            }
        }
        Console.WriteLine("Data inserted.");
    }

    public void selectCommits()
    {
        Console.WriteLine("Selecting data ...");
        StringBuilder sb = new StringBuilder();
        sb.Append("USE [bdsaproject];");
        sb.Append("SELECT * FROM [dbo].[Commits];");
        String sql = sb.ToString();
        SqlCommand command = new SqlCommand(sql, _connection);
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine("{0} {1} {2} {3}", reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2).ToShortDateString(), reader.GetInt32(3));
        }
        Console.WriteLine("Data selected.");
    }
}