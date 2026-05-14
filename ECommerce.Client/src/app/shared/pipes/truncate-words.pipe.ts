import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncateWords',
  standalone: true,
})
export class TruncateWordsPipe implements PipeTransform {
  transform(value: string | null | undefined, wordCount: number = 4): string {
    if (!value) {
      return '';
    }

    const words = value.trim().split(/\s+/);

    if (words.length <= wordCount) {
      return value;
    }

    return words.slice(0, wordCount).join(' ') + '...';
  }
}
