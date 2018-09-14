//import { GridActivityLogComponent } from './../../shipment-tracking/activity-log/grid-activity-log/grid-activity-log.component';
import { EntityStatusDTO, HouseDTO } from './../../shared/api/services.client';
import { Component, Input, Injectable, OnChanges, SimpleChange, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { ConfirmationList, Confirmation } from  '../../model/confirmation.dto';
import { ListsService } from '../../shared/constants/lists.service';
import { ContractMaintenanceComponent } from './contract-maintenance.component';

import { ContractClient, ContractSearchRequest, ContractChangeStatusRequest, ContractDeleteRequest, EntityStatusClient, HouseClient, FeatureClient } from '../../shared/api/services.client';

//SEARCH CRITERIA:End
import { AuthCheckDirective } from '../../shared/security/auth-check.directive';
import { EnvironmentComponent } from '../../shared/common/environment.component';

import { Router, ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs'
import { PeriodDTO } from '../../shared/index';
import { MasterDataService } from '../../shared/api/master-data-service';

declare var $: any;

export class modelDate {
    year: number;
    month: number;
    day: number;
}


@Component({
  selector: 'at-contract',
  templateUrl: './contract.component.html'
})

export class ContractComponent extends EnvironmentComponent implements OnInit {
  contractSearchDTOs: GridDataResult;
  model: ContractSearchRequest;
  public modelFrom: any;
  public modelTo: any;
  contractModel: any;
  _currentHouse: any;
  _currentPeriod: any;

  //GRID SELECT
  isColumnHeaderSelected: boolean = true;
  isValidToApprove: boolean = false;

  //DROPDOWNS
  _listEntityStatus: any[];
  _listProperties: any[];
  _listUnpaidPeriods: Confirmation[] = [];
  _listConfirmation: Confirmation[] = [];
  _listNextDaysToCollect: any[];

  //TOTALS
  totalResultCount: number = 0;

  //MULTISELECT
  public featureListMultiSelect: IMultiSelectOption[] = [];
  public selectedOptionsFeature: number[] = [];
  public selectedOptionsFeatureBackup: number[] = [];

  public mySettings: IMultiSelectSettings = {
      pullRight: false,
      enableSearch: true,
      checkedStyle: 'checkboxes',
      buttonClasses: 'btn btn-default',
      selectionLimit: 0,
      closeOnSelect: false,
      showCheckAll: true,
      showUncheckAll: true,
      dynamicTitleMaxItems: 1,
      maxHeight: '300px',
  };

  public myTexts: IMultiSelectTexts = {
      checkAll: 'Check all',
      uncheckAll: 'Uncheck all',
      checked: 'checked',
      checkedPlural: 'checked',
      searchPlaceholder: 'Search...',
      defaultTitle: 'Select',
  };

  //PAGINATION
  public buttonCount: number = 20;
  public info: boolean = true;
  public type: 'numeric' | 'input' = 'numeric';
  public pageSizes: any = [20, 50, 100, 200];
  public previousNext: boolean = true;
  public currentPage: number = 0;
  public skip: number = 0;

  public pageChange({ skip, take }: PageChangeEvent): void {
      this.currentPage = skip;
      this.model.pageSize = take;
      let isExport: boolean = false;
      this.getContract();
      //this.deselectColumnAll();
  }


constructor(
    private contractClient: ContractClient,
    private entityStatusClient: EntityStatusClient,
    private houseClient: HouseClient,
    private featureClient: FeatureClient,
    private listConfirmation: ConfirmationList,
    private listsService: ListsService,
    private route: ActivatedRoute,
    private router: Router,
    private masterDataService: MasterDataService
      ) {
      super();
  }

// ngOnDestroy() {
//     this.sub.unsubscribe();
// }

sub: Subscription;
ngOnInit() {

    // this.sub = this.route.params.subscribe(params => {
        
    //     setTimeout(() => {
    //         this.onSelect();
    //     }, 100);

    // });

    this.initializeForm();
    this.resetResults();
}

  public resetResults() {
        $(document).ready(() => {
            this.resizeGrid();
        });

        $(window).bind('load resize scroll', (e) => {
            this.resizeGrid();
        });
  }

  onReset(): void {
        this.initializeForm();
        //this.isColumnHeaderSelected = true;
        //$("#HeaderTemplate")[0].checked = !this.isColumnHeaderSelected;
  }

  initializeForm(): void {
      this.model = new ContractSearchRequest();
      this.setDatesFromTo();
      this.resetGrid();
      this.getEntityStatus();
      this.getProperties();
      this.confirmationFilter();
      this._listNextDaysToCollect =  this.listsService.NextDaysToCollectList();
      this.model.contractCode = undefined;
      this.model.tenantFullName = undefined;
      this.model.nextDaysToCollect = 1;
      this.model.contractStatusId = undefined;
      this.model.houseId = undefined;
      this.model.unpaidPeriods = undefined;
      this.model.nextDaysToCollect = undefined;
      this.model.pageSize = 20;
      this.totalResultCount = 0;
      this.setCurrentPeriod();
      
    }

  public setDatesFromTo() {
      var date = new Date();
      this.modelTo = new modelDate();
      this.modelFrom = new modelDate();
      this.onSelectModelFrom();
      this.onSelectModelTo();
  }

  onSelectModelFrom(): void {
    if (this.modelFrom != null)
        this.model.beginDate = new Date(this.modelFrom.year, this.modelFrom.month - 1, this.modelFrom.day, 0, 0, 0, 0);
    //else
    //    this.model.beginDate = new Date();
  }

  onSelectModelTo(): void {
    if (this.modelTo != null)
        this.model.endDate = new Date(this.modelTo.year, this.modelTo.month - 1, this.modelTo.day, 0, 0, 0, 0);
    //else
    //    this.model.endDate = new Date();
  }

  onSelect(): void {
        this.getContract();
  }

  getContract(): void {
        this.model.pageSize = +this.model.pageSize;
        this.model.page = (this.currentPage + this.model.pageSize) / this.model.pageSize;
        this.contractClient.search(
            this.model.periodId,
            this.model.contractCode,
            this.model.contractStatusId,
            this.model.beginDate,
            this.model.endDate,
            this.model.tenantFullName,
            this.model.houseId,
            this.model.unpaidPeriods,
            this.model.nextDaysToCollect,
            this.selectedOptionsFeature,
            this.model.page,
            this.model.pageSize)
            .subscribe(response => {
                var datagrid: any= response;

                this.contractSearchDTOs = {
                  data: datagrid.value.data.items,
                  total: datagrid.value.data.total
                };
                this.totalResultCount = datagrid.value.data.total;
            });
    }

    getEntityStatus(): void
    {

        this.entityStatusClient.getEntityStatusByEntityCodeAsync("CO")
            .subscribe(response => {
                var dataResult: any = response;
                this._listEntityStatus = dataResult.data;
                let entity= new EntityStatusDTO();
                entity.entityStatusId= null;
                entity.name = 'All';
                this._listEntityStatus.unshift(entity);
              })
    }

    getProperties(): void
    {
        this.houseClient.searchHouseAll()
            .subscribe(response => {
                var dataResult: any = response;
                this._listProperties = dataResult.value.data;
                let entity= new HouseDTO();
                entity.houseId= null;
                entity.name = 'All';
                this._listProperties.unshift(entity);
              })
    }

    //public getFeatures(): void {
    //    this.featureClient.searchFeatureAll("")
    //        .subscribe(response => {
    //            var dataResult: any = response;
    //            dataResult.value.data.forEach(item => {
    //                this.featureListMultiSelect.push({ id: item.featureId, name: item.description });
    //                this.selectedOptionsFeature.push(item.featureId);
    //                this.selectedOptionsFeatureBackup.push(item.featureId);
    //            });
    //        });
    //}

    public confirmationFilter(): void {
        var confirmation = this.listConfirmation.List;
        confirmation.forEach(obj => {
            this._listUnpaidPeriods.push(obj);
        });
    };

    //=========== 
    //GRID
    //===========
    public changeItemHeader() {
        let c = this.contractSearchDTOs.data.length;
        let index = this.model.page * this.model.pageSize - this.model.pageSize;
        for (let item in this.contractSearchDTOs.data) {
            //if (this.contractSearchDTOs.data[item].serviceStatus === null) {
                $("#" + index)[0].checked = this.isColumnHeaderSelected;
                this.contractSearchDTOs.data[item].isSelected = this.isColumnHeaderSelected;
            //}
            index++;
        }
        this.isColumnHeaderSelected = !this.isColumnHeaderSelected;
    }

    public resetGrid(): void {
        let grid: GridDataResult[] = [];
        this.contractSearchDTOs = {
            data: grid,
            total: 0
        };
        this.skip = 0;
    }

    private resizeGrid() {
        var grids = $(".grid-container > .k-grid");
        $.each(grids, (e, grid) => {
            var _combinedPageElementsHeight = 0;
            var _viewportHeight = 0;
            $.each($(grid).parent().siblings().not("kendo-dialog"), (e, v) => {
                _combinedPageElementsHeight += $(v).outerHeight();
            });
            $.each($(grid).find('.k-grid-content').parent().siblings(), (e, v) => {
                _combinedPageElementsHeight += $(v).outerHeight();
            });
            _combinedPageElementsHeight += $(".menu-top").outerHeight();
            _combinedPageElementsHeight += $(".page-header").outerHeight();
            _combinedPageElementsHeight += $(".ro-tab.tabs-top").outerHeight();
            _viewportHeight += $(window).outerHeight() - _combinedPageElementsHeight;
            $(grid).find('.k-grid-content').height(_viewportHeight);
        });
    }

    public changeItem(d) {
        d.isSelected = !d.isSelected;
    }

    // public deselectColumnAll()
    // {
    //     this.isColumnHeaderSelected = true;
    //     $("#HeaderTemplate")[0].checked = !this.isColumnHeaderSelected;
    // }


    //=========== 
    //PRINT
    //===========

    onPrint(data): void {
        // this.itemSelectedEdit.emit({
        //     'device': data,
        //     'platformsRawData': this.platformsRawData,
        //     'platforms': this.platforms,
        //     'modelsRawData': this.modelsRawData,
        //     'brands': this.brands
        // });
    }

    //=========== 
    //EDIT
    //===========

    onEdit(data): void {
        this.router.navigate(['/amigotenant/contract/edit', data.contractId]); // + data.contractId);
    }

    //=========== 
    //CHANGE STATUS
    //===========

    public changeStatusMessage: string = "Are you sure to Formalize this Lease?";
    contractToChangeStatus: any; 

    onChangeStatus(contract) {
        this.contractToChangeStatus = new ContractChangeStatusRequest();
        this.contractToChangeStatus.contractId = contract.contractId;
        this.openedChangeStatusConfimation = true;
    }

    yesChangeStatus() {
        this.contractClient.changeStatus(this.contractToChangeStatus)
            .subscribe(response => {
                this.onSelect();
                this.openedChangeStatusConfimation = false;
            });
    }

    public openedChangeStatusConfimation: boolean = false;

    public closeChangeStatusConfirmation() {
        this.openedChangeStatusConfimation= false;
    }

    //=========== 
    //INSERT
    //===========

    onInsert():void {
        //DataService.currentlocation = new LocationDTO();
        this.router.navigateByUrl('amigotenant/contract/new');
        // this.onAddDevice.emit({
        //     'platformsRawData': this.platformsRawData,
        //     'platforms': this.platforms,
        //     'modelsRawData': this.modelsRawData,
        //     'brands': this.brands
        // });
    }

    //===========
    //DELETE
    //===========
    
    public deleteMessage: string = "Are you sure to delete this Lease?";
    contractToDelete: any; //DeleteDeviceRequest;

    onDelete(entityToDelete) {
        this.contractToDelete = new ContractDeleteRequest();
        this.contractToDelete.contractId = entityToDelete.contractId;
        this.openedDeletionConfimation = true;
    }

    yesDelete() {
        this.contractClient.delete(this.contractToDelete)
             .subscribe(response => {
                 this.onSelect();
                 this.openedDeletionConfimation = false;
             });
    }

    public openedDeletionConfimation: boolean = false;

    public closeDeletionConfirmation() {
        this.openedDeletionConfimation = false;
    }

    //===========
    //EXPORT
    //===========

    //onExport():void
    //{

    //}

    getHouse = (item) => {
        if (item != null && item != undefined && item != "") {
            this.model.houseId = item.houseId;
            this._currentHouse = item;
        }
        else {
            this.model.houseId = undefined;
            this._currentHouse = undefined;
        }
    };

    getPeriod = (item) => {
        if (item != null && item != undefined && item != "") {
            this.model.periodId = item.periodId;
            this._currentPeriod = item;
        }
        else {
            this.model.periodId = undefined;
            this._currentPeriod = undefined;
        }
    };

    onExport(): void {
        this.contractClient.searchReport(
            this.model.periodId,
            this.model.contractCode,
            this.model.contractStatusId,
            this.model.beginDate,
            this.model.endDate,
            this.model.tenantFullName,
            this.model.houseId,
            this.model.unpaidPeriods,
            this.model.nextDaysToCollect,
            this.selectedOptionsFeature,
            this.model.page,
            20000);
    }

    setCurrentPeriod() {
        debugger;
        let period = this.masterDataService.getCurrentPeriod().subscribe(
            res => 
            {
                this._currentPeriod = res.Data;
            },
        error => { },
        () => { }).add(x=>
        {
            this.model.periodId = this.model.periodId==null || this.model.periodId === undefined?
            this._currentPeriod.PeriodId:this.model.periodId;
            this.getContract();
        });

        //this._currentPeriod = new PeriodDTO();
        //this._currentPeriod.periodId = 46;
        //this._currentPeriod.code = '201809';
        
    }

}
