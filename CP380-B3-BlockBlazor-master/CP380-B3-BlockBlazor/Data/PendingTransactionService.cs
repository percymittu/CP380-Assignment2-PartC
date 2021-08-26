using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using CP380_B1_BlockList.Models;
using System.Text;
using Newtonsoft.Json;

namespace CP380_B3_BlockBlazor.Data
{
    public class PendingTransactionService
    {
        public List<Payload> payloads { get; set; }
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;

        public PendingTransactionService() { }
        public PendingTransactionService(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("PayloadService");
        }

        public async Task<IEnumerable<Payload>> ListPayloads()
        {
            IEnumerable<Payload> payloadList = null;
            
            var res = await _httpClient.GetAsync("http://localhost:46688/PendingPayloads");
            if (res.IsSuccessStatusCode)
            {
                string responseStream = await res.Content.ReadAsStringAsync();
                var jsonResult = JsonConvert.DeserializeObject(responseStream).ToString();
                payloadList = JsonConvert.DeserializeObject<IEnumerable<Payload>>(jsonResult);
            }
            return payloadList;
        }

        public async Task<HttpResponseMessage> AddPayload(Payload payload)
        {
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri("http://localhost:46688/AddPayload"),
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"),
            };
            HttpResponseMessage res = await _httpClient.SendAsync(httpRequestMessage);
            
            return res;
        }
    }
}
