using System;
using System.Net.Http;
//using Newtonsoft.Json;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelloWorld
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            List<Task<ZipResponse>> t = new List<Task<ZipResponse>>();
            var zips = GetZips();
            Console.WriteLine("Starting...");
            foreach (var z in zips)
            {
                t.Add(GetCityState(z));
            }

            ZipResponse[] responses = await Task.WhenAll(t);

            foreach (var r in responses)
            {
                if (string.IsNullOrEmpty(r.error))
                { Console.WriteLine("{0}: {1}, {2}", r.zip, r.city, r.state); }
                else
                { Console.WriteLine("{0}: {1}", r.zip, r.error); }
            }
        }

        static void DoWork()
        {
            while (1 == 1)
            {
                Console.Write("Zipcode: ");
                var zip = Console.ReadLine();
                string baseUrl = "http://ziptasticapi.com/" + zip;
                var res = client.GetStringAsync(baseUrl).Result;
                //var response = JsonConvert.DeserializeObject<ZipResponse>(res);
                var response = JsonSerializer.Deserialize<ZipResponse>(res);

                if (string.IsNullOrEmpty(response.error))
                { Console.WriteLine(response.city + ", " + response.state); }
                else
                { Console.WriteLine(response.error); }
            }
        }
        static async Task<ZipResponse> GetCityState(string zip)
        {
            string baseUrl = "http://ziptasticapi.com/" + zip;
            Console.WriteLine("Zip: {0} Starting...", zip );
            var res = await client.GetStringAsync(baseUrl);
            Console.WriteLine("Zip: {0} Done", zip );
            //var response = JsonConvert.DeserializeObject<ZipResponse>(res);
            var response = JsonSerializer.Deserialize<ZipResponse>(res);
            response.zip = zip;
            return response;
        }

        static List<string> GetZips()
        {
            List<string> z = new List<string>();
            z.Add("92688");
            z.Add("92694");
            z.Add("92618");
            z.Add("92670");
            z.Add("92626");
            z.Add("92630");
            z.Add("90210");
            z.Add("92711");
            z.Add("92700");
            z.Add("92620");

            return z;
        }
    }

}
