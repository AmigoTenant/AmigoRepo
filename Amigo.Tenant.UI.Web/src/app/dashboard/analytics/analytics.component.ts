import { Component, OnInit } from '@angular/core';
import { ChartsModule } from '@progress/kendo-angular-charts';
import { EnvironmentComponent } from '../../shared/common/environment.component';
import { basename } from 'path';
import { DashboardDataService } from '../dasboard-data.service';
import { DashboardBalanceDto } from '../dto/dashboard-balance-dto';
import { DashboardBalanceRequest } from '../dto/dashboard-balance-request';
import { ResponseListDTO } from '../../shared/dto/response-list-dto';
import { MasterDataService } from '../../shared/api/master-data-service';

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
  public modelDashboardBalanceRequest: DashboardBalanceRequest;
  public _listFrecuencies: any[];
  public _listPeriods: any[];

  constructor(
    private dashboardDataService: DashboardDataService,
    private masterDataService: MasterDataService
  ) {
    super();
  }

  ngOnInit() {
    this.initializeForm();
    this.fill();
    
  }

  initializeForm(): void {
    this.modelDashboardBalanceRequest = new DashboardBalanceRequest();
    this.getFrecuencies();
  }

  fill() {
    this.dashboardBalanceRequest = new DashboardBalanceRequest();
    this.dashboardBalanceRequest.frecuency = this.modelDashboardBalanceRequest.frecuency;
    this.dashboardBalanceRequest.periodId = this.modelDashboardBalanceRequest.periodId;
    this.dashboardDataService.getDashboardBalance(this.dashboardBalanceRequest).subscribe(
      response => {
        let datagrid: any = new ResponseListDTO(response);
        this.dashboardBalanceDto = datagrid.data;
        this.series = [{
          name: "Ingresos",
          data: this.dashboardBalanceDto.map(function (x) {
            return x.totalIncomePaidAmount;
          })
        }, {
          name: "Gastos",
          data: this.dashboardBalanceDto.map(function (x) {
            return x.totalExpenseAmount;
          })
        }];

        this.categories = this.dashboardBalanceDto.map(function (x) {
          return x.periodCode;
        });

      }
    );
  }


  changePeriod(data){
    this.getPeriodsByYear(data);
    this.executeDashboardBalance();
  }

  executeDashboardBalance(){
    this.fill();
  }
  getFrecuencies(): void {
    this.masterDataService.getYearsFromPeriods()
      .subscribe(res => {
        let dataResult = new ResponseListDTO(res);
        this._listFrecuencies = [];
        for (let i = 0; i < dataResult.data.length; i++) {
          this._listFrecuencies.push({
            'anio' : dataResult.data[i].anio,
          });
        }
      });
  }

  getPeriodsByYear(year: number): void {
    this.masterDataService.getPeriodsByYear(year)
        .subscribe(res => {
            let dataResult = new ResponseListDTO(res);
            this._listPeriods = dataResult.data;
        });
}

}
