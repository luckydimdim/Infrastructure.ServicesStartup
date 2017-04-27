using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.Extensions;
using Nancy.IO;
using System.IO;
using Nancy.Responses;
using System.Text.RegularExpressions;

namespace Cmas.Infrastructure.ServicesStartup
{
    public static class LoggingHelper
    {
        private static string HeadersToString(RequestHeaders headers)
        {
            var result = string.Empty;

            if (headers == null || headers.Count() == 0)
                return result;

            foreach (var kvp in headers)
            {
                result += String.Format("\t{0}: {1}\n", kvp.Key, string.Join(",", kvp.Value));
            }

            return result;
        }

        private static string HeadersToString(IDictionary<string, string> headers)
        {
            var result = string.Empty;

            if (headers == null || headers.Count() == 0)
                return result;

            foreach (var kvp in headers)
            {
                result += String.Format("\t{0}: {1}\n", kvp.Key, kvp.Value);
            }

            return result;
        }

        private static string BodyToString(Stream stream)
        {
            var input = (stream as RequestStream).AsString();

            string pattern = "\"password\"[\\s]*:[\\s]*\".*\"";
            string replacement = "\"password\": \"********\"";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);

            string result = rgx.Replace(input, replacement);

            return "\t" + result;
        }

        private static string ContentsToString(Action<Stream> actionStream)
        {

            string result = string.Empty;

            using (MemoryStream memStream = new MemoryStream())
            {
                actionStream(memStream);

                memStream.Position = 0;

                var sr = new StreamReader(memStream);
                result = sr.ReadToEnd();
            }
                 
            return "\t" + result;
        } 

        private static string UrlToString(Request request)
        {
            return string.Format("{0} {1} {2}", request.Method, request.Url,
                    request.ProtocolVersion)
                .Trim();
        }

        public static string RequestToString(Request request)
        {
            if (request == null)
                return string.Empty;

            try
            {
                var url = UrlToString(request);

                var headers = HeadersToString(request.Headers);

                var body = BodyToString(request.Body);

                return string.Format("\nRequest: {0}\nHeaders:\n{1}Body:\n{2}", url, headers, body);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string ResponseToString(Response response)
        {
            if (response == null)
                return string.Empty;

            try
            {
                var statusCode = response.StatusCode.ToString();

                var headers = HeadersToString(response.Headers);

                var body = ContentsToString(response.Contents);

                return string.Format("\nResponse: {0}\nHeaders:\n{1}Body:\n{2}", statusCode, headers, body);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}