export class Constants {

    public static get MASTER_DATA_URL_PATH(): any {
        return {
            'getConceptsByTypeId': '/getConceptsByTypeId',
            'getHouseTypes': 'house/getHouseTypes'
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
}
