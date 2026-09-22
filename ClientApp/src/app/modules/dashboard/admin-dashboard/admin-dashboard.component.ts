import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ChartModule } from 'primeng/chart';
import { SkeletonModule } from 'primeng/skeleton';
import { DashboardService, AdminDashboardStats } from '../../../core/services/dashboard.service';

@Component({
  selector: 'app-admin-dashboard',
  imports: [
    ButtonModule,
    RouterModule,
    CommonModule,
    ChartModule,
    SkeletonModule,
  ],
  standalone: true,
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss',
})
export class AdminDashboardComponent implements OnInit {
  stats = signal<AdminDashboardStats | null>(null);
  isLoading = signal<boolean>(true);

  barData: any;
  barOptions: any;

  pieData: any;
  pieOptions: any;

  constructor(
    private readonly dashboardService: DashboardService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.initChartOptions();
    this.loadDashboardStats();
  }

  loadDashboardStats() {
    this.isLoading.set(true);
    this.dashboardService.getAdminDashboardStats().subscribe({
      next: (response) => {
        this.isLoading.set(false);
        if (response && response.isSuccess && response.data) {
          this.stats.set(response.data);
          this.populateCharts(response.data);
        } else {
          this.setDefaultFallback();
        }
      },
      error: (err) => {
        console.warn('Could not load live dashboard stats, applying fallback visualization', err);
        this.isLoading.set(false);
        this.setDefaultFallback();
      },
    });
  }

  private initChartOptions() {
    this.barOptions = {
      maintainAspectRatio: false,
      aspectRatio: 0.8,
      plugins: {
        legend: {
          labels: {
            color: '#475569',
          },
        },
      },
      scales: {
        x: {
          ticks: {
            color: '#64748b',
            font: {
              weight: 500,
            },
          },
          grid: {
            color: '#f8fafc',
            drawBorder: false,
          },
        },
        y: {
          ticks: {
            color: '#64748b',
          },
          grid: {
            color: '#f1f5f9',
            drawBorder: false,
          },
        },
      },
    };

    this.pieOptions = {
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: 'bottom',
          labels: {
            usePointStyle: true,
            color: '#475569',
            padding: 20,
          },
        },
      },
    };
  }

  private populateCharts(data: AdminDashboardStats) {
    // Bar chart: Monthly revenue
    if (data.revenueOverview && data.revenueOverview.length > 0) {
      this.barData = {
        labels: data.revenueOverview.map((r) => r.month),
        datasets: [
          {
            label: 'Revenue',
            backgroundColor: '#046492',
            borderColor: '#046492',
            data: data.revenueOverview.map((r) => r.revenue),
            borderRadius: 6,
          },
          {
            label: 'Due / Expenses',
            backgroundColor: '#cbd5e1',
            borderColor: '#cbd5e1',
            data: data.revenueOverview.map((r) => r.expenses),
            borderRadius: 6,
          },
        ],
      };
    } else {
      this.setDefaultBarData();
    }

    // Pie chart: Student demographics
    if (data.studentDemographics && data.studentDemographics.length > 0) {
      const palette = ['#046492', '#f59e0b', '#10b981', '#6366f1', '#ec4899', '#8b5cf6'];
      this.pieData = {
        labels: data.studentDemographics.map((d) => d.className || 'General'),
        datasets: [
          {
            data: data.studentDemographics.map((d) => d.studentCount),
            backgroundColor: palette.slice(0, data.studentDemographics.length),
            borderWidth: 0,
          },
        ],
      };
    } else {
      this.setDefaultPieData();
    }
  }

  private setDefaultFallback() {
    this.stats.set({
      totalStudents: 0,
      totalRevenue: 0,
      totalAdmissions: 0,
      totalTeachers: 0,
      totalDue: 0,
      revenueOverview: [],
      studentDemographics: [],
      recentAdmissions: [],
    });
    this.setDefaultBarData();
    this.setDefaultPieData();
  }

  private setDefaultBarData() {
    this.barData = {
      labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
      datasets: [
        {
          label: 'Revenue',
          backgroundColor: '#046492',
          borderColor: '#046492',
          data: [0, 0, 0, 0, 0, 0],
          borderRadius: 6,
        },
        {
          label: 'Expenses',
          backgroundColor: '#cbd5e1',
          borderColor: '#cbd5e1',
          data: [0, 0, 0, 0, 0, 0],
          borderRadius: 6,
        },
      ],
    };
  }

  private setDefaultPieData() {
    this.pieData = {
      labels: ['Primary', 'Middle School', 'High School'],
      datasets: [
        {
          data: [0, 0, 0],
          backgroundColor: ['#046492', '#f59e0b', '#10b981'],
          borderWidth: 0,
        },
      ],
    };
  }
}
