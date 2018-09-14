export class Confirmation {
    code: string;
    name: string;
}

export class ConfirmationIntResult {
    code: boolean;
    name: string;
}

export class ConfirmationList {
    public List: Confirmation[] = [
        { code: null, name: 'All' },
        { code: 'Y', name: 'Yes' },
        { code: 'N', name: 'No' }
    ];

    public ListIntResult: ConfirmationIntResult[] = [
        { code: null, name: 'All' },
        { code: true, name: 'Yes' },
        { code: false, name: 'No' }
    ];
}