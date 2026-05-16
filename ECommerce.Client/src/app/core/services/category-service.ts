import { Injectable } from '@angular/core';
import { ICategory } from '../models/icategory';
//import { httpResource } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root',
})
export class CategoryService {

  private apiUrl: string = 'http://localhost:5253/api/Category';

  constructor(private httpClient: HttpClient) { }

  allCatgs?: ICategory[];

  getCatgs(): Observable<ICategory[]> {
    return this.httpClient.get<ICategory[]>(this.apiUrl)
  }

  createCatg(data: any): Observable<any> {
    return this.httpClient.post(this.apiUrl, data);
  }

  deleteCatg(Id: any): Observable<any> {
    return this.httpClient.delete(this.apiUrl.concat('/', Id));
  }

  updateCatg(id:any,data: any): Observable<any> {
    return this.httpClient.post(this.apiUrl.concat('/id/', id), data);
  }



}
