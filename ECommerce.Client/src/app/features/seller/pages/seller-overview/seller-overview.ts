import { Component, computed, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { SellerProfileDto, SellerStatsDto } from '../../../../core/models/auth.model';
import { SellerService } from '../../services/seller-service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DecimalPipe } from '@angular/common';
import { EditStoreModal } from '../../components/edit-store-modal/edit-store-modal';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration } from 'chart.js';

@Component({
  selector: 'app-seller-overview',
  imports: [ReactiveFormsModule,DecimalPipe, BaseChartDirective],
  templateUrl: './seller-overview.html',
  styleUrl: './seller-overview.css',
})
export class SellerOverview implements OnInit {
profile = signal<SellerProfileDto | null>(null);
  stats = signal<SellerStatsDto | null>(null);
  loading = signal(false);

  salesChartData = computed(() => ({
    labels: this.stats()?.monthlySales.map(m => m.month) ?? [],
    datasets: [
      {
        label: 'Revenue',
        data: this.stats()?.monthlySales.map(m => m.revenue) ?? [],
        borderColor: '#7c6ff7',
        backgroundColor: '#7c6ff722',
        fill: true,
        tension: 0.4
      }
    ]
  }));

  salesChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: { legend: { position: 'top' } }
  };

  ordersChartData = computed(() => ({
    labels: this.stats()?.orderStatusStats.map(o => o.status) ?? [],
    datasets: [{
      data: this.stats()?.orderStatusStats.map(o => o.count) ?? [],
      backgroundColor: ['#7c6ff7', '#10b981', '#f59e0b', '#f43f5e', '#3b82f6']
    }]
  }));

  topProductsChartData = computed(() => ({
    labels: this.stats()?.topProducts.map(p => p.productName) ?? [],
    datasets: [{
      label: 'Units Sold',
      data: this.stats()?.topProducts.map(p => p.totalSold) ?? [],
      backgroundColor: ['#7c6ff7', '#10b981', '#f59e0b', '#f43f5e', '#3b82f6'],
      borderRadius: 8
    }]
  }));

  topProductsChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    indexAxis: 'y',
    plugins: { legend: { display: false } },
    scales: {
      x: { beginAtZero: true },
      y: { ticks: { color: '#2d2b55' } }
    }
  };

  constructor(private sellerService: SellerService, private modalService: NgbModal) {}

  ngOnInit(): void {
    this.loadProfile();
    this.loadStats();
  }

  loadProfile() {
    this.loading.set(true);
    this.sellerService.getMyProfile().subscribe({
      next: (data) => { this.profile.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  loadStats() {
    this.sellerService.getMyStats().subscribe({
      next: (data) => this.stats.set(data)
    });
  }

  openEditModal() {
    const modal = this.modalService.open(EditStoreModal, { centered: true });
    modal.componentInstance.profile = this.profile();

    modal.result.then((result) => {
      if (result) {
        this.sellerService.updateMyProfile(this.profile()!.id, result).subscribe({
          next: () => this.loadProfile()
        });
      }
    }).catch(() => {});
  }

  getLogoUrl(): string {
    const logo = this.profile()?.logoUrl;
    if (!logo) return `https://ui-avatars.com/api/?name=${this.profile()?.storeName}&background=7c6ff7&color=fff`;
    if (logo.startsWith('http')) return logo;
    return `https://localhost:7018${logo}`;
  }
}
