import { Component, OnInit, inject, AfterViewInit } from '@angular/core';
import { BannerDto } from '../../../../core/models/banner.model';
import { CommonModule, NgClass, NgStyle } from '@angular/common';
import { BannerService } from '../../../home/services/banner-service'
import { FormsModule } from '@angular/forms';
import { NgbPagination } from '@ng-bootstrap/ng-bootstrap/pagination';
import { flip } from '@popperjs/core';
import { NgbAlert } from '@ng-bootstrap/ng-bootstrap/alert';
import { NgbInputDatepicker, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap/datepicker';
import { JsonPipe } from '@angular/common';
import { NgbToast } from '@ng-bootstrap/ng-bootstrap/toast';
import { DateCleanPipe } from '../../../../shared/pipes/date-clean-pipe'
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap/modal';


@Component({
  selector: 'app-banner',
  imports: [
    CommonModule,
    NgStyle,
    NgClass,
    FormsModule,
    NgbPagination,
    NgbInputDatepicker,
    NgbAlert,
    NgbToast,
    DateCleanPipe
  ],
  templateUrl: './banner.html',
  styleUrl: './banner.css',
  providers: [BannerService]
})
export class Banner {
  constructor(public MyService: BannerService) { }
  public Bnrs: BannerDto[] = [];

  ngOnInit(): void { this.getBnrs(); }

  ////Read////
  getBnrs(): void {
    this.MyService.getBnrs().subscribe({
      next: (data) => {
        console.log(data);
        this.Bnrs = [...data];
        console.log("Banners are:")
        console.log(this.Bnrs)
      },
      error: (err) => {
        console.error(err);
      }
    }); //End of function
  }
  //End of Read//


  /// Delete///
  showDeleteToast: Boolean = false;

  public delItem?: BannerDto;
  public deleteBnr(deldid: number) {
    this.delItem = this.Bnrs.find(e => e.id == deldid)
    if (!this.delItem) { console.log("item is already deleted"); return; }
    else {

      console.log(this.delItem);
      this.MyService.deleteBnr(deldid).subscribe(
        {
          next: (response) => {
            console.log(response);
            // this.getPromos();

            this.getBnrs();
            this.showDeleteToast = true;

          },
          error: (err) => {
            console.error(err);
          }
        }
      );
    }

  } //End of function
  //end of delete//

  //Create//
  itemToAdd: boolean = false;
  public createdBnr: BannerDto = {
    "id": 0,

    "title": "",
    "imageUrl": " ",
    "link": " ",
    "isActive": false,
    "displayOrder": 0,
  }

  showSaveToast: Boolean = false;

  public createBnr(newitem: any) {

    //validating code
    if ((newitem.title).length < 3 || (newitem.imageUrl).length == 0) {
      console.log(newitem);

      alert("enter valid data"); return;
    }


    else {
      console.log(newitem);
      this.MyService.createBnr(newitem).subscribe(
        {

          next: (response) => {
            console.log(response);

            this.getBnrs();
            this.showSaveToast = true;
            this.createdBnr = {
              "id": 0,

              "title": "",
              "imageUrl": " ",
              "link": " ",
              "isActive": false,
              "displayOrder": 0,
            };


          },
          error: (err) => {
            console.error(err);
          }
        }
      );
    }
  } //end of creation function


  //End of Create//

  //Update//

//End of Update//


}
