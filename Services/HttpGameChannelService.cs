using System;
using System.Net.Http;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using angular_signalr.Models;
using System.Net;
using Newtonsoft.Json;

namespace angular_signalr.Services
{
   internal class HttpGameChannelService : IHttpGameChannelService, IDisposable
   {
      private readonly IHttpClientFactory _clientFactory;

      public HttpGameChannelService(IHttpClientFactory clientFactory) 
      {
         _clientFactory = clientFactory;
      }
      
      public async Task<GameChannelModel> GetGameChannelData(string channelName, string cursor = "")
      {
         GameChannelModel model;

         var client = _clientFactory.CreateClient(channelName);

         if (cursor.Length > 0) 
         {
            var p = new Dictionary<string, string> { { "after", cursor } };

            string uri = client.BaseAddress.ToString();

            client.BaseAddress = new Uri(QueryHelpers.AddQueryString(uri, p));
         } 

         var request = new HttpRequestMessage(HttpMethod.Get, String.Empty);  

         var response = await client.SendAsync(request);
 
         response.EnsureSuccessStatusCode();

         var jsonString = response.Content.ReadAsStringAsync();
         jsonString.Wait();

         model = JsonConvert.DeserializeObject<GameChannelModel>(jsonString.Result);
        
         return model;
        
      }


      public void Dispose()
      {
          
      }
   }
}
