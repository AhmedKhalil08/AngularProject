import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductDeatils } from './product-deatils';

describe('ProductDeatils', () => {
  let component: ProductDeatils;
  let fixture: ComponentFixture<ProductDeatils>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductDeatils],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductDeatils);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
