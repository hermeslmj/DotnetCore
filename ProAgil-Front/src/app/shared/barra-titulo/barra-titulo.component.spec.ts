/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { BarraTituloComponent } from './barra-titulo.component';

describe('BarraTituloComponent', () => {
  let component: BarraTituloComponent;
  let fixture: ComponentFixture<BarraTituloComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BarraTituloComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BarraTituloComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
