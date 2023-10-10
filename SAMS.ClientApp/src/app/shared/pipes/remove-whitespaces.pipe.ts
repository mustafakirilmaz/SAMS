import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'removewhitespaces'
})

export class RemovewhitespacesPipe implements PipeTransform {
    transform(value: string, args?: any): string {
        if (value === null || value === undefined) {
            return '';
        }
        return value.replace(/ /g, '');
    }
}
