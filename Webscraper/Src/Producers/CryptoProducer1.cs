namespace Webscraper.Src.Producers;

public class CryptoProducer1
{
    public CryptoProducer1()
    {
    }

    public async Task<string?> GetCurrentValueAsync(ACCEPTED_COIN coin)
    {
        HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://rest.coinapi.io"),
        };

        // Create an HttpRequestMessage and set headers
        var request = new HttpRequestMessage(HttpMethod.Get, "/v1/exchangerate/BTC/USD");
        request.Headers.Add("X-CoinAPI-Key", "3FFF6FFC-F6AF-41B9-801C-1AFF4099B9DF");

        // Send the request and get the response
        var response = await sharedClient.SendAsync(request);

        // Check the response status and handle it as needed
        if (response.IsSuccessStatusCode)
        {
            // Process the response content
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        else
        {
            return null;
        }
    }
}

public enum ACCEPTED_COIN
{
    BTC,
    ETC,
    BAR
}
