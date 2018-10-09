export class Constants {

    public static get MASTER_DATA_URL_PATH(): any {
        return {
            'getConceptByTypeIdList': 'concept/getConceptByTypeIdList',
            'getHouseTypes': 'house/getHouseTypes',
            'getGeneralTableByTableNameAll': 'generalTable/getGeneralTableByTableNameAll',
            };
    }

    public static get PERIOD_URL_PATH(): any {
        return {
            'getCurrentPeriod': 'Period/getCurrentPeriod'
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
