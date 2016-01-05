using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace Crate {

    public static class SqlClient
    {
        public static async Task<SqlResponse> execute(string sqlUri, SqlRequest request) {
            using (var client = new HttpClient()) {
                var resp = await client.PostAsJsonAsync(sqlUri, request);
                if (!resp.IsSuccessStatusCode) {
                    throw new CrateException(resp.ReasonPhrase + " " + resp.Content.ReadAsStringAsync().Result);
                }
                return await resp.Content.ReadAsAsync<SqlResponse>();
            }
        }
    }
}
