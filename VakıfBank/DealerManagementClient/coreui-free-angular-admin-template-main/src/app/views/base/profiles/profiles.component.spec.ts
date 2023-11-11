import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

import { BadgeModule, ButtonModule, CardModule, FormModule, GridModule, ListGroupModule } from '@coreui/angular';
import { IconSetService } from '@coreui/icons-angular';
import { iconSubset } from '../../../icons/icon-subset';
import { DocsComponentsModule } from '../../../../components';
import { ProfilesComponent } from './profiles.component';

describe('ProfilesComponent', () => {
  let component: ProfilesComponent;
  let fixture: ComponentFixture<ProfilesComponent>;
  let iconSetService: IconSetService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProfilesComponent],
      imports: [ListGroupModule, ButtonModule, ReactiveFormsModule, BadgeModule, FormModule, GridModule, CardModule, DocsComponentsModule, RouterTestingModule],
      providers: [IconSetService]
    })
      .compileComponents();
  });

  beforeEach(() => {
    iconSetService = TestBed.inject(IconSetService);
    iconSetService.icons = { ...iconSubset };

    fixture = TestBed.createComponent(ProfilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
