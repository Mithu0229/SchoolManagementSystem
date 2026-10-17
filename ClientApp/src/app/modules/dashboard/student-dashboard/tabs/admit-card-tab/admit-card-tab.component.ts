import { CommonModule } from '@angular/common';
import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Subscription } from 'rxjs';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import {
  AdmitCardService,
  StudentAdmitCardDto,
  StudentAdmitCardListResponse,
} from '../../../../../core/services/admit-card.service';
import { AuthService } from '../../../../../core/services/auth.service';

@Component({
  selector: 'app-student-dashboard-tab-admit-card',
  standalone: true,
  imports: [CommonModule, FormsModule, DialogModule, ButtonModule, ToastModule],
  providers: [MessageService],
  templateUrl: './admit-card-tab.component.html',
  styleUrl: './admit-card-tab.component.scss',
})
export class AdmitCardTabComponent implements OnInit, OnDestroy {
  private route = inject(ActivatedRoute);
  private admitCardService = inject(AdmitCardService);
  private authService = inject(AuthService);
  private sanitizer = inject(DomSanitizer);
  private messageService = inject(MessageService);

  studentId: string = '';
  cutoffType: 'exam_date' | 'current_date' = 'exam_date';

  loading: boolean = false;
  downloadingMap: Record<string, boolean> = {};

  studentData: StudentAdmitCardListResponse | null = null;
  admitCards: StudentAdmitCardDto[] = [];

  // Preview Dialog State
  previewVisible: boolean = false;
  previewTitle: string = '';
  previewSafeUrl: SafeResourceUrl | null = null;
  previewBlobUrl: string | null = null;
  previewExamType: string = '';

  private querySub?: Subscription;

  ngOnInit(): void {
    this.querySub = this.route.queryParams.subscribe((params) => {
      const qId = params['studentId'];
      const currentAuthUser = this.authService.getCurrentUser();
      const localId = localStorage.getItem('studentId');

      this.studentId =
        qId ||
        localId ||
        currentAuthUser?.userLoginInfo?.studentId ||
        '';

      if (this.studentId) {
        this.loadAdmitCards();
      } else {
        this.loading = false;
      }
    });
  }

  ngOnDestroy(): void {
    this.querySub?.unsubscribe();
    if (this.previewBlobUrl) {
      window.URL.revokeObjectURL(this.previewBlobUrl);
    }
  }

  loadAdmitCards(): void {
    if (!this.studentId) return;

    this.loading = true;
    this.admitCardService
      .getStudentAdmitCards(this.studentId, this.cutoffType)
      .subscribe({
        next: (res) => {
          this.loading = false;
          if (res.isSuccess && res.data) {
            this.studentData = res.data;
            this.admitCards = res.data.admitCards || [];
          } else {
            this.messageService.add({
              severity: 'warn',
              summary: 'Notice',
              detail:
                res.notificationMessage ||
                res.errors?.[0] ||
                'No admit cards available.',
            });
          }
        },
        error: (err) => {
          this.loading = false;
          console.error('Error fetching admit cards:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to load admit card information.',
          });
        },
      });
  }

  onCutoffChange(type: 'exam_date' | 'current_date'): void {
    if (this.cutoffType !== type) {
      this.cutoffType = type;
      this.loadAdmitCards();
    }
  }

  downloadCard(card: StudentAdmitCardDto): void {
    if (!card.isEligible) {
      this.messageService.add({
        severity: 'error',
        summary: 'Download Restricted',
        detail: `Cannot download admit card. Unpaid due of ৳ ${card.dueAmount.toLocaleString()} must be cleared first.`,
      });
      return;
    }

    this.downloadingMap[card.examType] = true;

    this.admitCardService
      .downloadAdmitCard(this.studentId, card.examType)
      .subscribe({
        next: (response) => {
          this.downloadingMap[card.examType] = false;
          const blob = response.body;
          if (blob) {
            const fileName =
              this.extractFileName(response) ||
              `AdmitCard_${card.examType}_${this.studentData?.studentName || 'Student'}.pdf`;

            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);

            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: `${card.examName} admit card downloaded successfully!`,
            });
          }
        },
        error: (err) => {
          this.downloadingMap[card.examType] = false;
          console.error('Download error:', err);
          let errorMsg = 'Failed to download admit card.';
          if (err?.error instanceof Blob) {
            // Read JSON from blob error
            const reader = new FileReader();
            reader.onload = () => {
              try {
                const parsed = JSON.parse(reader.result as string);
                errorMsg = parsed.message || errorMsg;
              } catch {}
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: errorMsg,
              });
            };
            reader.readAsText(err.error);
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: err?.error?.message || errorMsg,
            });
          }
        },
      });
  }

  previewCard(card: StudentAdmitCardDto): void {
    this.previewExamType = card.examType;
    this.previewTitle = `${card.examName} - Admit Card Preview`;

    if (this.previewBlobUrl) {
      window.URL.revokeObjectURL(this.previewBlobUrl);
      this.previewBlobUrl = null;
    }

    this.admitCardService
      .previewAdmitCard(this.studentId, card.examType)
      .subscribe({
        next: (response) => {
          const blob = response.body;
          if (blob) {
            this.previewBlobUrl = window.URL.createObjectURL(blob);
            this.previewSafeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
              this.previewBlobUrl
            );
            this.previewVisible = true;
          }
        },
        error: (err) => {
          console.error('Preview error:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'Preview Error',
            detail: 'Unable to generate preview for admit card.',
          });
        },
      });
  }

  closePreview(): void {
    this.previewVisible = false;
    if (this.previewBlobUrl) {
      window.URL.revokeObjectURL(this.previewBlobUrl);
      this.previewBlobUrl = null;
    }
    this.previewSafeUrl = null;
  }

  private extractFileName(response: any): string | null {
    const disposition = response.headers?.get('content-disposition');
    if (disposition && disposition.indexOf('filename=') !== -1) {
      const match = disposition.match(/filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/);
      if (match && match[1]) {
        return match[1].replace(/['"]/g, '').trim();
      }
    }
    return null;
  }
}
