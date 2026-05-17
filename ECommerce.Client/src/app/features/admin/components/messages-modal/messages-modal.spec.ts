import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessagesModal } from './messages-modal';

describe('MessagesModal', () => {
  let component: MessagesModal;
  let fixture: ComponentFixture<MessagesModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MessagesModal],
    }).compileComponents();

    fixture = TestBed.createComponent(MessagesModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
