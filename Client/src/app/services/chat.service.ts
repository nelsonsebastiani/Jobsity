import { EventEmitter, Injectable } from '@angular/core';  
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';  
import Message from '../models/message';  
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
  
@Injectable({
  providedIn: 'root'
})
export class ChatService {  
  messageReceived = new EventEmitter<Message>();  
  connectionEstablished = new EventEmitter<Boolean>();  
  
  private connectionIsEstablished = false;  
  private _hubConnection: HubConnection;  
  private baseUrl: string = 'http://localhost:5000';
  
  constructor(private http: HttpClient) {  
    this.createConnection();  
    this.registerOnServerEvents();  
    this.startConnection();  
  }  
  
  sendMessage(message: Message) {  
    this._hubConnection.invoke('NewMessage', message);
    if(!message.Text.startsWith("/stock="))
      this.addMessage(message).subscribe();
  }  
  
  private createConnection() {  
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/MessageHub`)
      .build();
  }  
  
  private startConnection(): void {  
    this._hubConnection  
      .start()  
      .then(() => {  
        this.connectionIsEstablished = true;  
        console.log('Hub connection started');
        this.connectionEstablished.emit(true);
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        setTimeout(function () { this.startConnection(); }, 5000);
      });
  }
  
  private registerOnServerEvents(): void {
    this._hubConnection.on('MessageReceived', (data: any) => {
      this.messageReceived.emit(data);
    });  
  }  

  private addMessage(msg: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/message`, msg);
  }

  public getMesagges(): Observable<any> {
    return this.http.get(`${this.baseUrl}/message`);
  }
}