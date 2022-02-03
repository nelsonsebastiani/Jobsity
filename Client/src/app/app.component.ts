import { Component } from '@angular/core';  
  
@Component({  
  selector: 'app-root',  
  templateUrl: './app.component.html'
})  
export class AppComponent {  
  loggedIn: boolean;
  public userId: Number;
  
  constructor() { }
  
  toggleLoggedIn(){
    this.loggedIn = !this.loggedIn;
  }

  setUserId(event){
    this.userId = event;
  }
  
}  