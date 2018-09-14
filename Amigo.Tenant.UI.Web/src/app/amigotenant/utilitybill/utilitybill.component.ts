import { Component, OnInit } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { HouseSearchRequest, HouseClient } from '../../shared/index';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../house/dataService';

@Component({
  selector: 'app-utilitybill',
  templateUrl: './utilitybill.component.html',
  styleUrls: ['./utilitybill.component.css']
})
export class UtilityBillComponent implements OnInit {

  public gridData: GridDataResult;
  public skip: number = 0;
  public listHouseTypes = [];
  public listHouseStatuses = [];
  listServices: any[];
  public pageSizes: any = [20, 50, 100, 200];
  public currentPage: number = 0;
  countItems: number = 0;
  public visible: boolean = true;
  buttonCount: number;
  type: any;
  previousNex: any;
  public sub: Subscription;
  
  searchCriteria = new HouseSearchRequest();


  constructor(private router: Router, 
    private route: ActivatedRoute, 
    private houseDataService: HouseClient) { }

  ngOnInit() {
    this.searchCriteria.pageSize = 40;

    this.houseDataService.getHouseTypes()
    .subscribe(res => {
        var dataResult: any = res;
        this.listHouseTypes = dataResult.data;

        this.houseDataService.getHouseStatuses()
            .subscribe(res => {
                var dataResult: any = res;
                this.listHouseStatuses = dataResult.data;

                this.sub = this.route.params.subscribe(params => {
                    this.onSearch();
                });
            });
    });
  }

  onSearch() {
    this.searchCriteria.pageSize = +this.searchCriteria.pageSize;
    this.searchCriteria.page = (this.currentPage + this.searchCriteria.pageSize) / this.searchCriteria.pageSize;

    this.houseDataService.search(
        this.searchCriteria.address,
        this.searchCriteria.houseTypeId,
        this.searchCriteria.phoneNumber,
        this.searchCriteria.name,
        this.searchCriteria.code,
        this.searchCriteria.houseStatusId,
        this.searchCriteria.page,
        this.searchCriteria.pageSize)
        .subscribe(res => {
            var dataResult: any = res;
            this.countItems = dataResult.data.total;
            this.gridData = {
                data: dataResult.data.items,
                total: dataResult.data.total,
            }
        });
  };

  public onEdit(dataItem): void {
    DataService.currentHouse = dataItem;
    this.router.navigateByUrl('amigotenant/utilitybill/edit/' + dataItem.houseId);
  }

  public pageChange({ skip, take }: PageChangeEvent): void {
    this.currentPage = skip;
    this.searchCriteria.pageSize = take;
    this.onSearch();
  }

  deleteFilters(){}


}
