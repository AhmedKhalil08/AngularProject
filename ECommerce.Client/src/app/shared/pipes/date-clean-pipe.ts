import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateClean',
})
export class DateCleanPipe implements PipeTransform {
  transform(value: Date): any {
     const cleanedDate = new Date(value);

    const day = cleanedDate.getUTCDate().toString();
const month = (cleanedDate.getMonth() + 1).toString();
const year = cleanedDate.getFullYear().toString();
    return "".concat(day,'/',month,'/',year);
  }
}
