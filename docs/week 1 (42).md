# Week 1 (42) - Functional requirements and use cases

## Commit frequency mode
### Functional requirement
When running in the mode "commit frequency" the user should be shown an overview of the number of commits for each day.
### Use case
| Use case #1      	|                                                                                                                                                                                                                                                                                      	|
|------------------	|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	|
| Name             	| Commit frequency mode                                                                                                                                                                                                                                                                	|
| Purpose          	| The user is to be able to see the number of commits for each day.                                                                                                                                                                                                                    	|
| Initiating actor 	| User                                                                                                                                                                                                                                                                                 	|
| Initiating event 	| -                                                                                                                                                                                                                                                                                    	|
| Main flow        	| The system is to take input from the user, this determining the mode and the repository. The mode should be "commit frequency mode" and the repository should be valid. The system should in a coherent way display an overview of the commits for all days in the specified repository. 	|


## Commit author mode
### Functional requirement
When running in the mode "commit author" the user should be shown an overview of the number of commits for each day for all participating authors.

### Use case
| Use case #2      	|                                                                                                                                                                                                                                                                                                                          	|
|------------------	|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	|
| Name             	| Commit author mode                                                                                                                                                                                                                                                                                                       	|
| Purpose          	| The user is to be able to see the number of commits for each user for each day                                                                                                                                                                                                                                           	|
| Initiating actor 	| User                                                                                                                                                                                                                                                                                                                     	|
| Initiating event 	| -                                                                                                                                                                                                                                                                                                                        	|
| Main flow        	| The system is to take input from the user, this determining the mode and the repository. The mode should be "commit author mode" and the repository should be valid. The system should in a coherent way display an overview of the commits from all the participating authors for all days in the specified repository. 	|