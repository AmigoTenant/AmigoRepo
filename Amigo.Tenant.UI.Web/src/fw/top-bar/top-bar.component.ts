import { Component, OnInit } from '@angular/core';

import { FrameworkConfigService } from '../services/framework-config.service';
import { UserApi } from '../users/user-api';

@Component({
  selector: 'fw-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent implements OnInit {

  constructor(public frameworkConfigService: FrameworkConfigService,
    public userApi: UserApi) { }

  ngOnInit() {
  }

  signOut() {
    this.userApi.signOut();
  }
}
