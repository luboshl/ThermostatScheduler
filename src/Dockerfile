FROM mcr.microsoft.com/dotnet/aspnet:5.0

ENV TZ=Europe/Prague

COPY ./ app/
WORKDIR /app
ENTRYPOINT ["dotnet", "ThermostatScheduler.WebApp.dll"]
