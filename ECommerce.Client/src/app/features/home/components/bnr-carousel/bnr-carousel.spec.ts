import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BnrCarousel } from './bnr-carousel';

describe('BnrCarousel', () => {
  let component: BnrCarousel;
  let fixture: ComponentFixture<BnrCarousel>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BnrCarousel],
    }).compileComponents();

    fixture = TestBed.createComponent(BnrCarousel);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
