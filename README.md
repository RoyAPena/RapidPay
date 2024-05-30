# Project Title

Api for process payment transaction and add balance to cards.

## Installation

This project use sql server database, so you need to have a sql server installed in your machine. the version that is used for this project is SQL Server 2019.

The connection string is in the appsettings.json file, you can change the connection string to your database.

For test the project you only need to run the command below:

``'update-database

when you finish run the project you will has a user created in the database with the following credentials:
username: Admin
password: Abc_12345

this is necessary for test the project, if you have some issue triying to login with this credentials, you can create a new user using the endpoint /api/auth/register
for do this you need to remove first the RequireAuthorization in the UserModule inside RapidPay.Presentation layer.
## Usage

You can test the project using the postman collection attached or consume in your project, for this one you need to ensure has the last version of psotman.

the postman collection has 3 environment variable:
bearerToken: you need to put the token that you get when you login in the application.
cardId: you need to put the card id that you get when you create a card.
url: this is the url that you need to put to test the project.