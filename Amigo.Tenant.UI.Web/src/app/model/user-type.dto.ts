export class UserType {
    id: string;
    code: string;
    name: string;
}

export class UserTypeList {
    public List: UserType[] = [
        //{ id: '', code: '', name: '-Select-' },
        { id: '1', code: '1', name: 'Driver' },
        { id: '2', code: '2', name: 'Internal' },
        { id: '3', code: '3', name: 'External' }
    ];
}