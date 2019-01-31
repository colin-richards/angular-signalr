import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { GameData } from '../models/gameData';
import { IRealTimeGameStatisticsService } from './real-time-game-stats.interface';

import * as signalR  from "@aspnet/signalr";

@Injectable({
  providedIn: 'root'
})
export class RealTimeGameStatisticsService implements IRealTimeGameStatisticsService {

   private _connection;

   private _gameData: BehaviorSubject<GameData[]>;
   public gameData: Observable<GameData[]>;

   constructor() {
      this._gameData = new BehaviorSubject([]);
      this.gameData = this._gameData.asObservable();
   }
 

   public connect () {

      this._connection = new signalR.HubConnectionBuilder()
            .withUrl("/hub")
            .build();
      
      this._connection.start().catch(err => document.write(err));

      this._connection.on("messageReceived", (message_name, payload) => {
        
         this._gameData.next(payload);             
      });
      
   }

   public sendMessage() {
      this._connection.send("newMessage", "client", "this message is from the client")
                .then(() => console.log("Message was sent to the server"));
   }
}
