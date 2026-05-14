import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderCheckOut } from './order-check-out';

describe('OrderCheckOut', () => {
  let component: OrderCheckOut;
  let fixture: ComponentFixture<OrderCheckOut>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderCheckOut],
    }).compileComponents();

    fixture = TestBed.createComponent(OrderCheckOut);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
