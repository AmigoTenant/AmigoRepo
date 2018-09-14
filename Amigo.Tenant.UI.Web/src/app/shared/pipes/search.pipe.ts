import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
    name: 'searchPipe',
    pure: false
})
export class SearchPipe implements PipeTransform {
    transform(data: any[], searchTerm: string): any[] {
        searchTerm = searchTerm.toUpperCase();
        return data.filter(item => {
                return item.activityTypeName.toUpperCase().indexOf(searchTerm) !== -1 && item.activityTypeName.toUpperCase().indexOf(searchTerm) !== -1;
        });
    }
}