import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiService {

  constructor(private http:HttpClient){  }
  private baseUrl = environment.apiUrl;

  get<T>(endpoint:string):Observable<T>{
    return this.http.get<T>(`${this.baseUrl}/${endpoint}`);
  }
  post<T>(endpoint:string,body:any):Observable<T>{
    return this.http.post<T>(`${this.baseUrl}/${endpoint}`, body);
  }
  put<T>(endpoint: string, body: any): Observable<T> {
  return this.http.put<T>(`${this.baseUrl}/${endpoint}`, body);
}
delete<T>(endpoint: string): Observable<T> {
  return this.http.delete<T>(`${this.baseUrl}/${endpoint}`);
}
}
