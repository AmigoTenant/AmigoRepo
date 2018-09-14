import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder, Validators, FormControl } from "@angular/forms";

import { UtilityHouseServicePeriodDTO } from '../../shared/dtos/UtilityBill/UtilityHouseServicePeriodDTO';
import { UtilityBillServiceClient } from '../../shared/api/utilityBill.services.client';
import { HouseServiceClient } from '../../shared/api/amigotenant.service';
import { HouseServiceDTO } from '../../shared/dtos/House/HouseServiceDTO';
import { ServiceHouseDTO } from '../../shared/dtos/House/ServiceHouseDTO';

@Component({
  selector: 'at-utilitybill-maintenance',
  templateUrl: './utilitybill-maintenance.component.html'
})
export class UtilityBillMaintenanceComponent implements OnInit {
   private sub: Subscription;
   private houseId: number;

   public houseServices: HouseServiceDTO[];
   public selectedHouseService: HouseServiceDTO;

   public servicePeriods: UtilityHouseServicePeriodDTO[];
   public edittingServicePeriods:  UtilityHouseServicePeriodDTO[];
   public servicePeriod: UtilityHouseServicePeriodDTO;
   public displayDialog: boolean;
   flgEdition: any;

   public months = [ { value: 1, label: "January" }, { value: 2, label: "February" }, { value: 3, label: "March" }, { value: 4, label: "April" }, { value: 5, label: "May" }, { value: 6, label: "June" }, { value: 7, label: "July" }, { value: 8, label: "August" }, { value: 9, label: "September" }, { value: 10, label: "October" }, { value: 11, label: "November" }, { value: 12, label: "December" }]
   public days = [ { day: "01" }, { day: "02" }, { day: "03" }, { day: "04" }, { day: "05" }, { day: "06" }, { day: "07" }, { day: "08" }, { day: "09" }, { day: "10" }, { day: "11" }, { day: "12" }, { day: "13" }, { day: "14" }, { day: "15" }, { day: "16" }, { day: "17" }, { day: "18" }, { day: "19" },  { day: "20" }, { day: "21" }, { day: "22" }, { day: "23" }, { day: "24" }, { day: "25" }, { day: "26" }, { day: "27" }, { day: "28" }, { day: "29" }, { day: "30" }, { day: "31" } ]
   
  constructor(private route: ActivatedRoute,
      private houseServiceDataService: HouseServiceClient, 
      private utilityBillService: UtilityBillServiceClient) {
         this.servicePeriods = new Array<UtilityHouseServicePeriodDTO>();
       }

   ngOnInit() {
      this.selectedHouseService = new HouseServiceDTO();

      this.sub = this.route.params.subscribe(params => {
         let houseId = params['houseId'];

         this.houseServiceDataService.getHouseServicesByHouse(houseId, 2018)
         .subscribe(response => {
            var dataResult: any = response;
            this.houseServices = dataResult.data;

            this.utilityBillService.getHouseServicePeriod(houseId, 2018)
            .subscribe(respuesta => {
               var result: any = respuesta;
               this.servicePeriods = result.data;
               let firstServiceId = this.houseServices[0].houseServiceId;
               this.edittingServicePeriods = this.servicePeriods.filter((value, index) => { return value.houseServiceId == firstServiceId });
            });
         });
      });
   }

   public onChangeService(selectedValueId): void {
      this.edittingServicePeriods = this.servicePeriods.filter((value, index) => { return value.houseServiceId == this.selectedHouseService.houseServiceId });
   }

   public showDialog() {
      this.displayDialog = true;
   }

   public save() {

   }

   public delete() {

   }
       
   public getMonthById(id: number): string {
    var m = this.months.filter((value, index) => { return value.value == id; });
    if(m.length > 0)
        return m[0].label;
    else
        return null;
}
}
