import { Component, OnDestroy } from '@angular/core';
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

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private masterDataService: MasterDataService,
        private expenseDataService: ExpenseDataService) {
            super();
    }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            let expenseId = params['expenseId'];
            //let periodId = params['periodId'];
            if (expenseId != null && typeof (expenseId) !== 'undefined') {
                this.expenseId = expenseId;
                this.periodId = 47; //periodId;
                this.getExpenseDetails(expenseId);
            }
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
                $("#" + index)[0].checked = this.isColumnHeaderSelected;
                this.expenseDetailData.data[item].isSelected = this.isColumnHeaderSelected;
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
        this.getExpenseDetails(item);
    };


    onAddDetail(): void {
        this.openDialog = true;
        this.selectedDetail = new ExpenseDetailRegisterRequest();
        this.selectedDetail.expenseId = this.expenseId;
     }

    onChangeStatus(): void {
        this.openChangeStatusConfirmation = true;
    }

    public close(status) {
        console.log(`Dialog result: ${status}`);
        this.openChangeStatusConfirmation = false;
      }

    closePopupConfirmation(): void {
        this.openChangeStatusConfirmation = false;
        this.getExpenseDetails(6); //TODO: QUITAR ESTE HARDCODE
    }

    acceptChangeStatus(): void {
        debugger;
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
                this.getExpenseDetails(this.expenseId);
            }
        );
    }

    changeItem(data: any): void {
        data.isSelected = !data.isSelected;
    }

}
