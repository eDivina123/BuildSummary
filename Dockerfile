#build stage: the full sdk 
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

#copy projct files first so we can restore dependencies before copying the rest of the files and building
COPY src/BuildSummary.Core/BuildSummary.Core.csproj src/BuildSummary.Core/
COPY src/BuildSummary.Cli/BuildSummary.Cli.csproj src/BuildSummary.Cli/

#we dont need to do dotnet on buildsummary.core as it is a dependency of the cli project
RUN dotnet restore src/BuildSummary.Cli/BuildSummary.Cli.csproj 


#copy all and  compile 
COPY src/ src/
RUN dotnet publish src/BuildSummary.Cli/BuildSummary.Cli.csproj -c Release -o /app --no-restore


#just run time so it is smaller and faster to download
FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
#extract only the app file without the sdk from the build stage
COPY --from=build /app .
#when image is loaded run the cli app with dotnet and the dll file as arguments
ENTRYPOINT ["dotnet", "BuildSummary.Cli.dll"]

