export const environment = {
  production: true,
  serviceUrl: "http://amigotenantapplicationserviceswebapi.azurewebsites.net/",
  authenticationUrl: "http://amigotenantidentityserverwebapi.azurewebsites.net/",
  applicationId: "amigo.tenant.web",
  redirectUri: "http://amigotenant.azurewebsites.net/",
  logoutRedirectUri: "http://amigotenant.azurewebsites.net/",
  scopes: "openid profile email roles XST.Services",
  raygunApikey: "EfjFencSOl80YFmtcuzOzQ=)",
  raygunTag: "web",
  version: "1.0.0",
  deploymentEnvironment: "PROD",
  localization: {
        dateTimeFormat: "MM/DD/YYYY hh:mm",
        dateFormat: "MM/DD/YYYY",
        timeFormat: "HH:mm"
  },
  zoomMap: 14,
  zoomMarker: 17,
  latitude: 49.2,
  longitude: -122.8,
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
