import { Component, OnInit, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { UsersClient, AmigoTenantTUserDTO, AmigoTenantTRoleClient, AmigoTenantTRoleDTO, LocationClient, LocationDTO, LocationTypeAheadDTO } from '../../../shared/api/services.client';
import { UserType, UserTypeList } from '../../../model/user-type.dto';
import { UserPaidBy, UserPaidByList } from '../../../model/user-paid-by.dto';
import { Confirmation, ConfirmationList, ConfirmationIntResult } from '../../../model/confirmation.dto';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import { BotonsComponent } from '../../../controls/common/boton.component';
import { ValidationService } from '../../../shared/validations/validation.service';
import { BlurForwarder } from '../../../shared/directives/blur-forwarder.directive';
//import { NgbdTypeaheadLocation } from  '../../../shared/typeahead-location/typeahead-location';
import { Autofocus } from  '../../../shared/directives/autofocus.directive';
import { Audit, AuditModel } from '../../../shared/common/audit.component';

@Component({
    selector: 'st-dialog-user',
    templateUrl: './dialog-user.component.html',
    providers: [LocationClient, AmigoTenantTRoleClient, UserPaidByList, UserTypeList, ConfirmationList]
})
export class DialogUserComponent implements OnInit {

    @Input() userId = 0;
    @Input() userSelected:any;
    @Output() onClickCloseDialog = new EventEmitter<boolean>();
    @Input() isOnlyDriver: boolean = false;
    auditModel: any;

    //@ViewChild(NgbdTypeaheadLocation) ngbdTypeaheadLocation: NgbdTypeaheadLocation;

    public typeList: UserType[];
    public paidByList: UserPaidBy[];
    public confirmList: ConfirmationIntResult[];
    public isSave: boolean = true;
    public titleDialog: string = '';
    public isAdminDisabled = false;
    public userRoleId: number = 0;

    public roleList: AmigoTenantTRoleDTO[];
    public lblAllItems: AmigoTenantTRoleDTO = new AmigoTenantTRoleDTO();

    public user = new AmigoTenantTUserDTO();

    public disabledPasswordDetails: boolean = false;
    public isChangePassword: boolean = false;
    public isCancelChangePassword: boolean = false;
    public disabledUsername: boolean = false;
    public disabledUserType: boolean = true;
    public disabledForInternalUser: boolean = false;

    public isUserDetails: boolean = false;
    public isPasswordDetails: boolean = false;
    public isDriverDetails: boolean = false;

    public confirmPassword: string;
    public passwordConfirmationFailure: boolean;

    public createIconChangePassword: boolean = false;
    public createIconCancelChangePassword: boolean = false;

    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;
    public isNew: boolean = true;

    constructor(private userClient: UsersClient,
        private roleClient: AmigoTenantTRoleClient,
        private locationClient: LocationClient,
        private userTypeList: UserTypeList,
        private userPaidByList: UserPaidByList,
        private confirmationList: ConfirmationList,
        private _validationService: ValidationService) {
        this.confirmList = this.confirmationList.ListIntResult;
        this.fillSelects();
    }

    fillSelects() {
        this.cleanUserForm();
    }

    cleanUserForm() {
        this.user = new AmigoTenantTUserDTO();
        this.user.username = '';
        this.user.amigoTenantTRoleId = 0;
        this.user.userType = '';
        this.user.payBy = '';
        this.isChangePassword = false;
        this.isCancelChangePassword = false;
        this.isAdminDisabled = false;
        this.createPasswordControl = true;
        //if (this.ngbdTypeaheadLocation != undefined)
        //    this.ngbdTypeaheadLocation.cleanValue();
    }

    public resetUserForm() {
        this.user.firstName = '';
        this.user.lastName = '';
        this.user.email = '';
        this.user.userType = '';
        //this.user.amigoTenantTRoleId = 0;
        this.user.amigoTenantTRoleId = this.userRoleId;
        this.user.password = '';
        this.confirmPassword = '';
        this.user.payBy = '';
        //Falta location
        this.user.unitNumber = '';
        this.user.tractorNumber = '';
        this.user.bypassDeviceValidation = false;
        this.isChangePassword = false;
        this.isCancelChangePassword = false;
        this.isAdminDisabled = false;
        this.typeaheadLocationCleanValue();
    }

    saveUser() {

        if (this.userId > 0) {
            this.updateUser()
        } else {
            this.registerUser();
        }
    }

    cleanUser() {
        this.cleanUserForm();
    }

    ngOnInit() {

        this.refreshAfterSaving = false;
        this.passwordConfirmationFailure = false;

        this.createIconChangePassword = false;
        this.createIconCancelChangePassword = false;

        this.isSave = true;
        
        
        if (this.userId > 0) {
            this.isNew = false;
            this.isSave = false;
            this.serviceGetUser();
            this.titleDialog = 'Edit ';
            this.disabledPasswordDetails = true;
            this.disabledUsername = true;
            this.createIconChangePassword = true;
            this.auditModel = new AuditModel();
            this.auditModel.createdBy = this.userSelected.createdBy;
            this.auditModel.creationDate = this.userSelected.creationDate;
            this.auditModel.updatedBy = this.userSelected.updatedBy;
            this.auditModel.updatedDate = this.userSelected.updatedDate;

        } else {
            this.cleanUserForm();
            this.isNew = true;
            this.titleDialog = 'Add User';
            this.disabledPasswordDetails = false;
            this.disabledUsername = false;
        }

        this.serviceGetRoles();
    }

    public close() {
        this.onClickCloseDialog.emit(this.refreshAfterSaving);
    }

    public serviceGetUser(): void {
        this.userClient.searchById(this.userId, '', '', '', null, '', null, '', 1, 10)
            .subscribe(response => {
                let titleUsername = response.username;
                this.user = response;
                if (this.user.dedicatedLocationId != undefined && this.user.dedicatedLocationId != null && this.user.dedicatedLocationId > 0) {
                    this.currentOriginLocation = new LocationTypeAheadDTO();
                    this.currentOriginLocation.locationId = this.user.dedicatedLocationId;
                    this.currentOriginLocation.name = this.user.locationName;
                    this.currentOriginLocation.code = '';
                }
                this.user.id = 0;
                this.user.entityStatus = 1;
                this.user.rowStatus = true;
                setTimeout(() => { this.titleDialog = 'Edit User'; }, 100);
                this.showUser(this.user.userType, this.user, 'edit');
                if (this.user.isAdmin && !this.user.isAdminModifiedUser)
                    this.isAdminDisabled = true;

            });
    };

    public serviceGetRoles(): void {
        let firstEntryDataRole = new AmigoTenantTRoleDTO(); //TODO
        firstEntryDataRole.amigoTenantTRoleId = 0; firstEntryDataRole.name = 'Loading...';
        this.roleList = [];
        this.roleList.unshift(firstEntryDataRole);
        this.user.amigoTenantTRoleId = 0;
        this.userRoleId = 0;
        this.roleClient.customRoleSearch(0, '', '', null, 1, 1000)
            .subscribe(response => {
                var items = response.data.items;
                this.roleList = [];
                if (this.isOnlyDriver) {
                    //debugger;
                    for (var i = 0, j = items.length; i < j; i++) {
                        if (items[i].code == 'DRIVER') {
                            this.roleList = [items[i]];
                            //this.user.amigoTenantTRoleId = items[i].amigoTenantTRoleId;
                            this.userRoleId = items[i].amigoTenantTRoleId;
                            break;
                        }
                    }
                } else {
                    firstEntryDataRole.amigoTenantTRoleId = 0; firstEntryDataRole.name = '-Select-';
                    this.roleList = response.data.items;
                    this.roleList.unshift(firstEntryDataRole);
                    this.user.amigoTenantTRoleId = 0;
                }
            });
    }

    public registerUser() {
        this.user.amigoTenantTUserId = 0;
        this.user.id = 0;
        this.user.entityStatus = 1;
        this.user.rowStatus = true;
        this.user.isAdmin = false;
        this.user.isAdminModifiedUser = false;
        this.user.dedicatedLocationId = (this.user.dedicatedLocationId == null || this.user.dedicatedLocationId == 0) ? undefined : this.user.dedicatedLocationId;
        this.user.amigoTenantTRoleId = (this.user.amigoTenantTRoleId == null || this.user.amigoTenantTRoleId == 0) ? undefined : this.user.amigoTenantTRoleId;

        this.userClient.register(this.user)
            .subscribe(response => {
                var dataResult: any = response;
                this.successFlag = dataResult.isValid;
                this.errorMessages = dataResult.messages;
                this.successMessage = 'User added';
                setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);

                if (this.successFlag) {

                    this.refreshAfterSaving = true;

                    setTimeout(() => { this.titleDialog = 'Edit User'; }, 100);

                    this.userClient.search(undefined, this.user.username, undefined, undefined,
                        undefined, undefined, undefined, undefined, 1, 10)
                        .subscribe(response => {
                            this.isSave = false;
                            this.user = response.data.items[0];
                            this.userId = this.user.amigoTenantTUserId;
                            this.titleDialog = 'Edit ';
                            this.disabledPasswordDetails = true;
                            this.disabledUsername = true;
                            this.user.id = 0;
                            this.user.entityStatus = 1;
                            this.user.rowStatus = true;

                            this.disabledPasswordDetailControl = true;
                            this.user.password = '';
                            this.confirmPassword = '';
                            this.createBtnContinueProcess = false;
                            this.createBtnResetUsername = false;

                            this.createIconChangePassword = false;
                            this.createIconCancelChangePassword = false;
                            if (this.user.userType == this.userTypeList.List[0].code || this.user.userType == this.userTypeList.List[2].code) {
                                this.createIconChangePassword = true;
                                this.isChangePassword = false;
                                this.createPasswordControl = false;
                            }

                        });

                }



            });
    }

    public updateUser() {
        //////////debugger;
        //----------------------------------------------------
        //          validate password confirmation
        //----------------------------------------------------

        if (this.user.password != undefined && this.user.password != null && this.user.password != this.confirmPassword) {
            this.passwordConfirmationFailure = true;
            return;
        }
        else {
            this.passwordConfirmationFailure = false;
        }

        //------------------------
        //          Update
        //------------------------

        this.user.dedicatedLocationId = (this.user.dedicatedLocationId == null || this.user.dedicatedLocationId == 0) ? undefined : this.user.dedicatedLocationId;
        this.user.amigoTenantTRoleId = (this.user.amigoTenantTRoleId == null || this.user.amigoTenantTRoleId == 0) ? undefined : this.user.amigoTenantTRoleId;


        this.userClient.update(this.user)
            .subscribe(response => {
                //this.onClickCloseDialog.emit(false);

                var dataResult: any = response;

                this.successFlag = dataResult.isValid;
                this.errorMessages = dataResult.messages;
                this.successMessage = 'User saved';
                setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);

                if (this.successFlag) {

                    this.refreshAfterSaving = true;

                }
                this.disabledPasswordDetailControl = true;
                //this.createIconChangePassword = true;
                this.createIconCancelChangePassword = false;
                this.user.password = '';
                this.confirmPassword = '';
                if (this.user.userType == this.userTypeList.List[0].code || this.user.userType == this.userTypeList.List[2].code) {
                    this.createIconChangePassword = true;
                    this.isChangePassword = false;
                    this.createPasswordControl = false;
                }

            });
    }

    public changePassword() {
        this.isChangePassword = true;
        this.isCancelChangePassword = true;
        this.disabledPasswordDetailControl = false;
        this.createIconChangePassword = false;
        this.createIconCancelChangePassword = true;
        this.createPasswordControl = true;
    }

    public cancelChangePassword() {
        this.isChangePassword = false;
        this.isCancelChangePassword = false;
        this.disabledPasswordDetailControl = true;
        this.createIconChangePassword = true;
        this.createIconCancelChangePassword = false;
        this.validatePassword('');
        this.validateConfirmPassword('');
        this.createPasswordControl = false;
    }

    /*****************Location Autocomplete********************* */
    public currentOriginLocation: any;
    //public modelTypeAheadLocation: any;
    public geLocation = (item) => {
        this.user.dedicatedLocationId = undefined;
        if (item != null && item != undefined && item != '') {
            this.user.dedicatedLocationId = item.locationId;
        }
    };

    public typeaheadLocationCleanValue(): void {
        this.currentOriginLocation = new LocationTypeAheadDTO();
        this.currentOriginLocation.locationId = 0;
        this.currentOriginLocation.name = '';
        this.currentOriginLocation.code = '';
    }


    //****************************
    onExecuteEvent($event) {
        ////console.log('=============================>>>>>>>>>>>>>>>>', $event);
        switch ($event) {
            case "s":
                //this.onSave();
                //this.saveUser();
                var isValidUserName = this.validateUserName('showError');
                if (isValidUserName) {
                    switch (this.stateValidationUser) {
                        case '1':
                            var isValidRole = this.validateRole('showError');
                            if (isValidRole) {
                                //alert('podemos grabar del active directory a amigotenanttuser');
                                this.saveUser();
                            }
                            break;
                        case '2':
                            //////////debugger;
                            var isValidFirstName = this.validateFirstName('showError');
                            var isValidLastName = this.validateLastName('showError');
                            var isValidType = this.validateType('showError');
                            var isValidRole = this.validateRole('showError');
                            var isValidPassword = false;
                            var isValidConfirmPassword = false;
                            if (this.isSave) {
                                var isValidPassword = this.validatePassword('showError');
                                var isValidConfirmPassword = this.validateConfirmPassword('showError');
                            } else {
                                if (this.isChangePassword) {
                                    var isValidPassword = this.validatePassword('showError');
                                    var isValidConfirmPassword = this.validateConfirmPassword('showError');
                                }
                            }

                            var isValidPaidBy = true;
                            if (this.user.userType == this.userTypeList.List[0].code)
                                isValidPaidBy = this.validatePaidBy('showError');

                            if (this.isSave) {
                                if (this.user.userType == this.userTypeList.List[0].code) {
                                    if (isValidFirstName && isValidLastName && isValidType && isValidRole && isValidPassword && isValidConfirmPassword && isValidPaidBy) {
                                        //alert('podemos grabar en amigotenant t user. El usuario no existe en active directory');
                                        this.saveUser();
                                    }
                                } else if (isValidFirstName && isValidLastName && isValidType && isValidRole && isValidPassword && isValidConfirmPassword) {
                                    //alert('podemos grabar en amigotenant t user. El usuario no existe en active directory');
                                    this.saveUser();
                                }
                            } else {
                                if (this.isChangePassword) {
                                    if (this.user.userType == this.userTypeList.List[0].code) {
                                        if (isValidFirstName && isValidLastName && isValidType && isValidRole && isValidPassword && isValidConfirmPassword && isValidPaidBy) {
                                            //alert('podemos grabar en amigotenant t user. El usuario no existe en active directory');
                                            this.saveUser();
                                        }
                                    } else if (isValidFirstName && isValidLastName && isValidType && isValidRole && isValidPassword && isValidConfirmPassword) {
                                        //alert('podemos grabar en amigotenant t user. El usuario no existe en active directory');
                                        this.saveUser();
                                    }
                                } else {
                                    if (this.user.userType == this.userTypeList.List[0].code) {
                                        if (isValidFirstName && isValidLastName && isValidType && isValidRole && isValidPaidBy) {
                                            //alert('podemos grabar en amigotenant t user. El usuario no existe en active directory');
                                            this.saveUser();
                                        }
                                    } else if (isValidFirstName && isValidLastName && isValidType && isValidRole) {
                                        //alert('podemos grabar en amigotenant t user. El usuario no existe en active directory');
                                        this.saveUser();
                                    }
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }

            //break;
            case "c":
                //this.onClear();
                break;
            case "k":
                //this.onClear();
                this.close();
                break;
            default:
                confirm("Sorry, that Event is not exists yet!");
        }
    }

    //--------------------------------------------------------------------------------------
    //--------------------------   Refresh grid after  Saving    ---------------------------
    //--------------------------------------------------------------------------------------

    refreshAfterSaving: boolean;



    //=============================================>>>>>>>>>>>>>>>>>>>>>>>>>
    //public messageCurrentlyProcess: string = '';
    public createUserFormControl: boolean = false;

    public disabledUserId: boolean = false;
    public disabledUserDetailControl: boolean = false;
    public disabledPasswordDetailControl: boolean = false;
    public disabledDriverDetailControl: boolean = false;
    public disabledUserDetailControlType: boolean = false;

    public createBtnContinueProcess: boolean = true;
    public createBtnResetUsername: boolean = false;
    public createBtnOnlyCancel: boolean = true;

    public isMandatoryUserDetail: boolean = false;
    public isMandatoryPasswordDetail: boolean = false;
    public isMandatoryDriverDetail: boolean = false;
    public createPasswordControl: boolean = true;

    public stateValidationUser: string = '';
    public typeAction: string = 'add';

    public successFlagValidation: boolean;
    public errorMessagesValidation: any[];
    public successMessageValidation: string;

    public txtDriverIsRequired = 'Driver Id is required';
    public txtDriverIsInvalid = 'Driver Id is invalid';
    public txtDriverIsValid = 'Driver Id is valid';
    public messageDriverIdValidation = '';


    public onContinueProcess() {
        if (this.validateUserName('showError'))
            this.onValidateUser();
    }

    public onValidateUser() {
        //debugger;
        this.resetUserForm();
        this._cleanValidationMessage();

        this.createUserFormControl = false;
        this.createBtnContinueProcess = false;
        this.createBtnResetUsername = false;
        this.createBtnOnlyCancel = true;

        this.disabledUserId = true;
        this.disabledUserDetailControl = true;
        this.disabledPasswordDetailControl = true;
        this.disabledDriverDetailControl = true;
        this.disabledUserDetailControlType = true;

        this.successFlagValidation = null; this.errorMessagesValidation = null; this.successMessageValidation = null;

        if (!this.isOnlyDriver)
            this.user.amigoTenantTRoleId = 0;

        this.userNameClass = 'form-control input-control loading';

        this.userClient.exists(0, this.user.username, '', '', 0, '', 0, '', 1, 10)
            .subscribe(response => {
                this.userNameClass = 'form-control input-control'; //pendiente
                if (!response) {
                    this.userClient.validateUserName(0, this.user.username, '', '', 0, '', 0, '', 1, 10)
                        .subscribe(response => {
                            var type = '1';
                            if (response != null && response.userName != null)
                                type = '2';

                            if (type == '2' && this.isOnlyDriver) {
                                setTimeout(() => {
                                    this.userNameClass = 'form-control ng-pristine ng-invalid ng-touched';
                                    this.messageDriverIdValidation = 'You can only create drivers';
                                    this.isValidUserName = false;
                                    this.disabledUserId = false;
                                    this.createBtnContinueProcess = true;
                                }, 100);
                            } else
                                this.showUser(type, response, 'add');
                        });
                } else {
                    this.stateValidationUser = '0';
                    this.userNameMessageValidateClass = 'message error';
                    this.disabledUserId = false;
                    this.createBtnContinueProcess = true;
                    this.createBtnResetUsername = false;
                    this.showMessageValidation(false, 'User already exists', '');
                    setTimeout(() => {
                        this.userNameClass = 'form-control ng-pristine ng-invalid ng-touched';
                        this.messageDriverIdValidation = 'User already exists';
                        this.isValidUserName = false;
                    }, 100);
                }
            });
    }

    public showUser(userType, response, action) {
        this._cleanValidationMessage();

        this.createUserFormControl = false;
        this.createBtnContinueProcess = false;
        this.createBtnResetUsername = false;
        this.createBtnOnlyCancel = true;

        this.disabledUserId = true;
        this.disabledUserDetailControl = true;
        this.disabledPasswordDetailControl = true;
        this.disabledDriverDetailControl = true;
        this.disabledUserDetailControlType = true;

        this.successFlagValidation = null; this.errorMessagesValidation = null; this.successMessageValidation = null;

        if (action == 'add') {
            if (!this.isOnlyDriver)
                this.user.amigoTenantTRoleId = 0;
            this.userNameClass = 'form-control input-control loading';
        }



        this.typeList = [];
        if (userType == '2') {
            //usuario active directory
            this.stateValidationUser = '1';
            this.showMessageValidation(true, '', 'User found in active directory...');
            this.typeList = [this.userTypeList.List[1]];
            this.user.userType = this.userTypeList.List[1].code;
            this.disabledUserDetailControlType = true;
            this.user.firstName = response.firstName;
            this.user.lastName = response.lastName;
            this.user.email = response.email;
            this.createIconChangePassword = false;
            this.createIconCancelChangePassword = false;
            if (action == 'edit') {
                this.isChangePassword = false;
                this.isCancelChangePassword = false;
            }
            if (action == 'add') {
                setTimeout(() => {
                    this.userNameClass = 'form-control input-control input-success';
                    this.messageDriverIdValidation = 'User AD account is valid';
                    this.isValidUserName = false;
                }, 100);
            }

        } else if (userType == '1' || userType == '3') {
            //Usuario valido
            this.stateValidationUser = '2';
            this.showMessageValidation(true, '', 'User not found in active directory but is valid...');
            if (this.isOnlyDriver) {
                this.userTypeList.List.forEach(type => {
                    if (type.code == this.userTypeList.List[0].code) {
                        this.typeList.push(type);
                    }
                });
            } else {
                this.userTypeList.List.forEach(type => {
                    if (type.code != this.userTypeList.List[1].code) {
                        this.typeList.push(type);
                    }
                });
                let select = new UserType(); //TODO
                select.id = ''; select.code = '', select.name = '-Select-';//TODO                    
                this.typeList.unshift(select);
            }

            if (action == 'add') {
                if (this.isOnlyDriver) {
                    this.user.userType = '1';
                    this.onChangeType('1');
                    //userType = '1';
                } else {
                    this.user.userType = '';
                }
                //this.user.userType = this.isOnlyDriver ? '1' : '';
            }

            this.isMandatoryUserDetail = true;
            this.isMandatoryPasswordDetail = true;
            this.disabledUserDetailControlType = false;
            if (action == 'add')
                this.disabledPasswordDetailControl = false;
            this.disabledUserDetailControl = false;
            if (action == 'edit') {
                this.createPasswordControl = false;
            }
            if (action == 'edit' && userType == '1') {
                this.paidByList = [];
                this.disabledDriverDetailControl = false;
                let firstEntryDataPaidBy = new UserPaidBy(); //TODO
                firstEntryDataPaidBy.code = ''; firstEntryDataPaidBy.name = '-Select-';//TODO
                this.paidByList = [firstEntryDataPaidBy];
                this.userPaidByList.List.forEach(paidBy => {
                    this.paidByList.push(paidBy);
                });
            }
            if (action == 'add') {
                setTimeout(() => {
                    this.userNameClass = 'form-control input-control input-success';
                    this.messageDriverIdValidation = 'User account is valid';
                    this.isValidUserName = false;
                }, 100);
            }
        }

        if (action == 'add') {
            this.createBtnResetUsername = true;
            if (!this.isOnlyDriver)
                this._fillFirstEntryDataPaidBy();
        }
        this.createBtnContinueProcess = false;
        this.createUserFormControl = true;
        this.createBtnOnlyCancel = false;
        this.userNameMessageValidateClass = 'message success';

    }

    public onResetUserName() {
        this.createBtnContinueProcess = true;
        this.createBtnResetUsername = false;
        this.disabledUserId = false;
        this.createUserFormControl = false;
        this.user.username = '';
        this.userNameClass = 'form-control input-control';
        this.isValidUserName = true;
        this.createBtnOnlyCancel = true;
    }

    public showMessageValidation(successFlag: boolean, errorMessage: string, successMessage: string) {
        this.successFlagValidation = successFlag;
        this.errorMessagesValidation = [{ key: null, message: errorMessage }];
        this.successMessageValidation = successMessage;
        setTimeout(() => { this.successFlagValidation = null; this.errorMessagesValidation = null; this.successMessageValidation = null; }, 5000);
    }

    public onChangeType(value: string) {
        ////debugger;
        ////console.log('closeeeeeeeeeeeeeeeeeeee', value, this.userTypeList.List[0].code);
        this.disabledDriverDetailControl = true;
        this.user.unitNumber = '';
        this.user.tractorNumber = '';
        this._fillFirstEntryDataPaidBy();
        this.isMandatoryDriverDetail = false;
        if (value == this.userTypeList.List[0].code) {
            this.disabledDriverDetailControl = false;
            this.isMandatoryDriverDetail = true;
            this.userPaidByList.List.forEach(paidBy => {
                ////debugger;
                this.paidByList.push(paidBy);
            });
        }
        //if (this.ngbdTypeaheadLocation != undefined)
        //    this.ngbdTypeaheadLocation.cleanValue();
        this.validateType('');
        this.validatePaidBy('');
        this.typeaheadLocationCleanValue();
    }

    private _fillFirstEntryDataPaidBy() {
        let firstEntryDataPaidBy = new UserPaidBy(); //TODO
        firstEntryDataPaidBy.code = ''; firstEntryDataPaidBy.name = '-Select-';//TODO
        this.paidByList = [firstEntryDataPaidBy];
        this.user.payBy = '';
    }


    //Validateeeeeeeeeeeeeeeeeeeee    
    public messagePasswordNotEquals: string = '';
    public userNameMessageValidateClass: string = 'message error';
    public userNameClass: string = 'form-control input-control'; public isValidUserName: boolean = true;
    public firstNameClass: string = 'form-control input-control'; public isValidFirstName: boolean = true;
    public lastNameClass: string = 'form-control input-control'; public isValidLastName: boolean = true;
    public typeClass: string = 'form-control input-control'; public isValidType: boolean = true;
    public roleClass: string = 'form-control input-control'; public isValidRole: boolean = true;
    public paidByClass: string = 'form-control input-control'; public isValidPaidBy: boolean = true;
    public passwordClass: string = 'form-control input-control'; public isValidPassword: boolean = true;
    public confirmPasswordClass: string = 'form-control input-control'; private isValidConfirmPassword: boolean = true;

    private _cleanValidationMessage() {
        this.userNameClass = 'form-control input-control'; this.isValidUserName = true;
        this.firstNameClass = 'form-control input-control'; this.isValidFirstName = true;
        this.lastNameClass = 'form-control input-control'; this.isValidLastName = true;
        this.typeClass = 'form-control input-control'; this.isValidType = true;
        this.roleClass = 'form-control input-control'; this.isValidRole = true;
        this.passwordClass = 'form-control input-control'; this.isValidPassword = true;
        this.confirmPasswordClass = 'form-control input-control'; this.isValidConfirmPassword = true;
        //this.createIconValidatinUser = false;
        //this.createIconUserValid = false;
        this.isMandatoryUserDetail = false;
        this.isMandatoryPasswordDetail = false;
        this.isMandatoryDriverDetail = false;
    }

    public validateUserName(event: string) {
        this.userNameClass = 'form-control input-control';
        this.userNameMessageValidateClass = 'message error';
        this.isValidUserName = true;
        if ((this.user.username == undefined || this.user.username == null || this.user.username == '') && event == 'showError') {
            this.userNameClass = 'form-control ng-pristine ng-invalid ng-touched';
            this.messageDriverIdValidation = this.txtDriverIsRequired;
            this.isValidUserName = false;
        } else if ((this.user.username.length < 4 || this.user.username.length > 64) && event == 'showError') {
            this.userNameClass = 'form-control ng-pristine ng-invalid ng-touched';
            this.messageDriverIdValidation = 'User ID should have between 4 and 64 characters';
            this.isValidUserName = false;
        }
        else if (this.hasWhiteSpace(this.user.username) && event == 'showError') {
            this.userNameClass = 'form-control ng-pristine ng-invalid ng-touched';
            this.messageDriverIdValidation = 'White spaces are not allowed';
            this.isValidUserName = false;
        }
        else if (this.hasInvalidCharacters(this.user.username) && event == 'showError') {
            this.userNameClass = 'form-control ng-pristine ng-invalid ng-touched';
            this.messageDriverIdValidation = 'User ID should start with letter or number';
            this.isValidUserName = false;
        }

        return this.isValidUserName;
    }

    public hasWhiteSpace(ele) {
        var has = false;
        var vsExprReg = /\s/g;
        var len = ele.replace(vsExprReg, "");
        if (ele.length != len.length)
            has = true;
        return has;
    }

    public hasInvalidCharacters(len) {
        var has = false;
        var vsExprReg = /^[a-z0-9\s.\-\&\/]+$/i;
        if (!vsExprReg.test(len))
            has = true;
        return has;
    }

    public validateFirstName(event: string) {
        this.firstNameClass = 'form-control input-control';
        this.isValidFirstName = true;
        if ((this.user.firstName == undefined || this.user.firstName == null || this.user.firstName == '') && event == 'showError') {
            this.firstNameClass = 'form-control ng-pristine ng-invalid ng-touched';
            this.isValidFirstName = false;
        }
        return this.isValidFirstName;
    }

    public validateLastName(event: string) {
        this.lastNameClass = 'form-control input-control';
        this.isValidLastName = true;
        if ((this.user.lastName == undefined || this.user.lastName == null || this.user.lastName == '') && event == 'showError') {
            this.lastNameClass = 'form-control ng-pristine ng-invalid ng-touched';
            this.isValidLastName = false;
        }
        return this.isValidLastName;
    }

    public validateType(event: string) {
        this.typeClass = 'form-control input-control';
        this.isValidType = true;
        if ((this.user.userType == undefined || this.user.userType == null || this.user.userType == '') && event == 'showError') {
            this.typeClass = 'form-control ng-pristine ng-invalid ng-touched';
            this.isValidType = false;
        }
        return this.isValidType;
    }

    public validateRole(event: string) {
        this.roleClass = 'form-control input-control';
        this.isValidRole = true;
        if (this.user.amigoTenantTRoleId == 0 && event == 'showError') {
            this.roleClass = 'form-control ng-pristine ng-invalid ng-touched';
            this.isValidRole = false;
        }
        return this.isValidRole;
    }

    public validatePassword(event: string) {
        this.passwordClass = 'form-control input-control';
        this.isValidPassword = true;
        if ((this.user.password == undefined || this.user.password == null || this.user.password == '') && event == 'showError') {
            this.passwordClass = 'form-control ng-pristine ng-invalid ng-touched';
            this.isValidPassword = false;
        }
        return this.isValidPassword;
    }

    public validateConfirmPassword(event: string) {
        this.messagePasswordNotEquals = '';
        this.confirmPasswordClass = 'form-control input-control';
        this.isValidConfirmPassword = true;
        if ((this.confirmPassword == undefined || this.confirmPassword == null || this.confirmPassword == '') && event == 'showError') {
            this.messagePasswordNotEquals = '';
            this.confirmPasswordClass = 'Confirm Password is required';
            this.isValidConfirmPassword = false;
        } else if (this.user.password != this.confirmPassword && this.confirmPassword != '') {
            this.messagePasswordNotEquals = "Password and Password Confirm should be the same";
            this.confirmPasswordClass = 'form-control ng-pristine ng-invalid ng-touched';
            this.isValidConfirmPassword = false;
        } else if (this.confirmPassword == '') {
            //this.confirmPasswordClass = 'form-control input-control'; this.isValidConfirmPassword = true;
        }
        return this.isValidConfirmPassword;
    }

    public validatePaidBy(event: string) {
        this.paidByClass = 'form-control input-control';
        this.isValidPaidBy = true;
        if ((this.user.payBy == undefined || this.user.payBy == null || this.user.payBy == '') && event == 'showError') {
            this.paidByClass = 'form-control ng-pristine ng-invalid ng-touched';
            this.isValidPaidBy = false;
        }
        return this.isValidPaidBy;
    }

}
