import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LinkEditorComponent } from './link-editor.component';

describe('LinkEditorComponent', () => {
  let component: LinkEditorComponent;
  let fixture: ComponentFixture<LinkEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LinkEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LinkEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});