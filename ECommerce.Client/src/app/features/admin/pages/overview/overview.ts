import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { BaseChartDirective } from 'ng2-charts';
import { Chart, registerables } from 'chart.js';
import { NgxParticlesModule } from '@tsparticles/angular';
import { loadSlim } from '@tsparticles/slim';
import type { Engine } from '@tsparticles/engine';
import { ChartConfiguration } from 'chart.js';
import { OverviewStatsDto } from '../../../../core/models/auth.model';
import { OrderService } from '../../../orders/services/order-service';

Chart.register(...registerables);

@Component({
  selector: 'app-overview',
  imports: [BaseChartDirective, NgxParticlesModule],
  templateUrl: './overview.html',
  styleUrl: './overview.css',
})
export class Overview implements OnInit {
  stats = signal<OverviewStatsDto | null>(null);
  private orderService = inject(OrderService);
  orders = this.orderService.allOrders;
  loading = signal(false);
  particlesId = 'tsparticles';

  particlesOptions = {
    particles: {
      number: { value: 80 },
      color: { value: '#00f0ff' },
      links: {
        enable: true,
        color: '#7c6ff7',
        opacity: 0.3,
      },
      move: { enable: true, speed: 1.5 },
      size: { value: 3 },
      opacity: { value: 0.5 },
    },
    interactivity: {
      events: {
        onHover: { enable: true, mode: 'grab' }, // connects lines to mouse
        onClick: { enable: true, mode: 'push' }, // adds particles on click
      },
      modes: {
        grab: { distance: 140, links: { opacity: 1 } },
        push: { quantity: 4 },
      },
    },
  };

  async particlesInit(engine: Engine) {
    await loadSlim(engine);
  }

  salesChartData = computed(() => {
    const allOrders = this.orders();
    const monthlyData: Record<string, { revenue: number; orders: number }> = {};

    allOrders.forEach((order) => {
      const date = new Date(order.orderDate);
      const monthYear = date.toLocaleString('default', { month: 'short', year: 'numeric' });

      if (!monthlyData[monthYear]) {
        monthlyData[monthYear] = { revenue: 0, orders: 0 };
      }
      monthlyData[monthYear].orders += 1;
      monthlyData[monthYear].revenue += order.totalAmount || 0;
    });

    const labels = Object.keys(monthlyData);
    return {
      labels: labels,
      datasets: [
        {
          label: 'Revenue ($)',
          data: labels.map((m) => monthlyData[m].revenue),
          borderColor: '#7c6ff7',
          backgroundColor: '#7c6ff722',
          fill: true,
          tension: 0.4,
          yAxisID: 'y',
        },
        {
          label: 'Orders',
          data: labels.map((m) => monthlyData[m].orders),
          borderColor: '#10b981',
          backgroundColor: '#10b98122',
          fill: true,
          tension: 0.4,
          yAxisID: 'y1',
        },
      ],
    };
  });

  salesChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: { legend: { position: 'top' } },
  };
  ordersChartData = computed(() => {
    const allOrders = this.orders();
    const statusCounts: Record<string, number> = {};

    allOrders.forEach((order) => {
      const status = order.status || 'Unknown';
      statusCounts[status] = (statusCounts[status] || 0) + 1;
    });

    return {
      labels: Object.keys(statusCounts),
      datasets: [
        {
          data: Object.values(statusCounts),
          backgroundColor: ['#7c6ff7', '#10b981', '#f59e0b', '#f43f5e', '#3b82f6'],
        },
      ],
    };
  });

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadStats();
    this.orderService.loadAllOrders();
  }

  loadStats() {
    this.loading.set(true);
    this.adminService.getOverviewStats().subscribe({
      next: (data) => {
        this.stats.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }
  topProductsChartData = computed(() => {
    const allOrders = this.orders();

    const productSales: Record<string, number> = {};

    allOrders.forEach((order) => {
      const items = order.orderItems || [];

      items.forEach((item: any) => {
        const name = item.productName || item.name || 'Unknown Product';
        const qty = item.quantity || 1;
        productSales[name] = (productSales[name] || 0) + qty;
      });
    });

    const sortedProducts = Object.keys(productSales)
      .map((name) => ({ name, total: productSales[name] }))
      .sort((a, b) => b.total - a.total)
      .slice(0, 5);
    return {
      labels: sortedProducts.map((p) => p.name),
      datasets: [
        {
          label: 'Units Sold',
          data: sortedProducts.map((p) => p.total),
          backgroundColor: ['#7c6ff7', '#10b981', '#f59e0b', '#f43f5e', '#3b82f6'],
          borderRadius: 8,
        },
      ],
    };
  });

  topProductsChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    indexAxis: 'y',
    plugins: {
      legend: { display: false },
    },
    scales: {
      x: {
        beginAtZero: true,
        grid: { color: 'rgba(255,255,255,0.1)' },
        ticks: { color: '#c4b8ff' },
      },
      y: {
        grid: { color: 'rgba(255,255,255,0.1)' },
        ticks: { color: '#c4b8ff' },
      },
    },
  };
}
