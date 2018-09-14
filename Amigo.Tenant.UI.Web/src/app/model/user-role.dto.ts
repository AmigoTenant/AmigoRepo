export class UserRole {
    code: number;
    name: string;
}

export class UserRoleList {
    public List: UserRole[] = [
        { code: null, name: 'All' },
        { code: 1, name: 'James.Romero' },
        { code: 2, name: 'string' },
        { code: 4, name: 'Test3' },
        { code: 5, name: 'Test55551' },
        { code: 6, name: 'Test55551' },
        { code: 7, name: 'Reporter' },
        { code: 8, name: 'Reporter1' },
        { code: 9, name: 'Reporter' },
        { code: 10, name: '111111' },
        { code: 11, name: '12' },
        { code: 12, name: 'Test DomainStatus' },
        { code: 13, name: 'Root' }
    ];
}