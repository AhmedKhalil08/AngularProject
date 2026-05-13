import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';
import { Router } from '@angular/router';
import { matchPassword } from '../../validators/password-match.validator';

@Component({
  selector: 'app-register-seller',
  imports: [ReactiveFormsModule],
  templateUrl: './register-seller.html',
  styleUrl: './register-seller.css',
})
export class RegisterSeller {
  registerSellerForm:FormGroup;

  constructor( private router:Router, private authService:AuthService, private fb :FormBuilder, private cd:ChangeDetectorRef)
  {
    this.registerSellerForm= this.fb.group({
      fullName : ['', [Validators.required, Validators.minLength(3)]],
      userName:['', [Validators.required,Validators.minLength(3)]],
      email:['',[Validators.required,Validators.email]],
      password:['',[ Validators.required,Validators.minLength(6)]],
      confirmPassword : ['',[Validators.required]],
      phoneNumber:['', [Validators.required,Validators.pattern(/^01[0125][0-9]{8}$/)]],
      storeName : ['', [Validators.minLength(3), Validators.required]],
      storeDescription :['', [Validators.minLength(3), Validators.required]]
    },
    {validators:matchPassword('password','confirmPassword')}
  
  )
  }

  isLoading =false;
  errorMessage='';
  
  onSubmit(){
    if(this.registerSellerForm.invalid){return;}

    this.isLoading=true;
    this.errorMessage='';

    const {confirmPassword , ...registerData} = this.registerSellerForm.value;

    this.authService.registerSeller(registerData).subscribe({
      next:()=>{
        this.router.navigate(['/auth/login'])
      },
      error: (err)=>{
        this.isLoading=false;
        this.errorMessage=err.error?.message || "Registrationg Failed";
        this.cd.detectChanges();
      }
    })
  }
}
