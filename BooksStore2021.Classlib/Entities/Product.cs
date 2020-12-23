namespace BooksStore2021.Classlib.Entities
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json;

    public class Product
    {
        [Required]
        [StringLength(50, ErrorMessage = "Please enter the author of book")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Please specify a category")]
        public string Category { get; set; }

        [DataType(DataType.MultilineText)] // specify how a value is presented and edited
        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }

        [Required]
        [Range(0.01, 300.00, ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }

        [Key]
        [HiddenInput(DisplayValue = false)]   // render the property as a hidden form element
        public long ProductId { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Please enter a title of book")]
        public string Title { get; set; }

        public override string ToString()
        {

            return PrettyJson(ToJSON());
        }

        private static string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

            return JsonSerializer.Serialize(jsonElement, options);
        }



        public string ToJSON()
        {

            return JsonSerializer.Serialize(this);
        }
    }
}
