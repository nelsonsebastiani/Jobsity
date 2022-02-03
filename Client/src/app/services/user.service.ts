import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as rxjs from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export default class UserService {
    readonly baseUrl: string = 'http://localhost:5000';
    private token: any;

    constructor(private http: HttpClient) {}

    public login(username: string, password: string): rxjs.Observable<any> {
        let dto = {UserName: username, Password: password};
        return this.http.post(`${this.baseUrl}/auth/login`, dto);
    }

    public signup(userDto: any): rxjs.Observable<any>{        
        return this.http.post(`${this.baseUrl}/auth/register`, userDto);
    }

    getUserToken(){
        return localStorage.getItem('accessToken');
    }

    setUserToken(token: string){
        localStorage.setItem('accessToken', token)
        this.token = token;
    }
}