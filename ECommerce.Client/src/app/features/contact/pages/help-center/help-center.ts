import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

interface FAQ {
  question: string;
  answer: string;
  open: boolean;
}

@Component({
  selector: 'app-help-center',
  imports: [RouterLink, FormsModule, CommonModule],
  templateUrl: './help-center.html',
  styleUrl: './help-center.css',
})

export class HelpCenter {
  searchTerm = '';

  shippingFaqs: FAQ[] = [
    { question: 'How long does delivery take?', answer: 'Standard delivery takes 3-5 business days. Express delivery is available within 1-2 business days.', open: false },
    { question: 'Do you ship internationally?', answer: 'Yes! We ship to over 50 countries worldwide. International shipping takes 7-14 business days.', open: false },
    { question: 'How can I track my order?', answer: 'Once your order is shipped, you will receive a tracking number via email. You can track it in the Orders section of your account.', open: false },
    { question: 'What happens if my package is lost?', answer: 'If your package is lost in transit, contact our support team within 30 days of the estimated delivery date and we will resolve it.', open: false },
  ];

  returnFaqs: FAQ[] = [
    { question: 'What is your return policy?', answer: 'We accept returns within 30 days of delivery. Items must be unused, in original packaging, and in the same condition as received.', open: false },
    { question: 'How do I start a return?', answer: 'Go to your Orders page, select the order, click "Return Item" and follow the instructions. You will receive a return label via email.', open: false },
    { question: 'When will I receive my refund?', answer: 'Refunds are processed within 5-7 business days after we receive your return. The amount will be credited to your original payment method.', open: false },
    { question: 'Are there items that cannot be returned?', answer: 'Perishable goods, digital products, and items marked as "Final Sale" cannot be returned. Personal care items must be unopened.', open: false },
  ];

  paymentFaqs: FAQ[] = [
    { question: 'What payment methods do you accept?', answer: 'We accept Credit/Debit cards (Visa, Mastercard), PayPal, Cash on Delivery, and Vortex Wallet.', open: false },
    { question: 'Is my payment information secure?', answer: 'Yes! All payments are encrypted using SSL technology. We never store your card details on our servers.', open: false },
    { question: 'Can I use a promo code?', answer: 'Yes! Enter your promo code at checkout in the "Promo Code" field. Discounts are applied instantly.', open: false },
    { question: 'Why was my payment declined?', answer: 'Payments can be declined due to insufficient funds, incorrect card details, or bank restrictions. Try another payment method or contact your bank.', open: false },
  ];

  accountFaqs: FAQ[] = [
    { question: 'How do I create an account?', answer: 'Click "Join Now" on the navbar, fill in your details and verify your email. It takes less than 2 minutes!', open: false },
    { question: 'I forgot my password, what do I do?', answer: 'Click "Sign In" then "Forgot Password". Enter your email and we will send you a reset link.', open: false },
    { question: 'How do I become a seller?', answer: 'Register as a seller from the registration page. Fill in your store details and wait for admin approval.', open: false },
    { question: 'Can I change my email address?', answer: 'Currently email changes require contacting our support team. We are working on adding this feature to your profile settings.', open: false },
  ];

  toggle(faq: FAQ) {
    faq.open = !faq.open;
  }

  getFiltered(faqs: FAQ[]): FAQ[] {
    if (!this.searchTerm) return faqs;
    return faqs.filter(f =>
      f.question.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      f.answer.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

}
