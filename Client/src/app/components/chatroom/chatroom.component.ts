import { Component, NgZone, Input, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import Message from '../../models/message';
import User from '../../models/user';
import { ChatService } from '../../services/chat.service';
import UserService from '../../services/user.service';
import { map } from 'rxjs/operators'

@Component({
  selector: 'app-chatroom',
  styleUrls: ['./chatroom.component.scss'],
  templateUrl: './chatroom.component.html'
})
export class ChatRoomComponent {
 
  message: Message;
  messages = new Array<Message>();
  textValue: string = '';
  user: User; 
  data: any;

  constructor(
    private http: HttpClient, 
    private chatService: ChatService,
    private userService: UserService,
    private _el: ElementRef,
    private ngZone: NgZone) { 
      this.subscribeToEvents(); 
      this.getCurremtUser();
      this.chatService.getMesagges().subscribe((resp: Array<any>) => {
        return resp.map((msg) => {
          this.messages.push({
            Type: msg.type,
            Text: msg.text,
            Date: msg.date,
            UserId: msg.userId,
            User: msg.user
          });
        });
      });
      setTimeout(() => this.setScrollbar(), 500);      
    }

    setScrollbar() {
      const el: HTMLDivElement = this._el.nativeElement;
      el.scrollTop = Math.max(0, el.scrollHeight - el.offsetHeight);
    }

    getCurremtUser(): void {
      this.http.get(`http://localhost:5000/users/current`)
          .subscribe((response: any) => {
            let usr = {
              Id: response.id,
              UserName: response.userName,
              Name: response.name,
              Email: response.email
            };
            this.user = usr;
          });
    }

    sendMessage(): void {  
      if (this.textValue) {  
        this.message = new Message();  
        this.message.UserId = this.user.Id;
        this.message.Type = "sent";  
        this.message.Text = this.textValue;  
        this.message.Date = new Date();
              
        this.chatService.sendMessage(this.message);
        this.message = null;
        this.textValue = '';  
      }  
      this.setScrollbar();  
    } 

    private subscribeToEvents(): void {  
      this.chatService.messageReceived.subscribe((receivedMessage: any) => {
        this.ngZone.run(() => {
          let msg = {
            Type: 'received',
            Text: receivedMessage.text,
            Date: receivedMessage.date,
            User: receivedMessage.user,
            UserId: receivedMessage.userId
          }
          this.messages.push(msg);  
        });  
      });  
    }  

}
