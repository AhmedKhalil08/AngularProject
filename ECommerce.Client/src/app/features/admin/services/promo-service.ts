import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { IPromoCode} from '../../../core/models/ipromo-code';


@Injectable({
  providedIn: 'root',
})
export class PromoService {
  private apiUrl: string = 'http://localhost:5253/api/Admin/promocodes';

 constructor(private httpClient: HttpClient) { }

  allProms?: IPromoCode[];
  
  
  
    getPromos(): Observable<IPromoCode[]> {
      return this.httpClient.get<IPromoCode[]>(this.apiUrl)
    }
  
    createPromo(data: any): Observable<any> {
      return this.httpClient.post(this.apiUrl, data);
    }
  
    deletePromo(Id: any): Observable<any> {
      return this.httpClient.delete(this.apiUrl.concat('/', Id));
    }
  
    updatePromo(id:any,data: any): Observable<any> {
      return this.httpClient.put(this.apiUrl.concat('/id/', id), data);
    }
  


}
