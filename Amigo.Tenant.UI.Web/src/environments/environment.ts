// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `angular-cli.json`.

export const environment = {
    production: false,
    serviceUrl: "http://127.0.0.1:8072/",
    authenticationUrl: "http://127.0.0.1:7071/",
    applicationId: "amigo.tenant.web",
    redirectUri: "http://127.0.0.1:8070",
    logoutRedirectUri: "http://127.0.0.1:8070",
    scopes: "openid profile email roles XST.Services",
    raygunApikey: "EfjFencSOl80YFmtcuzOzQ==",
    raygunTag: "web",
    version: "1.0.0-local",
    deploymentEnvironment: "DEV",
    localization: {
        dateTimeFormat: "MM/DD/YYYY hh:mm",
        dateFormat: "MM/DD/YYYY",
        timeFormat: "HH:mm"
    },
    zoomMap: 14,
    zoomMarker: 17,
    latitude: -12,
    longitude: -77,
    timeAutoRefresh: 300000,
    service: {
        Loader: {
            delay: "200",
            timeout: "16000"
        }
    },

    waUrlSvcEndPoint: "https://www.waboxapp.com/api/send/chat",
    waUserId: "51920132774",
    waApikey: "b176c6be32c235d185afd5f88bce02e359f6c18fbd73e"
};
