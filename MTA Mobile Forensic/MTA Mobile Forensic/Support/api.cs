using MTA_Mobile_Forensic.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace MTA_Mobile_Forensic.Support
{
    internal class api
    {
        public string url = "http://localhost:5000";
        public string url_social_engineering = "http://192.168.43.107:5001";
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

        public async Task<List<string>> TimKiemGiongNoiVideo(string path, string[] listPathArray)
        {
            var requestUrl = $"{url}/api/TimKiemGiongNoiVideo";

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

        public async Task<AudioResponse> TrichXuatThongTinAudio(string path)
        {
            var requestUrl = $"{url}/api/TrichXuatThongTinAudio";

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
                var dataWebs = JsonConvert.DeserializeObject<AudioResponse>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<AudioResponse> TrichXuatThongTinAudioTrongVideo(string path)
        {
            var requestUrl = $"{url}/api/TrichXuatThongTinAudioTrongVideo";

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
                var dataWebs = JsonConvert.DeserializeObject<AudioResponse>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<AnalysisDocument> AskQuestionGroqAsync(string extension, string path)
        {
            var requestUrl = $"{url}/api/PhanTichNoiDungTaiLieu";

            // Create the JSON payload
            var jsonPayload = new
            {
                extension = extension,
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
                var dataWebs = JsonConvert.DeserializeObject<AnalysisDocument>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<string> ScanDocument(string destinationPath, string[] listPath)
        {
            var requestUrl = $"{url}/api/ScanDocument";

            // Create the JSON payload
            var jsonPayload = new
            {
                destinationPath = destinationPath,
                listPath = listPath
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
                var dataWebs = JsonConvert.DeserializeObject<string>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<OCR> XuLyOCRAnh(string filePath, string outputPath)
        {
            var requestUrl = $"{url}/api/OCRDocument";

            // Create the JSON payload
            var jsonPayload = new
            {
                filePath = filePath,
                outputPath = outputPath
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
                var dataWebs = JsonConvert.DeserializeObject<OCR>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<string> XoayAnhVaLuu(string imagepath, string targetfolder, string rotationdegrees)
        {
            var requestUrl = $"{url}/api/XoayVaLuuAnh";

            // Create the JSON payload
            var jsonPayload = new
            {
                imagepath = imagepath,
                targetfolder = targetfolder,
                rotationdegrees = rotationdegrees
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
                var dataWebs = JsonConvert.DeserializeObject<string>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<UrlIOS>> LayDanhSachURL_IOS(string path)
        {
            var requestUrl = $"{url}/api/DocDuongDanIOS";

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
                var dataWebs = JsonConvert.DeserializeObject<List<UrlIOS>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<CallHistoryIOS>> LayDanhSachCuocGoi_IOS(string path)
        {
            var requestUrl = $"{url}/api/DocCuocGoiIOS";

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
                var dataWebs = JsonConvert.DeserializeObject<List<CallHistoryIOS>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<ContactIOS>> LayDanhSachDanhBa_IOS(string path)
        {
            var requestUrl = $"{url}/api/DocDanhBaIOS";

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
                var dataWebs = JsonConvert.DeserializeObject<List<ContactIOS>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<CalendarIOS>> LayDanhSachLich_IOS(string path)
        {
            var requestUrl = $"{url}/api/DocLichIOS";

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
                var dataWebs = JsonConvert.DeserializeObject<List<CalendarIOS>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<SmsIOS>> LayDanhSachTinNhan_IOS(string path)
        {
            var requestUrl = $"{url}/api/DocTinNhanIOS";

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
                var dataWebs = JsonConvert.DeserializeObject<List<SmsIOS>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<string> UntrackBackup_IOS(string folder, string uuid)
        {
            var requestUrl = $"{url}/api/UntrackBackupIOS";

            // Create the JSON payload
            var jsonPayload = new
            {
                folder = folder,
                uuid = uuid
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
                var dataWebs = JsonConvert.DeserializeObject<string>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<Account_TrinhSat>> LayDanhSachTaiKhoanTrinhSat()
        {
            var requestUrl = $"{url_social_engineering}/GetAllAccount";

            // Create the JSON payload
            var jsonPayload = new
            {
                account_id = ""
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
                var dataWebs = JsonConvert.DeserializeObject<List<Account_TrinhSat>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<LoginHistory_TrinhSat>> LayDanhSachLichSuDangNhapTrinhSat(int id)
        {
            var requestUrl = $"{url_social_engineering}/GetLoginHistoryByAccountID";

            // Create the JSON payload
            var jsonPayload = new
            {
                account_id = id
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
                var dataWebs = JsonConvert.DeserializeObject<List<LoginHistory_TrinhSat>>(jsonResponse);

                return dataWebs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<List<string>> TimKiemKhuonMatVideo(string path, string[] listPathArray)
        {
            var requestUrl = $"{url}/api/TimKiemKhuonMatVideo";

            // Create the JSON payload
            var jsonPayload = new
            {
                pathImage = path,
                pathVideos = listPathArray
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(jsonPayload),
                System.Text.Encoding.UTF8,
                "application/json");

            // Tạo một CancellationTokenSource với thời gian chờ là 10 phút
            using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(10)))
            {
                try
                {
                    // Gửi yêu cầu HTTP với CancellationToken
                    var response = await client.PostAsync(requestUrl, content, cts.Token);
                    response.EnsureSuccessStatusCode();

                    // Hủy bỏ token khi đã nhận được phản hồi thành công
                    cts.Cancel(); // Hủy ngay khi hoàn thành

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var dataWebs = JsonConvert.DeserializeObject<List<string>>(jsonResponse);

                    return dataWebs;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                    return null;
                }
                catch (TaskCanceledException e)
                {
                    // Xử lý khi thời gian chờ hết hoặc yêu cầu bị hủy
                    Console.WriteLine($"The request was canceled or timed out: {e.Message}");
                    return null;
                }
            }
        }

        public async Task<List<string>> TrichXuatKhuonMatVideo(string path, string outputdirectory)
        {
            var requestUrl = $"{url}/api/TrichXuatKhuonMatVideo";

            // Create the JSON payload
            var jsonPayload = new
            {
                pathVideo = path,
                output_directory = outputdirectory
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(jsonPayload),
                System.Text.Encoding.UTF8,
                "application/json");

            // Tạo một CancellationTokenSource với thời gian chờ là 10 phút
            using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(10)))
            {
                try
                {
                    // Gửi yêu cầu HTTP với CancellationToken
                    var response = await client.PostAsync(requestUrl, content, cts.Token);
                    response.EnsureSuccessStatusCode();

                    // Hủy bỏ token khi đã nhận được phản hồi thành công
                    cts.Cancel(); // Hủy ngay khi hoàn thành

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var dataWebs = JsonConvert.DeserializeObject<List<string>>(jsonResponse);

                    return dataWebs;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                    return null;
                }
                catch (TaskCanceledException e)
                {
                    // Xử lý khi thời gian chờ hết hoặc yêu cầu bị hủy
                    Console.WriteLine($"The request was canceled or timed out: {e.Message}");
                    return null;
                }
            }
        }

    }
}
