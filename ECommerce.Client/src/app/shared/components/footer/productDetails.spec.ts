import { ComponentFixture, TestBed } from '@angular/core/testing';

<<<<<<<< HEAD:ECommerce.Client/src/app/shared/components/footer/productDetails.spec.ts
import { productDetails } from './productDetails';

describe('ProductDeatils', () => {
  let component: productDetails;
  let fixture: ComponentFixture<productDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [productDetails],
    }).compileComponents();

    fixture = TestBed.createComponent(productDetails);
========
import { Footer } from './footer';

describe('Footer', () => {
  let component: Footer;
  let fixture: ComponentFixture<Footer>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Footer],
    }).compileComponents();

    fixture = TestBed.createComponent(Footer);
>>>>>>>> Dev:ECommerce.Client/src/app/shared/components/footer/footer.spec.ts
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
