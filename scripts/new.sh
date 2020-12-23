#!/usr/bin/env bash
Root=${PWD##*/}

# Create projects Add into solution, Add references

mkdir -p $Root.Console && cd *Console && dotnet new console --force && cd ..
mkdir -p $Root.Classlib && cd *Classlib && dotnet new classlib --force && cd ..
mkdir -p $Root.NunitTests && cd *NunitTests && dotnet new nunit --force && cd ..
mkdir -p $Root.Mvc && cd *Mvc && dotnet new mvc  --force && cd ..
mkdir -p $Root.RestSharpTests && cd *RestSharpTests && dotnet new nunit --force && cd ..

dotnet new sln --force
dotnet sln add ./$Root.Console/$Root.Console.csproj
dotnet sln add ./$Root.NunitTests/$Root.NunitTests.csproj
dotnet sln add ./$Root.Classlib/$Root.Classlib.csproj
dotnet sln add ./$Root.Mvc/$Root.Mvc.csproj
#dotnet sln add ./$Root.RestSharpTests/$Root.RestSharpTests.csproj

dotnet add ./$Root.Console/$Root.Console.csproj reference ./$Root.Classlib/$Root.Classlib.csproj
dotnet add ./$Root.Mvc/$Root.Mvc.csproj reference ./$Root.Classlib/$Root.Classlib.csproj
dotnet add ./$Root.NunitTests/$Root.NunitTests.csproj reference ./$Root.Mvc/$Root.Mvc.csproj
dotnet add ./$Root.NunitTests/$Root.NunitTests.csproj reference ./$Root.Classlib/$Root.Classlib.csproj
dotnet add ./$Root.RestSharpTests/$Root.RestSharpTests.csproj reference ./$Root.Classlib/$Root.Classlib.csproj
