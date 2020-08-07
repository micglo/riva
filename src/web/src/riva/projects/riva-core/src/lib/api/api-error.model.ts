import { ApiErrorDetail } from './api-error-detail.model';

export interface ApiError {
    type: string;
    instance: string;
    statusCode: number;
    errorCode: string;
    errorMessage: string;
    details: Array<ApiErrorDetail>;
}
