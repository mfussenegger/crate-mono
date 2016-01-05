using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;


namespace Crate {

    public static class SqlClient
    {
        public static async Task<SqlResponse> Execute(string sqlUri, SqlRequest request) {
            using (var client = new WebClient()) {
                string data = JsonConvert.SerializeObject(request);
                var resp = await client.UploadStringTaskAsync(sqlUri, data);
                return JsonConvert.DeserializeObject<SqlResponse>(resp);
            }
        }
    }
}
