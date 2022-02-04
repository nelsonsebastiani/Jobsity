# Jobsity .NET Challenge
## Chatroom App

[![N|Solid](https://miro.medium.com/max/2000/1*7gauQRPKSzKJPEpVJnvORA.png)](https://miro.medium.com/max/2000/1*7gauQRPKSzKJPEpVJnvORA.png)
## Description
This project was proposed for a backend aptitude assessment, specifically in .NET. The project has a Chat between logged in users, a bot for API requests and unit tests

## Considerations
I chose not to make architecture too extensive, too complex, due to the size of the API's.

I chose to use everything in the API layer, as the API would only use 1 domain service.

I often weigh architecture with performance. In very small projects, a very extensive and complex architecture would make maintenance difficult, slow the system down a bit.

## Installation

Install dependencies : 
Microsoft SQL Server
Angular CLI

Download the source code and open the `JobsityChatApp.sln` file.

Configure multiples projects on solution startup (JobsityChatAPI and StockBotAPI)

Start the application and the ORM will take care of creating the database with its proper settings.

No need to use migration

## Client

Run this commands below:
```sh
npm install
```
After this you can start the application
```sh
ng serve -o
```