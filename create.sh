
mkdir -p ${PWD##*/}.Console
mkdir -p ${PWD##*/}.Classlib
mkdir -p ${PWD##*/}.Nunit
mkdir -p ${PWD##*/}.Mvc

cd *Console && dotnet new console && cd ..
cd *Classlib && dotnet new classlib && cd ..
cd *Nunit && dotnet new nunit && cd ..
cd *Mvc && dotnet new mvc && cd ..
