using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using angular_signalr.Hubs;
using angular_signalr.Services;
using angular_signalr.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace angular_signalr.Services
{
   internal class TimedBackgroundService : ITimedBackgroundService, IDisposable
   {
      private readonly int MAX_REQUESTS_PER_MIN = 30;

      private List<GameStatsModel> _game_settings;
      private readonly ILogger _logger;
      private Timer _timer;
      private readonly IHubContext<ChatHub> _hubContext;
      IHttpGameChannelService _httpGameChannel;

      public TimedBackgroundService(
         IOptions<List<GameStatsModel>> game_settings,
         ILogger<TimedBackgroundService> logger, 
         IHttpGameChannelService httpGameChannel,
         IHubContext<ChatHub> hubContext)
      {
         _game_settings = game_settings.Value;
         _logger = logger;
         _httpGameChannel = httpGameChannel;
         _hubContext = hubContext;
      }

      public Task StartAsync()
      {
         _logger.LogInformation("Timed Background Service is starting.");

         _timer = new Timer(DoWork, null, TimeSpan.Zero,
                  TimeSpan.FromSeconds(60)); // 1 min

         return Task.CompletedTask;
      }

      private async void DoWork(object state)
      {
         _logger.LogInformation("Timed Background Service is working.");
         try
         {
            List<GameStatsModel> gameStats = await GetGameChannelData();

            await _hubContext.Clients.All.SendAsync("messageReceived", "game_viewers", gameStats);
         } 
         catch(HttpRequestException ex)
         { 
            _logger.LogError(ex.Message);
         } 
         catch(Exception ex)
         {
            // unrecoverable exception
             _logger.LogError($"Fatal Error - {ex.Message}");
             
            await StopAsync();
         }
      }

      private async Task<List<GameStatsModel>> GetGameChannelData() 
      {
         // The game statistics models have some preconfigured settings that we copy
         List<GameStatsModel> gameStats = CopyGameSettings(_game_settings);
         GameChannelModel gameChannelData;
         int number_requests = 0;
         string cursor = String.Empty;
         
         do {            
            gameChannelData = await _httpGameChannel.GetGameChannelData("All_Games", cursor);
            gameStats = GetGameStatistics(gameChannelData, gameStats);
            cursor = gameChannelData.pagination.Cursor; 
            number_requests++;
         } while (cursor != null && number_requests < MAX_REQUESTS_PER_MIN);
        
         return gameStats;
      } 

      private List<GameStatsModel> GetGameStatistics(
         GameChannelModel gameChannels, List<GameStatsModel> gameStats)
      {
         foreach( GameStatsModel game in gameStats )
         {
            game.total_viewers += gameChannels.data
                                 .Where(channel => channel.game_id == game.game_id)
                                 .Sum(channel => channel.viewer_count);

            game.timestamp = DateTime.UtcNow.ToString();
         }
         return gameStats;
      }

      private List<GameStatsModel> CopyGameSettings(List<GameStatsModel> gameSettings)
      {
            List<GameStatsModel> newList = new List<GameStatsModel>();
            foreach(GameStatsModel model in gameSettings)
            {
               GameStatsModel newModel = new GameStatsModel() {

                   game_id = model.game_id,
                   game_key_name = model.game_key_name,
                   game_display_name = model.game_display_name
               };
               newList.Add(newModel);
            }
            return newList;

      }

      public Task StopAsync()
      {
         _logger.LogInformation("Timed Background Service is stopping.");

         _timer?.Change(Timeout.Infinite, 0);

         return Task.CompletedTask;
      }

      public void Dispose()
      {
         _timer?.Dispose();
      }
   }
}
