import { MenuItem } from '../fw/services/menu.service';

export let initialMenuItems: Array<MenuItem> = [
    {
        text: 'Maintenance',
        icon: 'fa fa-wrench',
        route: null,
        submenu: [
            {
                text: 'Module',
                icon: 'fa fa-th-list',
                route: '/authenticated/country-maint',
                submenu: null
            },
            {
                text: 'Role',
                icon: 'fa fa-cog',
                route: '/authenticated/settings',
                submenu: null
            },
            {
                text: 'User',
                icon: 'fa fa-cog',
                route: '/authenticated/settings',
                submenu: null
            }
        ]
    },
    {
        text: 'Amigo Tenant',
        icon: 'fa fa-wrench',
        route: null,
        submenu: [
            {
                text: 'Contract',
                icon: 'fa fa-th-list',
                route: '/authenticated/country-maint',
                submenu: null
            },
            {
                text: 'House',
                icon: 'fa fa-cog',
                route: '/authenticated/settings',
                submenu: null
            },
            {
                text: 'Payment',
                icon: 'fa fa-cog',
                route: '/authenticated/settings',
                submenu: null
            },
            {
                text: 'Tenant',
                icon: 'fa fa-cog',
                route: '/authenticated/settings',
                submenu: null
            },
            {
                text: 'Utilities',
                icon: 'fa fa-cog',
                route: '/authenticated/settings',
                submenu: null
            }
        ]
    },
    {
        text: 'Leasing',
        icon: 'fa fa-wrench',
        route: null,
        submenu: [
            {
                text: 'Rental Application',
                icon: 'fa fa-th-list',
                route: '/authenticated/country-maint',
                submenu: null
            }
        ]
    },
    {
        text: 'Utility Bill',
        icon: 'fa fa-wrench',
        route: null,
        submenu: [
            {
                text: 'Record Utility Bill',
                icon: 'fa fa-th-list',
                route: '/utilitybill/country-maint',
                submenu: null
            }
        ]
    }
];