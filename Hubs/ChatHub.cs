using System;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace angular_signalr.Hubs
{
   public class ChatHub : Hub
   {
      public ChatHub()
      {
         Debug.WriteLine($"ChatHub instance created: {this.GetHashCode().ToString()}");
      }

      public async Task NewMessage(string username, string message)
      {
         await Clients.All.SendAsync("messageReceived", username, message);
      }

      public override async Task OnConnectedAsync()
      {
         Debug.WriteLine($"Client connected to hub: {this.GetHashCode().ToString()}");
         await base.OnConnectedAsync();
      }

      public override async Task OnDisconnectedAsync(Exception exception)
      {
         Debug.WriteLine($"Client disconnected from hub: {this.GetHashCode().ToString()}");
         await base.OnDisconnectedAsync(exception);
      }
   }
}