import { INavData } from '@coreui/angular';

export const navItems_admin: INavData[] = [
  /*
  {
    name: 'COMPANY NAME',
    url: '/dashboard',
  },*/
  {
    name: 'Company Profile',
    url: 'company-profile',
    iconComponent: { name: 'cil-building' }
  },
  {
    name: 'Components',
    title: true
  },
  {
    name: 'Dealers',
    url: 'dealers',
    iconComponent: { name: 'cil-briefcase' }
  },
  {
    name: 'Orders',
    url: 'orders',
    iconComponent: { name: 'cil-clipboard' }
  },
  {
    name: 'Stocks',
    url: 'stocks',
    iconComponent: { name: 'cil-tag' }
  },
];