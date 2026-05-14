import { AbstractControl, ValidatorFn, Validators } from '@angular/forms';

// Custom validator that compares two fields
export function matchPassword(password: string, confirmPassword: string): ValidatorFn {
  return (group: AbstractControl): { [key: string]: any } | null => {
    const p = group.get(password);
    const cp = group.get(confirmPassword);

    if (!p || !cp) return null;

    const isMatch = p.value === cp.value;
    if (!isMatch) {
      cp.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    } else {
      if (cp.hasError('passwordMismatch')) {
        cp.setErrors(null);
      }
      return null;
    }
  };
}