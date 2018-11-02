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

declare var $: any;

 @Component({
     selector : 'at-expense-detail-grid',
     templateUrl: './expense-maintenance-search-grid.component.html'
 })

export class ExpenseMaintenanceSearchGridComponent extends EnvironmentComponent implements OnInit, OnDestroy {

    //private variables
    expenseDetailData: GridDataResult;
    totalResultCount: number
    sub: Subscription;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private masterDataService: MasterDataService,
        private expenseDataService: ExpenseDataService) {
            super();
            debugger;
    }

    ngOnInit() {
        debugger;
        this.sub = this.route.params.subscribe(params => {
            let expenseId = params['expenseId'];
            if (expenseId != null && typeof (expenseId) !== 'undefined') {
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

    // public changeItemHeader() {
    //     let c = this.expenseDetailData.data.length;
    //     let index = 0 ; 
    //     for (let item in this.expenseDetailData.data) {
    //             $("#" + index)[0].checked = this.isColumnHeaderSelected;
    //             this.expenseDetailData.data[item].isSelected = this.isColumnHeaderSelected;
    //         index++;
    //     }
    //     this.isColumnHeaderSelected = !this.isColumnHeaderSelected;
    // }

    openDialog: boolean = false;
    selectedDetail: any;

    onEdit(data): void {
        this.selectedDetail = data;
        this.openDialog = true;
     }

}
