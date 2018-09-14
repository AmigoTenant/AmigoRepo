export class AmigoTenantTServiceApproveRequest
{
    reportDate: Date;
    driverId: number;
    currentTime: Date;
    approvedBy: string;
    amigoTenantTServiceIdsListStatus?: AmigoTenantTServiceStatus[];
}

export class AmigoTenantTServiceStatus
{
    amigoTenantTServiceId: string;
    serviceStatus?: boolean;
}