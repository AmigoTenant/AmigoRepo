import {RouterStateSnapshot, ActivatedRouteSnapshot,  CanActivateChild,   CanActivate} from '@angular/router';
import { Injectable } from '@angular/core';
import { SecurityService } from '../security/security.service';

@Injectable()
export class LoginRouteGuard implements CanActivate, CanActivateChild {

  constructor(private securityService: SecurityService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let isAuthorized = this.securityService.IsAuthorized;

    if (isAuthorized)
      return true;
    else
      this.securityService.Authorize();
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let isAuthorized = this.securityService.IsAuthorized;

    if (isAuthorized)
      return true;
    else
      this.securityService.Authorize();
  }
}