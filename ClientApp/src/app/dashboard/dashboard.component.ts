import { Component, OnInit, OnDestroy } from '@angular/core';
import { RealTimeGameStatisticsService } from '../services/real-time-game-stats.service';
import { GameData } from '../models/gameData';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  
   private subscription;

   public gameData: GameData[];

   constructor (public realTimeGameStatisticsService: RealTimeGameStatisticsService) {

   }

   ngOnInit() {
  
      const observer = {
         next: (payload: GameData[]) => {
            this.gameData = payload;
            console.log("Recevied data from observer");
         },
         error: err => console.error('Observer got an error: ' + err),
         complete: () => console.log('Observer got a complete notification'),
      };
   
      this.subscription = 
         this.realTimeGameStatisticsService.gameData.subscribe (observer); 

      this.realTimeGameStatisticsService.connect();   

   } 

   ngOnDestroy(): void {
      
      if (this.subscription) {
         this.subscription.dispose();
         this.subscription = undefined;
      }
   }

}
