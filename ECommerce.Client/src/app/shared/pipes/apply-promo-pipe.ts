import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'applyPromo',
})
export class ApplyPromoPipe implements PipeTransform {
  transform(value: unknown, ...args: unknown[]): unknown {
    return null;
  }
}
