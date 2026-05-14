import { Component, OnInit, inject, AfterViewInit } from '@angular/core';
import { ICategory } from '../../../../core/models/icategory';
import { CommonModule, NgClass, NgStyle } from '@angular/common';
import { CategoryService } from '../../../../core/services/category-service'
import { HttpClient } from '@angular/common/http';
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
  selector: 'app-category',
  imports: [CommonModule, NgStyle, NgClass, FormsModule, NgbPagination, NgbInputDatepicker, NgbAlert, JsonPipe, NgbToast, DateCleanPipe],
  templateUrl: './category.html',
  styleUrl: './category.css',
  providers: [CategoryService]
})
export class Category {

public Catgs: ICategory[] = [];
public  ctgNames:string[]=[]
  private modalService = inject(NgbModal);

  constructor(public MyService: CategoryService) {
    this.refreshCatgs();
  }


  ngOnInit(): void {
    this.getCatgs();

  }

  getCatgs(): void {
    this.MyService.getCatgs().subscribe({
      next: (data) => {
        console.log(data);
        // this.PromoCodess = data;
        this.Catgs = [...data];
        console.log("promos are:")
        console.log(this.Catgs)
        this.ctgNames=this.Catgs.map(e=>e.name)
        console.log(this.ctgNames)


      },

      error: (err) => {
        console.error(err);
      }
    });

  }


  // ngOnInit(): void {
  //   while(!this.MyService.productApiResource.hasValue());

  //  console.log(this.MyService.productApiResource.value());


  // }
  page = 1;
  pageSize = 4;
  collectionSize = this.Catgs.length;
  catgs_arr: ICategory[] = [];

  refreshCatgs() {
    console.log(this.pageSize)
    this.catgs_arr = this.Catgs
      .map(({ id, ...p }, i) => ({ id: i + 1, ...p }))
      .slice(
        (this.page - 1) * this.pageSize,
        (this.page - 1) * this.pageSize + this.pageSize,
      );
  }
  toEdit: boolean = false;
  toSave: boolean = true;
  editedId: number = -1;

  
  public savedItem?: ICategory;
  public delItem?: ICategory;

  

  showDeleteToast: Boolean = false;

  public deleteCatg(deldid: number) {
    this.delItem = this.Catgs.find(e => e.id == deldid)
    if (!this.delItem) { console.log("item is already deleted"); return; }
    else {

      console.log(this.delItem);
      this.MyService.deleteCatg(deldid).subscribe(
        {
          next: (response) => {
            console.log(response);
            // this.getPromos();

            this.getCatgs();
            this.showDeleteToast = true;

          },
          error: (err) => {
            console.error(err);
          }
        }
      );
    }

  } //end of delete

itemToAdd: boolean = false;
  public createdCatg: ICategory = {
    "id": 0,
              "description":" ",
             "name":" ",
              "imageUrl":" "
  }

  showSaveToast: Boolean = false;

  public createCatg(newitem: any) {

    //validating code
    if ((newitem.name).length < 3||(newitem.imageUrl).length==0) {
      console.log(newitem);
      
      alert("enter valid data"); return; }

    
    else {
      console.log(newitem);
      this.MyService.createCatg(newitem).subscribe(
        {
           
          next: (response) => {
            console.log(response);
            // this.getPromos();

            this.getCatgs();
            this.showSaveToast = true;
            this.createdCatg = {
              "id": 0,
               "name":" ",
              "description":" ",
              "imageUrl":" "
            };


          },
          error: (err) => {
            console.error(err);
          }
        }
      );
    }
  } //end of creation function


}//end of cat comp. class
