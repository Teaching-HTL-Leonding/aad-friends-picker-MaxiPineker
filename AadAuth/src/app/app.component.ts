import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { AuthenticationResult } from '@azure/msal-browser';
import * as MicrosoftGraph from "@microsoft/microsoft-graph-types"

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html', 
  styles: []
})
export class AppComponent implements OnInit{
  loggedIn = false;
  profile?: MicrosoftGraph.User;
  users?: MicrosoftGraph.User[];
  userNameFilter = '';
  friends?: string[]; 

  constructor(private authService: MsalService, private client: HttpClient) {}

  ngOnInit(): void {
    this.checkAccount();
  }

  checkAccount() {
    this.loggedIn = this.authService.instance.getAllAccounts().length > 0;
  }

  login() {
    this.authService
      .loginPopup()
      .subscribe((response: AuthenticationResult) => {
        this.authService.instance.setActiveAccount(response.account);
        this.checkAccount();
      });
  }
  logout(){
    this.authService.logout();
  }
  getProfile(){
     this.client.get('https://graph.microsoft.com/v1.0/me/').subscribe(profile => this.profile = profile)
  }

  getFriends(){
    this.client.get<any>('https://localhost:5001/api/friends').subscribe(friends => this.friends = friends)
 }

  getUsers() {
    let params = new HttpParams().set("$top", "10");
    if (this.userNameFilter) {
      params = params.set(
        "$filter",
        `startsWith(displayName, '${this.userNameFilter}')`
      );
    }
    let url = `https://graph.microsoft.com/v1.0/users?${params.toString()}`;
    this.client
      .get<any>(url)
      .subscribe((users) => (this.users = users.value));
  }
}
