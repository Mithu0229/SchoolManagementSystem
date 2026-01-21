import { Pipe, PipeTransform } from '@angular/core';
import dayjs from 'dayjs';

@Pipe({
    name: 'formatDate',
    standalone: true
})
export class FormatDatePipe implements PipeTransform {
    transform(value: string, type: 'date' | 'time' | 'datetime' = 'date'): string {
        if (!value) return '';

        switch (type) {
            case 'time':
                return dayjs(value).format('hh:mm A'); // or 'hh:mm A' for 12-hour format
            case 'datetime':
                return dayjs(value).format('MM/DD/YYYY HH:mm:ss'); // or 'MM/DD/YYYY hh:mm A'
            case 'date':
            default:
                return dayjs(value).format('MM/DD/YYYY');
        }
    }
}
