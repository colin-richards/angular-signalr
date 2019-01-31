import { TestBed } from '@angular/core/testing';

import { RealTimeGameStatisticsService } from './real-time-game-stats.service';

describe('RealTimeGameStatisticsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RealTimeGameStatisticsService = TestBed.get(RealTimeGameStatisticsService);
    expect(service).toBeTruthy();
  });
});
