import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { BannerDto } from '../../../core/models/banner.model';
@Injectable({
  providedIn: 'root',
})
export class BannerService {

private apiUrl: string = 'http://localhost:5253/api/Admin/banners';

  constructor(private httpClient: HttpClient) { }

  allBnrs?: BannerDto[];

  getBnrs(): Observable<BannerDto[]> {
    return this.httpClient.get<BannerDto[]>(this.apiUrl)
  }

  createBnr(data: any): Observable<any> {
    return this.httpClient.post(this.apiUrl, data);
  }

  deleteBnr(Id: any): Observable<any> {
    return this.httpClient.delete(this.apiUrl.concat('/', Id));
  }

  updateBnr(Id:any,data: any): Observable<any> {
    return this.httpClient.put(this.apiUrl.concat('/', Id), data);
  }


}
