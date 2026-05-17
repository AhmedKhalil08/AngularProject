import { Component, EventEmitter, Input, Output, output } from '@angular/core';
import { UserDto } from '../../../../core/models/auth.model';

@Component({
  selector: 'app-customer-card',
  imports: [],
  templateUrl: './customer-card.html',
  styleUrl: './customer-card.css',
})
export class CustomerCard {
  @Input() customer !:UserDto;
  @Output() onToggleStatus = new EventEmitter<UserDto>();
  @Output() onDelete = new EventEmitter<UserDto>();
  @Output() onRestore = new EventEmitter<UserDto>();
  getInitials(name:string):string{
     return name.split(' ').map(n => n[0]).join('').substring(0, 2).toUpperCase();
  }

    getAvatarColor(id: string): string {
    const colors = ['#00f0ff', '#7b2fff', '#ff2f7b', '#2fff7b', '#ff7b2f'];
    const index = id.charCodeAt(0) % colors.length;
    return colors[index];
  }
}
