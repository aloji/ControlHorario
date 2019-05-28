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
                var decode = WebUtility.UrlDecode(source);
                var split = decode.Split(',');
                if (split.Length == 2)
                {
                    var base64Data = split[1];
                    result = Convert.FromBase64String(base64Data);
                }
            }
            return result;
        }
    }
}
