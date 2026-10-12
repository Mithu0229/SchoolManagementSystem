import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TrialBalanceItemDto, TrialBalanceService } from '../../services/trial-balance.service';

@Component({
  selector: 'app-trial-balance',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trial-balance.component.html',
  styleUrls: ['./trial-balance.component.scss'],
  providers: [DatePipe]
})
export class TrialBalanceComponent implements OnInit {

  fromDate: string = '';
  toDate: string = '';
  items: TrialBalanceItemDto[] = [];
  isLoading = false;

  constructor(
    private trialBalanceService: TrialBalanceService,
    private datePipe: DatePipe
  ) {
    const today = new Date();
    this.toDate = this.formatDate(today);
    
    // Set fromDate to first day of current month by default
    const firstDay = new Date(today.getFullYear(), today.getMonth(), 1);
    this.fromDate = this.formatDate(firstDay);
  }

  ngOnInit(): void {
    this.onSearch();
  }

  formatDate(date: Date): string {
    return this.datePipe.transform(date, 'yyyy-MM-dd') || '';
  }

  onSearch(): void {
    if (!this.fromDate || !this.toDate) {
      return;
    }

    this.isLoading = true;
    this.trialBalanceService.getTrialBalance(this.fromDate, this.toDate).subscribe({
      next: (res) => {
        this.items = res.items;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  get totalOpening(): number {
    return this.items.reduce((sum, item) => sum + (item.openingBalance || 0), 0);
  }

  get totalDebit(): number {
    return this.items.reduce((sum, item) => sum + (item.debitTrans || 0), 0);
  }

  get totalCredit(): number {
    return this.items.reduce((sum, item) => sum + (item.creditTrans || 0), 0);
  }

  isDownloadingExcel = false;

  downloadExcel(): void {
    if (!this.items || this.items.length === 0) {
      return;
    }

    this.isDownloadingExcel = true;

    try {
      const escapeXml = (str: string | number) => {
        if (str === null || str === undefined) return '';
        return String(str)
          .replace(/&/g, '&amp;')
          .replace(/</g, '&lt;')
          .replace(/>/g, '&gt;')
          .replace(/"/g, '&quot;')
          .replace(/'/g, '&apos;');
      };

      const fromStr = this.fromDate || 'All';
      const toStr = this.toDate || 'All';

      let rowsXml = '';

      // Data rows
      this.items.forEach((item) => {
        rowsXml += `
        <Row>
          <Cell ss:StyleID="CellText"><Data ss:Type="String">${escapeXml(item.particulars)}</Data></Cell>
          <Cell ss:StyleID="CellNumber"><Data ss:Type="Number">${Number(item.openingBalance || 0).toFixed(2)}</Data></Cell>
          <Cell ss:StyleID="CellNumber"><Data ss:Type="Number">${Number(item.debitTrans || 0).toFixed(2)}</Data></Cell>
          <Cell ss:StyleID="CellNumber"><Data ss:Type="Number">${Number(item.creditTrans || 0).toFixed(2)}</Data></Cell>
        </Row>`;
      });

      // Total row
      const totalRowXml = `
        <Row ss:StyleID="TotalRow">
          <Cell ss:StyleID="TotalText"><Data ss:Type="String">Total</Data></Cell>
          <Cell ss:StyleID="TotalNumber"><Data ss:Type="Number">${Number(this.totalOpening).toFixed(2)}</Data></Cell>
          <Cell ss:StyleID="TotalNumber"><Data ss:Type="Number">${Number(this.totalDebit).toFixed(2)}</Data></Cell>
          <Cell ss:StyleID="TotalNumber"><Data ss:Type="Number">${Number(this.totalCredit).toFixed(2)}</Data></Cell>
        </Row>`;

      const excelXml = `<?xml version="1.0"?>
<?mso-application progid="Excel.Sheet"?>
<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
 xmlns:o="urn:schemas-microsoft-com:office:office"
 xmlns:x="urn:schemas-microsoft-com:office:excel"
 xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
 xmlns:html="http://www.w3.org/TR/REC-html40">
 <Styles>
  <Style ss:ID="Default" ss:Name="Normal">
   <Alignment ss:Vertical="Center"/>
   <Borders/>
   <Font ss:FontName="Calibri" x:Family="Swiss" ss:Size="11" ss:Color="#000000"/>
   <Interior/>
   <NumberFormat/>
   <Protection/>
  </Style>
  <Style ss:ID="TitleStyle">
   <Font ss:FontName="Calibri" ss:Size="15" ss:Bold="1" ss:Color="#1E3A8A"/>
   <Alignment ss:Horizontal="Left" ss:Vertical="Center"/>
  </Style>
  <Style ss:ID="SubtitleStyle">
   <Font ss:FontName="Calibri" ss:Size="10" ss:Color="#475569" ss:Italic="1"/>
   <Alignment ss:Horizontal="Left" ss:Vertical="Center"/>
  </Style>
  <Style ss:ID="HeaderStyle">
   <Font ss:FontName="Calibri" ss:Size="11" ss:Bold="1" ss:Color="#FFFFFF"/>
   <Interior ss:Color="#1E3A8A" ss:Pattern="Solid"/>
   <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
   <Borders>
    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#0F172A"/>
    <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#0F172A"/>
    <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#0F172A"/>
    <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#0F172A"/>
   </Borders>
  </Style>
  <Style ss:ID="CellText">
   <Alignment ss:Horizontal="Left" ss:Vertical="Center"/>
   <Borders>
    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
    <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
    <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
    <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
   </Borders>
  </Style>
  <Style ss:ID="CellNumber">
   <Alignment ss:Horizontal="Right" ss:Vertical="Center"/>
   <NumberFormat ss:Format="#,##0.00"/>
   <Borders>
    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
    <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
    <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
    <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
   </Borders>
  </Style>
  <Style ss:ID="TotalText">
   <Font ss:FontName="Calibri" ss:Size="11" ss:Bold="1" ss:Color="#000000"/>
   <Interior ss:Color="#F1F5F9" ss:Pattern="Solid"/>
   <Alignment ss:Horizontal="Left" ss:Vertical="Center"/>
   <Borders>
    <Border ss:Position="Bottom" ss:LineStyle="Double" ss:Weight="3" ss:Color="#0F172A"/>
    <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="2" ss:Color="#0F172A"/>
    <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
    <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
   </Borders>
  </Style>
  <Style ss:ID="TotalNumber">
   <Font ss:FontName="Calibri" ss:Size="11" ss:Bold="1" ss:Color="#000000"/>
   <Interior ss:Color="#F1F5F9" ss:Pattern="Solid"/>
   <Alignment ss:Horizontal="Right" ss:Vertical="Center"/>
   <NumberFormat ss:Format="#,##0.00"/>
   <Borders>
    <Border ss:Position="Bottom" ss:LineStyle="Double" ss:Weight="3" ss:Color="#0F172A"/>
    <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="2" ss:Color="#0F172A"/>
    <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
    <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#CBD5E1"/>
   </Borders>
  </Style>
 </Styles>
 <Worksheet ss:Name="Trial Balance">
  <Table ss:ExpandedColumnCount="4" x:FullColumns="1" x:FullRows="1" ss:DefaultRowHeight="20">
   <Column ss:Width="220"/>
   <Column ss:Width="140"/>
   <Column ss:Width="140"/>
   <Column ss:Width="140"/>
   <Row ss:Height="26">
    <Cell ss:MergeAcross="3" ss:StyleID="TitleStyle"><Data ss:Type="String">Trial Balance Report</Data></Cell>
   </Row>
   <Row ss:Height="18">
    <Cell ss:MergeAcross="3" ss:StyleID="SubtitleStyle"><Data ss:Type="String">Period: ${escapeXml(fromStr)} to ${escapeXml(toStr)}</Data></Cell>
   </Row>
   <Row ss:Height="6"/>
   <Row ss:Height="22">
    <Cell ss:StyleID="HeaderStyle"><Data ss:Type="String">Particulars</Data></Cell>
    <Cell ss:StyleID="HeaderStyle"><Data ss:Type="String">Opening Balance</Data></Cell>
    <Cell ss:StyleID="HeaderStyle"><Data ss:Type="String">Debit Trans.</Data></Cell>
    <Cell ss:StyleID="HeaderStyle"><Data ss:Type="String">Credit Trans.</Data></Cell>
   </Row>
   ${rowsXml}
   ${totalRowXml}
  </Table>
 </Worksheet>
</Workbook>`;

      const blob = new Blob([excelXml], {
        type: 'application/vnd.ms-excel;charset=utf-8',
      });
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `Trial_Balance_${fromStr}_to_${toStr}.xls`;
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
    } catch (err) {
      console.error('Failed to export Excel', err);
    } finally {
      this.isDownloadingExcel = false;
    }
  }
}
