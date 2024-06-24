using System;

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


        public string GetValue(string source, string key, string nextKey)
        {
            string searchKey = key + "=";
            int startIndex = source.IndexOf(searchKey) + searchKey.Length;
            int endIndex = source.IndexOf(", " + nextKey, startIndex);
            return source.Substring(startIndex, endIndex - startIndex);
        }
    }
}
