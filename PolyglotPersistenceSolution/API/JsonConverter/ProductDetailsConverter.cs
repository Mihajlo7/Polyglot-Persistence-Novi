using Core.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.JsonConverter
{
    public class ProductDetailsConverter : JsonConverter<ProductDetailsModel>
    {
        public override ProductDetailsModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = document.RootElement;
                Console.WriteLine(root.ToString());

                
                if (root.TryGetProperty("enginePower", out _) && root.TryGetProperty("yearManufactured", out _))
                {
                    return JsonSerializer.Deserialize<CarDetailsModel>(root.GetRawText(), options);
                }
                
                if (root.TryGetProperty("storage", out _) && root.TryGetProperty("screenDiagonal", out _))
                {
                    return JsonSerializer.Deserialize<MobileDetailsModel>(root.GetRawText(), options);
                }
                
                throw new Exception("Bice bolje");
            }
        }

        public override void Write(Utf8JsonWriter writer, ProductDetailsModel value, JsonSerializerOptions options)
        {
            if (value is CarDetailsModel carDetails)
            {
                JsonSerializer.Serialize(writer, carDetails, options);
            }
            else if (value is MobileDetailsModel mobileDetails)
            {
                JsonSerializer.Serialize(writer, mobileDetails, options);
            }
            else
            {
                JsonSerializer.Serialize(writer, value, options);
            }
        }
    }
}
