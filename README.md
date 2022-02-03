# Jobsity .NET Challenge
## Chatroom App

[![N|Solid](https://raw.githubusercontent.com/json-api-dotnet/JsonApiDotnetCore/master/logo.png)](https://raw.githubusercontent.com/json-api-dotnet/JsonApiDotnetCore/master/logo.png)
## Description
This project is designed to test your knowledge of back-end web technologies, specifically in 
.NET and assess your ability to create back-end products with attention to details, standards,
and reusability.


## Assignment
The goal of this exercise is to create a simple browser-based chat application using .NET.

This application should allow several users to talk in a chatroom and also to get stock quotes
from an API using a specific command.


## Installation

Clone or download the repo and open the `JobsityChatApp.sln` file. 

Run the next command to create database and tables.
```sh
dotnet ef database update
```
Open the `Client` directory in the terminal and run the next command to install dependencies.
```sh
npm install
```
Run `client` with
```sh
ng serve -o
```