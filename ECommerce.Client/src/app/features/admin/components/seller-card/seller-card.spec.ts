import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerCard } from './seller-card';

describe('SellerCard', () => {
  let component: SellerCard;
  let fixture: ComponentFixture<SellerCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerCard],
    }).compileComponents();

    fixture = TestBed.createComponent(SellerCard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
