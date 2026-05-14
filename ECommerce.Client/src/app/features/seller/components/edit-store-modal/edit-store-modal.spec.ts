import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditStoreModal } from './edit-store-modal';

describe('EditStoreModal', () => {
  let component: EditStoreModal;
  let fixture: ComponentFixture<EditStoreModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditStoreModal],
    }).compileComponents();

    fixture = TestBed.createComponent(EditStoreModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
