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
    public class BlockService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;

        public BlockService() { }

        public BlockService(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("BlockService");
        }

        public async Task<IEnumerable<Block>> ListBlocks()
        {
            IEnumerable<Block> blockList = null;

            var res = await _httpClient.GetAsync("http://localhost:46688/blocks");
            if (res.IsSuccessStatusCode)
            {
                var responseStream = await res.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject(responseStream).ToString();
                blockList = JsonConvert.DeserializeObject<IEnumerable<Block>>(json);
               
            }
            return blockList;
        }

        public async Task<Block> SubmitNewBlockAsync(Block block)
        {
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri("http://localhost:46688/blocks"),
                Content = new StringContent(JsonConvert.SerializeObject(block), Encoding.UTF8, "application/json"),
            };
            var res = await _httpClient.SendAsync(httpRequestMessage);
            if (res.IsSuccessStatusCode)
            {
                res = await _httpClient.GetAsync("http://localhost:46688/blocks");
                if (res.IsSuccessStatusCode && res.Content != null)
                {
                    string responseStream = await res.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject(responseStream).ToString();
                    block = JsonConvert.DeserializeObject<Block>(json);
                    return block;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
