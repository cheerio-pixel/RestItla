#!/bin/bash

echo "This shel script is not meant to be run as-is."
echo "Is only a reference to copy-paste into the terminal"
echo "and make it easier to write new migrations names"
echo "and rutinary taks."

return;

"User ID=MSSQLTest;Password=TestTest123;Server=localhost,1433;Database=RestItla;Trusted_Connection=false;MultipleActiveResultSets=True;TrustServerCertificate=true"

# Instaling tools and setting up
dotnet new tool-manifest

dotnet tool install dotnet-ef --version 7.0.0

systemctl start mssql-server.service



# Remove last migration
dotnet ef migrations remove \
       --context RestItlaIdentityContext \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Identity/RestItla.Infrastructure.Identity.csproj

# Reset everything
dotnet ef database update 0 \
       --context RestItlaIdentityContext \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Identity/RestItla.Infrastructure.Identity.csproj


## Identity

## InitialCreate

# Create migration
dotnet ef migrations add InitialCreate \
       --context RestItlaIdentityContext \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Identity/RestItla.Infrastructure.Identity.csproj

# Apply migration
dotnet ef database update InitialCreate \
       --context RestItlaIdentityContext \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Identity/RestItla.Infrastructure.Identity.csproj


## Normal

# Remove last migration
dotnet ef migrations remove \
       --context MainContext  \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Persistence/RestItla.Infrastructure.Persistence.csproj

# Reset everything
dotnet ef database update 0 \
       --context MainContext  \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Persistence/RestItla.Infrastructure.Persistence.csproj


## InitialCreate

# Create migration
dotnet ef migrations add InitialCreate \
       --context MainContext  \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Persistence/RestItla.Infrastructure.Persistence.csproj

# Apply migration
dotnet ef database update InitialCreate \
       --context MainContext  \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Persistence/RestItla.Infrastructure.Persistence.csproj

## RenameColumnIngredient

# Create migration
dotnet ef migrations add RenameColumnIngredient \
       --context MainContext  \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Persistence/RestItla.Infrastructure.Persistence.csproj

# Apply migration
dotnet ef database update RenameColumnIngredient \
       --context MainContext  \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Persistence/RestItla.Infrastructure.Persistence.csproj

## AddJoinEntities

# Create migration
dotnet ef migrations add AddJoinEntities \
       --context MainContext  \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Persistence/RestItla.Infrastructure.Persistence.csproj

# Apply migration
dotnet ef database update AddJoinEntities \
       --context MainContext  \
       --startup-project RestItla.WebApi/RestItla.WebApi.csproj \
       --project RestItla.Infrastructure.Persistence/RestItla.Infrastructure.Persistence.csproj
