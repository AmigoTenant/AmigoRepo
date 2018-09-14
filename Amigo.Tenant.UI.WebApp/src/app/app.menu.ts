import { MenuItem } from '../fw/services/menu.service';

export let initialMenuItems: Array<MenuItem> = [
    {
        text: 'Dashboard',
        icon: 'fa fa-dashboard',
        route: '/authenticated/dashboard',
        submenu: null
    },
    {
        text: 'Countries',
        icon: 'fa fa-flag',
        route: null,
        submenu: [
            {
                text: 'Select',
                icon: 'fa fa-expand',
                route: null,
                submenu: [
                    {
                        text: 'USA',
                        icon: 'fa fa-flag',
                        route: '/authenticated/country-detail/USA',
                        submenu: null
                    },
                    {
                        text: 'India',
                        icon: 'fa fa-flag',
                        route: '/authenticated/country-detail/India',
                        submenu: null
                    },
                    {
                        text: 'Switzerland',
                        icon: 'fa fa-flag',
                        route: '/authenticated/country-detail/Switzerland',
                        submenu: null
                    }
                ]
            },
            {
                text: 'Top 3',
                icon: 'fa fa-flag',
                route: '/authenticated/country-list/3',
                submenu: null
            },
            {
                text: 'Top 10',
                icon: 'fa fa-flag',
                route: '/authenticated/country-list/10',
                submenu: null
            },
            {
                text: 'All',
                icon: 'fa fa-flag',
                route: '/authenticated/country-list/0',
                submenu: null
            }
        ],
    },
    {
        text: 'Maintenance',
        icon: 'fa fa-wrench',
        route: null,
        submenu: [
            {
                text: 'Country List',
                icon: 'fa fa-th-list',
                route: '/authenticated/country-maint',
                submenu: null
            },
            {
                text: 'Settings',
                icon: 'fa fa-cog',
                route: '/authenticated/settings',
                submenu: null
            }
        ]
    }
];