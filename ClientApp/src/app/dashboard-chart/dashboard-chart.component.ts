import { Component, OnInit, OnChanges, Input } from '@angular/core';
import { ChangeDetectionStrategy } from '@angular/core';
import { GameData } from '../models/gameData';
import * as Highcharts from 'highcharts';


@Component({
  selector: 'dashboard-chart',
  templateUrl: './dashboard-chart.component.html',
  styleUrls: ['./dashboard-chart.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardChartComponent implements OnInit, OnChanges {
 
   private Highcharts = Highcharts;
   private chartOptions; 
   private updateFlag: Boolean;

   private readonly MAX_DATA_POINTS = 30;

   private colors = { 
      'scarlet':'#ff1a1a',
      'dark_blue': '#000099',
      'lawn_green': '#009900',
      'deep-yellow': '#ffaa00',
      'crimson': '#ff1a8c',
      'light_blue': '#33ccff',
      'medium_green': '#33cc33',
      'deep_red': '#cc3300',
      'purple': '#9900cc',
      'sienna': '#bf8040'
   };

   constructor () {}
   
   @Input()
   chartTitle: string;

   @Input()
   gameName: string;

   @Input()
   gameId: string;

   @Input()
   data: GameData[];

   ngOnChanges() {
      // fires when parent pushes new data to this component
      console.log("Changes to input data dectected");
      this.updateChart(this.data);
   }

   ngOnInit() {

      Highcharts.setOptions({
         global: {
           useUTC: false
         }
       });

      this.chartOptions = {
         chart: {
            type: 'line'
         },
         title: {
            text: this.chartTitle
         },
         xAxis: {
            title: {
                  text: "Time"
            },
            type: 'datetime',
            
            labels:{
               formatter:function(){
                     return Highcharts.dateFormat('%H:%M', this.value);
               }
            }
         },
         yAxis: {
               title: {
                  text: "Total Viewers"
               }
         }, 
         plotOptions: {
            series: {
               marker: {
                  enabled: true
               }
            }
         },
         series: []
      }; 
      
      this.addSeries();

   }

   private updateChart(gameData: GameData[]) {

      console.log(gameData);
      // this chart component can plot one data series or several
      // data series depending on the number of game ids that are 
      // passed to the gameId input variable

      if (Array.isArray(this.gameId)) {
         for (let i = 0; i < this.gameId.length; i++) {
            let data = this.getGameData(this.gameId[i], gameData);
            if (data != null) {
               this.addToSeries(this.gameName[i], data, i);
               this.trimData(i, this.MAX_DATA_POINTS);
            }
         }
      } else {
         let data = this.getGameData(this.gameId, gameData);
         if (data != null) {
            this.addToSeries(this.gameName, data, 0);
            this.trimData(0, this.MAX_DATA_POINTS);
         }
      } 

      this.updateFlag = true;
   } 

   private addToSeries(seriesName: string, gameData: GameData, seriesIndex: number) {
      
      this.chartOptions.series[seriesIndex].name = seriesName;

      this.chartOptions.series[seriesIndex].data.push(
         [new Date().getTime(), gameData.total_viewers]
      );
   }

   private trimData(seriesIndex: number, maxLength: number) {

      // if data points exceed maximum drop first element in the data array 
      if (this.chartOptions.series[seriesIndex].data.length > maxLength) {
         this.chartOptions.series[seriesIndex].data.shift();
      }
   }

   private getGameData(gameId: string, gameData: GameData[]) : GameData {
      // the gameData array contains the game statistics for all the games
      // that are being monitored. Select game statistics by game id.

      for (let d of gameData) {
         if (d.game_id == gameId) {
            return d;
         }
      } 
      return null;
   }

   private addSeries() {

      // add one or more data series to the chart depending on the number
      // of game ids that are assigned to the gameId variable

      if (Array.isArray(this.gameId)) {
         for (let i = 0; i < this.gameId.length; i++) {
            let series = {
               name: this.gameName[i],
               color: this.getRandomColor(),
               data: []
            };
            this.chartOptions.series.push(series);
          }
         
      } else {
         let series = {
            name: this.gameName,
            color: this.getRandomColor(),
            data: []
         };
         this.chartOptions.series.push(series);
      } 
   }

   private getRandomColor(): string {
      let num_colors = Object.values(this.colors).length;
      let index = Math.floor(Math.random() * num_colors) - 1; 
      return Object.values(this.colors)[index];
   }

}
