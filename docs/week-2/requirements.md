# Functional requirements
- The application has to implement a database.
- The application should be able to read from a local repository and store the data inside the database.
- The application should be able to print the data in the terminal. It should be able to print only the commits, that being the date and number of commits on that date. It should also be able to print the commits the same way, but including the name of the author of those commits.

# Non-functional requirements
- The application needs to be written in C#
- The application have to use the libgit2sharp library
- It should implement a database.
- The application collects data from a local repository and stores it in the database.
- When analyzing/reading from the repo, if the analyzed repo contains commits that are newer than the last analysis, the database should be updated with the new reading. If the repo is analyzed but does not contain new commits, the application will skip it.
- The commits are then visible in the terminal.
