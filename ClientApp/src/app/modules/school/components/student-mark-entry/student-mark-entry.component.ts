import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ExamMarkService, StudentMarksheetResponse, MarksheetSubjectRowDto, ExamComponentDto, BatchStudentMarkItem } from '../../services/exam-mark.service';
import { AcademicSessionService } from '../../services/academic-session.service';
import { AcademicClassService } from '../../services/academic-class.service';
import { SectionService } from '../../services/section.service';
import { SubjectService } from '../../services/subject.service';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-student-mark-entry',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DropdownModule,
    ButtonModule,
    InputTextModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './student-mark-entry.component.html',
  styleUrl: './student-mark-entry.component.scss'
})
export class StudentMarkEntryComponent implements OnInit {
  private markService = inject(ExamMarkService);
  private sessionService = inject(AcademicSessionService);
  private classService = inject(AcademicClassService);
  private sectionService = inject(SectionService);
  private subjectService = inject(SubjectService);
  private messageService = inject(MessageService);

  // Filter states
  sessions: any[] = [];
  classes: any[] = [];
  sections: any[] = [];
  students: any[] = [];
  subjects: any[] = [];
  examComponents: ExamComponentDto[] = [];

  selectedSessionId: string | null = null;
  selectedClassId: string | null = null;
  selectedSectionId: string | null = null;
  selectedStudentId: string | null = null;

  // Active mode: 'student' or 'batch'
  activeMode: 'student' | 'batch' = 'student';

  // Batch entry filters
  selectedBatchSubjectId: string | null = null;
  selectedBatchComponentId: string | null = null;
  batchStudents: BatchStudentMarkItem[] = [];

  // Marksheet Data for Single Student
  marksheetData: StudentMarksheetResponse | null = null;
  loading: boolean = false;
  saving: boolean = false;

  ngOnInit(): void {
    this.loadFilters();
  }

  loadFilters(): void {
    this.sessionService.getAcademicSessionDropdown().subscribe(res => {
      if (res.isSuccess && res.data) {
        this.sessions = (res.data || []).map((x: any) => ({
          label: x.name || x.text || x.sessionName,
          value: x.id || x.value
        }));
        if (this.sessions.length > 0) {
          this.selectedSessionId = this.sessions[0].value;
          this.loadExamComponents();
        }
      }
    });

    this.classService.getAcademicClassDropdown().subscribe(res => {
      if (res.isSuccess && res.data) {
        this.classes = (res.data || []).map((x: any) => ({
          label: x.name || x.text || x.className,
          value: x.id || x.value
        }));
        if (this.classes.length > 0) {
          this.selectedClassId = this.classes[0].value;
          this.onClassChange();
        }
      }
    });

    this.sectionService.getSectionDropdown().subscribe(res => {
      if (res.isSuccess && res.data) {
        const mapped = (res.data || []).map((x: any) => ({
          label: x.name || x.text || x.sectionName,
          value: x.id || x.value
        }));
        this.sections = [{ label: 'All Sections', value: null }, ...mapped];
      }
    });
  }

  loadExamComponents(): void {
    if (!this.selectedSessionId) return;
    this.markService.getExamComponents(this.selectedSessionId).subscribe(res => {
      if (res.isSuccess && res.data) {
        this.examComponents = res.data;
        if (this.examComponents.length > 0 && !this.selectedBatchComponentId) {
          this.selectedBatchComponentId = this.examComponents[0].id;
        }
      }
    });
  }

  onSessionChange(): void {
    this.loadExamComponents();
    this.onClassChange();
  }

  onClassChange(): void {
    if (!this.selectedClassId || !this.selectedSessionId) return;

    // Load subjects for this class
    this.subjectService.getSubjectsByClass(this.selectedClassId).subscribe(res => {
      if (res.isSuccess && res.data) {
        this.subjects = res.data.map(s => ({
          label: s.subjectName || (s as any).name || (s as any).text,
          value: s.subjectId || (s as any).id || (s as any).value
        }));
        if (this.subjects.length > 0) {
          this.selectedBatchSubjectId = this.subjects[0].value;
        }
      }
    });

    // Load students in this class
    this.markService.getStudentsByClass(this.selectedSessionId, this.selectedClassId, this.selectedSectionId || undefined).subscribe(res => {
      if (res.isSuccess && res.data) {
        this.students = res.data.map(s => ({
          label: `${s.studentName} (Roll: ${s.rollNo})`,
          value: s.studentId,
          roll: s.rollNo,
          name: s.studentName
        }));

        if (this.students.length > 0) {
          this.selectedStudentId = this.students[0].value;
          this.loadMarksheet();
        } else {
          this.selectedStudentId = null;
          this.marksheetData = null;
        }
      }
    });
  }

  onSectionChange(): void {
    this.onClassChange();
  }

  onStudentChange(): void {
    this.loadMarksheet();
  }

  findComponentId(aliases: string[]): string | undefined {
    return this.examComponents.find(c => {
      const clean = (c.componentName || '').replace(/[\s-_]/g, '').toLowerCase();
      return aliases.some(a => {
        const cleanA = a.replace(/[\s-_]/g, '').toLowerCase();
        return clean === cleanA || clean.includes(cleanA);
      });
    })?.id;
  }

  loadMarksheet(): void {
    if (!this.selectedSessionId || !this.selectedClassId || !this.selectedStudentId) {
      return;
    }

    this.loading = true;
    this.markService.getStudentMarksheet(
      this.selectedSessionId,
      this.selectedClassId,
      this.selectedStudentId,
      this.selectedSectionId || undefined
    ).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.isSuccess && res.data) {
          this.marksheetData = res.data;
          if (this.marksheetData.components && this.marksheetData.components.length > 0) {
            this.examComponents = this.marksheetData.components;
          }
          if (this.marksheetData.subjects) {
            this.marksheetData.subjects.forEach(row => {
              const anyRow = row as any;
              row.ct1 = row.ct1 ?? anyRow.cT1 ?? anyRow.CT1;
              row.ct2 = row.ct2 ?? anyRow.cT2 ?? anyRow.CT2;
              row.halfYearly = row.halfYearly ?? anyRow.HalfYearly;
              row.ct3 = row.ct3 ?? anyRow.cT3 ?? anyRow.CT3;
              row.ct4 = row.ct4 ?? anyRow.cT4 ?? anyRow.CT4;
              row.yearlyExam = row.yearlyExam ?? anyRow.YearlyExam;
            });
          }
          if (this.marksheetData.summary) {
            const s = this.marksheetData.summary as any;
            s.totalCT1 = s.totalCT1 ?? s.totalCt1 ?? s.TotalCT1;
            s.totalCT2 = s.totalCT2 ?? s.totalCt2 ?? s.TotalCT2;
            s.totalHalfYearly = s.totalHalfYearly ?? s.TotalHalfYearly;
            s.totalCT3 = s.totalCT3 ?? s.totalCt3 ?? s.TotalCT3;
            s.totalCT4 = s.totalCT4 ?? s.totalCt4 ?? s.TotalCT4;
            s.totalYearlyExam = s.totalYearlyExam ?? s.TotalYearlyExam;
          }
          this.recalculateAll();
        }
      },
      error: () => {
        this.loading = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load marksheet.' });
      }
    });
  }

  // Real-time calculation on mark change in the grid
  onMarkChange(row: MarksheetSubjectRowDto): void {
    // Validate boundaries
    if (row.ct1 !== undefined && row.ct1 !== null && row.ct1 > 15) row.ct1 = 15;
    if (row.ct2 !== undefined && row.ct2 !== null && row.ct2 > 15) row.ct2 = 15;
    if (row.halfYearly !== undefined && row.halfYearly !== null && row.halfYearly > 70) row.halfYearly = 70;
    if (row.ct3 !== undefined && row.ct3 !== null && row.ct3 > 15) row.ct3 = 15;
    if (row.ct4 !== undefined && row.ct4 !== null && row.ct4 > 15) row.ct4 = 15;
    if (row.yearlyExam !== undefined && row.yearlyExam !== null && row.yearlyExam > 70) row.yearlyExam = 70;

    // Term 1 = CT1 + CT2 + HalfYearly (100)
    row.term1Total = Number((row.ct1 || 0)) + Number((row.ct2 || 0)) + Number((row.halfYearly || 0));

    // Term 2 = CT3 + CT4 + YearlyExam (100)
    row.term2Total = Number((row.ct3 || 0)) + Number((row.ct4 || 0)) + Number((row.yearlyExam || 0));

    // Overall Average = (Term 1 + Term 2) / 2
    row.overallAverage = Math.round(((row.term1Total + row.term2Total) / 2) * 100) / 100;

    // Calculate Grade from active scale
    this.assignGrade(row);
    this.recalculateSummary();
  }

  private assignGrade(row: MarksheetSubjectRowDto): void {
    const avg = row.overallAverage;
    if (avg >= 80) { row.grade = 'A+'; row.gradePoint = 5.00; }
    else if (avg >= 70) { row.grade = 'A'; row.gradePoint = 4.00; }
    else if (avg >= 60) { row.grade = 'A-'; row.gradePoint = 3.50; }
    else if (avg >= 50) { row.grade = 'B'; row.gradePoint = 3.00; }
    else if (avg >= 40) { row.grade = 'C'; row.gradePoint = 2.00; }
    else if (avg >= 33) { row.grade = 'D'; row.gradePoint = 1.00; }
    else { row.grade = 'F'; row.gradePoint = 0.00; }
  }

  recalculateAll(): void {
    if (!this.marksheetData) return;
    this.marksheetData.subjects.forEach(row => {
      this.onMarkChange(row);
    });
  }

  recalculateSummary(): void {
    if (!this.marksheetData || !this.marksheetData.subjects) return;
    const subs = this.marksheetData.subjects;

    const summary = this.marksheetData.summary;
    summary.totalCT1 = subs.reduce((acc, x) => acc + (Number(x.ct1) || 0), 0);
    summary.totalCT2 = subs.reduce((acc, x) => acc + (Number(x.ct2) || 0), 0);
    summary.totalHalfYearly = subs.reduce((acc, x) => acc + (Number(x.halfYearly) || 0), 0);
    summary.totalTerm1 = subs.reduce((acc, x) => acc + x.term1Total, 0);

    summary.totalCT3 = subs.reduce((acc, x) => acc + (Number(x.ct3) || 0), 0);
    summary.totalCT4 = subs.reduce((acc, x) => acc + (Number(x.ct4) || 0), 0);
    summary.totalYearlyExam = subs.reduce((acc, x) => acc + (Number(x.yearlyExam) || 0), 0);
    summary.totalTerm2 = subs.reduce((acc, x) => acc + x.term2Total, 0);

    summary.overallAverage = subs.length > 0
      ? Math.round((subs.reduce((acc, x) => acc + x.overallAverage, 0) / subs.length) * 100) / 100
      : 0;

    summary.finalGPA = subs.length > 0
      ? Math.round((subs.reduce((acc, x) => acc + x.gradePoint, 0) / subs.length) * 100) / 100
      : 0;

    if (summary.overallAverage >= 80) summary.finalGrade = 'A+';
    else if (summary.overallAverage >= 70) summary.finalGrade = 'A';
    else if (summary.overallAverage >= 60) summary.finalGrade = 'A-';
    else if (summary.overallAverage >= 50) summary.finalGrade = 'B';
    else if (summary.overallAverage >= 40) summary.finalGrade = 'C';
    else if (summary.overallAverage >= 33) summary.finalGrade = 'D';
    else summary.finalGrade = 'F';
  }

  saveMarksheet(): void {
    if (!this.marksheetData || !this.selectedSessionId || !this.selectedClassId || !this.selectedStudentId) {
      return;
    }

    // Match component IDs with flexible matching
    const ct1Id = this.findComponentId(['ct1', 'ct-1', 'ct 1']);
    const ct2Id = this.findComponentId(['ct2', 'ct-2', 'ct 2']);
    const hyId = this.examComponents.find(c => (c.componentName || '').toLowerCase().includes('half'))?.id;
    const ct3Id = this.findComponentId(['ct3', 'ct-3', 'ct 3']);
    const ct4Id = this.findComponentId(['ct4', 'ct-4', 'ct 4']);
    const yearlyId = this.examComponents.find(c => (c.componentName || '').toLowerCase().includes('yearly') && !(c.componentName || '').toLowerCase().includes('half'))?.id;

    const subjectMarks = this.marksheetData.subjects.map(s => {
      const compMarks: any[] = [];
      const valCt1 = s.ct1 ?? (s as any).cT1 ?? (s as any).CT1;
      const valCt2 = s.ct2 ?? (s as any).cT2 ?? (s as any).CT2;
      const valHy = s.halfYearly ?? (s as any).HalfYearly;
      const valCt3 = s.ct3 ?? (s as any).cT3 ?? (s as any).CT3;
      const valCt4 = s.ct4 ?? (s as any).cT4 ?? (s as any).CT4;
      const valYearly = s.yearlyExam ?? (s as any).YearlyExam;

      if (ct1Id && valCt1 !== null && valCt1 !== undefined && valCt1 !== '') compMarks.push({ examComponentId: ct1Id, marksObtained: Number(valCt1), isAbsent: false });
      if (ct2Id && valCt2 !== null && valCt2 !== undefined && valCt2 !== '') compMarks.push({ examComponentId: ct2Id, marksObtained: Number(valCt2), isAbsent: false });
      if (hyId && valHy !== null && valHy !== undefined && valHy !== '') compMarks.push({ examComponentId: hyId, marksObtained: Number(valHy), isAbsent: false });
      if (ct3Id && valCt3 !== null && valCt3 !== undefined && valCt3 !== '') compMarks.push({ examComponentId: ct3Id, marksObtained: Number(valCt3), isAbsent: false });
      if (ct4Id && valCt4 !== null && valCt4 !== undefined && valCt4 !== '') compMarks.push({ examComponentId: ct4Id, marksObtained: Number(valCt4), isAbsent: false });
      if (yearlyId && valYearly !== null && valYearly !== undefined && valYearly !== '') compMarks.push({ examComponentId: yearlyId, marksObtained: Number(valYearly), isAbsent: false });

      return {
        subjectId: s.subjectId,
        componentMarks: compMarks
      };
    });

    this.saving = true;
    this.markService.saveStudentMarks({
      academicSessionId: this.selectedSessionId,
      classId: this.selectedClassId,
      sectionId: this.selectedSectionId || undefined,
      studentId: this.selectedStudentId,
      subjectMarks
    }).subscribe({
      next: (res) => {
        this.saving = false;
        if (res.isSuccess) {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'All marks saved and updated successfully.' });
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: res.notificationMessage || 'Save failed.' });
        }
      },
      error: () => {
        this.saving = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Network error saving marks.' });
      }
    });
  }

  // Batch Entry Handlers
  loadBatchMarks(): void {
    if (!this.selectedSessionId || !this.selectedClassId || !this.selectedBatchSubjectId || !this.selectedBatchComponentId) {
      this.messageService.add({ severity: 'warn', summary: 'Incomplete Filter', detail: 'Please choose Session, Class, Subject, and Exam Component.' });
      return;
    }

    this.loading = true;
    this.markService.getBatchMarks(
      this.selectedSessionId,
      this.selectedClassId,
      this.selectedBatchSubjectId,
      this.selectedBatchComponentId,
      this.selectedSectionId || undefined
    ).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.isSuccess && res.data) {
          this.batchStudents = res.data;
        }
      },
      error: () => {
        this.loading = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load batch students.' });
      }
    });
  }

  saveBatchMarks(): void {
    if (!this.selectedSessionId || !this.selectedClassId || !this.selectedBatchSubjectId || !this.selectedBatchComponentId) {
      return;
    }

    this.saving = true;
    this.markService.saveBatchMarks({
      academicSessionId: this.selectedSessionId,
      classId: this.selectedClassId,
      sectionId: this.selectedSectionId || undefined,
      subjectId: this.selectedBatchSubjectId,
      examComponentId: this.selectedBatchComponentId,
      studentMarks: this.batchStudents
    }).subscribe({
      next: (res) => {
        this.saving = false;
        if (res.isSuccess) {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Batch marks saved successfully.' });
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: res.notificationMessage || 'Save failed.' });
        }
      },
      error: () => {
        this.saving = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to save batch marks.' });
      }
    });
  }

  printMarksheet(): void {
    window.print();
  }
}
