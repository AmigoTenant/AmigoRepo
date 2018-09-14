export class UserPaidBy {
    code: string;
    name: string;
}

export class UserPaidByList {
    public List: UserPaidBy[] = [
        //{ code: '', name: 'All' },
        { code: 'M', name: 'Move' },
        { code: 'H', name: 'Hour' }
    ];
}