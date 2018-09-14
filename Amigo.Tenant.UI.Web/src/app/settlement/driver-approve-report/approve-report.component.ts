import { Component, Input, Injectable, OnChanges, SimpleChange, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
//import { AmigoTenanttServiceClient, AmigoTenantTServiceApproveRequest, AmigoTenantTServiceStatus, AmigoTenantTServiceApproveSearchRequest } from '../../shared/api/services.client';
import { AmigoTenantTServiceApproveRequest, AmigoTenantTServiceStatus, AmigoTenantTServiceApproveSearchRequest } from '../../shared/api/services.client';
import { Http, Jsonp, URLSearchParams} from '@angular/http';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { UsersClient } from '../../shared/api/services.client';
import { ApproveServiceMaintenanceComponent } from './approve-service-maintenance.component'
import { SidePanelComponent } from "../../controls/sidepanel/sidepanel.component";
import { ListsService } from '../../shared/constants/lists.service';
import { AuthCheckDirective } from  '../../shared/security/auth-check.directive';
import { NgbdTypeaheadUserNew } from '../../shared/typeahead-usernew/typeahead-usernew';
import { AmigoTenantUserBasicDTO } from  '../../shared/api/services.client';
import { environment } from '../../../environments/environment.prod';
import { EnvironmentComponent } from  '../../shared/common/environment.component';
import { UserPaidBy, UserPaidByList } from  '../../model/user-paid-by.dto';
declare var $: any;

export class modelServiceDate {
    day: number;
    month: number;
    year: number;
}

@Component({
    selector: 'st-approve-report',
    templateUrl: './approve-report.component.html',
    providers: [UserPaidByList]
})


export class ApproveReportComponent extends EnvironmentComponent implements OnInit {
    maintenance: AmigoTenantTServiceApproveRequest;
    amigoTenantTServiceSearchResult: GridDataResult;
    amigoTenantTServiceReportDTOs: GridDataResult;
    amigoTenantTServiceStatus: AmigoTenantTServiceStatus[];
    isColumnHeaderSelected: boolean = true;
    public skip: number = 0;
    @ViewChild(ApproveServiceMaintenanceComponent) maintenanceComponent: ApproveServiceMaintenanceComponent
    @ViewChild(SidePanelComponent) sidePanelComponent: SidePanelComponent;
    isValidToApprove: boolean = false;
    modelFrom: any;
    modelTo: any;

    //PAID BY
    public paidByList: UserPaidBy[];
    //public paidByModel;

    //TYPEAHEAD USER
    currentUser: any;

    //TOTALS
    approvedTotals: number = 0;
    rejectedTotals: number = 0;
    pendingTotals: number = 0;
    totalResultCount: number = 0;

    // SEARCH CRITERIA
    amigoTenantTServiceApproveSearchRequest: AmigoTenantTServiceApproveSearchRequest;
    modelServiceDate: any;
    listEquipmentStatus = [];
    listMoveTypes = [];



    //PAGINATION
    public buttonCount: number = 20;
    public info: boolean = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [20, 50, 100, 200];
    public previousNext: boolean = true;
    public currentPage: number = 0;

    public pageChange({ skip, take }: PageChangeEvent): void {
        this.currentPage = skip;
        this.amigoTenantTServiceApproveSearchRequest.pageSize = take;
        this.getApproveReport();
        this.deselectColumnAll();
    }


    public successFlag: boolean;
    public errorMessages: any[];
    public successMessage: string;


    @Output() itemSelected = new EventEmitter<any>();

    ngOnInit() {
        this.maintenance = new AmigoTenantTServiceApproveRequest();
        this.initializeForm();
        this.resetResults();
    }

    public resetResults() {
        $(document).ready(() => {
            this.sidePanelComponent.bindSidePanelControls();
            this.resizeGrid();
        });

        $(window).bind('load resize scroll', (e) => {
            this.resizeGrid();
        });
    }
    constructor(
        //private amigoTenanttServiceClient: AmigoTenanttServiceClient,
        //private equipmentTypeService: EquipmentTypeClient,
        //private equipmentSizeService: EquipmentSizeClient,
        //private equipmentStatusClient: EquipmentStatusClient,
        //private serviceClient: ServiceClient,
        private userClient: UsersClient,
        private _listsService: ListsService,
        private userPaidByList: UserPaidByList) {
        super();
        this.paidByList = userPaidByList.List;
    }

    initializeForm(): void {
        this.amigoTenantTServiceApproveSearchRequest = new AmigoTenantTServiceApproveSearchRequest();
        this.amigoTenantTServiceApproveSearchRequest.serviceDate = new Date();
        this.amigoTenantTServiceApproveSearchRequest.driverId = 0;
        this.amigoTenantTServiceApproveSearchRequest.serviceStatusId = -1;
        this.amigoTenantTServiceApproveSearchRequest.approvedBy = "";
        this.amigoTenantTServiceApproveSearchRequest.approvalComments = "";
        this.amigoTenantTServiceApproveSearchRequest.pageSize = 20;
        this.amigoTenantTServiceApproveSearchRequest.username = "";
        this.amigoTenantTServiceApproveSearchRequest.paidBy = "M";
        this.currentPage = 0;
        this.getEquipmentStatus();
        this.getMoveTypes();
        this.resetGrid();
        this.totalResultCount = 0;
        this.onClearMessages();
        this.onClearTotals();
        this.sidePanelComponent.closeSidePanel(null);
        //this.sidePanelComponent.bindSidePanelControls();
        this.setDatesFrom();
        this.setDatesFromPerMove();
        this.setDatesToPerMove();
        this.typeaheadUserCleanValue();
    }

    getEquipmentStatus(): void {
        //this.equipmentStatusClient.getAllAll()
        //    .subscribe(res => {

        //        var dataResult: any = res;
        //        this.listEquipmentStatus = dataResult.data;
        //    }
        //    );
    }


    getMoveTypes(): void {
        //this.serviceClient.getServiceAll()
        //    .subscribe(res => {
        //        var dataResult: any = res;
        //        this.listMoveTypes = dataResult.data;
        //    }
        //    );
    }

    getBaseUserInfoById(): void {
        this.userClient.searchBaseInfoById(0)
            .subscribe(response => {
                let userName = (response != null)
                    ? response.firstName + ' ' + response.lastName + ' - ' + response.username
                    : "";
                this.amigoTenantTServiceApproveSearchRequest.approvedBy = (userName.length > 50)
                    ? userName.substring(0, 49)
                    : userName;
            }
            );

        this.amigoTenantTServiceApproveSearchRequest.approvalComments = "";
    }

    onSelectModelServiceDate(): void {
        if (this.modelServiceDate != null)
            this.amigoTenantTServiceApproveSearchRequest.serviceDate = new Date(this.modelServiceDate.year, this.modelServiceDate.month - 1, this.modelServiceDate.day, 0, 0, 0, 0);
        //else
        //    this.amigoTenantTServiceApproveSearchRequest.serviceDate = new Date();
    }


    getDriver = (item) => {
        if (item != null && item != undefined) {
            this.amigoTenantTServiceApproveSearchRequest.driverId = item.amigoTenantTUserId;
            this.amigoTenantTServiceApproveSearchRequest.username = item.username;
        }
        else {
            this.amigoTenantTServiceApproveSearchRequest.driverId = 0;
            this.amigoTenantTServiceApproveSearchRequest.username = item.username;
        }
    };

    getUpdate = () => {
        this.getApproveReport();
    };

    onSearch(): void {
        this.sidePanelComponent.closeSidePanel(null);
        this.onClearMessages();
        this.getApproveReport();
        this.deselectColumnAll();
    }


    getApproveReport(): void {
        if (this.amigoTenantTServiceApproveSearchRequest.paidBy == "H" && 
            (this.amigoTenantTServiceApproveSearchRequest.driverId == null ||
            this.amigoTenantTServiceApproveSearchRequest.driverId == undefined ||
            this.amigoTenantTServiceApproveSearchRequest.driverId <= 0)) {
            this.writeMessages("E", "Driver is mandatory");
            return;
        }

        if (this.amigoTenantTServiceApproveSearchRequest.paidBy == "H") {
            if (this.modelServiceDate == null || this.modelServiceDate == undefined) {
                this.writeMessages("E", "Date is mandatory");
                return;
            }
        } else if (this.amigoTenantTServiceApproveSearchRequest.paidBy == "M") {
            if (this.modelFrom == null || this.modelFrom == undefined) {
                this.writeMessages("E", "Date range is mandatory");
                return;
            }
            if (this.modelTo == null || this.modelTo == undefined) {
                this.writeMessages("E", "Date range is mandatory");
                return;
            }
        }


        this.amigoTenantTServiceApproveSearchRequest.pageSize = +this.amigoTenantTServiceApproveSearchRequest.pageSize;
        this.amigoTenantTServiceApproveSearchRequest.page = (this.currentPage + this.amigoTenantTServiceApproveSearchRequest.pageSize) / this.amigoTenantTServiceApproveSearchRequest.pageSize;
        
        //this.amigoTenanttServiceClient.searchAmigoTenantTServiceForApprove(
        //    this.amigoTenantTServiceApproveSearchRequest.serviceDate,
        //    this.amigoTenantTServiceApproveSearchRequest.reportDateFrom,
        //    this.amigoTenantTServiceApproveSearchRequest.reportDateTo,
        //    this.amigoTenantTServiceApproveSearchRequest.paidBy,
        //    this.amigoTenantTServiceApproveSearchRequest.driverId,
        //    this.amigoTenantTServiceApproveSearchRequest.username,
        //    this.amigoTenantTServiceApproveSearchRequest.serviceStatusId,
        //    this.amigoTenantTServiceApproveSearchRequest.approvedBy,
        //    this.amigoTenantTServiceApproveSearchRequest.approvalComments,
        //    this.amigoTenantTServiceApproveSearchRequest.page,
        //    this.amigoTenantTServiceApproveSearchRequest.pageSize)
        //    .subscribe(res => {

        //        var serviceStatusId = this.amigoTenantTServiceApproveSearchRequest.serviceStatusId == 2 ? null : this.amigoTenantTServiceApproveSearchRequest.serviceStatusId;
        //        var dataResult: any = res;
        //        this.amigoTenantTServiceSearchResult = dataResult.data.items;
        //        this.onCalculateTotals(dataResult.data);
        //        this.amigoTenantTServiceReportDTOs = {
        //            data: dataResult.data.items, //dataResult.data.items,
        //            total: dataResult.data.total //dataResult.data.total
        //        }
        //    }
        //    );

    }


    public onCalculateTotals(data) {
        this.approvedTotals = data.totalApproved;
        this.rejectedTotals = data.totalRejected;
        this.pendingTotals = data.totalPending;
        this.totalResultCount = data.total;
    }

    onReset(): void {
        this.initializeForm();
        this.isColumnHeaderSelected = true;
        $("#HeaderTemplate")[0].checked = !this.isColumnHeaderSelected;
    }

    public resetGrid(): void {
        let grid: GridDataResult[] = [];
        this.amigoTenantTServiceReportDTOs = {
            data: grid,
            total: 0
        };
        this.skip = 0;
    }

    public resizeGrid() {
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


    ///APPROVE
    ///APPROVE
    ///APPROVE

    approveOrReject: string = "";
    approveOrRejectDesc: string = "";

    //POPUP
    public approvedByPopupOpened: boolean = false;

    public approvedByPopupClose() {
        this.approvedByPopupOpened = false;
    }
    public approvedPopupOpen(code) {
        if (this.isThereRowSelected()) {
            // if (code == "A")
            // {
            this.approvedByPopupOpened = true;
            // }
            // else{
            //     this.openConfirmation();
            //}

            this.getBaseUserInfoById();
            this.approveOrReject = code;
            this.approveOrRejectDesc = code === "A" ? "Approve" : "Reject";
        }
        else {
            this.writeMessages('E', 'Select at least one activity to ' + (code === 'A' ? 'Approve' : 'Reject'));
        }
    }


    public changeItem(d) {
        d.isSelected = !d.isSelected;
    }


    public onApprove(): void {
        this.onSelectServiceStatus(true);
        this.onApproveRejectProcess();
        this.deselectColumnAll();
    }

    public deselectColumnAll()
    {
        this.isColumnHeaderSelected = true;
        $("#HeaderTemplate")[0].checked = !this.isColumnHeaderSelected;
    }

    public onReject(): void {
        this.onSelectServiceStatus(false);
        this.onApproveRejectProcess();
    }

    public onSelectPaidBy(): void {
        //this.typeaheadUserCleanValue();
        this.setDatesFrom();
        this.setDatesFromPerMove();
        this.setDatesToPerMove();
        this.amigoTenantTServiceApproveSearchRequest.serviceStatusId = -1;
        //this.amigoTenantTServiceApproveSearchRequest.driverId = 0;

    }

    onApproveRejectProcess() {
        this.maintenance.reportDate = this.amigoTenantTServiceApproveSearchRequest.serviceDate;
        this.maintenance.driverId = this.amigoTenantTServiceApproveSearchRequest.driverId;
        this.maintenance.driverName = this.amigoTenantTServiceApproveSearchRequest.username;
        this.maintenance.approvedBy = this.amigoTenantTServiceApproveSearchRequest.approvedBy;
        this.maintenance.amigoTenantTServiceIdsListStatus = this.amigoTenantTServiceStatus;
        this.maintenance.approvalComments = this.amigoTenantTServiceApproveSearchRequest.approvalComments;
        this.maintenance.fromDate = this.amigoTenantTServiceApproveSearchRequest.reportDateFrom;
        this.maintenance.toDate = this.amigoTenantTServiceApproveSearchRequest.reportDateTo;
        this.maintenance.moveOrHour = this.amigoTenantTServiceApproveSearchRequest.paidBy;
        //this.amigoTenanttServiceClient.approve(this.maintenance)
        //    .subscribe(res => {
        //        var dataResult: any = res;

        //        this.successFlag = dataResult.isValid;
        //        this.errorMessages = dataResult.messages;
        //        this.successMessage = 'The activities were ' + (this.approveOrReject === "A" ? "approved" : "rejected") + ' successfully';
        //        this.getApproveReport();
        //    }
        //    );
    }

    public onSelectServiceStatus(isApproveStatus: boolean) {
        this.maintenance.isApprove = isApproveStatus;
        this.amigoTenantTServiceStatus = new Array();
        for (let item in this.amigoTenantTServiceReportDTOs.data) {
            if (this.amigoTenantTServiceReportDTOs.data[item].isSelected === true) {
                let servStatus = new AmigoTenantTServiceStatus();
                servStatus.amigoTenantTServiceId = this.amigoTenantTServiceReportDTOs.data[item].amigoTenantTServiceId;
                servStatus.serviceStatus = isApproveStatus;
                servStatus.driverId = this.amigoTenantTServiceReportDTOs.data[item].amigoTenantTUserId;
                this.amigoTenantTServiceStatus.push(servStatus);
            }
        }
    }

    public selectionChange(event: any): void {
        let dataItem = this.amigoTenantTServiceReportDTOs.data[event.index - this.currentPage];
        this.maintenanceComponent.sendDataToChild(dataItem);
        this.sidePanelComponent.sidePanelRowSelected = true;
        this.sidePanelComponent.openSidePanel(true);
    }

    public serviceStatuses: AmigoTenantTServiceStatus[];

    public onChangeServiceStatus() {
        //this.isValidToApprove = this.amigoTenantTServiceApproveSearchRequest.serviceStatusId==2?false:true;
        //this.sidePanelComponent.closeSidePanel(null);
        //this.resetGrid();
        //var serviceStatusId = this.amigoTenantTServiceApproveSearchRequest.serviceStatusId==2?null:this.amigoTenantTServiceApproveSearchRequest.serviceStatusId;
        // this.onApplyStatusFilter(serviceStatusId, this.amigoTenantTServiceSearchResult);
        // this.getApproveReport();
    }

    public listServiceStatus: any[] = this._listsService.ApprovalStatus();

    public changeItemHeader() {
        let c = this.amigoTenantTServiceReportDTOs.data.length;
        let index = this.amigoTenantTServiceApproveSearchRequest.page * this.amigoTenantTServiceApproveSearchRequest.pageSize - this.amigoTenantTServiceApproveSearchRequest.pageSize;
        for (let item in this.amigoTenantTServiceReportDTOs.data) {
            if (this.amigoTenantTServiceReportDTOs.data[item].serviceStatus === null) {
                $("#" + index)[0].checked = this.isColumnHeaderSelected;
                this.amigoTenantTServiceReportDTOs.data[item].isSelected = this.isColumnHeaderSelected;
                
            }
            index++;
        }
        this.isColumnHeaderSelected = !this.isColumnHeaderSelected;
    }


    onClick = () => {
        this.amigoTenantTServiceReportDTOs.data[0].isSelected = !this.amigoTenantTServiceReportDTOs.data[0].isSelected;
    }

    onReloadGrid() {
        this.amigoTenantTServiceApproveSearchRequest.pageSize = 20;
        this.currentPage = 0;
        this.getApproveReport();
    }

    onSelectModelFrom(): void {
        if (this.modelFrom != null)
            this.amigoTenantTServiceApproveSearchRequest.reportDateFrom = new Date(this.modelFrom.year, this.modelFrom.month - 1, this.modelFrom.day, 0, 0, 0, 0);
        else
            this.amigoTenantTServiceApproveSearchRequest.reportDateFrom = new Date();
    }

    onSelectModelTo(): void {
        if (this.modelTo != null)
            this.amigoTenantTServiceApproveSearchRequest.reportDateTo = new Date(this.modelTo.year, this.modelTo.month - 1, this.modelTo.day, 0, 0, 0, 0);
        else
            this.amigoTenantTServiceApproveSearchRequest.reportDateTo = new Date();
    }

    ///CONFIRMATION

    confirmProcessVisible: boolean = false;
    confirmResponse: boolean = false;

    processConfirmation(status) {

        if (status == "yes") {
            if (this.approveOrReject == "A")
                this.onApprove();
            else if (this.approveOrReject == "R")
                this.onReject();

            this.approvedByPopupClose();
        }

        this.confirmProcessVisible = false;

    }

    openConfirmation() {
        this.confirmProcessVisible = true;
    }

    isThereRowSelected(): boolean {
        this.amigoTenantTServiceStatus = new Array();
        var rowsSelected = this.amigoTenantTServiceReportDTOs.data.filter(service => service.isSelected === true).length;
        if (rowsSelected > 0)
            return true;
        else
            return false;
    }

    writeMessages(type, msg) {
        if (type === 'E') {
            this.successFlag = false;
            this.errorMessages = [{ key: null, message: msg }];
        } else if (type === 'I') {
            this.successMessage = msg;
        }
    }

    onClearMessages() {
        this.successFlag = null;
        this.errorMessages = null;
        this.successMessage = null;
    }


    public onClearTotals() {
        this.approvedTotals = 0;
        this.rejectedTotals = 0;
        this.pendingTotals = 0;
        this.totalResultCount = 0;
    }

    public setDatesFrom() {
        var date = new Date();
        this.modelServiceDate = new modelServiceDate();
        this.modelServiceDate.day = date.getDate();
        this.modelServiceDate.month = date.getMonth() + 1;
        this.modelServiceDate.year = date.getFullYear();
        this.onSelectModelServiceDate();
    }

    public setDatesFromPerMove() {
        var date = new Date();
        this.modelFrom = new modelServiceDate();
        this.modelFrom.day = date.getDate();
        this.modelFrom.month = date.getMonth() + 1;
        this.modelFrom.year = date.getFullYear();
        if (this.modelFrom != null)
            this.amigoTenantTServiceApproveSearchRequest.reportDateFrom = new Date(this.modelFrom.year, this.modelFrom.month - 1, this.modelFrom.day, 0, 0, 0, 0);
    }

    public setDatesToPerMove() {
        var date = new Date();
        this.modelTo = new modelServiceDate();
        this.modelTo.day = date.getDate();
        this.modelTo.month = date.getMonth() + 1;
        this.modelTo.year = date.getFullYear();
        if (this.modelTo != null)
            this.amigoTenantTServiceApproveSearchRequest.reportDateTo = new Date(this.modelTo.year, this.modelTo.month - 1, this.modelTo.day, 0, 0, 0, 0);
    }

    public typeaheadUserCleanValue(): void {
        this.currentUser = new AmigoTenantUserBasicDTO();
        this.currentUser.username = '';
        this.currentUser.amigoTenantTUserId = 0;
        this.currentUser.customUsername = '';
    }
}
