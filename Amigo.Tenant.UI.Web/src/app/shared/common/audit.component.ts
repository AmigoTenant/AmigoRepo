import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { UsersClient } from '../api/services.client';
import {Observable} from 'rxjs/Observable';
import { AmigoTenantTUserAuditDTO } from '../api/services.client';
import {TooltipModule} from "ngx-tooltip";

@Component(
{
    selector: 'st-audit',
    templateUrl: './audit.html'
})

export class Audit implements OnInit, AfterViewInit
{
    @Input() auditModel: AuditModel;
    user: AmigoTenantTUserAuditDTO;

    constructor(private userClient: UsersClient) {}

    ngAfterViewInit() {
        this.getAmigoTenantTUsers();
    }

    ngOnInit() {
        console.log('ngOnInit Audit');
    }

    getAmigoTenantTUsers(): void {
        var created = this.auditModel.createdBy === null ? 0 : this.auditModel.createdBy;
        var updated = this.auditModel.updatedBy === null ? 0 : this.auditModel.updatedBy;

        if (created > 0 || updated > 0) {
            this.userClient.searchByIdForAudit(created, updated)
                .subscribe(res => {
                    var dataResult: any = res;
                    this.user = dataResult;
                    this.auditModel.createdBy = dataResult.createdBy;
                    this.auditModel.createdByCode = dataResult.createdByCode;
                    //this.auditModel.creationDate = dataResult.creationDate;
                    this.auditModel.updatedBy = dataResult.updatedBy;
                    this.auditModel.updatedByCode = dataResult.updatedByCode;
                    //this.auditModel.updatedDate = dataResult.updatedDate;
                    
                }
                );
        } 
    }

}

export class AuditModel
{
    createdBy: number;
    createdByCode: string;
    creationDate: Date;
    updatedBy: number;
    updatedByCode: string;
    updatedDate: Date;

}