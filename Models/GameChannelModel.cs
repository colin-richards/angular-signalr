using System;
using System.Collections.Generic;

namespace angular_signalr.Models
{
   public class GameChannelModel 
   {
      public List<ChannelData> data;
      public Pagination pagination;
   }

   public class ChannelData
   {
      public string id {get; set;}
      public string game_id {get; set;}
      public int viewer_count {get; set;}
   }

   public class Pagination 
   {
      public string Cursor;
   }

}
           