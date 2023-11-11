import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';

import { ButtonModule, CardModule, CollapseModule, GridModule } from '@coreui/angular';
import { IconSetService } from '@coreui/icons-angular';
import { iconSubset } from '../../../icons/icon-subset';
import { DocsComponentsModule } from '../../../../components';
import { CartsComponent } from './carts.component';

describe('CartsComponent', () => {
  let component: CartsComponent;
  let fixture: ComponentFixture<CartsComponent>;
  let iconSetService: IconSetService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CartsComponent],
      imports: [CardModule, CollapseModule, NoopAnimationsModule, GridModule, ButtonModule, DocsComponentsModule, RouterTestingModule],
      providers: [IconSetService]
    })
      .compileComponents();
  });

  beforeEach(() => {
    iconSetService = TestBed.inject(IconSetService);
    iconSetService.icons = { ...iconSubset };

    fixture = TestBed.createComponent(CartsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
