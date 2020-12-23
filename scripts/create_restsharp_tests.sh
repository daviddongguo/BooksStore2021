#!/usr/bin/env bash
Root=${PWD##*/}

./scripts/update_mvc.sh
cd $Root.Mvc && dotnet run &
sleep 6

cd $Root.RestSharpTests && pwd
dotnet add package RestSharp
rm UnitTest1.css
cat > AlwaysPassTest.cs << EOF
using NUnit.Framework;
using RestSharp;

namespace BooksStore2021.RestSharpTests
{
    public class LocalHost_Test
    {
        private RestClient _client;
        private readonly string BASE_URL = "https://localhost:5001";

        [SetUp]
        public void Setup()
        {
            _client = new RestClient(BASE_URL);
            _client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        }

        [TestCase(200), Timeout(8000)]
        public void Test_DefaultHome(int expectedstatusCode)
        {
            // Arrange
            var request = new RestRequest("/", Method.GET);

            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert
            Assert.That((int)response.StatusCode == expectedstatusCode);
            Assert.That(response.Content, Contains.Substring("bootstrap.min.css"));
            System.Console.WriteLine(response.ResponseUri);
            System.Console.WriteLine(response.StatusCode);
        }

        [TestCase(200), Timeout(8000)]
        public void Test_TodoesController(int expectedstatusCode)
        {
            // Arrange
            var request = new RestRequest("/todoes", Method.GET);

            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert
            Assert.That((int)response.StatusCode == expectedstatusCode);
            Assert.That(response.Content, Contains.Substring("kxytxr0cak4ay1nj4d"));
            System.Console.WriteLine(response.ResponseUri);
            System.Console.WriteLine(response.StatusCode);
        }
    }
}
EOF

dotnet test -l "console;verbosity=detailed"
cd ..

./scripts/killport.sh
