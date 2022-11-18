#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
EXPOSE 5050 
COPY /DeliveryCorporation_Bot/bin/Release/net6.0 . 
ENTRYPOINT ["dotnet", "DeliveryCorporationBot.dll"] 