FROM mcr.microsoft.com/dotnet/sdk:8.0 AS img-build
WORKDIR /home/app
COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done
RUN dotnet restore 
COPY . .
RUN dotnet test ./AfpCompanyApi.Tests/AfpCompanyApi.Tests.csproj

FROM img-build AS img-publish
COPY --from=img-build /home/app /home/app
RUN dotnet publish ./AfpCompanyApi/AfpCompanyApi.csproj -o /publish/
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS img-deploy
COPY --from=img-publish /publish /publish
WORKDIR /publish
ENV ASPNETCORE_URLS=https://+:5001;http://+:5000
ENV SECRET=ThisIsAmazing!
ENTRYPOINT ["dotnet", "AfpCompanyApi.dll"]