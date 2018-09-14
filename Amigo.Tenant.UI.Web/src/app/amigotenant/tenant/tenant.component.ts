import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { TenantSearchComponent } from './tenant-search.component';

import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';

import { TenantClient, TenantSearchRequest, EntityStatusClient } from '../../shared/api/services.client';
import { LocationClient, LocationSearchRequest, LocationDTO, DeleteLocationRequest } from '../../shared/api/services.client';
import { EnvironmentComponent } from '../../shared/common/environment.component';
declare var $: any;

@Component({
  selector: 'at-tenant',
  templateUrl: './tenant.component.html',
  styleUrls: ['./tenant.component.less']
})
export class TenantComponent extends EnvironmentComponent implements OnInit {
  totalRecords: number;
  selectedTenant: any;

  entityStatuses: any;
  tenantTypes: any;
  countries: any;

  public openDialog: boolean = false;

  @ViewChild(TenantSearchComponent) tenantSearchComponent: TenantSearchComponent;

  constructor(private tenantClient: TenantClient) {
      super();
   }

  ngOnInit() {

      this.resetResults();
  }

  onDataRefreshed(totalRecords: number): void {
      this.totalRecords = totalRecords;
  }


  onItemSelectedEdit(parameters: any): void {
       this.selectedTenant = parameters.tenant;
       this.tenantClient.getTenantById(this.selectedTenant.tenantId)
        .subscribe(res => {
            var dataResult: any = res;
            this.selectedTenant = dataResult.data;
            this.countries = parameters.countries;
            this.tenantTypes = parameters.tenantTypes;
            this.entityStatuses = parameters.entityStatuses;

            this.openDialog = true;
        });
  }

  onAddTenant(parameters: any) {

      this.openDialog = true;
      this.selectedTenant = undefined;

      this.countries = parameters.countries;
      this.tenantTypes = parameters.tenantTypes;
      this.entityStatuses = parameters.entityStatuses;
  }

  onClickCloseDialog(refreshGridAfterSaving: boolean) {
      this.openDialog = false;
      console.log('onClickCloseDialog ===>' + this.openDialog);

      if (refreshGridAfterSaving) {
          this.tenantSearchComponent.onReset();
          //this.isReloadGrid = true;
          //setTimeout(() => { this.isReloadGrid = false; }, 100);
      }
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


}
