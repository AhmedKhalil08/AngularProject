import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { BannerDto} from '../../../core/models/banner.model';
import { environment } from '../../../environment/environment'; 


@Injectable({
  providedIn: 'root',
})
export class BannerService {

 private apiUrl: string = environment.apiUrl.concat('/Admin/banners');
  
 

 constructor(private httpClient: HttpClient) { }

  allBnrs?: BannerDto[];
  
  
  
    getBnrs(): Observable<BannerDto[]> {
      return this.httpClient.get<BannerDto[]>(this.apiUrl)
    }
  
    createBnr(data: any): Observable<any> {
      return this.httpClient.post(this.apiUrl, data);
    }
  
    deleteBnr(Id: any): Observable<any> {
      console.log("deleted");
      return this.httpClient.delete(this.apiUrl.concat('/', Id));
      

    }
  
    updateBnr(id:any,data: any): Observable<any> {
      return this.httpClient.put(this.apiUrl.concat('/', id), data);
    }
  

}
