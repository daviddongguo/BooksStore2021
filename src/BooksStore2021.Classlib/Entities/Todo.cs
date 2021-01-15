using System.Text.Json;

namespace BooksStore2021.Classlib.Entities
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
