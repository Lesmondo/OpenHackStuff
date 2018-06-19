using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


public class OMS
{
    public string Workspace { get; set; }
    public string SharedKey { get; set; }
    public string LogName { get; set; }

    public void Log(string json)
    {
        // Create a hash for the API signature
        var datestring = DateTime.UtcNow.ToString("r");
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        string stringToHash = "POST\n" + jsonBytes.Length + "\napplication/json\n" + "x-ms-date:" + datestring + "\n/api/logs";
        string hashedString = BuildSignature(stringToHash, SharedKey);
        string signature = "SharedKey " + Workspace + ":" + hashedString;

        PostData(signature, datestring, json);
    }

    // Build the API signature
    string BuildSignature(string message, string secret)
    {
        var encoding = new System.Text.ASCIIEncoding();
        byte[] keyByte = Convert.FromBase64String(secret);
        byte[] messageBytes = encoding.GetBytes(message);
        using (var hmacsha256 = new HMACSHA256(keyByte))
        {
            byte[] hash = hmacsha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hash);
        }
    }

    // Send a request to the POST API endpoint
    void PostData(string signature, string date, string json)
    {

        string url = "https://" + Workspace + ".ods.opinsights.azure.com/api/logs?api-version=2016-04-01";

        using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Log-Type", LogName);
            client.DefaultRequestHeaders.Add("Authorization", signature);
            client.DefaultRequestHeaders.Add("x-ms-date", date);
            client.DefaultRequestHeaders.Add("time-generated-field", "");

            System.Net.Http.HttpContent httpContent = new StringContent(json, Encoding.UTF8);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = client.PostAsync(new Uri(url), httpContent);

            System.Net.Http.HttpContent responseContent = response.Result.Content;
            string result = responseContent.ReadAsStringAsync().Result;
        }

    }
}