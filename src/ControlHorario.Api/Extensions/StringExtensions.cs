using System;
using System.Net;

namespace ControlHorario.Api.Extensions
{
    public static class StringExtensions
    {
        public static byte[] FromDataUrl(this string source)
        {
            var result = default(byte[]);
            if (!string.IsNullOrWhiteSpace(source))
            {
                var split = source.Split(',');
                if (split.Length == 2)
                {
                    var base64Data = split[1];
                    try
                    {
                        result = Convert.FromBase64String(base64Data);
                    }
                    catch { }
                }
            }
            return result;
        }

        public static bool IsUrl(this string source)
        {
            var result = true;
            try
            {
                new Uri(source);
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}
