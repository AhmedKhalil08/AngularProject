import { Component, OnInit, signal, computed } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { BaseChartDirective } from 'ng2-charts';
import { Chart, registerables } from 'chart.js';
import { NgxParticlesModule } from '@tsparticles/angular';
import { loadSlim } from '@tsparticles/slim';
import type { Engine } from '@tsparticles/engine';
import { ChartConfiguration } from 'chart.js';
import { OverviewStatsDto } from '../../../../core/models/auth.model';

Chart.register(...registerables);

@Component({
  selector: 'app-overview',
  imports: [BaseChartDirective, NgxParticlesModule],
  templateUrl: './overview.html',
  styleUrl: './overview.css',
})
export class Overview implements OnInit {
  stats = signal<OverviewStatsDto | null>(null);
  loading = signal(false);
  particlesId = 'tsparticles';

  particlesOptions = {
    particles: {
      number: { value: 80 },
      color: {  value: '#00f0ff' },
      links: {
        enable: true,
        color: '#7c6ff7',
        opacity: 0.3
      },
      move: { enable: true, speed: 1.5 },
      size: { value: 3 },
      opacity: { value: 0.5 }
    },
interactivity: {
  events: {
    onHover: { enable: true, mode: 'grab' },  // connects lines to mouse
    onClick: { enable: true, mode: 'push' }    // adds particles on click
  },
  modes: {
    grab: { distance: 140, links: { opacity: 1 } },
    push: { quantity: 4 }
  }
}
  };

  async particlesInit(engine: Engine) {
    await loadSlim(engine);
  }

  salesChartData = computed(() => ({
    labels: this.stats()?.monthlySales.map(m => m.month) ?? [],
    datasets: [
      {
        label: 'Revenue ($)',
        data: this.stats()?.monthlySales.map(m => m.revenue) ?? [],
        borderColor: '#7c6ff7',
        backgroundColor: '#7c6ff722',
        fill: true,
        tension: 0.4
      },
      {
        label: 'Orders',
        data: this.stats()?.monthlySales.map(m => m.orders) ?? [],
        borderColor: '#10b981',
        backgroundColor: '#10b98122',
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

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats() {
    this.loading.set(true);
    this.adminService.getOverviewStats().subscribe({
      next: (data) => { this.stats.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }
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
  plugins: { 
    legend: { display: false }
  },
  scales: {
    x: { 
      beginAtZero: true,
      grid: { color: 'rgba(255,255,255,0.1)' },
      ticks: { color: '#c4b8ff' }
    },
    y: { 
      grid: { color: 'rgba(255,255,255,0.1)' },
      ticks: { color: '#c4b8ff' }
    }
  }
};}