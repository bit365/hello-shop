```shell
dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations add InitialCreate --output-dir Infrastructure/Migrations
dotnet ef database update
```