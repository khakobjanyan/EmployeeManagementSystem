import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmplyeeListComponent } from './emplyee-list.component';

describe('EmplyeeListComponent', () => {
  let component: EmplyeeListComponent;
  let fixture: ComponentFixture<EmplyeeListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EmplyeeListComponent]
    });
    fixture = TestBed.createComponent(EmplyeeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
