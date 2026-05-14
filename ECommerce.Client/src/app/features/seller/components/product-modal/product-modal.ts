import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Product } from '../../../../core/models/product';
import { Category } from '../../../../core/models/category';

@Component({
  selector: 'app-product-modal',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './product-modal.component.html',
  styleUrl: './product-modal.component.css'
})
export class ProductModal implements OnInit {
  @Input() product: Product | null = null;
  @Input() categories: Category[] = [];

  form!: FormGroup;
  selectedImages: File[] = [];
  previewUrls: string[] = [];
  isEditMode = false;

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.isEditMode = !!this.product;

    this.form = this.fb.group({
      name: [this.product?.name ?? '', [Validators.required, Validators.minLength(3)]],
      price: [this.product?.price ?? '', [Validators.required, Validators.min(0)]],
      stock: [this.product?.stock ?? '', [Validators.required, Validators.min(0)]],
      description: [this.product?.description ?? '', [Validators.required, Validators.minLength(10)]],
      categoryId: [this.product?.categoryId ?? '', Validators.required]
    });

    if (this.product?.imageUrls) {
      this.previewUrls = this.product.imageUrls;
    }
  }

  onImagesSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.selectedImages = Array.from(input.files);
      this.previewUrls = this.selectedImages.map(f => URL.createObjectURL(f));
    }
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formData = new FormData();
    formData.append('name', this.form.value.name);
    formData.append('price', this.form.value.price);
    formData.append('stock', this.form.value.stock);
    formData.append('description', this.form.value.description);
    formData.append('categoryId', this.form.value.categoryId);

    if (this.isEditMode) formData.append('id', this.product!.id.toString());

    this.selectedImages.forEach(img => formData.append('images', img));

    this.activeModal.close({ formData, isEdit: this.isEditMode });
  }

  get name() { return this.form.get('name'); }
  get price() { return this.form.get('price'); }
  get stock() { return this.form.get('stock'); }
  get description() { return this.form.get('description'); }
  get categoryId() { return this.form.get('categoryId'); }
}