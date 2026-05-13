import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';
import { Router } from '@angular/router';
import { ParseSourceFile } from '@angular/compiler';
import { matchPassword } from '../../validators/password-match.validator';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  registerForm:FormGroup;
  constructor(private router:Router, private authService:AuthService,private fb :FormBuilder, private cd : ChangeDetectorRef ){
    this.registerForm=this.fb.group({
      fullName : ['', [Validators.required, Validators.minLength(3)]],
      userName:['', [Validators.required,Validators.minLength(3)]],
      email:['',[Validators.required,Validators.email]],
      password:['',[ Validators.required,Validators.minLength(6)]],
      confirmPassword : ['',[Validators.required]],
      phoneNumber:['', [Validators.required,Validators.pattern(/^01[0125][0-9]{8}$/)]]

    },
  {validators:matchPassword('password','confirmPassword')}
)

  }

  isLoading=false;
  errorMessage='';
onSubmit() {
  if (this.registerForm.invalid) return;

  this.isLoading = true;
  this.errorMessage = '';

  const { confirmPassword, ...registerData } = this.registerForm.value;

  this.authService.registerCustomer(registerData).subscribe({
    next: () => {
      this.router.navigate(['/auth/login']);
    },
    error: (err) => {
      this.isLoading = false;
      this.errorMessage = err.error?.message || 'Registration failed';
      this.cd.detectChanges();
    }
  });
}
registerWithGoogle() {
  window.location.href = 'https://localhost:7018/api/auth/google-login';
}
}
