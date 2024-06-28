using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

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

                return dateTime.ToString("HH:mm:ss dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it or return an error message)
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
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it or return an error message)
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

        public double ParseCoordinate(string gpsString, char positiveDirection, char negativeDirection)
        {
            int degreeStart = gpsString.IndexOf("deg") - 3; // Bắt đầu lấy số độ
            int minuteStart = gpsString.IndexOf("'") - 2;   // Bắt đầu lấy số phút
            int secondStart = gpsString.IndexOf("\"") - 5;  // Bắt đầu lấy số giây

            // Lấy phần số
            string degreeStr = gpsString.Substring(degreeStart, 3).Trim();
            string minuteStr = gpsString.Substring(minuteStart, 2).Trim();
            string secondStr = gpsString.Substring(secondStart, 5).Trim();

            // Chuyển đổi sang số
            double degree = double.Parse(degreeStr);
            double minute = double.Parse(minuteStr);
            double second = double.Parse(secondStr);

            // Tính toán giá trị toàn cầu
            double coordinate = degree + (minute / 60) + (second / 3600);

            // Xác định hướng
            int directionIndex = gpsString.IndexOf(positiveDirection);
            if (directionIndex == -1)
            {
                directionIndex = gpsString.IndexOf(negativeDirection);
                coordinate = -coordinate; // Đổi dấu nếu là hướng phía Nam hoặc phía Tây
            }

            return coordinate;
        }

        public string xuly(string gpsString)
        {

            // Xử lý vĩ độ (Latitude)
            double latitude = ParseCoordinate(gpsString, 'N', 'S');

            // Xử lý kinh độ (Longitude)
            double longitude = ParseCoordinate(gpsString, 'E', 'W');

            Console.WriteLine($"Latitude: {latitude}");
            Console.WriteLine($"Longitude: {longitude}");

            string googleMapsUrl = $"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}";
            return googleMapsUrl;
        }


        public string GetGPSPositionToLink(string input)
        {
            string pattern = @"GPS Position\s+:\s+([0-9]+ deg [0-9]+\' [0-9]+(\.[0-9]+)?"" [NS], [0-9]+ deg [0-9]+\' [0-9]+(\.[0-9]+)?"" [EW])";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                return xuly(match.Groups[1].Value);
            }

            return "GPS Position not found";
        }
    }
}
