# BDSA Project

## How to run the project

### Prerequisites

-   [Docker](https://www.docker.com/)
-   [.NET 7.0](https://dotnet.microsoft.com/download/dotnet/7.0)
-   [.NET Tool for EF Core](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
-   Initialized User Secrets


### Run the project

1.  Setup a terminal instance in the root of the project

2.  Run the following command

    ```bash
    docker-compose up -d
    ```

3. If it's the first time you run the project, you need to run the following command to create the database

    ```bash
    dotnet ef database update --project .\GitInsight.Infrastructure\ --startup-project .\GitInsight.API\
    ```

4.  Run the following command

    ```bash
    dotnet watch run --project .\GitInsight.API\
    ```
