import { Component, OnDestroy, EventEmitter, Output, Input } from '@angular/core';
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
     selector : 'at-expense-detail-grid',
     templateUrl: './expense-maintenance-search-grid.component.html'
 })

export class ExpenseMaintenanceSearchGridComponent extends EnvironmentComponent implements OnInit, OnDestroy {

    expenseDetailData: GridDataResult;
    totalResultCount: number
    sub: Subscription;
    expenseId: number;
    periodId: number;
    @Output() onCloseDetail = new EventEmitter<any>();
    @Input() expenseIdAfterNew: any;
    @Input() periodIdAfterNew: any;

    public successFlag: boolean;
    public errorMessages: any[];
    public successMessage: string;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private masterDataService: MasterDataService,
        private expenseDataService: ExpenseDataService) {
            super();
    }

    ngOnInit() {
        debugger; //Inicializacion de la Grilla
        this.sub = this.route.params.subscribe(params => {
            let expenseId = params['expenseId'];
            let periodId = params['periodId'];
            if (expenseId != null && typeof (expenseId) !== 'undefined') {
                this.expenseId = expenseId;
                this.periodId = periodId;
            }
            else{
                //New
                this.expenseId = this.expenseIdAfterNew;
                this.periodId = this.periodIdAfterNew;
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
        .add( r => {
            this.onCloseDetail.emit(this.getDetailAmounts());
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    isColumnHeaderSelected: boolean = false;

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

    public openDialog: boolean = false;
    public openChangeStatusConfirmation: boolean = false;
    public selectedDetail: any;

    onEdit(data): void {
        this.selectedDetail = data;
        this.openDialog = true;
     }


     closePopup() {
        this.openDialog = false;
     }

     eventoCloseParent= (item) => {
        this.openDialog = false;
        debugger;
        this.getExpenseDetails(item)
    };

    // getExpenseDetailAndEmit(expenseId: number): void {
    //     debugger;
    //     this.expenseDataService.getExpenseDetailByExpenseId(expenseId).subscribe(resp => {
    //         let datagrid = new ResponseListDTO(resp);
    //         this.expenseDetailData = {
    //             data: datagrid.items,
    //             total: datagrid.total
    //         };
    //         this.totalResultCount = datagrid.total;
    //     })
    //     .add( r => {
    //         this.onCloseDetail.emit(this.getDetailAmounts());
    //     });
    // }

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
        debugger;
        this.openDialog = true;
        this.selectedDetail = new ExpenseDetailRegisterRequest();
        this.selectedDetail.expenseId = this.expenseId;
     }

    onChangeStatus(): void {
        let count = this.expenseDetailData.data.filter(q => q.isSelected);
        if (count.length === 0) {
            this.successFlag = false;
            this.errorMessages = [{ message: 'Error please select the details' }];
            setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);
        } else {
            this.openChangeStatusConfirmation = true;
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
        this.expenseDataService.changeStatusExpenseDetail(changeDetailStatusRequest).subscribe().add(
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

