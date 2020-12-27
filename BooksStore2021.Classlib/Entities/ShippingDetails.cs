﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BooksStore2021.Classlib.Entities
{
    public class ShippingDetails
    {
        //[Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }
        public string Email { get; set; } = "guest@email.com";
        //[Required(ErrorMessage = "Please enter the first address line")]
        [Display(Name = "Main address")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        //[Required(ErrorMessage = "Please enter a city name")]
        public string City { get; set; }
        //[Required(ErrorMessage = "Please enter a state or province name")]
        public string State { get; set; }
        public string Zip { get; set; }
        //[Required(ErrorMessage = "Please enter a country name")]
        public string Country { get; set; }
        public bool GiftWrap { get; set; }

        public override string ToString() => ToJSON();
        public string ToJSON() => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
