import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetCategoriesToolComponent } from './get-categories-tool.component';

describe('GetCategoriesToolComponent', () => {
  let component: GetCategoriesToolComponent;
  let fixture: ComponentFixture<GetCategoriesToolComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GetCategoriesToolComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetCategoriesToolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
