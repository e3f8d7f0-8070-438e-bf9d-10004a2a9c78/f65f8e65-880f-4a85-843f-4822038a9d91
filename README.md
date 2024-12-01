# Meter Reading App

## What It Is

This app aims to satisfy the requirements of the technical assessment set and takes the form of a web based app that:

* Provides a mechanism for importing a meter reading CSV file as per the example provided

It's written as a C# MVC ASP.Net (.Net Core 8) application with jQuery on the front end using `XMLHttpRequest` objects to communicate with the back end.

I have only included a minimal set of custom CSS styling, and have left the default page layout and navigation from the project template largely in place.

I've followed the Façade design pattern and Single Responsibility Principle. Dependency Injection is used throughout, as is the async/await pattern.

## Getting Up and Running

The app is a self-contained Visual Studio solution with no dependencies other than NuGet packages.

Select Build > Run and the app will open in a browser.

You may be asked to allow the self-signed certificate that Visual Studio has created for localhost in IIS Express. It's safe to do so.

## Testing

There is a second project in the solution for running unit tests. These can be run using the built-in Visual Studio testing tools or 3rd party tools such as dotCover.

The majority of the app is covered by unit tests. Non-covered parts are the endpoints themselves, although those have been tested manually, and the code in `Program.cs` and `Startup.cs`.

I have used the latest versions of NUnit and Moq for tests.

## Data Storage

Data is persisted on the file system in a SQLite database which is provided within the solution. It has been populated with the account data supplied with the test.

## Assumptions

 I have presumed that the validation requirement for meter readings being in the format NNNNN means the majority of rows in the supplied example file
 fail validation and that internal storage of the meter reading values is fine as a number in the database as it could be formatted padded to 5 characters if required for output.
 