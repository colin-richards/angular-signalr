using System;
using System.Threading;
using System.Threading.Tasks;

namespace angular_signalr.Services
{
   public interface ITimedBackgroundService
   {
      Task StartAsync();
      Task StopAsync();
   }
}