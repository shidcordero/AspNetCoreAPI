# AspNetCoreAPI

Note: Don't forget to change the Connection String to match your database
1. Under API project, look for appsettings.json
2. Update both appsettings.json & appsettings.Development.json user and pass under ConnectionStrings - DefaultConnection

Default User:
username: superuser
password: Superuser@12345!


Product and Category api has authentication:
1. To start, use authenticate to get the token
2. Copy the token, above the API list, there's a Authorize button.
3. click and paste "Bearer token" and click Authorize
