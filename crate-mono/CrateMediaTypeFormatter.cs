using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Crate.Client
{
    internal class CrateMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var task = Task.Factory.StartNew(() =>
            {
                string jsonData = string.Empty;
                if (value is SqlRequest)
                {
                    jsonData = ((SqlRequest)value).ToJSON();
                }
                else
                {
                    jsonData = JsonConvert.SerializeObject(value);
                }
                var buffer = this.SelectCharacterEncoding(content.Headers).GetBytes(jsonData);
                writeStream.Write(buffer, 0, buffer.Length);
                writeStream.Flush();
            });

            return task;
        }
    }
}
