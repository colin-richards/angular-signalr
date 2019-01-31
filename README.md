# angular-signalr
 
This system provides near real time monitoring of Ubisoft games on the Twitch platform.

The system has an Angular 7 web application and an ASP.NET Core 2.2 server.

Signalr for dotnet core 2.2 is used for websocket push notifications from the server to the client.

The client application shows viewer count per game in a dashboard format.

Highcharts for Angular is used for the charting components.

A background timer fires every minute to collect data from the Twitch API. 

The viewer count for games is computed and pushed to the client by Signalr.

The dashboard shows the viewer count over time for each monitored game.

Prerequisites:

Requires the dotnet core 2.2 sdk on your local machine or a higher version of the framework.

https://dotnet.microsoft.com/download

Installation

1. Clone this Git repository to a folder on your local machine
2. cd to the angular-signalr/ClientApp folder
3. install dependencies for the Angular app by typing npm i 
4. cd to the angular-signalr folder
5. Install dependencies for the ASP.NET Core 2.2 app by typing dotnet restore

Build

1. cd to the angular-signalr/ClientApp folder
2. type npm run build to build the Angular web application
3. cd to the angular-signalr folder
4. type dotnet build to build the server

Run

1. cd to the angular-signalr folder
2. type dotnet run to start the server
3. open a web browser and type http://localhost:5000 to load the web application
4. the web application will update every minute with game statistics












