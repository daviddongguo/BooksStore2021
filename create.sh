#!/usr/bin/env bash
Root=${PWD##*/}

# Create projects Add into solution, Add references
./scripts/new.sh
pwd
ls -l
sleep 10

# Console project for debugging
cd $Root.Console && pwd
cat > Program.cs << EOF
namespace $Root.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Console project Created, \n");
        }
    }
}
EOF
dotnet run
cd ..

# Nunit project
cd $Root.NunitTests && pwd
rm UnitTest1.cs
cat > AlwaysPassTest.cs << EOF
using NUnit.Framework;

namespace $Root.NunitTests
{
    public class AlwaysPassTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Always_Pass_Test()
        {
            System.Console.WriteLine("Always Pass");
            Assert.Pass();
            Assert.That(true);
            Assert.That("Hello!", Is.EqualTo("HELLO!").IgnoreCase);
        }
    }
}
EOF
dotnet test -l "console;verbosity=detailed"
cd ..
