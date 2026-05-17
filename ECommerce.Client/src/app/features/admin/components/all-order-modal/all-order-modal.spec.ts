import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllOrderModal } from './all-order-modal';

describe('AllOrderModal', () => {
  let component: AllOrderModal;
  let fixture: ComponentFixture<AllOrderModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllOrderModal],
    }).compileComponents();

    fixture = TestBed.createComponent(AllOrderModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
