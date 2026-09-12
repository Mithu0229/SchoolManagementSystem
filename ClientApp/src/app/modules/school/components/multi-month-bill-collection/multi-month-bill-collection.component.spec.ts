import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiMonthBillCollectionComponent } from './multi-month-bill-collection.component';

describe('MultiMonthBillCollectionComponent', () => {
  let component: MultiMonthBillCollectionComponent;
  let fixture: ComponentFixture<MultiMonthBillCollectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MultiMonthBillCollectionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MultiMonthBillCollectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
