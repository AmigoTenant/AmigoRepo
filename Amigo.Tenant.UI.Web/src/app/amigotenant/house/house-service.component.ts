import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { HouseServiceClient, HouseServiceRequest } from '../../shared/api/amigotenant.service';
import { RegisterHouseRequest } from '../../shared/index';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs/Rx';
import { HouseServiceDTO } from '../../shared/dtos/House/HouseServiceDTO';
import { ServiceHousePeriodDTO } from '../../shared/dtos/House/ServiceHousePeriodDTO';
import { ServiceHouseDTO } from '../../shared/dtos/House/ServiceHouseDTO';
import { HouseServicePeriodDTO } from '../../shared/dtos/House/HouseServicePeriodDTO';

@Component({
    selector: 'at-house-service',
    templateUrl: './house-service.component.html',
    styles: []
})
export class HouseServiceComponent implements OnInit {
    @Input() selectedHouseService: HouseServiceRequest;
    @Input() alreadyAddedHouseServices: HouseServiceDTO[];
    @Output() onClickCloseDialog = new EventEmitter<boolean>();
    
    public services: ServiceHouseDTO[];
    public houseServicePeriods: ServiceHousePeriodDTO[]; 
    public changes: any = {};

    public refreshAfterSaving: boolean = false;

    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;

    public checkAll: boolean;
    public flgEdition: string;

    public months = [ { value: 1, label: "January" }, { value: 2, label: "February" }, { value: 3, label: "March" }, { value: 4, label: "April" }, { value: 5, label: "May" }, { value: 6, label: "June" }, { value: 7, label: "July" }, { value: 8, label: "August" }, { value: 9, label: "September" }, { value: 10, label: "October" }, { value: 11, label: "November" }, { value: 12, label: "December" }]
    public days = [ { day: "01" }, { day: "02" }, { day: "03" }, { day: "04" }, { day: "05" }, { day: "06" }, { day: "07" }, { day: "08" }, { day: "09" }, { day: "10" }, { day: "11" }, { day: "12" }, { day: "13" }, { day: "14" }, { day: "15" }, { day: "16" }, { day: "17" }, { day: "18" }, { day: "19" },  { day: "20" }, { day: "21" }, { day: "22" }, { day: "23" }, { day: "24" }, { day: "25" }, { day: "26" }, { day: "27" }, { day: "28" }, { day: "29" }, { day: "30" }, { day: "31" } ]
    
    constructor(private houseDataService: HouseServiceClient,
                private formBuilder: FormBuilder) { }

    ngOnInit() {
        if (this.selectedHouseService != null && this.selectedHouseService != undefined) {
            this.houseDataService.getServiceHousesAll()
                .subscribe(res => {
                    var dataResult: any = res;
                    var result = dataResult.data;
                    this.services = result;

                    if(this.selectedHouseService.isNew) {
                        this.flgEdition = "N";
                        this.services = this.services.filter((value, index) => { 
                            return !this.alreadyAddedHouseServices.some((value1, index1) => {
                                return value1.serviceId == value.serviceId;
                            });
                        });
                    }
                    else {
                        this.flgEdition = "E";
                        this.onChangeService(this.selectedHouseService.serviceId);
                    }
                });
        }
    }
    
    public close() {
        this.onClickCloseDialog.emit(this.refreshAfterSaving);
    }

    //#endregion

    public onExecuteEvent($event) {
        switch ($event) {
            case "s":
                this.onSave();
                break;
            case "c":
                //this.onClear();
                break;
            case "k":
                this.onClear();
                this.close();
                break;
            default:
                confirm("Sorry, that Event is not exists yet!");
        }
    }

    public onSave() {
        // Get selectedHouseService
        let houseService = this.services.filter((value, index ) => {
            return value.serviceId == this.selectedHouseService.serviceId
        });
        if(houseService != undefined && houseService.length > 0) {
            let request = this.MapRequestfromDTO(houseService[0]);
            this.houseDataService.registerHouseService(request)
            .subscribe(res => {
                var dataResult: any = res;
    
                this.successFlag = dataResult.isValid;
                this.errorMessages = dataResult.messages;
                this.successMessage = 'Service added';
                if (this.successFlag) {
                    this.refreshAfterSaving = true;
                    this.close();
                }
            });
        }
    }

    private onClear(): void {
        this.refreshAfterSaving = false;
    }

    public onChangeService(selectedValueId): void {
        //this.houseServicePeriods = eventData.target.value.serviceHousePeriods;
        var service = this.services.filter((value, index) => { return value.serviceId == this.selectedHouseService.serviceId});
        this.houseServicePeriods = service[0].serviceHousePeriods;
    }

    private MapRequestfromDTO(data?: ServiceHouseDTO): HouseServiceRequest {
        let houseServiceRequest = new HouseServiceRequest();
        if (data !== undefined) {
            houseServiceRequest.houseId = this.selectedHouseService.houseId;
            houseServiceRequest.houseServiceId = this.selectedHouseService.houseServiceId;

            houseServiceRequest.serviceId = data["serviceId"] !== undefined ? data["serviceId"] : null;
            houseServiceRequest.rowStatus = true;
            
            houseServiceRequest.creationDate = new Date();
            houseServiceRequest.userId = data["userId"] !== undefined ? data["userId"] : null;
            houseServiceRequest.userName = data["userName"] !== undefined ? data["userName"] : null;

            houseServiceRequest.houseServicePeriods = [];
            data.serviceHousePeriods.forEach(element => {
                let mapped = this.MapPeriodRequestfromDTO(element);
                houseServiceRequest.houseServicePeriods.push(mapped);
            })
        }
        return houseServiceRequest;
    }

    private MapPeriodRequestfromDTO(data?: ServiceHousePeriodDTO): HouseServicePeriodDTO {
        let request = new HouseServicePeriodDTO();
        if (data !== undefined) {
            request.houseServiceId = this.selectedHouseService.houseServiceId; 
            request.houseServicePeriodId = 0;

            request.monthId = data["monthId"] !== undefined ? data["monthId"] : null;
            request.dueDateDay = data["dueDateDay"] !== undefined ? data["dueDateDay"] : null;
            request.dueDateMonth = data["dueDateMonth"] !== undefined ? data["dueDateMonth"] : null;
            request.cutOffDay = data["cutOffDay"] !== undefined ? data["cutOffDay"] : null;
            request.cutOffMonth = data["cutOffMonth"] !== undefined ? data["cutOffMonth"] : null;

            request.rowStatus = true;
            request.createdBy = data["createdBy"] !== undefined ? data["createdBy"] : null;
            request.creationDate = new Date();
        }

        return request;
    }
    
    public getMonthById(id: number): string {
        var m = this.months.filter((value, index) => { return value.value == id; });
        if(m.length > 0)
            return m[0].label;
        else
            return null;
    }
}
