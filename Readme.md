# Stratasys - 3D Printer Queue 

The project was written in Asp.NET Core 3.1 + Angular8
and simulate printer and it's printing queue

### Installing

To run the solution correctly you should:
* Visual Studio
* .Net Core 3.1 sdk/runtime
* Angular 8, NodeJS cli's
* run 'npm install' in ClientApp, to install the packages I used in the client app.

You should run it with the F5 in visual studio, the backend is going to build and start the client app and run
it as a SPA in the same port the IIS is assigning and running the backend.

### Personal Notes
I did the assignment and was ready to submit, 
somehow I thought it should be only some static queue management without the actual printer and a background work,
later after talking to Ariel I understand what was expected.
Performing background long time jobs in webserver application is not a good practice, 
this stuff should handled with external windows service or with the actual printer and it's probaly exposed API.
Anyway I succeeded doing something that makes it look and feel near the desired mission
with background loop task informing the client with signalr, it's not so thread safe.
Also since I'm using json file as db, I asking the wholde "DB" after each operation made, to keep it consistent as I can.
For my convenience I assumed that a cancelled job is going to the end of the queue and will be reprinted when it's new time will come unless you delete it before, also you can't cancel printing job if it's the only job left in the queue, 
beyond that the app should do what was asked, working and looks nice.


Thanks!
Kobby Maman
0545844313