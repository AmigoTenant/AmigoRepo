import { Amigo.Tenant.UI.WebAppPage } from './app.po';

describe('amigo.tenant.ui.web-app App', () => {
  let page: Amigo.Tenant.UI.WebAppPage;

  beforeEach(() => {
    page = new Amigo.Tenant.UI.WebAppPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
