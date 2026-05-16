import { Component, signal } from '@angular/core';
import { UserDto } from '../../../../core/models/auth.model';
import { FormBuilder, FormGroup,  ReactiveFormsModule, Validators } from '@angular/forms';
import { ProfileService } from '../../services/profile-service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../../../core/services/auth.service';
import { BecomeSellerModal } from '../../components/become-seller-modal/become-seller-modal';
import { ConfirmModal } from '../../../admin/components/confirm-modal/confirm-modal';

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile {

    user = signal<UserDto | null>(null);
  loading = signal(false);
  isEditing = signal(false);
  form!: FormGroup;
  selectedImage: File | null = null;
  previewUrl: string | null = null;

    constructor(private profileService: ProfileService, private fb: FormBuilder, private modalService:NgbModal,private authService:AuthService) {}

  ngOnInit(): void {
    this.loadProfile();
  }
  loadProfile() {
    this.loading.set(true);
    this.profileService.getMyProfile().subscribe({
      next: (data) => {
        this.user.set(data);
        this.loading.set(false);
        this.initForm(data);
        this.previewUrl = data.profileImageUrl ?? null;
      },
      error: () => this.loading.set(false)
    });
  }

  initForm(user: UserDto) {
    this.form = this.fb.group({
      fullName: [user.fullName, [Validators.required, Validators.minLength(3)]],
      phoneNumber: [user.phoneNumber ?? ''],
      profileImageUrl: [user.profileImageUrl ?? '']
    });
  }
    onImageSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.selectedImage = input.files[0];
      this.previewUrl = URL.createObjectURL(this.selectedImage);
    }
  }
    saveProfile() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.profileService.updateMyProfile({
      fullName: this.form.value.fullName,
      phoneNumber: this.form.value.phoneNumber,
      profileImageUrl: this.form.value.profileImageUrl,
      profileImage: this.selectedImage ?? undefined
    }).subscribe({
      next: (data) => { 
        this.user.set(data); 
        this.isEditing.set(false);
      }
    });
  }
    cancelEdit() {
    this.isEditing.set(false);
    this.initForm(this.user()!);
    this.previewUrl = this.user()?.profileImageUrl ?? null;
    this.selectedImage = null;
  }
getAvatarUrl(): string {
  if (this.previewUrl) {
    if (this.previewUrl.startsWith('blob') || this.previewUrl.startsWith('http')) return this.previewUrl;
    return `https://localhost:7018${this.previewUrl}`;
  }
  const name = this.user()?.fullName ?? 'User';
  return `https://ui-avatars.com/api/?name=${name}&background=7c6ff7&color=fff&size=200`;
}
    confirmDeleteAccount() {
  const modal = this.modalService.open(ConfirmModal, { centered: true });
  modal.componentInstance.title = 'Delete Account';
  modal.componentInstance.message = 'Are you sure? This action cannot be undone!';
  modal.componentInstance.confirmText = 'Delete';
  modal.componentInstance.confirmClass = 'danger';

  modal.result.then((confirmed) => {
    if (confirmed) {
      this.profileService.deleteAccount().subscribe({
        next: () => this.authService.clearUser()
      });
    }
  }).catch(() => {});
}


openBecomeSellerModal() {
  const modal = this.modalService.open(BecomeSellerModal, { centered: true });
  modal.result.then((result) => {
    if (result) {
      this.profileService.becomeSeller(result).subscribe({
        next: () => {
          this.authService.loadCurrentUser().subscribe({
            next: () => {
              this.loadProfile();
            }
          });
        }
      });
    }
  }).catch(() => {});
}
    get fullName() { return this.form?.get('fullName'); }
get phoneNumber() { return this.form?.get('phoneNumber'); }
}
