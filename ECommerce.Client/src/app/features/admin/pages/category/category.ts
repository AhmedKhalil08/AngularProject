import { Component, OnInit, inject, AfterViewInit } from '@angular/core';
import { ICategory} from '../../../../core/models/Icategory';
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
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap/modal';

@Component({
  selector: 'app-category',
  imports: [
    CommonModule,
    NgStyle,
    NgClass,
    FormsModule,
    NgbPagination,
    NgbInputDatepicker,
    NgbAlert,
    NgbToast,
    ],
  templateUrl: './category.html',
  styleUrl: './category.css',
  providers: [CategoryService]
})
export class Category {

  public Catgs: ICategory[] = [];
  public ctgNames: string[] = []
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
        console.log("catgs are:")
        //this.showSaveToast = true;

        console.log(this.Catgs)
        this.ctgNames = this.Catgs.map(e => e.name)
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
  pageSize = 8;
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
    "description": " ",
    "name": " ",
    "imageUrl": " "
  }

  showSaveToast: Boolean = false;

  public createCatg(newitem: any) {

    //validating code
    if ((newitem.name).length < 3 || (newitem.imageUrl).length == 0) {
      console.log(newitem);

      alert("enter valid data"); return;
    }
   else if(this.ctgNames.includes(newitem.name)==true){ alert("this category already exists!"); return;}

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
              "name": " ",
              "description": " ",
              "imageUrl": " "
            };


          },
          error: (err) => {
            console.error(err);
          }
        }
      );
    }
  } //end of creation function

  itemToUpdate: boolean = false;
  UpdatedId: number = 0;
  public updatedCatg: ICategory = {
    "id": 0,
    "description": " ",
    "name": "lol ",
    "imageUrl": " "
  }

  //Update//
  updateCatg(id: any, item: any) {
    this.createdCatg = {
      "id": item.id,
      "name": item.name,
      "description": item.description,
      "imageUrl": item.imageUrl
    };
    console.log(item);


    if ((item.name).length < 3 || (item.imageUrl).length == 0) {
      console.log(item);

      alert("enter valid data"); return;
    }


    else {
      console.log(item);


      this.MyService.updateCatg(id, this.createdCatg).subscribe(
        {

          next: (response) => {
            console.log("updated");
            console.log(response);
            // this.getPromos();

            this.getCatgs();
            this.showSaveToast = true;
            this.createdCatg = {
              "id": 0,
              "name": " ",
              "description": " ",
              "imageUrl": " "
            };


          },
          error: (err) => {
            console.error(err);
          }
        }
      );
    }
  }

  openModal(id: number) {
    const modalRef = this.modalService.open(NgbdModalConfirm);
    modalRef.result.then((result) => {
      console.log('Result:', result);
      this.deleteCatg(id);
    }).catch((error) => {
      // Handle dismissal
    });
  }


}//end of cat comp. class

@Component({
  selector: 'ngbd-modal-confirm',
  template: `
		<div class="modal-header">
			<h4 class="modal-title" id="modal-title">Category deletion</h4>
			<button
				type="button"
				class="btn-close"
				aria-describedby="modal-title"
				(click)="modal.dismiss('Cross click')"
			></button>
		</div>
		<div class="modal-body">
			<p>
				<strong>Are you sure you want to delete this category?</strong>
			</p>
			<p>
				<span class="text-danger">This operation can not be undone.</span>
			</p>
		</div>
		<div class="modal-footer">
			<button type="button" class="btn btn-outline-secondary" (click)="modal.dismiss('cancel click')">Cancel</button>
			<button type="button" class="btn btn-danger" (click)="modal.close('1')">Ok</button>
		</div>
	`,
})
export class NgbdModalConfirm {

  modal = inject(NgbActiveModal);
  constructor(public activeModal: NgbActiveModal) { }
  public response: boolean = false;

  passBack() {
    this.activeModal.close(this.response);
  }
}
