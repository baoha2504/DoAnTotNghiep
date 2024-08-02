using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Data;
using MTA_Mobile_Forensic.Model;


namespace MTA_Mobile_Forensic.Support
{
    internal class api
    {
        public string url = "http://localhost:5000";
        public readonly HttpClient client = new HttpClient();

        public async Task<List<AppInfo>> GetDataApplication(string[] packages)
        {
            var requestUrl = $"{url}/api/GetDataApplication";

            // Create the JSON payload
            var jsonPayload = new
            {
                packages = packages
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(jsonPayload),
                System.Text.Encoding.UTF8,
                "application/json");

            try
            {
                var response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var dataWebs = JsonConvert.DeserializeObject<List<AppInfo>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<string>> TimKiemKhuonMatAnh(string path, string[] listPathArray)
        {
            var requestUrl = $"{url}/api/TimKiemKhuonMatAnh";

            // Create the JSON payload
            var jsonPayload = new
            {
                path = path,
                listpath = listPathArray
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(jsonPayload),
                System.Text.Encoding.UTF8,
                "application/json");

            try
            {
                var response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var dataWebs = JsonConvert.DeserializeObject<List<string>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<string>> TrichXuatKhuonMatAnh(string path)
        {
            var requestUrl = $"{url}/api/TrichXuatKhuonMatAnh";

            // Create the JSON payload
            var jsonPayload = new
            {
                path = path
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(jsonPayload),
                System.Text.Encoding.UTF8,
                "application/json");

            try
            {
                var response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var dataWebs = JsonConvert.DeserializeObject<List<string>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<string>> LayPathNhieuKhuonMatTimKiem(string path1, string path2, string[] listPathArray)
        {
            var requestUrl = $"{url}/api/TimKiemNhieuKhuonMatAnh";

            // Create the JSON payload
            var jsonPayload = new
            {
                path1 = path1,
                path2 = path2,
                listpath = listPathArray
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(jsonPayload),
                System.Text.Encoding.UTF8,
                "application/json");

            try
            {
                var response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var dataWebs = JsonConvert.DeserializeObject<List<string>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<string>> TimKiemGiongNoiAudio(string path, string[] listPathArray)
        {
            var requestUrl = $"{url}/api/TimKiemGiongNoiAudio";

            // Create the JSON payload
            var jsonPayload = new
            {
                path = path,
                listpath = listPathArray
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(jsonPayload),
                System.Text.Encoding.UTF8,
                "application/json");

            try
            {
                var response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var dataWebs = JsonConvert.DeserializeObject<List<string>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        
        }
    }
}
