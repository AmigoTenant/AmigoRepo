import { Component, Injectable, Input,Output, OnChanges, SimpleChange,EventEmitter } from '@angular/core';
import {Http, Jsonp, URLSearchParams} from '@angular/http';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { ModuleClient,DeleteModuleRequest } from '../../shared/api/services.client';
import { SearchCriteriaModule } from  '../../model/searchCriteria-Module'; 
import {SortDescriptor,orderBy } from '@progress/kendo-data-query';
import { AuthCheckDirective } from  '../../shared/security/auth-check.directive';

declare var $: any;

@Component({
    selector: 'st-module-grid',
    templateUrl: './module-grid.component.html'
})

export class ModuleGridComponent {

    constructor(private moduleDataService: ModuleClient) {}
    public flgParents: boolean = false;
    public gridData: GridDataResult;
    //public skip: number = 0;
    countItems: number=0;

    public buttonCount: number = 20;
    public info: boolean = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [20, 50, 100, 200];
    public previousNext: boolean = true;
    public currentPage: number = 0;
    public skip: number = 0;
    public  selectedCode: string;
    flgEdition :string;
    public sort: SortDescriptor[] = [];

    confirmDeletionVisible: boolean = false;
    confirmDeletionResponse: boolean = false;

  
    public onDeleteModule = new DeleteModuleRequest();
    @Input() imodulo : string;
    @Input() modelParameters : SearchCriteriaModule;
    @Output() itemSelected = new EventEmitter<any>();
    @Output() counterGridEvent = new EventEmitter<any>();

    // Verify the Changes Events of @Input
    ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
        
        if (this.imodulo != null) {
            this.onSearch();
        }
    }

    ngOnInit() {
        $(document).ready(() => {
            this.resizeGrid();
        });

        $(window).bind('load resize scroll', (e) => {
            this.resizeGrid();
        });

        this.modelParameters.pageSize = 20;
        this.currentPage = 0;
    }

     public resizeGrid() {
        var grids = $(".grid-container > .k-grid");
        $.each(grids, (e, grid) => {
            var _combinedPageElementsHeight = 0;
            var _viewportHeight = 0;
            $.each($(grid).parent().parent().siblings().not("kendo-dialog"), (e, v) => {
                _combinedPageElementsHeight += $(v).outerHeight();
            });

            $.each($(grid).find('.k-grid-content').parent().siblings(), (e, v) => {
                _combinedPageElementsHeight += $(v).outerHeight();
            });

            _combinedPageElementsHeight += $(".menu-top").outerHeight();
            _combinedPageElementsHeight += $(".page-header").outerHeight();
            _combinedPageElementsHeight += $(".ro-tab.tabs-top").outerHeight();
            _viewportHeight += $(window).outerHeight() - _combinedPageElementsHeight;
            $(grid).find('.k-grid-content').height(_viewportHeight );
        });
    }
     public sortChange(sort: SortDescriptor[]): void {
         this.sort = sort;
         this.onSearch();
     }

     public onSearch(): void {
                this.modelParameters.pageSize = +this.modelParameters.pageSize;
this.modelParameters.page = (this.currentPage + this.modelParameters.pageSize) / this.modelParameters.pageSize;

            this.moduleDataService
                .search(this.modelParameters.code,
                this.modelParameters.name,
                this.modelParameters.parentName,
                    false,
                    this.modelParameters.page,
                    this.modelParameters.pageSize)
                .subscribe(res => {
                        var dataResult: any = res;
                        this.countItems = dataResult.data.items.length;
                        this.gridData = {
                            data: orderBy(dataResult.data.items, this.sort),
                            total: dataResult.data.total
                        }
                        this.counterGridEvent.emit(dataResult.data.total);
                    });

    }

    // protected selectionChange(event: any): void {                
    //     let dataItem = this.gridData.data[event.index - this.skip];        
    //     this.itemSelected.emit(dataItem);
    // }

 onReloadGrid() {
        this.modelParameters.pageSize = 20;
        this.currentPage = 0;
        this.onSearch();
    }
 public pageChange({ skip, take }: PageChangeEvent): void {
        this.currentPage = skip;
        this.modelParameters.pageSize = take;
        this.onSearch();
    }

    closeConfirmation(status):void {
      this.confirmDeletionVisible = false;
    }

    onEdit(dataItem){
         this.itemSelected.emit(dataItem);
    }

    onDelete(code){
        this.confirmDeletionVisible = true;
        this.onDeleteModule.code = code;
    }

    onConfirmation=(status)=>{
        
    this.confirmDeletionResponse = (status === "YES");

    if (this.confirmDeletionResponse)
        {
            this.moduleDataService.delete(this.onDeleteModule)
                            .subscribe(res => {
                            var dataResult: any = res;
                            this.onSearch();  
                            });

        }
    this.confirmDeletionVisible = false;
 }


}
