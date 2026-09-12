import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentPaidBillsComponent } from './student-paid-bills.component';

describe('StudentPaidBillsComponent', () => {
  let component: StudentPaidBillsComponent;
  let fixture: ComponentFixture<StudentPaidBillsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentPaidBillsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentPaidBillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
