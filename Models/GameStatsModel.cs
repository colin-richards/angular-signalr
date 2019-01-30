using System;
using System.Collections.Generic;

namespace angular_signalr.Models
{
   
   public class GameStatsModel
   {
      public string game_key_name {get; set;}
      public string game_display_name {get; set;}
      public string game_id {get; set;}
      public int total_viewers {get; set;}
      public string timestamp {get; set; }
   } 

}
           