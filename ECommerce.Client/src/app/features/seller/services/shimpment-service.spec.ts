import { TestBed } from '@angular/core/testing';

import { ShimpmentService } from './shimpment-service';

describe('ShimpmentService', () => {
  let service: ShimpmentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShimpmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
