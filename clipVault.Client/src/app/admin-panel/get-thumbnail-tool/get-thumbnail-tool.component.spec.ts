import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetThumbnailToolComponent } from './get-thumbnail-tool.component';

describe('GetThumbnailToolComponent', () => {
  let component: GetThumbnailToolComponent;
  let fixture: ComponentFixture<GetThumbnailToolComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GetThumbnailToolComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetThumbnailToolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
