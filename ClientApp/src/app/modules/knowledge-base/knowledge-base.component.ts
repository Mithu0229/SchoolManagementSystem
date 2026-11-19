import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-knowledge-base',
  standalone: true,
  imports: [CommonModule, RouterModule, ButtonModule, TooltipModule],
  templateUrl: './knowledge-base.component.html',
  styleUrl: './knowledge-base.component.scss'
})
export class KnowledgeBaseComponent {
  activeTab = signal<string>('all');
  copiedCommand = signal<string | null>(null);

  tabs = [
    { id: 'all', label: 'All Sections', icon: 'pi pi-th-large' },
    { id: 'pipeline', label: 'Master Pipeline', icon: 'pi pi-sitemap' },
    { id: 'master', label: '1. Master Setup', icon: 'pi pi-cog' },
    { id: 'students', label: '2. Student Onboarding', icon: 'pi pi-users' },
    { id: 'billing', label: '3. Billing & Fees', icon: 'pi pi-wallet' },
    { id: 'accounting', label: '4. Accounting', icon: 'pi pi-chart-line' },
    { id: 'exams', label: '5. Exams & Admit Cards', icon: 'pi pi-id-card' },
    { id: 'system', label: '6. System Run & Setup', icon: 'pi pi-server' },
    { id: 'troubleshooting', label: '7. Troubleshooting', icon: 'pi pi-wrench' }
  ];

  setActiveTab(tabId: string) {
    this.activeTab.set(tabId);
  }

  copyToClipboard(text: string, identifier: string) {
    navigator.clipboard.writeText(text).then(() => {
      this.copiedCommand.set(identifier);
      setTimeout(() => {
        this.copiedCommand.set(null);
      }, 2000);
    });
  }

  printGuide() {
    window.print();
  }

  downloadDiagramPdf() {
    const link = document.createElement('a');
    link.href = '/docs/EDUGATES_SMS_Admin_Process_Flow_Diagram_Guide.pdf';
    link.download = 'EDUGATES_SMS_Admin_Process_Flow_Diagram_Guide.pdf';
    link.target = '_blank';
    link.click();
  }

  downloadManualPdf() {
    const link = document.createElement('a');
    link.href = '/docs/EDUGATES_SMS_Admin_User_Manual_and_Setup_Guide.pdf';
    link.download = 'EDUGATES_SMS_Admin_User_Manual_and_Setup_Guide.pdf';
    link.target = '_blank';
    link.click();
  }
}
