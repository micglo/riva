import { AccountTokenType } from './../enums/account-token-type.enum';

export interface AccountToken {
    issued: Date;
    expires: Date;
    type: AccountTokenType;
    value: string;
}
