# BDSA Project

## How to run the project

### Prerequisites

- [Docker](https://www.docker.com/)
- [.NET 7.0](https://dotnet.microsoft.com/download/dotnet/7.0)
- Initialized User Secrets

### Run the project

1.  Setup a terminal instance in the root of the project

2.  Run the following command

    ```bash
    docker-compose up -d --build
    ```

3.  Run the following command

    ```bash
    dotnet watch run --project GitInsight.API
    ```

4.  If new docker image run following commands:
    ```
    dotnet ef database update --project .\GitInsight.Infrastructure\ --startup-project .\GitInsight.API\
    ```
