import { Component, OnInit } from '@angular/core';
import { ChartsModule } from '@progress/kendo-angular-charts';
import { EnvironmentComponent } from '../../shared/common/environment.component';
import { basename } from 'path';
import { DashboardDataService } from '../dasboard-data.service';
import { dataDetailClass } from '../../amigotenant/payment/payment-maintenance.component';
import { DashboardBalanceDto } from '../dto/dashboard-balance-dto';
import { DashboardBalanceRequest } from '../dto/dashboard-balance-request';
import { ResponseListDTO } from '../../shared/dto/response-list-dto';

console.log('analytics');

@Component({
  selector: 'st-analytics',
  templateUrl: './analytics.component.html',
  styleUrls: ['./analytics.component.css']
})

export class AnalyticsComponent extends EnvironmentComponent implements OnInit {
  public series: any[];
  public categories: number[];
  public dashboardBalanceDto: DashboardBalanceDto[];
  public dashboardBalanceRequest: DashboardBalanceRequest;

  constructor(
    private dashboardDataService : DashboardDataService
  ) {
    super();
  }

  ngOnInit() {
    debugger;
    this.fill();
  }

  fill(){
    this.dashboardBalanceRequest = new DashboardBalanceRequest();
    this.dashboardBalanceRequest.frecuency = 79;
    this.dashboardBalanceRequest.periodId = 2018;

    debugger;
    this.dashboardDataService.getDashboardBalance(this.dashboardBalanceRequest).subscribe(
      response => {
        let datagrid: any = new ResponseListDTO(response);
        this.dashboardBalanceDto = datagrid.data;
        this.series = [{
          name: "Ingresos",
          data: this.dashboardBalanceDto.map(function(x) {
            return x.totalIncomeAmount;
          })
        }, {
          name: "Gastos",
          data: this.dashboardBalanceDto.map(function(x) {
            return x.totalExpenseAmount;
          })
        }];
        
        this.categories = this.dashboardBalanceDto.map(function(x) {
          return x.periodCode;
        });

      }
    ).add(
      r =>{
        this.series = [{
          name: "Ingresos",
          data: this.dashboardBalanceDto.map(function(x) {
            return x.totalIncomeAmount;
          })
        }, {
          name: "Gastos",
          data: this.dashboardBalanceDto.map(function(x) {
            return x.totalExpenseAmount;
          })
        }];
        
        this.categories = this.dashboardBalanceDto.map(function(x) {
          return x.periodCode;
        });

      }
    );

    // this.series = [{
    //   name: "Ingresos",
    //   data: [103907, 70000, 712848, 999284, 911263]
    // }, {
    //   name: "Gastos",
    //   data: [404743, 70295, 99175, 69376, 80153]
    // }];

    // this.categories = [201901, 201902, 201903, 201904, 201905];

  }
  
  

}
