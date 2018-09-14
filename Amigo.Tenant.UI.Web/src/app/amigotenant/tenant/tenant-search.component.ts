import { Component, Injectable, OnInit, EventEmitter, Output, ViewChild} from '@angular/core';
import { Http, Jsonp, URLSearchParams} from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { Observable, Subscription} from 'rxjs'
import { Router, ActivatedRoute } from '@angular/router';

import { SearchCriteriaTenant } from '../../model/searchCriteria-Tenant';
import { TenantClient, DeleteTenantRequest, EntityStatusClient, GeneralTableClient } from '../../shared/api/services.client';
import { CountryClient, LocationSearchRequest, LocationDTO, DeleteLocationRequest } from '../../shared/api/services.client';
import { Autofocus } from  '../../shared/directives/autofocus.directive';
//import { StatusDTO  } from '../../model/statusDTO';  
import { TypeDTO } from '../../model/typeDTO';  
import { CountryDTO } from '../../model/countryDTO';  
import { ListsService } from '../../shared/constants/lists.service';
import { NativeWindowService } from '../../shared/service/native-window.service';
import { NotificationService } from '../../shared/service/notification.service';

declare var $: any;

@Component({
  selector: 'at-tenant-search',
  templateUrl: './tenant-search.component.html',
  styleUrls: ['./tenant-search.component.less']
})
export class TenantSearchComponent implements OnInit {
  model: SearchCriteriaTenant;
  tenantsDTOs: GridDataResult;
  totalRecords: number;
  nativeWindow: any;

  public activeInactiveStatus: any[] = this._listsService.ActiveInactiveStatus();

  // DropDowns
  //entityStatuses: StatusDTO[];
  countries: CountryDTO[];
  tenantTypes: TypeDTO[];

  //PAGINATION
  public buttonCount: number = 20;
  public info: boolean = true;
  public type: 'numeric' | 'input' = 'numeric';
  public pageSizes: any = [20, 50, 100, 200];
  public previousNext: boolean = true;
  public currentPage: number = 0;

  public pageChange({ skip, take }: PageChangeEvent): void {
      //debugger;
      this.currentPage = skip;
      this.model.pageSize = take;
      let isExport: boolean = false;
      this.getTenants();
  }

  @Output() dataRefreshed: EventEmitter<number> = new EventEmitter<number>();
  @Output() itemSelectedEdit: EventEmitter<any> = new EventEmitter<any>();
  @Output() onAddTenant: EventEmitter<any> = new EventEmitter<any>();

  constructor(private tenantDataService: TenantClient, private countryDataService: CountryClient,
            private router: Router, private route: ActivatedRoute,
            private generalTableService: GeneralTableClient,
            private _listsService: ListsService,
            private winRef: NativeWindowService,
            private notificationService: NotificationService) { 
    this.nativeWindow = winRef.getNativeWindow(); 
  }


  public deleteMessage: string = "Delete tenant?";

  ngOnDestroy() {
        this.sub.unsubscribe();
  }

  sub: Subscription;
  ngOnInit() {
      this.sub = this.route.params.subscribe(params => {

            setTimeout(() => {
                this.onSelect();
            }, 100);

        });
    this.initializeSearchForm();
  }

  initializeSearchForm(): void {
    this.model = new SearchCriteriaTenant();

    this.model.tenantId = undefined;
    this.model.statusId = undefined;
    this.model.countryId = undefined;
    this.model.typeId = undefined;
    this.model.code = undefined;
    this.model.fullName = undefined;
    this.model.phoneNumber = undefined;
    this.model.rowStatus = undefined;
    this.model.page = undefined;
    this.model.pageSize = 20;

    this.tenantsDTOs = {
        data: [],
        total: 0
    };

    this.totalRecords = 0;
    this.dataRefreshed.emit(this.totalRecords);

    //this.onGetStatus();
    this.onGetCountries();
    this.onGetTenantTypes();
    this.resetResults();
  }

  //-----------------------------------------------------------------------------------
  //--------------------------    Status     ---------------------------
  //-----------------------------------------------------------------------------------

  //private onGetStatus(): void {
  //  this.entityStatusClient.getEntityStatusByEntityCodeAsync("CO")
  //    .subscribe(response => {
  //        var dataResult: any = response;
  //        this.entityStatuses = dataResult.value.data;
  //    });
  //}

    //---------------------------------------------------------------------------
    //--------------------------    Countries     ---------------------------
    //---------------------------------------------------------------------------

  private onGetCountries(): void {
    this.countryDataService.getCountriesAll()
        .subscribe(res => {
            var dataResult: any = res;
            this.countries = this.getCountries(dataResult.value.data);


        });
  }

  private getCountries(data: any): any {
      var countries = [];
      var count = data.length;
      for (var i = 0; i < count; i++) {
          if (!this.containsDuplicates(data[i].isoCode, countries)) {
              countries.push({
                  "id": data[i].countryId,
                  "isoCode": data[i].isoCode,
                  "name": data[i].Name,
              });
          }
      }
      return countries;
  };

  private containsDuplicates = function (v, data) {
      var count = data.length;
      for (var i = 0; i < count; i++) {
          if (data[i].isoCode === v) return true;
      }
      return false;
  }

  private onGetTenantTypes() : void {
    this.generalTableService.getGeneralTableByTableNameAsync("TenantType")
        .subscribe(res => {
          var dataResult: any = res;
          this.tenantTypes = new Array<TypeDTO>();
          for (var i = 0; i < dataResult.value.data.length; i++) {
            this.tenantTypes.push({ 
                "typeId": dataResult.value.data[i].generalTableId,
                "name":  dataResult.value.data[i].value
            });
          }
        });
  }
  //--------------------------------------------------------------------
  //--------------------------    Search     ---------------------------
  //--------------------------------------------------------------------

  onSelect(): void {
      this.getTenants();
  }

  getTenants(): void {
    this.model.pageSize = +this.model.pageSize;
    this.model.page = (this.currentPage + this.model.pageSize) / this.model.pageSize;

    this.tenantDataService.search
        (this.model.tenantId,
            this.model.statusId,
            this.model.countryId,
            this.model.typeId,
            this.model.code,
            this.model.fullName,
            this.model.phoneNumber,
            this.model.rowStatus,
            this.model.page,
            this.model.pageSize
        )
        .subscribe(res => {
            // debugger;
            var dataResult: any = res;
            this.tenantsDTOs = {
                data: dataResult.data.items,
                total: dataResult.data.total
            };
            this.totalRecords = dataResult.data.total;
            this.dataRefreshed.emit(this.totalRecords);
        });
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

  public resetResults() {
        $(document).ready(() => {
            this.resizeGrid();
        });

        $(window).bind('load resize scroll', (e) => {
            this.resizeGrid();
        });
    }

  onReset(): void {      
      this.model = new SearchCriteriaTenant();
      // this.typeaheadUserCleanValue();
      this.getTenants();
      //this.currentUser = null;
  }

    //-------------------------------------------------------------------------
    //--------------------------    Maintenance     ---------------------------
    //-------------------------------------------------------------------------

  onEdit(data): void {
      //debugger;
      this.itemSelectedEdit.emit({
          'tenant': data,
          'entityStatuses': this.activeInactiveStatus,
          'tenantTypes': this.tenantTypes,
          'countries': this.countries
      });

  }


  public addItem() {
      this.onAddTenant.emit({
          'entityStatuses': this.activeInactiveStatus,
          'tenantTypes': this.tenantTypes,
          'countries': this.countries
      });
  }

  public sendNotification(dataItem): void {
    // var newWindow = this.nativeWindow.open("https://api.whatsapp.com/send" + 
    //     "?phone=" + JSON.stringify(dataItem.phoneN01).replace(/\W/g, '') + 
    //     "&text=" + "Mr " + dataItem.contactName);
    this.notificationService
        .sendNotification(dataItem.phoneN01, encodeURIComponent("http://rpp.pe/"))
        .subscribe(res => {
            //debugger;
            var dataResult: any = res;
        });

  }
    //-----------------------------------------------------------------------------------
    //--------------------------    Deletion Confirmation     ---------------------------
    //-----------------------------------------------------------------------------------

    tenantToDelete: DeleteTenantRequest;

    deleteItem(tenantToDelete) {

        this.tenantToDelete = new DeleteTenantRequest();
        this.tenantToDelete.tenantId = tenantToDelete.tenantId;
        // if (tenantToDelete.assignedAmigoTenantTUserId != null && tenantToDelete.assignedAmigoTenantTUserId > 0)
        //     this.deleteMessage = "There is an assigned driver, delete Tenant?";
        // else 
        this.deleteMessage = "Delete Tenant?";
        this.openedDeletionConfimation = true;
    }

    yesDeleteItem() {
        this.tenantDataService.delete(this.tenantToDelete)
            .subscribe(response => {
                this.onReset()
                this.openedDeletionConfimation = false;
            });
    }

    public openedDeletionConfimation: boolean = false;

    public closeDeletionConfirmation() {
        this.openedDeletionConfimation = false;
    }



}
