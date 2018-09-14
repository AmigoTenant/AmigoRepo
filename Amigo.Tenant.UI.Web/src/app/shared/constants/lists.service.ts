import { Injectable } from '@angular/core';
declare var $: any;

@Injectable()
export class ConstantsEnvironments{
    public getDateFormat()
    {
        return "MM/dd/yyyy";
    }
}

@Injectable()
export class ListsService {

    constructor() { }

    public ApprovalStatus() {

        return [
                {
                    "serviceStatusId": -1,
                    "name": "All"
                },
                {
                    "serviceStatusId": 2,
                    "name": "Pending"
                },
                {
                    "serviceStatusId": 1,
                    "name": "Approved"
                },
                {
                    "serviceStatusId": 0,
                    "name": "Rejected"
                }];
    }

    public ReportType() {
        return [
                    {
                        "type": "current",
                        "name": "Ongoing"
                    },
                    {
                        "type": "history",
                        "name": "Completed"
                    }];
    }

    public ServiceStatusOffOn() {
        return [
                    {
                        "id": -1,
                        "name": "All"
                    },
                    {
                        "id": 0,
                        "name": "Offline"
                    },
                    {
                        "id": 1,
                        "name": "Ongoing"
                    }];
    }

    public ActivityLogResult() {
        //debugger;
        return [
            {
                "resultCode": "",
                "name": "All"
            },
            {
                "resultCode": "ERR",
                "name": "Error"
            },
            {
                "resultCode": "Ok",
                "name": "Ok"
            }];
    }

    public NextDaysToCollectList()
    {
        return [
            {
                "id": null,
                "name": "All"
            },
            {
                "id": 1,
                "name": "0 - 5 days"
            },
            {
                "id": 2,
                "name": "6 - 10 days"
            },
            {
                "id": 3,
                "name": "11 - 15 days"
            }
        ]
    }

    public ActiveInactiveStatus() {
        return [
            {
                "id": undefined,
                "name": "All"
            },
            {
                "id": false,
                "name": "Inactive"
            },
            {
                "id": true,
                "name": "Active"
            }];
    }
    
}