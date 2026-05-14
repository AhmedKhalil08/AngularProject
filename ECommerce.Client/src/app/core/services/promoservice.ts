import { Injectable } from '@angular/core';
import { IPromoCode } from '../models/IPromoCode';
//import { httpResource } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root',
})
export class PromoService {
  // productApiResource = httpResource<IPromoCode[]>(() => ({
  //   url: `http://localhost:5253/api/Admin/promocodes`,
  //   method: 'GET'
  // }));


 private apiUrl:string = 'http://localhost:5253/api/Admin/promocodes';

  constructor(private httpClient: HttpClient) {
    
  }

  // getPatents(): Observable<IPromoCode[]> {
  //   return this.httpClient.get<IPromoCode[]>(this.apiUrl);
  // }
  allPromos?:IPromoCode[];
   getPromos(): Observable<IPromoCode[]> {
    return this.httpClient.get<IPromoCode[]>(this.apiUrl)
   }

  //  getPromoById(id:number):Observable<IPromoCode[]> {
  //   return this.httpClient.get<IPromoCode[]>(this.apiUrl.concat(this.apiUrl,'/',id.toString()))
  //  }
  createPromo(data: any): Observable<any> {
    return this.httpClient.post(this.apiUrl, data);
  }

  deletePromo(Id: any): Observable<any> {
    return this.httpClient.delete(this.apiUrl.concat('/',Id));
  }
}
