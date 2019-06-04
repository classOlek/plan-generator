
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
<center><img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/0.png" /></center>
Then fill it with your credentials.

2. After creating account you should login with account created in step 1.
<center><img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/1.png" /></center>

After logging in you'll notice control panel looking like this:
<center><img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/2.png" /></center>

3. To generate timetable you need to upload your data source first. To do so you can use scripts prepared to fetch them from your university website. Eventually you can prepare them yourself. You can find dataSimple.json file in "sample" folder.
<center><img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/3.png" /></center>

4. Right side of control panel is dedicated for user conditions that are global for all data sources. You can set up conditions like days off or class limitations.
5. After setting up conditions, you should select data source from list on the left side.
6. In next step you should choose which courses you're interested in. Also it is possible to avoid specific lecturers by simply unchecking them.
<center><img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/5.png" /></center>
7. After clicking "Generate" button there is last step that require you to browse through generated timetables.
<center><img src="https://raw.githubusercontent.com/classOlek/plan-generator/master/doc/4.png" /></center>

# Database Scheme
Since there is very few data stored on the server side, I decided to use No-SQL database. MongoDB is document based (JSON-like) db, that is really easy to modify, with no need to declare it structure.
User accounts and data sources are stored in two separated collections, configurable in Configuration.cs file.
