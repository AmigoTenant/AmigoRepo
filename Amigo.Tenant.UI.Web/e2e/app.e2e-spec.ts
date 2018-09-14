import { AmigoTenantPage } from './app.po';

describe('amigo-tenant App', function() {
    let page: AmigoTenantPage;

  beforeEach(() => {
      page = new AmigoTenantPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
