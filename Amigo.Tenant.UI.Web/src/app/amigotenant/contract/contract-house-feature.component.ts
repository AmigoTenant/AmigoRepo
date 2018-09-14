import { Component, Input, Injectable, OnChanges, SimpleChange, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { Http, Jsonp, URLSearchParams } from '@angular/http';
// import { ConfirmationList, Confirmation } from  '../../model/confirmation.dto';
// import { ListsService } from '../../shared/constants/lists.service';
import { HouseFeatureAndDetailDTO } from '../../shared/api/services.client';
import { EnvironmentComponent } from '../../shared/common/environment.component';

declare var $: any;

@Component({
  selector: 'at-contract-house-feature',
  templateUrl: './contract-house-feature.component.html'
})

export class ContractHouseFeatureComponent extends EnvironmentComponent implements OnInit {
    @Input() _listHouseFeature: HouseFeatureAndDetailDTO[];
    @Output() outputPrices = new EventEmitter<any>();
    @Input() _isDisabled: boolean;
 
constructor(
      ) {
      super();
  }


  ngOnInit() {
    this.initializeForm();
  }

  initializeForm(): void {
  }

  onChange(item): void{
      this.syncronizeChecks(item);
      this.outputPrices.emit(this._listHouseFeature);
  }

  syncronizeChecks(item)
  {
      if (item.isAllHouse) {
          for (let i in this._listHouseFeature) {
              var houseFeature = this._listHouseFeature[i];
              if (!houseFeature.isAllHouse) {
                  houseFeature.marked = item.marked;
              }
          }
      }
      else
      {
          var unCheck = false;
          for (let i in this._listHouseFeature) {
              var houseFeature = this._listHouseFeature[i];
              if (!houseFeature.isAllHouse) {
                  if (!houseFeature.marked) {
                      unCheck = true;
                      break;
                  }
              }
          }

          var completeHouseOrApartment = this._listHouseFeature.filter(r => r.isAllHouse == true);
          if (!completeHouseOrApartment[0].isDisabled)
              completeHouseOrApartment[0].marked = !unCheck;
      }
  }

//Llamar esta funcion al hacer un save
 setTableStatus()
 {
     for (let i in this._listHouseFeature)
     {
         var houseFeature = this._listHouseFeature[i];
         var marked = houseFeature.marked;

        if (!marked) {
            if (houseFeature.couldBeDeleted)
                houseFeature.tableStatus = 3;//Delete
            else
                houseFeature.tableStatus = 0;//Unchanged
        }
        else {
            if (houseFeature.couldBeDeleted)
                houseFeature.tableStatus = 0;//Unchanged
            else
                houseFeature.tableStatus = 1;//Insert
        }
     }
}

}
