import { Injectable } from '@angular/core';
import { ICategory } from '../models/Icategory';
//import { httpResource } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environment/environment';


@Injectable({
  providedIn: 'root',
})
export class CategoryService {

  private apiUrl: string = environment.apiUrl.concat('/Category');
  

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
