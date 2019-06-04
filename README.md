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
```
public class Configuration
{
    public static string DatabaseIpAddress = "localhost";
    public static int DatabasePort = 27017;
    public static string DatabaseName = "TimetableGenerator";
    public static string DatabaseUsersCollectionName = "Users";
    public static string DatabaseTimetableDataCollectionName = "TimetableData";
}
```
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
This application is designed to work as web api, with client written in JS.

## Authentication
- <code>POST</code> api/account/login
- <code>POST</code> api/account/register
- <code>GET</code> api/account/logout

## Account Management
- <code>POST</code> account/updateConditions

## Timetable
- <code>POST</code> timetable/uploadCourses
- <code>GET</code> timetable/getCourseData
- <code>POST</code> timetable/generateTimetable

# Usage
1. First of all you should create user account. To do so, please click on "Register!" button in top right corner.
<img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/0.png" />
Then fill it with your credentials.

2.After creating account you should login with account created in step 1.
<img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/1.png" />

After logging in you'll notice control panel looking like this:
<img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/2.png" />

3. 
<img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/3.png" />
<img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/4.png" />
<img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/5.png" />