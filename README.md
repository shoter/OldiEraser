# OldiEraser
Program used to automatically remove old files from our disk. 

Core of the system was written in .NET Standard. You can easilly add new platforms to the project because of that.


## Directory delete behaviours:

- Do nothing
  - Directories will not be touched inside given folder.
- Delete old directories
  - Program will decide whether to delete directory or not only based on it's age.
- Delete old files inside
  - Program will go inside the directory and try to remove files inside. If there is a .oldieconfig file inside then it will use delete behaviour specified by the directory.


![Example image](https://raw.githubusercontent.com/shoter/OldiEraser/master/Img/Example.png)