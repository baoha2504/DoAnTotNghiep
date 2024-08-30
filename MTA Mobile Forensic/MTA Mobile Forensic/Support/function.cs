using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.Support
{
    internal class function
    {

        private long ConvertStringToLong(string input)
        {
            if (long.TryParse(input, out long result))
            {
                if (result < 0)
                {
                    throw new ArgumentException("Input string is not a valid positive long.");
                }
                return result;
            }
            else
            {
                throw new ArgumentException("Input string is not a valid long.");
            }
        }

        public string ConvertTimeStamp(string timestampString)
        {
            try
            {
                long timestamp = ConvertStringToLong(timestampString);

                DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;

                dateTime = dateTime.AddHours(7);

                return dateTime.ToString("HH:mm:ss dd/MM/yyyy");
            }
            catch 
            {
                return "Invalid timestamp";
            }
        }

        public string ConvertTimeStamp_Day(string timestampString)
        {
            try
            {
                long timestamp = ConvertStringToLong(timestampString);

                DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;

                return dateTime.ToString("dd");
            }
            catch
            {
                return "Invalid timestamp";
            }
        }


        public string GetValue(string source, string key, string nextKey)
        {
            string searchKey = key + "=";
            int startIndex = source.IndexOf(searchKey) + searchKey.Length;
            int endIndex = source.IndexOf(", " + nextKey, startIndex);
            return source.Substring(startIndex, endIndex - startIndex);
        }

        public string GetLastModified(string filePath)
        {
            if (File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                return fileInfo.LastWriteTime.ToString("HH:mm dd/MM/yyyy");
            }
            else
            {
                throw new FileNotFoundException("File not found: " + filePath);
            }
        }

        public string GetFileSizeInMB(string path)
        {
            if (File.Exists(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                long fileSizeInBytes = fileInfo.Length;
                double fileSizeInMB = (double)fileSizeInBytes / (1024 * 1024);
                return fileSizeInMB.ToString("F2") + " MB";
            }
            else
            {
                return "File does not exist.";
            }
        }

        public double[] ParseCoordinates(string input)
        {
            // Define the regular expression pattern
            string pattern = @"(\d+)\s*deg\s*(\d+)'\s*(\d+\.\d+)\""\s*[NSEW],\s*(\d+)\s*deg\s*(\d+)'\s*(\d+\.\d+)\""\s*[NSEW]";

            // Match the pattern
            Match match = Regex.Match(input, pattern);

            // If a match is found, parse the values
            if (match.Success)
            {
                double[] values = new double[6];
                for (int i = 1; i <= 6; i++)
                {
                    values[i - 1] = double.Parse(match.Groups[i].Value);
                }
                return values;
            }
            else
            {
                throw new FormatException("Input string is not in the correct format.");
            }
        }

        public static double DmsToDecimalDegrees(double degrees, double minutes, double seconds)
        {
            return degrees + (minutes / 60.0) + (seconds / 3600.0);
        }


        public string GetGPSPositionToText(string input)
        {
            try
            {
                string gps = GetGPSPositionFromInfo(input);
                double[] result = new double[6];
                result = ParseCoordinates(gps);

                double gps1 = Math.Round(DmsToDecimalDegrees(result[0], result[1], result[2]), 8);
                string latitude = gps1.ToString();

                double gps2 = Math.Round(DmsToDecimalDegrees(result[3], result[4], result[5]), 8);
                string longitude = gps2.ToString();

                string googleMapsUrl = $"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}";

                return googleMapsUrl;
            }
            catch
            { 
                return "Error"; 
            }
        }

        public string GetGPSPositionNoLinkToText(string input)
        {
            try
            {
                string gps = GetGPSPositionFromInfo(input);
                double[] result = new double[6];
                result = ParseCoordinates(gps);

                double gps1 = Math.Round(DmsToDecimalDegrees(result[0], result[1], result[2]), 8);
                string latitude = gps1.ToString();

                double gps2 = Math.Round(DmsToDecimalDegrees(result[3], result[4], result[5]), 8);
                string longitude = gps2.ToString();

                string googleMapsUrl = $"{longitude}, {latitude}";

                return googleMapsUrl;
            }
            catch
            {
                return "Error";
            }
        }

        public string GetGPSPositionFromInfo(string input)
        {
            string pattern = @"GPS Position\s+:\s+([0-9]+ deg [0-9]+\' [0-9]+(\.[0-9]+)?"" [NS], [0-9]+ deg [0-9]+\' [0-9]+(\.[0-9]+)?"" [EW])";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return "GPS Position not found";
        }

        public void CopyFileToFolder(string filePath, string folderPath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("The source file does not exist.", filePath);
                }

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(folderPath, fileName);

                File.Copy(filePath, destFilePath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        public string FindFile(string directoryPath, string filename)
        {
            try
            {
                // Tìm kiếm file trong thư mục hiện tại
                string[] files = Directory.GetFiles(directoryPath, filename);
                if (files.Length > 0)
                {
                    return files[0]; // Trả về đường dẫn file nếu tìm thấy
                }

                // Tìm kiếm file trong các thư mục con
                string[] directories = Directory.GetDirectories(directoryPath);
                foreach (string dir in directories)
                {
                    string result = FindFile(dir, filename); // Gọi đệ quy với tên file
                    if (result != null)
                    {
                        return result; // Trả về kết quả nếu tìm thấy file trong thư mục con
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Xử lý trường hợp không có quyền truy cập thư mục
                Console.WriteLine($"Không thể truy cập thư mục: {directoryPath}");
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"Lỗi: {ex.Message}");
            }

            // Trả về null nếu không tìm thấy file
            return null;
        }

        public string ConvertToCustomFormat(string dateTimeString)
        {
            try
            {
                DateTime dateTime = DateTime.ParseExact(dateTimeString, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                return dateTime.ToString("HH:mm:ss dd/MM/yyyy");
            }
            catch (FormatException)
            {
                Console.WriteLine("Định dạng thời gian không hợp lệ.");
                return null;
            }
        }
    }
}
