using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using angular_signalr.Models;

namespace angular_signalr.Services
{
   public interface IHttpGameChannelService {
      Task<GameChannelModel> GetGameChannelData(string channelName, string cursor = "");
   }
}