import { Component, Input, Injectable, OnChanges, SimpleChange, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
//import { AmigoTenantTServiceReportDTO } from './amigoTenantTServiceReportDTO';

import { ListsService } from '../../shared/constants/lists.service';

import { AmigoTenantTServiceSearchRequest } from './amigoTenantTServiceSearchRequest';
//import { AmigoTenanttServiceClient, ReportClient, ProductDTO } from '../../shared/api/services.client';
//import { ProductDTO } from '../../shared/api/services.client';

import {Http, Jsonp, URLSearchParams} from '@angular/http';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';

import { ExportReportStructure } from '../../shared/utils/export-report-structure';
import { ExportCsv } from '../../shared/utils/export-csv';

//SEARCH CRITERIA:begin
import { EquipmentTypeDTO } from '../../shared/dtos/equipmentTypeDTO';
import { EquipmentSizeDTO } from '../../shared/dtos/equipmentSizeDTO';
import { EquipmentStatusDTO } from '../../shared/dtos/equipmentStatusDTO';
import { ServiceDTO } from '../../shared/dtos/serviceDTO';
import { UsersClient} from '../../shared/api/services.client';
//SEARCH CRITERIA:End

import {ActivatedRoute} from '@angular/router';
import {TitleCasePipe} from '../../shared/pipes/title-case.pipe';
import { AmigoTenantUserBasicDTO, LocationTypeAheadDTO } from  '../../shared/api/services.client';
import { EnvironmentComponent } from '../../shared/common/environment.component';

declare var $: any;

@Component({
    selector: 'app-report',
    templateUrl: './report.component.html'
})
export class ReportComponent extends EnvironmentComponent implements OnInit {

    amigoTenantTServiceReportDTOs: GridDataResult;
    //Search Model
    model: AmigoTenantTServiceSearchRequest;
    modelFrom: any;
    totalRecords: number;
    modelTo: any;
    equipmentTypeDTOs: EquipmentTypeDTO[];
    equipmentSizeDTOs: EquipmentSizeDTO[];
    equipmentSizeByEqpTypeDTOs: EquipmentSizeDTO[];
    listMoveTypes = [];
    listEquipmentStatus = [];

    //TYPEAHEAD USER
    currentUser: any;
    currentOriginLocation: any;
    currentDestinationLocation: any;

    //TYPEAHEAD PRODUCT
    currentProduct: any;
    isValidToApprove: boolean = false;

    //PAGINATION
    public buttonCount: number = 20;
    public info: boolean = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [20, 50, 100, 200, 500, 1000];
    public previousNext: boolean = true;
    public currentPage: number = 0;
    public isDriverCommentsVisible: boolean = false;
    public driverComments: string = "";

    public pageChange({ skip, take }: PageChangeEvent): void {
        this.currentPage = skip;
        this.model.pageSize = take;
        let isExport: boolean = false;
        let isExportForDow: boolean = false;
        this.getReport(isExport, isExportForDow);
    }


    columnsExternalCurrent: ExportReportStructure[] = [
        { field: 'username', header: 'Driver', type: 'string' },
        { field: 'equipmentNumber', header: 'Equipment', type: 'string' },
        { field: 'equipmentStatusName', header: 'Equipment Status', type: 'string' },
        { field: 'equipmentSize', header: 'Size', type: 'string' },
        { field: 'equipmentType', header: 'Type', type: 'string' },
        { field: 'chassisNo', header: 'Chassis No', type: 'string' },
        { field: 'service', header: 'Service', type: 'string' },
        { field: 'product', header: 'Product', type: 'string' },
        { field: 'isHazardousLabel', header: 'Haz', type: 'string' },
        { field: 'chargeNo', header: 'Charge No', type: 'string' },
        { field: 'originBlock', header: 'Origin Block', type: 'string' },
        { field: 'destinationBlock', header: 'Destination Block', type: 'string' },
        { field: 'approver', header: 'Approver', type: 'string' },
        { field: 'approvalStatus', header: 'Approval Status', type: 'string' },
        { field: 'dispatchingParty', header: 'Dispatching Party', type: 'string' },
        { field: 'serviceStartDate', header: 'Start Date', type: 'date' },
        { field: 'serviceStartDate', header: 'Start Time', type: 'time' },
        { field: 'serviceStartDayName', header: 'Start Day', type: 'string' },
        { field: 'customerBill', header: 'Customer Billing', type: 'string' }
    ];

    columnsExternalHistory: ExportReportStructure[] = [
        { field: 'username', header: 'Driver', type: 'string' },
        { field: 'equipmentNumber', header: 'Equipment', type: 'string' },
        { field: 'equipmentStatusName', header: 'Equipment Status', type: 'string' },
        { field: 'equipmentSize', header: 'Size', type: 'string' },
        { field: 'equipmentType', header: 'Type', type: 'string' },
        { field: 'chassisNo', header: 'Chassis No', type: 'string' },
        { field: 'service', header: 'Service', type: 'string' },
        { field: 'product', header: 'Product', type: 'string' },
        { field: 'isHazardousLabel', header: 'Haz', type: 'string' },
        { field: 'chargeNo', header: 'Charge No', type: 'string' },
        { field: 'originBlock', header: 'Origin Block', type: 'string' },
        { field: 'destinationBlock', header: 'Destination Block', type: 'string' },
        { field: 'approver', header: 'Approver', type: 'string' },
        { field: 'approvalStatus', header: 'Approval Status', type: 'string' },
        { field: 'dispatchingParty', header: 'Dispatching Party', type: 'string' },
        { field: 'serviceStartDate', header: 'Start Date', type: 'date' },
        { field: 'serviceStartDate', header: 'Start Time', type: 'time' },
        { field: 'serviceStartDayName', header: 'Start Day', type: 'string' },
        { field: 'serviceFinishDate', header: 'Finish Date', type: 'date' },
        { field: 'serviceFinishDate', header: 'Finish Time', type: 'time' },
        { field: 'serviceFinishDayName', header: 'Finish Day', type: 'string' },
        { field: 'serviceTotalHours', header: 'Total Hours', type: 'decimal' },
        { field: 'customerBill', header: 'Customer Billing', type: 'string' }
    ];

    columnsInternalCurrent: ExportReportStructure[] = [
        { field: 'username', header: 'Driver', type: 'string' },
        { field: 'equipmentNumber', header: 'Equipment', type: 'string' },
        { field: 'equipmentStatusName', header: 'Equipment Status', type: 'string' },
        { field: 'equipmentSize', header: 'Size', type: 'string' },
        { field: 'equipmentType', header: 'Type', type: 'string' },
        { field: 'chassisNo', header: 'Chassis No', type: 'string' },
        { field: 'service', header: 'Service', type: 'string' },
        { field: 'product', header: 'Product', type: 'string' },
        { field: 'isHazardousLabel', header: 'Haz', type: 'string' },
        { field: 'chargeNo', header: 'Charge', type: 'string' },
        { field: 'originBlock', header: 'Origin Block', type: 'string' },
        { field: 'destinationBlock', header: 'Destination Block', type: 'string' },
        { field: 'approver', header: 'Approver', type: 'string' },
        { field: 'approvalStatus', header: 'Approval Status', type: 'string' },
        { field: 'dispatchingParty', header: 'Dispatching Party', type: 'string' },
        { field: 'serviceStartDate', header: 'Start Date', type: 'date' },
        { field: 'serviceStartDate', header: 'Start Time', type: 'time' },
        { field: 'serviceStartDayName', header: 'Start Day', type: 'string' },
        { field: 'driverPay', header: 'Driver Pay', type: 'string' },
        { field: 'customerBill', header: 'Customer Billing', type: 'string' },
        { field: 'driverComments', header: 'Driver Comments', type: 'string' }
    ];

    columnsInternalHistory: ExportReportStructure[] = [
        { field: 'username', header: 'Driver', type: 'string' },
        { field: 'equipmentNumber', header: 'Equipment', type: 'string' },
        { field: 'equipmentStatusName', header: 'Equipment Status', type: 'string' },
        { field: 'equipmentSize', header: 'Size', type: 'string' },
        { field: 'equipmentType', header: 'Type', type: 'string' },
        { field: 'chassisNo', header: 'Chassis No', type: 'string' },
        { field: 'service', header: 'Service', type: 'string' },
        { field: 'product', header: 'Product', type: 'string' },
        { field: 'isHazardousLabel', header: 'Haz', type: 'string' },
        { field: 'chargeNo', header: 'Charge No', type: 'string' },
        { field: 'originBlock', header: 'Origin Block', type: 'string' },
        { field: 'destinationBlock', header: 'Destination Block', type: 'string' },
        { field: 'approver', header: 'Approver', type: 'string' },
        { field: 'approvalStatus', header: 'Approval Status', type: 'string' },
        { field: 'dispatchingParty', header: 'Dispatching Party', type: 'string' },
        { field: 'serviceStartDate', header: 'Start Date', type: 'date' },
        { field: 'serviceStartDate', header: 'Start Time', type: 'time' },
        { field: 'serviceStartDayName', header: 'Start Day', type: 'string' },
        { field: 'serviceFinishDate', header: 'Finish Date', type: 'date' },
        { field: 'serviceFinishDate', header: 'Finish Time', type: 'time' },
        { field: 'serviceFinishDayName', header: 'Finish Day', type: 'string' },
        { field: 'serviceTotalHours', header: 'Total Hours', type: 'decimal' },
        { field: 'driverPay', header: 'Driver Pay', type: 'string' },
        { field: 'customerBill', header: 'Customer Billing', type: 'string' },
        { field: 'driverComments', header: 'Driver Comments', type: 'string' }
    ];


    @Output() clearModelOutput = new EventEmitter<any>();

    //@Input() amigoTenantTServiceSearchRequest;

    constructor(
        //private weeklyReportService: AmigoTenanttServiceClient,
        //private reportService: ReportClient,
        private exportACsv: ExportCsv,
        //private equipmentTypeService: EquipmentTypeClient,
        //private equipmentSizeService: EquipmentSizeClient,
        //private serviceClient: ServiceClient,
        private _listsService: ListsService,
        private _route: ActivatedRoute,
        //private equipmentStatusClient: EquipmentStatusClient
    ) {
        super();
    }

    onSelect(): void {
        let isExport: boolean = false;
        let isExportForDow: boolean = false;
        this.getReport(isExport, isExportForDow);
    }

    ngOnInit() {
        $(document).ready(() => {
            this.resizeGrid(0);
        });

        $(window).bind('load resize scroll', (e) => {
            this.resizeGrid(0);
        });

        this.initializeForm();
        this.onSelect();
        //this.onReset();
    }

    public listApprovalStatus: any[] = this._listsService.ApprovalStatus();
    public reportTypes: any[] = this._listsService.ReportType();

    initializeForm(): void {
        this.model = new AmigoTenantTServiceSearchRequest();
        this.model.from = undefined;
        this.model.to = undefined;
        this.model.driver = undefined;
        this.model.serviceCode = undefined;
        this.model.approvalStatus = undefined;
        this.model.chargeNo = undefined;
        this.model.approver = undefined;
        this.model.equipment = undefined;
        this.totalRecords = 0;
        this.model.status = "history";
        this.model.equipmentStatusId = 0;
        this.model.serviceCode = "";
        this.model.approvalStatus = -1;
        this._route.params
            .map(params => params['target'])
            .subscribe((t) => {
                this.model.target = t;
                this.amigoTenantTServiceReportDTOs = {
                    data: [],
                    total: 0
                };
                this.totalRecords = 0;
            });


        this.getEquipmentTypes();
        this.getEquipmentSizes();
        this.getMoveTypes();

        this.model.pageSize = 20;
        this.typeaheadUserCleanValue();
        this.typeaheadProductCleanValue();
        this.typeaheadOriginLocationCleanValue();
        this.typeaheadDestinationLocationCleanValue();
        this.getEquipmentStatus();
        this.clearEquipmentSize();
    }

    getEquipmentStatus(): void {
        //this.equipmentStatusClient.getAllAll()
        //    .subscribe(res => {

        //        var dataResult: any = res;
        //        this.listEquipmentStatus = dataResult.data;

        //        if (this.listEquipmentStatus != undefined)
        //            this.listEquipmentStatus.splice(0, 0, new EquipmentStatusDTO(0, "All"));
        //    }
        //    );
    }
    onSelectModelFrom(): void {
        if (this.modelFrom != null)
            this.model.from = new Date(this.modelFrom.year, this.modelFrom.month - 1, this.modelFrom.day, 0, 0, 0, 0);
        else
            this.model.from = undefined;
    }

    onSelectModelTo(): void {
        if (this.modelTo != null)
            this.model.to = new Date(this.modelTo.year, this.modelTo.month - 1, this.modelTo.day, 0, 0, 0, 0);
        else
            this.model.to = undefined;
    }

    getEquipmentTypes(): void {
        //this.equipmentTypeService.getAllAll()
        //    .subscribe(res => {
        //        var dataResult: any = res;
        //        this.equipmentTypeDTOs = dataResult.data;

        //        if (this.equipmentTypeDTOs != undefined)
        //            this.equipmentTypeDTOs.splice(0, 0, new EquipmentTypeDTO(0, "All"));
        //    }
        //    );


    }

    getEquipmentSizes(): void {
        //this.equipmentSizeService.getAllAll()
        //    .subscribe(res => {
        //        var dataResult: any = res;
        //        this.equipmentSizeDTOs = dataResult.data;
        //    }
        //    );
    }

    clearEquipmentSize() {
        this.equipmentSizeByEqpTypeDTOs = [];
        this.model.equipmentSizeCode = "";

        if (this.equipmentSizeByEqpTypeDTOs != undefined)
            this.equipmentSizeByEqpTypeDTOs.splice(0, 0, new EquipmentSizeDTO(0, "All", ""));
    }

    getEquipmentSizesByEqpType(eqpTypeCode: string): void {

        this.equipmentSizeByEqpTypeDTOs = this.equipmentSizeDTOs.filter(function (item) {
            return item.equipmentTypeCode == eqpTypeCode
        });

        this.model.equipmentSizeCode = "";

        if (this.equipmentSizeByEqpTypeDTOs != undefined)
            this.equipmentSizeByEqpTypeDTOs.splice(0, 0, new EquipmentSizeDTO(0, "All", ""));

    }

    onStatusChange(statusCode: string): void {
        if (statusCode == "history")
            this.resizeGrid(40);
        else
            this.resizeGrid(-40);
    }


    getDriver = (item) => {
        if (item != null && item != undefined)
            this.model.driver = item.amigoTenantTUserId;
        else
            this.model.driver = undefined;
    };

    getOrigin = (item) => {
        if (item != null && item != undefined)
            this.model.origin = item.code;
        else
            this.model.origin = undefined;
    };


    getDestination = (item) => {
        console.log('-->', item);
        if (item != null && item != undefined)
            this.model.destination = item.code;
        else
            this.model.destination = undefined;
    };




    getMoveTypes(): void {
        //this.serviceClient.getServiceAll()
        //    .subscribe(res => {
        //        var dataResult: any = res;
        //        this.listMoveTypes = dataResult.data;

        //        if (this.listMoveTypes != undefined)
        //            this.listMoveTypes.splice(0, 0, new ServiceDTO(0, "", "All"));
        //    }
        //    );
    }

    public resizeGrid(extraHeight) {
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
            _viewportHeight += $(window).outerHeight() - _combinedPageElementsHeight - extraHeight;
            $(grid).find('.k-grid-content').height(_viewportHeight);
        });
    }


    getReport(isExport: boolean, isExportToDow: boolean): void {
        let pageSize = 20000;
        let currentPage = 1;
        if (!isExport) {
            this.model.pageSize = +this.model.pageSize;
            this.model.page = (this.currentPage + this.model.pageSize) / this.model.pageSize;
            pageSize = this.model.pageSize;
            currentPage = this.model.page;
        }

        var search_equipmentTypeCode = (this.model.equipmentTypeCode) ? this.model.equipmentTypeCode : undefined;
        var search_equipmentSizeCode = (this.model.equipmentSizeCode) ? this.model.equipmentSizeCode : undefined;
        var search_serviceCode = (this.model.serviceCode) ? this.model.serviceCode : undefined;
        var search_serviceStatus = (this.model.approvalStatus == null || this.model.approvalStatus == undefined || this.model.approvalStatus == -1) ? undefined : this.model.approvalStatus;

        var search_equipmentNumber = (this.model.equipment) ? this.model.equipment : undefined;
        var search_chargeNumber = (this.model.chargeNo) ? this.model.chargeNo : undefined;
        var search_approver = (this.model.approver) ? this.model.approver : undefined;
        if (!isExportToDow) {
            if (this.model.status == "current") {
                if (this.model.target == "external") {

                    //=========================================================================
                    //============================== EXTERNAL - CURRENT =======================
                    //=========================================================================

                    //if (!isExport) {
                    //    this.reportService.searchExternalCurrent
                    //        (this.model.driver,
                    //        search_serviceCode,
                    //        search_equipmentNumber,
                    //        search_equipmentSizeCode,
                    //        search_equipmentTypeCode,
                    //        search_approver,
                    //        search_chargeNumber,
                    //        this.model.origin,
                    //        this.model.destination,
                    //        search_serviceStatus,
                    //        this.model.equipmentStatusId,
                    //        this.model.productId,
                    //        currentPage,
                    //        pageSize)
                    //        .subscribe(response => {
                    //            this.setDataToTableOrExport(response, isExport);
                    //        });
                    //} else {
                    //    this.reportService.searchExternalCurrentReport
                    //        (this.model.driver,
                    //        search_serviceCode,
                    //        search_equipmentNumber,
                    //        search_equipmentSizeCode,
                    //        search_equipmentTypeCode,
                    //        search_approver,
                    //        search_chargeNumber,
                    //        this.model.origin,
                    //        this.model.destination,
                    //        search_serviceStatus,
                    //        this.model.equipmentStatusId,
                    //        this.model.productId,
                    //        currentPage,
                    //        pageSize);
                    //}
                }
                else //internal
                {

                    //=========================================================================
                    //============================== INTERNAL - CURRENT =======================
                    //=========================================================================
                    //if (!isExport) {
                    //    this.reportService.searchInternalCurrent
                    //        (this.model.driver,
                    //        search_serviceCode,
                    //        search_equipmentNumber,
                    //        search_equipmentSizeCode,
                    //        search_equipmentTypeCode,
                    //        search_approver,
                    //        search_chargeNumber,
                    //        this.model.origin,
                    //        this.model.destination,
                    //        search_serviceStatus,
                    //        this.model.equipmentStatusId,
                    //        this.model.productId,
                    //        currentPage,
                    //        pageSize)
                    //        .subscribe(response => {
                    //            this.setDataToTableOrExport(response, isExport);
                    //        });
                    //} else {
                    //    this.reportService.searchInternalCurrentReport
                    //        (this.model.driver,
                    //        search_serviceCode,
                    //        search_equipmentNumber,
                    //        search_equipmentSizeCode,
                    //        search_equipmentTypeCode,
                    //        search_approver,
                    //        search_chargeNumber,
                    //        this.model.origin,
                    //        this.model.destination,
                    //        search_serviceStatus,
                    //        this.model.equipmentStatusId,
                    //        this.model.productId,
                    //        currentPage,
                    //        pageSize);
                    //}
                }
            }
            else //history
            {
                if (this.model.target == "external") {
                    //=========================================================================
                    //============================== EXTERNAL - HISTORY =======================
                    //=========================================================================
                    //if (!isExport) {
                    //    this.reportService.searchExternalHistory
                    //        (this.model.from,
                    //        this.model.to,
                    //        this.model.driver,
                    //        search_serviceCode,
                    //        search_equipmentNumber,
                    //        search_equipmentSizeCode,
                    //        search_equipmentTypeCode,
                    //        search_approver,
                    //        search_chargeNumber,
                    //        this.model.origin,
                    //        this.model.destination,
                    //        search_serviceStatus,
                    //        this.model.equipmentStatusId,
                    //        this.model.productId,
                    //        currentPage,
                    //        pageSize)
                    //        .subscribe(response => {
                    //            this.setDataToTableOrExport(response, isExport);
                    //        });
                    //} else {
                    //    this.reportService.searchExternalHistoryReport
                    //        (this.model.from,
                    //        this.model.to,
                    //        this.model.driver,
                    //        search_serviceCode,
                    //        search_equipmentNumber,
                    //        search_equipmentSizeCode,
                    //        search_equipmentTypeCode,
                    //        search_approver,
                    //        search_chargeNumber,
                    //        this.model.origin,
                    //        this.model.destination,
                    //        search_serviceStatus,
                    //        this.model.equipmentStatusId,
                    //        this.model.productId,
                    //        currentPage,
                    //        pageSize);
                    //}
                }
                else //internal
                {

                    //=========================================================================
                    //============================== INTERNAL - HISTORY =======================
                    //=========================================================================
                    //if (!isExport) {
                    //    this.reportService.searchInternalHistory
                    //        (this.model.from,
                    //        this.model.to,
                    //        this.model.driver,
                    //        search_serviceCode,
                    //        search_equipmentNumber,
                    //        search_equipmentSizeCode,
                    //        search_equipmentTypeCode,
                    //        search_approver,
                    //        search_chargeNumber,
                    //        this.model.origin,
                    //        this.model.destination,
                    //        search_serviceStatus,
                    //        this.model.equipmentStatusId,
                    //        this.model.productId,
                    //        currentPage,
                    //        pageSize)
                    //        .subscribe(response => {
                    //            this.setDataToTableOrExport(response, isExport);
                    //        });
                    //} else {
                    //    this.reportService.searchInternalHistoryReport
                    //        (this.model.from,
                    //        this.model.to,
                    //        this.model.driver,
                    //        search_serviceCode,
                    //        search_equipmentNumber,
                    //        search_equipmentSizeCode,
                    //        search_equipmentTypeCode,
                    //        search_approver,
                    //        search_chargeNumber,
                    //        this.model.origin,
                    //        this.model.destination,
                    //        search_serviceStatus,
                    //        this.model.equipmentStatusId,
                    //        this.model.productId,
                    //        currentPage,
                    //        pageSize);
                    //}
                }
            }
        } else {
            //if (this.model.status == "current") {
            //    //if (this.model.target == "internal") {
            //    //    //=========================================================================
            //    //    //============================== INTERNAL - CURRENT =======================
            //    //    //=========================================================================
            //    //    this.reportService.searchInternalCurrentReportForDow
            //    //        (this.model.driver,
            //    //        search_serviceCode,
            //    //        search_equipmentNumber,
            //    //        search_equipmentSizeCode,
            //    //        search_equipmentTypeCode,
            //    //        search_approver,
            //    //        search_chargeNumber,
            //    //        this.model.origin,
            //    //        this.model.destination,
            //    //        search_serviceStatus,
            //    //        this.model.equipmentStatusId,
            //    //        this.model.productId,
            //    //        currentPage,
            //    //        pageSize);
            //    //}
            //}
            //else //history
            //{
            //    if (this.model.target == "internal") {

            //        //=========================================================================
            //        //============================== INTERNAL - HISTORY =======================
            //        //=========================================================================
            //        this.reportService.searchInternalHistoryReportForDow
            //            (this.model.from,
            //            this.model.to,
            //            this.model.driver,
            //            search_serviceCode,
            //            search_equipmentNumber,
            //            search_equipmentSizeCode,
            //            search_equipmentTypeCode,
            //            search_approver,
            //            search_chargeNumber,
            //            this.model.origin,
            //            this.model.destination,
            //            search_serviceStatus,
            //            this.model.equipmentStatusId,
            //            this.model.productId,
            //            currentPage,
            //            pageSize);
            //    }
            //}
        }
    }

    setDataToTableOrExport(response, isExport): void {
        if (!isExport) {
            this.amigoTenantTServiceReportDTOs = {
                data: response.data.items,
                total: response.data.total
            };
            this.totalRecords = response.data.total;
        } else {
            var dataResult: any = [];
            response.data.items.forEach(service => {
                dataResult.push(service);
            });
            this.exportACsv.exportingCSV(this.model.target + "_" + this.model.status + "_Report.csv", dataResult);
        }
    };

    onExport(): void {
        if (this.model.status == "current") {
            if (this.model.target == "external") {
                this.exportACsv.reportStructure = this.columnsExternalCurrent;
            }
            else //internal
            {
                this.exportACsv.reportStructure = this.columnsInternalCurrent;
            }
        }
        else //history
        {
            if (this.model.target == "external") {
                this.exportACsv.reportStructure = this.columnsExternalHistory;
            }
            else //internal
            {
                this.exportACsv.reportStructure = this.columnsInternalHistory;
            }
        }
        let isExport: boolean = true;
        let isExportForDow: boolean = false;
        this.getReport(isExport, isExportForDow);
    }

    onExportForDow(): void {
        let isExport: boolean = true;
        let isExportForDow: boolean = true;
        this.getReport(isExport, isExportForDow);
    }

    onReset(): void {
        this.initializeForm();
        this.amigoTenantTServiceReportDTOs = {
            data: [],
            total: 0
        }
    }

    ngOnDestroy() {
        this.clearModelOutput.unsubscribe();
    }

    public typeaheadUserCleanValue(): void {
        this.currentUser = new AmigoTenantUserBasicDTO();
        this.currentUser.username = '';
        this.currentUser.amigoTenantTUserId = 0;
        this.currentUser.customUsername = '';
    }
    public typeaheadProductCleanValue(): void {
        //this.currentProduct = new ProductDTO();
        //this.currentProduct.name = '';
        //this.currentProduct.productId = 0;
        //this.currentProduct.code = '';
    }
    public typeaheadOriginLocationCleanValue(): void {
        this.currentOriginLocation = new LocationTypeAheadDTO();
        this.currentOriginLocation.locationId = 0;
        this.currentOriginLocation.code = '';
        this.currentOriginLocation.name = '';
    }
    public typeaheadDestinationLocationCleanValue(): void {
        this.currentDestinationLocation = new LocationTypeAheadDTO();
        this.currentDestinationLocation.locationId = 0;
        this.currentDestinationLocation.code = '';
        this.currentDestinationLocation.name = '';
    }

    onViewDriverComments(dataItem) {
        this.isDriverCommentsVisible = true;
        this.driverComments = dataItem.driverComments;
    }

    closeDriverComments() {
        this.isDriverCommentsVisible = false;
        this.driverComments = "";
    }

    //Add product filter
    getProduct = (item) => {
        if (item != null && item != undefined)
            this.model.productId = item.productId;
        else
            this.model.productId = null;
    };

}
