export class IUtilityHouseServiceDTO {
    public houseServiceId: number | null;
    public houseId: number | null;
    public serviceId: number | null;
    public conceptId: number | null;
    public conceptCode: string | null;
    public conceptDescription: string | null;
    public conceptTypeId: number | null;
    public businessPartnerId: number | null;
    public businessPartnerName: string | null;
    public serviceTypeId: number | null;
    public serviceTypeCode: string | null;
    public serviceTypeValue: string | null;

    public rowStatus: boolean | null;
    public createdBy: number | null;
    public creationDate: Date | null;
     
}