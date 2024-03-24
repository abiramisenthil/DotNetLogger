# Interview Tasks
The source repository hosting this test is read-only.  
Before you begin, you will need to import it into your own hosted repository.  

## Branching Strategy
Create your own branch based on master.  
Name it test/{YourFullName}.  
Do all work in this branch.  

## Interfaces
Create an interface for the Logger class.  
Update the logger class to implement the interface.  
Add a new method to the logger which takes a list of strings & writes them as a CSV.  
Create a new implementation of the logger class.  
* The LogMessage method should include a timestamp.  
* The other methods should not change.  

## Dependency Injection
Alter the Program code to implement dependency injection for the Logger.  

## JSON Tasks
Do the following tasks where indicated in the Program code:  
* Create a class structure based on the SampleData.json file.  
* Load the json file into your class structure.  
* Create a linq query which produces a list of all the material descriptions for the "FOIL" part.  
* Write the results of your material descriptions to the logger.  
* Change the PartWeight value of a part of your choice.  
* Serialise the edited object back to a new JSON file.  
* Add the new file to the Data folder within the project. Name it appropriately.  

## Unit Tests
Create a unit test project.  
Use a mocking nuget package of your choice, create mocked test methods for the Logger class.  
* Mock for a successful log messages.  
* Mock for an exception being thrown by the method.  

Create a non-mocked test which asserts on the number of Meta objects in each Part.  
Feel free to leave any other unit tests you may have created in testing your work.  

## YML
Create an example YAML build file for the project. It should:  
* Trigger from your branch name.  
* Build the project.  
* Run your unit tests.  

## Finish
Please let us know where you have hosted your solution!
