using System.Reflection;
using Newtonsoft.Json;

namespace Webscraper.Src.Consumers;

public class CryptoConsumer1
{
    public string RawData { get; set; }

    public CryptoConsumer1(string rawData)
    {
        RawData = rawData;
    }

    public ExchangeRate? FormatItem()
    {
        // Attempt to deserialize the JSON RawData into a TodoItem object
        try
        {
            var exchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(RawData);
            return exchangeRate;
        }
        catch (JsonException ex)
        {
            return null;
        }
    }

    public bool WriteToFile()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Data", "Processed", "bitcoinRate.json");


        ExchangeRate? exchangeRate = FormatItem();
        if(exchangeRate == null)
        {
            return false;
        }

        



        // Read the existing JSON from the file (if it exists)
        string existingJson = string.Empty;
        if (File.Exists(filePath))
        {
            existingJson = File.ReadAllText(filePath);
        }

        var existingExchangeRates = JsonConvert.DeserializeObject<ExchangeRate[]>(existingJson) ?? new ExchangeRate[0];

        // Add the new exchange rate to the list
        var updatedExchangeRates = new List<ExchangeRate>(existingExchangeRates)
        {
            exchangeRate
        };

        // Serialize the updated list of ExchangeRate objects back to JSON
        string updatedJson = JsonConvert.SerializeObject(updatedExchangeRates, Formatting.Indented);

        // Write the updated JSON to the file
        
        File.WriteAllText(filePath, updatedJson);

        return true;
        
    }
}


    public class ExchangeRate
    {
        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("asset_id_base")]
        public string AssetIdBase { get; set; }

        [JsonProperty("asset_id_quote")]
        public string AssetIdQuote { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }
    }

