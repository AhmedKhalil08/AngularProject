import { Component, OnInit, inject, AfterViewInit } from '@angular/core';
import { IPromoCode } from '../../../../core/models/ipromo-code';
import { CommonModule, NgClass, NgStyle } from '@angular/common';
import { PromoService } from '../../services/promo-service'
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgbPagination } from '@ng-bootstrap/ng-bootstrap/pagination';
import { flip } from '@popperjs/core';
import { NgbAlert } from '@ng-bootstrap/ng-bootstrap/alert';
import { NgbInputDatepicker, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap/datepicker';
import { JsonPipe } from '@angular/common';
import { NgbToast } from '@ng-bootstrap/ng-bootstrap/toast';
// import { DateCleanPipe } from '../../../../shared/pipes/date-clean-pipe'
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap/modal';



@Component({
  selector: 'app-promo-code',
  imports: [CommonModule, NgStyle, NgClass, FormsModule, NgbPagination, NgbInputDatepicker, NgbAlert, JsonPipe, NgbToast,],//DateCleanPipe
  templateUrl: './promo-code.html',
  styleUrl: './promo-code.css',
  providers: [PromoService]
})
export class PromoCode implements OnInit {
  public PromoCodess: IPromoCode[] = [];
  private modalService = inject(NgbModal);

  constructor(public MyService: PromoService) {
    this.refreshPromos();
  }


  ngOnInit(): void {
    this.getPromos();

  }

  getPromos(): void {
    this.MyService.getPromos().subscribe({
      next: (data) => {
        console.log(data);
        // this.PromoCodess = data;
        this.PromoCodess = [...data];
        console.log("promos are:")
        console.log(this.PromoCodess)
        console.log(this.PromoCodess[0].id)


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
  collectionSize = this.PromoCodess.length;
  proms_arr: IPromoCode[] = [];

 
  refreshPromos() {
    console.log(this.pageSize)
    this.proms_arr = this.PromoCodess
      .map(({ id, ...p }, i) => ({ id: i + 1, ...p }))
      .slice(
        (this.page - 1) * this.pageSize,
        (this.page - 1) * this.pageSize + this.pageSize,
      );
  }
  toEdit: boolean = false;
  toSave: boolean = true;
  editedId: number = -1;

  public EditMode(id: number): void {

    this.editedId = id;
    console.clear();
    console.log(id);
    console.log(this.editedId);
  }
  public savedItem?: IPromoCode;
  public delItem?: IPromoCode;

  public savePromo(savedid: number) {
    this.savedItem = this.PromoCodess.find(e => e.id == savedid)
    console.log(this.savedItem);

  }

  showDeleteToast: Boolean = false;

  public deletePromo(deldid: number) {
    this.delItem = this.PromoCodess.find(e => e.id == deldid)
    if (!this.delItem) { console.log("item is already deleted"); return; }
    else {

      console.log(this.delItem);
      this.MyService.deletePromo(deldid).subscribe(
        {
          next: (response) => {
            console.log(response);
            // this.getPromos();

            this.getPromos();
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
  public createdPromo: IPromoCode = {
    "code":"string",
    "discountPercent": 0,
    "maxUsageCount": 0,
    "currentUsageCount":0,
    "expiryDate": new Date(),
    "id": 7,
    "isActive":true
  }

  showSaveToast: Boolean = false;

  public createPromo(newitem: any) {
    newitem.isActive = true;
    const selectedDate = new Date(newitem.expiryDate);
    const today = new Date();

    today.setHours(0, 0, 0, 0);
    selectedDate.setHours(0, 0, 0, 0);



    //validating code
    if (newitem.code.length < 3) { alert("enter a valid code"); return; }

    //validating max
    else if (newitem.maxUsageCount < newitem.currentUsageCount || newitem.maxUsageCount == 0) { { alert("enter a valid Max. Number"); return; } }

    //validating date
    else if (selectedDate < today) { alert("Enter a valid Date"); return; }

    else {
      this.MyService.createPromo(newitem).subscribe(
        {
          next: (response) => {
            console.log(response);
            // this.getPromos();

            this.getPromos();
            this.showSaveToast = true;
            this.createdPromo = {
              id: 0,
              code: '',
              discountPercent: 0,
              maxUsageCount: 0,
              currentUsageCount:0,
              expiryDate: new Date(),
              isActive:false
            };



          },
          error: (err) => {
            console.error(err);
          }
        }
      );
    }
  } //end of creation function



  public showAddForm() {
    this.itemToAdd = true;

  }

  public cancelAdd() {
    this.itemToAdd = false;
  }

  selectedAv: any;

  openModal(id:number) {
    const modalRef = this.modalService.open(NgbdModalConfirm);
  modalRef.result.then((result) => {
    console.log('Result:', result);
    this.deletePromo(id);
  }).catch((error) => {
    // Handle dismissal
  });
    
  }

  open() {
  const modalRef = this.modalService.open(NgbdModalConfirm);
  modalRef.result.then((result) => {
    console.log('Result:', result);
  }).catch((error) => {
    // Handle dismissal
  });
}
}//end of class




@Component({
  selector: 'ngbd-modal-confirm',
  template: `
		<div class="modal-header">
			<h4 class="modal-title" id="modal-title">Promo Code deletion</h4>
			<button
				type="button"
				class="btn-close"
				aria-describedby="modal-title"
				(click)="modal.dismiss('Cross click')"
			></button>
		</div>
		<div class="modal-body">
			<p>
				<strong>Are you sure you want to delete this code?</strong>
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
  constructor(public activeModal: NgbActiveModal) {}
public response:boolean=false;

  passBack() {
  this.activeModal.close(this.response);}
}


