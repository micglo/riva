import { AccountToken } from './account-token.model';

export interface MyAccount {
    id: string;
    email: string;
    confirmed: boolean;
    created: Date;
    passwordAssigned: boolean;
    lastLogin: Date | null;
    roles: Array<string>;
    tokens: Array<AccountToken>;
}
