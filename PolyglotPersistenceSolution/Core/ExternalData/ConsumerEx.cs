using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.ExternalData
{
    public class ConsumerEx
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("birth_day")]
        public DateTime BirthDay { get; set; }
        [JsonPropertyName("telephone")]
        public string Telephone { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("credit_card_type_1")]
        public string CreditCardType1 { get; set; }
        [JsonPropertyName("credit_card_number_1")]
        public string CreditCardNumber1 { get; set; }
        [JsonPropertyName("credit_card_type_2")]
        public string CreditCardType2 { get; set; }
        [JsonPropertyName("credit_card_number_2")]
        public string CreditCardNumber2 { get; set; }
    }
}
