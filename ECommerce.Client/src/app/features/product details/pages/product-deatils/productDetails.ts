import { Component, OnInit, inject, AfterViewInit } from '@angular/core';
import { Product } from '../../../../core/models/product';
import { ProductService } from '../../../products/services/productService';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-product-details',
  imports: [CommonModule],
  templateUrl: './productDetails.html',
  styleUrl: './productDetails.css',
  providers: [ProductService]
})
export class productDetails implements OnInit {

   product?: Product;
   
  
  prdImgs?:string[]=[];
  id: number = 0;
  main_img?:string="";
  constructor(private MyService: ProductService,private route: ActivatedRoute) {
    

//     this.product= {
//   id: 1,
//   name: "laptop",
//   description: "a powerful device",
//   price:50,
//   stock:34,
//   categoryId: 0,
//   categoryName: "electronics",
 
//   imageUrls: ["https://m.media-amazon.com/images/I/710XDUjdgTL._AC_SX300_SY300_QL70_ML2_.jpg","https://m.media-amazon.com/images/I/61wIn3qoz0L._AC_SX569_.jpg",
//     "https://m.media-amazon.com/images/I/51fMF70CPWL._AC_SL1500_.jpg","https://m.media-amazon.com/images/I/51fMF70CPWL._AC_SL1500_.jpg",
//     "https://m.media-amazon.com/images/I/51fMF70CPWL._AC_SL1500_.jpg"
//   ],
//   rating:4,
//   sellerName:" Mohamed",
//   storeDes: "AN steamed selles"
// }
// this.prdImgs=this.product.imageUrls?.slice();
//         this.main_img=this.prdImgs?this.prdImgs[0]:" ";
//         console.log (this.prdImgs);
//         console.log(this.product.imageUrls);

        
       }

  ngOnInit(): void { 
        const id = this.route.snapshot.paramMap.get('id');
        this.id=id?+id:0;
        this.getPrd(this.id); }

  getPrd(id: number): void {
    this.MyService.getProductById(id).subscribe({
      next: (data) => {
        this.product = data
        this.prdImgs=this.product.imageUrls?.slice();
        this.main_img=this.prdImgs?this.prdImgs[0]:" ";

        console.log('✅ Product loaded successfully:', data);

      },
      error: (err) => {
        console.error('❌ Error loading product:', err);

      },
    });

  }

  changeImage(img:string){
this.main_img=img;
  }

}
