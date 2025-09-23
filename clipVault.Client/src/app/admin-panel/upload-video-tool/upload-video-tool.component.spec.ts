import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadVideoToolComponent } from './upload-video-tool.component';

describe('UploadVideoToolComponent', () => {
  let component: UploadVideoToolComponent;
  let fixture: ComponentFixture<UploadVideoToolComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UploadVideoToolComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UploadVideoToolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
