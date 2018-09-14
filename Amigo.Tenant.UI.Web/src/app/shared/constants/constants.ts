export class Constants {

    public static get MASTER_DATA_URL_PATH(): any {
        return {
            'getConceptsByTypeId': '/getConceptsByTypeId'
            };
    }

    public static get PERIOD_URL_PATH(): any {
        return {
            'getCurrentPeriod': 'Period/getCurrentPeriod'
        };
    }
}
