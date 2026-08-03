import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ChartModule } from 'primeng/chart';
import { UserService } from '../../../core/services/user.service';

@Component({
  selector: 'app-admin-dashboard',
  imports: [
    ButtonModule,
    RouterModule,
    CommonModule,
    ChartModule,
  ],
  standalone: true,
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss',
})
export class AdminDashboardComponent implements OnInit {
  barData: any;
  barOptions: any;

  pieData: any;
  pieOptions: any;

  constructor(
    private readonly userService: UserService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.initCharts();
  }

  initCharts() {
    this.barData = {
      labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
      datasets: [
        {
          label: 'Revenue',
          backgroundColor: '#046492',
          borderColor: '#046492',
          data: [15000, 18000, 22000, 19500, 25000, 21000, 28000],
          borderRadius: 6
        },
        {
          label: 'Expenses',
          backgroundColor: '#cbd5e1',
          borderColor: '#cbd5e1',
          data: [12000, 13500, 11000, 15000, 14000, 16000, 13000],
          borderRadius: 6
        }
      ]
    };

    this.barOptions = {
      maintainAspectRatio: false,
      aspectRatio: 0.8,
      plugins: {
        legend: {
          labels: {
            color: '#475569'
          }
        }
      },
      scales: {
        x: {
          ticks: {
            color: '#64748b',
            font: {
              weight: 500
            }
          },
          grid: {
            color: '#f8fafc',
            drawBorder: false
          }
        },
        y: {
          ticks: {
            color: '#64748b'
          },
          grid: {
            color: '#f1f5f9',
            drawBorder: false
          }
        }
      }
    };

    this.pieData = {
      labels: ['Primary', 'Middle School', 'High School'],
      datasets: [
        {
          data: [450, 320, 280],
          backgroundColor: [
            '#046492',
            '#f59e0b',
            '#10b981'
          ],
          hoverBackgroundColor: [
            '#034d70',
            '#d97706',
            '#059669'
          ],
          borderWidth: 0
        }
      ]
    };

    this.pieOptions = {
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: 'bottom',
          labels: {
            usePointStyle: true,
            color: '#475569',
            padding: 20
          }
        }
      }
    };
  }
}
