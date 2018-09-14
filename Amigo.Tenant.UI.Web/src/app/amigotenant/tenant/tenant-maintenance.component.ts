import { Component, Input, Output, OnInit, EventEmitter, OnChanges, SimpleChange, ViewChild } from '@angular/core';

import { TenantClient, RegisterTenantRequest, UpdateTenantRequest } from '../../shared/api/services.client';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { BotonsComponent } from '../../controls/common/boton.component';
import { ValidationService } from '../../shared/validations/validation.service';

import { CountryDTO } from '../../model/countryDTO';
import { TypeDTO } from '../../model/typeDTO';
import { StatusDTO } from '../../model/statusDTO';
import { ModelDTO } from '../../model/modelDTO';

import { AmigoTenantUserBasicDTO } from '../../shared/api/services.client';
import { Audit, AuditModel } from '../../shared/common/audit.component';
declare var $: any;

export class FormError {
    fullNameError: boolean;
    countryIdError: boolean;
    emailError:boolean;
    emailFormatError:boolean;
}


@Component({
  selector: 'at-tenant-maintenance',
  templateUrl: './tenant-maintenance.component.html',
  styleUrls: ['./tenant-maintenance.component.less']
})
export class TenantMaintenanceComponent implements OnInit {
    @Input() inputSelectedTenant: any;
    @Output() onClickCloseDialog = new EventEmitter<boolean>();

    @Input() countries: CountryDTO[];
    @Input() tenantTypes: TypeDTO[];
    @Input() entityStatuses: StatusDTO[];

    _formError: FormError;

    selectedStatusId: number;
    auditModel: any;

    public mainPopupOpened: boolean = false;

    public tenant = new UpdateTenantRequest();

    public flgEdition: string;

    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;
    public user: any;

    constructor(private tenantDataService: TenantClient,
        private _validationService: ValidationService) { }

    initializeForm(): void {
        this.resetFormError();
    }

    ngOnInit() {
        console.log(this.inputSelectedTenant);
        this.refreshAfterSaving = false;
        this.auditModel = new AuditModel();
        this.initializeForm();

        if (this.inputSelectedTenant != undefined && this.inputSelectedTenant != null) {
            this.onClearValidation();

            this.tenant.tenantId = this.inputSelectedTenant.tenantId;
            this.tenant.statusId = this.inputSelectedTenant.statusId;
            this.tenant.typeId = this.inputSelectedTenant.typeId;
            this.tenant.countryId = this.inputSelectedTenant.countryId;
            this.tenant.code = this.inputSelectedTenant.code;
            this.tenant.fullName = this.inputSelectedTenant.fullName;
            this.tenant.userId = this.inputSelectedTenant.userId;
            this.tenant.phoneN01 = this.inputSelectedTenant.phoneN01;
            this.tenant.idRef = this.inputSelectedTenant.idRef;

            this.tenant.address = this.inputSelectedTenant.address;
            this.tenant.contactEmail = this.inputSelectedTenant.contactEmail;
            this.tenant.contactName = this.inputSelectedTenant.contactName;
            this.tenant.contactPhone = this.inputSelectedTenant.contactPhone;
            this.tenant.contactRelation = this.inputSelectedTenant.contactRelation;
            this.tenant.passportNo = this.inputSelectedTenant.passportNo;
            this.tenant.reference = this.inputSelectedTenant.reference;
            this.tenant.phoneNo2 = this.inputSelectedTenant.phoneNo2;
            this.tenant.email = this.inputSelectedTenant.email;
            
            this.tenant.rowStatus = this.inputSelectedTenant.rowStatus;
            this.tenant.createdBy = this.inputSelectedTenant.createdBy;
            this.tenant.creationDate = this.inputSelectedTenant.creationDate;

            this.auditModel.createdBy = this.inputSelectedTenant.createdBy;
            this.auditModel.creationDate = this.inputSelectedTenant.creationDate;
            this.auditModel.updatedBy = this.inputSelectedTenant.updatedBy;
            this.auditModel.updatedDate = this.inputSelectedTenant.updatedDate;

            this.flgEdition = "E";
        } else {
            this.onClearValidation();
            this.cleanTenantForm();
            this.tenant.rowStatus = true;
            this.flgEdition = "N";
        }
    }

    public close() {
        this.onClickCloseDialog.emit(this.refreshAfterSaving);
    }
  
    cleanTenantForm() {
        this.tenant = new UpdateTenantRequest();
        this.tenant.rowStatus = true;
    }

    onChange(selectedValue) {
        //console.log(selectedValue);
    }
   
    /*-------------------------------------------------------------------------------------*/

    onNew(): void {
        this.onClear();
        this.onClearValidation();
        //this.mainPopupOpen();
    }

    private onClear(): void {
        this.tenant = new UpdateTenantRequest();
        this.flgEdition = "N";
    }

    onClearValidation() {
        this.successFlag = null;
        this.errorMessages = null;
        this.successMessage = null;
    }

    //---------------------------------------------------------------------------------
    //--------------------------   Save, Update, Delete     ---------------------------
    //---------------------------------------------------------------------------------

    //This Event came from BotonComponent
    onExecuteEvent($event) {
        switch ($event) {
            case "s":
                this.onSave();
                //break;
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

    onSave = () => {

        if (this.isValidData()) {
            if (this.flgEdition === 'N') {
                this.tenantDataService.register(this.tenant)
                    .subscribe(res => {
                        var dataResult: any = res;

                        this.successFlag = dataResult.isValid;
                        this.errorMessages = dataResult.messages;
                        this.successMessage = 'Tenant added';
                        setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);
                        if (this.successFlag) {
                            this.getTenantById(dataResult.pk);
                            this.refreshAfterSaving = true;
                            this.flgEdition = "E";
                        }

                    });
                console.log('INSERT');

            } else {
                this.tenantDataService.update(this.tenant)
                    .subscribe(res => {
                        var dataResult: any = res;

                        this.successFlag = dataResult.isValid;
                        this.errorMessages = dataResult.messages;
                        this.successMessage = 'Tenant updated';
                        if (this.successFlag) {
                            this.refreshAfterSaving = true;

                            // prepare the EDIT Form  register
                            this.flgEdition = "E";
                        }
                    });
            }
        }
    }

    getTenantById(id): void {
        this.tenantDataService.getTenantById(id).subscribe(
            response => {
                var dataResult: any = response;
                this.tenant = dataResult.data;
            });
    }

    onSelectCountry(): void {
        if (this.tenant.countryId !== null || this.tenant.countryId !== undefined)
        {
            this._formError.countryIdError = false;
        }
        else
            this._formError.countryIdError = true;
    }

    onChangeName(): void {
        if (this.tenant.fullName !== null || this.tenant.fullName !== undefined || this.tenant.fullName !== "") {
            this._formError.fullNameError = false;
        }
        else
            this._formError.fullNameError = true;
    }

    onChangeEmail(): void {
        this._formError.emailError = true;
        this._formError.emailFormatError = true;
        if (this.tenant.email !== null || this.tenant.email !== undefined || this.tenant.email !== "") {
            this._formError.emailError = false;
        }
        if (this.isEmailFormatValid()) {
            this._formError.emailFormatError = false;
        }
    }

    //--------------------------------------------------------------------------------------
    //--------------------------   Refresh grid after  Saving    ---------------------------
    //--------------------------------------------------------------------------------------

    refreshAfterSaving: boolean;

    isValidData(): boolean {
        var isValid = true;
        this.resetFormError();

        if (this.tenant.fullName == undefined || this.tenant.fullName == "" || this.tenant.fullName == null) {
            this._formError.fullNameError = true;
            isValid = false;
        }
        if (this.tenant.countryId == undefined || this.tenant.countryId == 0 || this.tenant.countryId == null) {
            this._formError.countryIdError = true;
            isValid = false;
        }
        if (this.tenant.email == undefined || this.tenant.email == '' || this.tenant.email == null) {
            this._formError.emailError = true;
            isValid = false;
        }
        if (!this.isEmailFormatValid()) {
            this._formError.emailFormatError = true;
            isValid = false;
        }
        return isValid;
    }

    emailMessageError: string;
    isEmailFormatValid(){
        this.emailMessageError = '';
        var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        if (!emailPattern.test(this.tenant.email))
        {
            this.emailMessageError='EMail has an invalid format';
            return false;
        }
        return true
    }

    private resetFormError = () => {
        if (this._formError == undefined)
            this._formError = new FormError();

        this._formError.fullNameError = false;
        this._formError.countryIdError = false;
    }

}
