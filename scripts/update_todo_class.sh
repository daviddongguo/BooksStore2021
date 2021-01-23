#!/usr/bin/env bash
Root=${PWD##*/}

cd $Root.Domain && pwd
dotnet add package Microsoft.EntityFrameworkCore -v 3.1.10
dotnet add package Microsoft.AspNetCore.Mvc.DataAnnotations -v 2.2.0



rm Class1.cs
mkdir -p Entities
mkdir -p Services

cd Entities
cat > Todo.cs << EOF
using System.Text.Json;

namespace $Root.Domain.Entities
{
    public class Todo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; } = false;

        public override string ToString() => PrettyJson(ToJSON());

        public static string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

            return JsonSerializer.Serialize(jsonElement, options);
        }

        public string ToJSON() => JsonSerializer.Serialize(this);
    }
}
EOF

cd ../Services
cat > EFDbContext.cs << EOF
using $Root.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace $Root.Domain.Services
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Todo> Todoes { get; set; }
    }
}
EOF
cd ../.. && pwd
