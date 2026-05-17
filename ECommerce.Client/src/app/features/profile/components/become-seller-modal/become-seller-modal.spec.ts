import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BecomeSellerModal } from './become-seller-modal';

describe('BecomeSellerModal', () => {
  let component: BecomeSellerModal;
  let fixture: ComponentFixture<BecomeSellerModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BecomeSellerModal],
    }).compileComponents();

    fixture = TestBed.createComponent(BecomeSellerModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
