using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Common
{

    public class TupleList<T1, T2> : List<Tuple<T1, T2>>
    {
        public void Add(T1 item, T2 item2)
        {
            Add(new Tuple<T1, T2>(item, item2));
        }
    }

    public static class Constants
    {
        public struct RowStatus
        {
            public const int Active = 1;
            public const int Inactive = 0;
        }

        public struct PaidBy
        {
            public const string PerMove = "M";
            public const string PerHour = "H";
        }

        public struct RowStatusString
        {
            public const string Active = "1";
            public const string Inactive = "0";
        }

        public struct Entity
        {

            public struct AmigoTenantTUser
            {

                public struct ErrorMessage
                {
                    public const string UserExist = "User already exists";
                    public const string UserNoExist = "User does not exists";
                    public const string ClaimNoCreated = "Claim was not created";
                    public const string ClaimNoRemoved = "Claim was not removed";
                    public const string PasswordNotUpdated = "Password was not updated";
                    public const string UserNoCreatedInIdentity = "There is problem in identity server to register";
                }

            }


            public struct Common
            {

                public struct ErrorMessage
                {

                    public const string NonRegister = "Record was not Register";
                }

            }

            public struct AmigoTenantTRole
            {

                public struct ErrorMessage

                {

                    public const string RoleExist = "Role already exists";

                }

                public const string RolePrefix = "XST.";

            }

        }

        public struct Entities
        {
            public const string AmigoTenantTUser = "AmigoTenantTUser";

        }

        public struct ErrorMessages
        {

            public struct ApproveRejectProcess
            {
                public const string ApproveRejectNoEnougthInfo = "There is not enough information to process the services selected.";
                public const string MissingStartOrFinishWorkDay = "There is not Start WorkDay or Finish WorkDay activity, is not possible to continue with the process.";
                public const string MissingRate = "There is not Rate information to calculate Services by hour, is not possible to continue with the process.";
            }

            public struct Authorization
            {
                public const string ExpiredAccessToLogin = "Access has expired, please request a password reset.";
                public const string CurrentMobileDeviceDoesNotMatch = "Current device does not match with the assigned to you";
                public const string MoreThanOneDeviceAsociatedToYou = "Exists more than one devices asociated to you";
                public const string UserIsNotRegisterInDB = "User or device is not registered";
                public const string CellphoneNoAsociatedToOtherUser = "The Cellphone Number is associated to other user";
                public const string CellphoneNotAsociated = "The Cellphone Number is not associated";
                public const string UserDeviceCellphoneDifferentToIdentifierCellphone = "User has registered different Cellphone from the Identifier sent as parameter";
                public const string IdentifierWasAssignedMoreThanOneTime= "Identifier was assigned more than one time";
            }
        }

        public struct SuccessMessages
        {

            public struct ApproveRejectProcess
            {
                public const string ApproveRejectSuccessfully = "The processs was finished successfully!.";
            }

            public struct Common
            {
                public const string SaveSuccessfull = "The record was saved successfully!.";
            }
        }

        public struct Services
        {
            public const string AmigoTenantService = "AmigoTenantService";
            public const string IdentityService = "IdentityService";
        }

        public struct ServiceStatus
        {
            public const string Ongoing = "Ongoing";
            public const string Done = "Done";
            public const string Offline = "Offline";
            public const string Dispatched = "Dispatched";
        }

        public struct YesNo
        {
            public const string Yes = "Y";
            public const string No = "N";
        }

        public static TupleList<int?, string> ApprovalStatus = new TupleList<int?, string>
            {
                { 0, "Rejected" },
                { 1, "Approved" },
                { 2, "Pending" },
                { null, "Pending" }
            };


        public struct ActivityTypeCode
        {
            public const string ApproveDriverReport = "ADR";
            public const string EditbeforeApproval = "EFA";
            public const string Authentication = "AUT";

            public const string StartWorkday = "STW";
            public const string FinishWorkday = "FNW";
            public const string OnBreak = "ONB";
            public const string OnDuty = "OFB";
        }

        public struct ServiceCode
        {
            public const string DET = "DET"; //Detention
            public const string LFT = "LFT"; // Operate Lift;
        }

        public struct RateCode
        {
            public const string Hourly = "HOURLY"; //Hourly rate for dedicated services
        }

        public struct ChargeTypeCode
        {
            public const string Shipment = "S";
            public const string CostCenter = "C";
        }
        public struct AmigoTenantTEventLogType
        {
            public const string In = "OK";
            public const string Err = "ERR";
        }

        public struct EntityCode
        {
            public const string Contract = "CO";
            public const string Active = "ACTIVE";
            public const string Future = "FUTURE";
            public const string ContractDetail = "CD";
            public const string Concept = "CP";
            public const string PaymentPeriod = "PP";
            public const string HouseServicePeriodStatus = "HP";
            public const string Invoice = "IN";
        }


        public struct EntityStatus
        {
            public struct Contract
            {
                public const string Draft = "DRAFT";
                public const string Formalized = "FORMAL";
                public const string Expired = "EXPIRED";
                public const string Canceled = "CANCELED";
            }
            public struct ContractDetail
            {
                public const string Pending = "PENDING";
                public const string Payed = "PAYED";
            }
            public struct PaymentPeriod
            {
                public const string Pending = "PPPENDING";
                public const string Payed = "PPPAYED";
            }

            public struct PaymentPeriodStatusName
            {
                public const string Pending = "PENDING";
                public const string Payed = "PAYED";
            }

            public struct HouseServicePeriodStatus
            {
                public const string Draft = "DRAFT";
                public const string Registered = "REGISTERED";
            }
            public struct Invoice
            {
                public const string Pending = "INPENDING";
                public const string Payed = "INPAYED";
            }
        }

        public struct GeneralTableCode
        {
            public struct ConceptType
            {
                public const string Income = "INCOME";
                public const string Expense= "EXPENSE";
                public const string Penalty = "PENALTY";
                public const string Obligation = "OBLIGATION";
                public const string Rent = "RENT";
                public const string Deposit = "DEPOSIT";
            }

            public struct HouseType
            {
                public const string House = "HOUSE";
                public const string Apartment = "APARTMENT";
                public const string MiniApartment = "MINIAPART";
            }

            public struct PaymentType
            {
                public const string Rent = "RENT";
                public const string LateFee = "LATEFEE";
                public const string OnAccount = "ONACCOUNT";
                public const string Deposit = "DEPOSIT";
            }
        }

        public struct GeneralTableName
        {
            public const string PaymentType = "PaymentType";
            
        }

        public struct ConceptCode
        {
            public const string Rent = "RENT";
            public const string Deposit = "DEPOSIT";
            public const string LateFee = "LATEFEE";
            public const string Energy = "SVCENERGY";
            public const string Fine = "FINEINFRAC";
            public const string OnAccount = "ONACCOUNT";

        }
    }


}
