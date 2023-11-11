import { INavData } from '@coreui/angular';

export const navItems_dealer: INavData[] = [
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
    name: 'Supplier',
    url: 'supplier',
    iconComponent: { name: 'cil-briefcase' }
  },
  {
    name: 'Products',
    url: 'products',
    iconComponent: { name: 'cil-square' }
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