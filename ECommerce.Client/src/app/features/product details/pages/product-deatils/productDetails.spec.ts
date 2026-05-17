import { ComponentFixture, TestBed } from '@angular/core/testing';

import { productDetails } from './productDetails';

describe('ProductDeatils', () => {
  let component: productDetails;
  let fixture: ComponentFixture<productDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [productDetails],
    }).compileComponents();

    fixture = TestBed.createComponent(productDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
