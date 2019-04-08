import { Component, OnDestroy, EventEmitter, Output, Input, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MasterDataService } from '../../shared/api/master-data-service';
import { ExpenseDataService } from './expense-data.service';
import { OnInit } from '@angular/core';
import { ExpenseDetailDto } from './dto/expense-detail-dto';
import { ResponseListDTO } from '../../shared/dto/response-list-dto';
import { GridDataResult } from '@progress/kendo-angular-grid';
import { EnvironmentComponent } from '../../shared/common/environment.component';
import { Subscription } from 'rxjs/Subscription';
import { ExpenseDetailRegisterRequest } from './dto/expense-detail-register-request';
import { ExpenseDetailChangeStatusRequest } from './dto/expense-detail-change-status-request';
import { DetailAmountsDto } from './dto/detail-amounts-dto';

declare var $: any;

@Component({
    selector: 'at-expense-detail-grid',
    templateUrl: './expense-maintenance-search-grid.component.html'
})

export class ExpenseMaintenanceSearchGridComponent extends EnvironmentComponent implements OnInit, OnDestroy, OnChanges {

    expenseDetailData: GridDataResult;
    totalResultCount: number
    sub: Subscription;
    expenseId: number;
    periodId: number;
    paymentTypeId: number;
    @Output() onCloseDetail = new EventEmitter<any>();
    @Input() expenseIdAfterNew: any;
    @Input() periodIdAfterNew: any;
    @Input() paymentTypeIdAfterNew: any;

    public successFlag: boolean;
    public errorMessages: any[];
    public successMessage: string;

    public openDialog = false;
    public openChangeStatusConfirmation = false;
    public selectedDetail: any;

    public isColumnHeaderSelected = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private masterDataService: MasterDataService,
        private expenseDataService: ExpenseDataService) {
        super();
    }

    ngOnChanges() {
        this.paymentTypeId = this.paymentTypeIdAfterNew !== undefined ? this.paymentTypeIdAfterNew : this.paymentTypeId;
        this.periodId = this.periodIdAfterNew !== undefined ? this.periodIdAfterNew : this.periodId;
        this.expenseId = this.expenseIdAfterNew !== undefined ? this.expenseIdAfterNew : this.expenseId;
    }

    ngOnInit() {
        // Inicializacion de la Grilla
        this.sub = this.route.params.subscribe(params => {
            let expenseId = params['expenseId'];
            let periodId = params['periodId'];
            let paymentTypeId = params['paymentTypeId'];
            if (expenseId != null && typeof (expenseId) !== 'undefined') {
                this.expenseId = expenseId;
                this.periodId = periodId;
                this.paymentTypeId = paymentTypeId;
            } else {
                // New
                this.expenseId = this.expenseIdAfterNew;
                this.periodId = this.periodIdAfterNew;
                this.paymentTypeId = this.paymentTypeIdAfterNew;
            }
            this.getExpenseDetails(this.expenseId);
        });
    }

    getExpenseDetails(expenseId: number): void {
        this.expenseDataService.getExpenseDetailByExpenseId(expenseId).subscribe(resp => {
            let datagrid = new ResponseListDTO(resp);
            this.expenseDetailData = {
                data: datagrid.items,
                total: datagrid.total
            };
            this.totalResultCount = datagrid.total;
        })
            .add(r => {
                this.onCloseDetail.emit(this.getDetailAmounts());
            });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    public changeItemHeader() {
        let c = this.expenseDetailData.data.length;
        let index = 0;
        for (let item in this.expenseDetailData.data) {
            $("#" + index)[0].checked = !this.isColumnHeaderSelected;
            this.expenseDetailData.data[item].isSelected = !this.isColumnHeaderSelected;
            index++;
        }
        this.isColumnHeaderSelected = !this.isColumnHeaderSelected;
    }

    onEdit(data): void {
        this.selectedDetail = data;
        this.openDialog = true;
    }

    closePopup() {
        this.openDialog = false;
    }

    eventoCloseParent = (item) => {
        this.openDialog = false;
        this.getExpenseDetails(item)
    };

    getDetailAmounts() {
        let totalAmount = 0;
        let subTotalAmount = 0;
        let tax = 0;
        for (let i = 0; i < this.expenseDetailData.data.length; i++) {
            totalAmount += this.expenseDetailData.data[i].totalAmount;
            subTotalAmount += this.expenseDetailData.data[i].subTotalAmount;
            tax += this.expenseDetailData.data[i].tax;
        }
        let detailAmounts = new DetailAmountsDto();
        detailAmounts.totalAmount = totalAmount;
        detailAmounts.subTotalAmount = subTotalAmount;
        detailAmounts.tax = tax;
        return detailAmounts;
    }

    onAddDetail(): void {
        this.openDialog = true;
        this.selectedDetail = new ExpenseDetailRegisterRequest();
        this.selectedDetail.expenseId = this.expenseId;
    }

    public messageToMigrate: string;
    onChangeStatus(): void {
        this.messageToMigrate = 'Do you accept to MIGRATE the item to PAYMENT?';
        let count = this.expenseDetailData.data.filter(q => q.isSelected);
        if (count.length === 0) {
            this.successFlag = false;
            this.errorMessages = [{ message: 'Error please select the details' }];
            setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);
        } else {
            this.openChangeStatusConfirmation = true;
            let rowsMigratedAndSelected = this.expenseDetailData.data.filter(q => q.isSelected && q.expenseDetailStatusName === 'MIGRATED' ).length;
            if (rowsMigratedAndSelected > 0)
                this.messageToMigrate = 'You have expenses that already has been MIGRATED to PAYMENTS, if you continue you will duplicate the concepts on PAYMENT. To avoid duplicated you must to eliminate the information on PAYMENT. Are you sure to Continue?';
        }
    }

    public close(status) {
        console.log(`Dialog result: ${status}`);
        this.openChangeStatusConfirmation = false;
    }

    closePopupConfirmation(): void {
        this.openChangeStatusConfirmation = false;
    }

    acceptChangeStatus(): void {
        let changeDetailStatusRequest = new ExpenseDetailChangeStatusRequest();
        changeDetailStatusRequest.ExpenseId = this.expenseId;
        changeDetailStatusRequest.PeriodId = this.periodId;
        let expenseDetailListId: number[] = [];
        this.expenseDetailData.data.filter(q => q.isSelected).forEach(element => {
            expenseDetailListId.push(element.expenseDetailId);
        });
        changeDetailStatusRequest.ExpenseDetailListId = expenseDetailListId;
        this.expenseDataService.changeStatusExpenseDetail(changeDetailStatusRequest).subscribe(
            res => {
                let dataResult: any = res;
                this.successFlag = dataResult.IsValid;
                this.errorMessages = dataResult.Messages.length > 0 ?  [{ message: dataResult.Messages[0].Message }] : dataResult.Messages;
            }
        ).add(
            res => {
                this.closePopupConfirmation();
                this.getExpenseDetails(this.expenseId);
            }
        );
    }

    changeItem(data: any): void {
        data.isSelected = !data.isSelected;
    }

}


