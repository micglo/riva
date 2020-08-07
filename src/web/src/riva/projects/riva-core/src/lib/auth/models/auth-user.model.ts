import { AuthRole } from './../enums/auth-role.enum';

export interface AuthUser {
    id: string;
    email: string;
    confirmed: boolean;
    roles: Array<AuthRole>;
}
