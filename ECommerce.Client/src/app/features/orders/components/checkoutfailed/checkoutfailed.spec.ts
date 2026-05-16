import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Checkoutfailed } from './checkoutfailed';

describe('Checkoutfailed', () => {
  let component: Checkoutfailed;
  let fixture: ComponentFixture<Checkoutfailed>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Checkoutfailed],
    }).compileComponents();

    fixture = TestBed.createComponent(Checkoutfailed);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
