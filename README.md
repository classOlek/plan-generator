# Timetable Selector
It is a web application designed to select proper timetable from available course pool.

## Prerequisites
- .NET Core 2.2
- MongoDB

## Quickstart
1. Download repository as zip archive or clone it with following command (Git required):
```
git clone https://github.com/classOlek/plan-generator.git
```
2. Edit 'Configuration.cs' file, fill it with MongoDB credentials
3. Build project
```
dotnet build
```
4. Run server
```
dotnet run --project TimetableGenerator
```
5. Open web browser and visit url displayed in cmd.

# Api information
This application is designed to work as web api, with client written in VUE.JS framework.

## Authentication
- <code>POST</code> account/login
- <code>POST</code> account/register
- <code>GET</code> account/logout

## Account Management
- <code>POST</code> account/updateConditions

## Timetable
- <code>POST</code> timetable/uploadCourses
- <code>GET</code> timetable/getCourseData
- <code>GET</code> timetable/generateTimetable