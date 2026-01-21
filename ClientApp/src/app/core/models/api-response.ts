export interface ApiResponse<T = any> {
    isSuccess: boolean;
    statusCode: number;
    data: T | null;
    errors: string[];
    notificationMessage: string;
}