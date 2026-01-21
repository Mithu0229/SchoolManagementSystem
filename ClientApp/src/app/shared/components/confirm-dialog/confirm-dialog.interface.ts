export interface ConfirmDialogConfig {
    visible: boolean;
    title?: string;
    message: string;
    icon?: string;
    acceptLabel?: string;
    rejectLabel?: string;
    showReject?: boolean;
    type?: 'info' | 'warn' | 'error' | 'delete';
}
