export class Constants {

    public static get MASTER_DATA_URL_PATH(): any {
        return {
            'getConceptByTypeIdList': 'concept/getConceptByTypeIdList',
            'getHouseTypes': 'house/getHouseTypes',
            'getHouseAll': 'house/searchForTypeAhead',
            'getGeneralTableByTableNameAll': 'generalTable/getGeneralTableByTableNameAll',
            'getBusinessPartnerByBPTypeCode': 'businessPartner/getBusinessPartnerByBPType'
            };
    }

    public static get PERIOD_URL_PATH(): any {
        return {
            'getCurrentPeriod': 'Period/getCurrentPeriod',
            'getPeriodLastestNumberPeriods': 'Period/getPeriodLastestNumberPeriods',
            'getYearsFromPeriods': 'Period/getYearsFromPeriods',
            'getPeriodsByYear': 'Period/getPeriodsByYear'
        };
    }

    public static get PAYMENTPERIOD_URL_PATH(): any {
        return {
            'exportToExcel': 'api/payment/exportToExcel'
        };
    }

    // public static get PAYMENTPERIOD_URL_PATH(): any {
    //     return {
    //         'getConceptsByTypeId': '/sendPaymentNotificationEmail',
    //         'getHouseTypes': 'house/getHouseTypes'
    //         };
    // }
}
