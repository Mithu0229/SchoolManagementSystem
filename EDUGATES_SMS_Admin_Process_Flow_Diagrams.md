# EDUGATES School Management System (ERP)
## Administrator Process Flow & Visual Diagram Guide

> **Official PDF Download:**  
> The complete 9-page vector-rendered visual diagram manual has been generated and is ready to download or view at:  
> **[EDUGATES_SMS_Admin_Process_Flow_Diagram_Guide.pdf](file:///d:/Company/Eninas/Projects/Noman/SchoolManagementSystem/EDUGATES_SMS_Admin_Process_Flow_Diagram_Guide.pdf)**

---

## 1. Master High-Level Administrator Process Pipeline

```mermaid
flowchart TD
    P1["<b>PHASE 01: Secure Login</b><br/>Input credentials, JWT verification, Branch context active<br/><i>(/auth/login)</i>"]
    P2["<b>PHASE 02: Master Setup</b><br/>Institute ➔ Branch ➔ FY ➔ Session ➔ Class ➔ Section ➔ Shift ➔ Fee Templates<br/><i>(/school/master)</i>"]
    P3["<b>PHASE 03: Student Onboarding</b><br/>Interactive Admission Form OR Excel Bulk Import ➔ Active Student Registry<br/><i>(/school/admission & /school/student-list)</i>"]
    P4["<b>PHASE 04: Billing Lifecycle</b><br/>Batch Bill Process ➔ Invoices ➔ Bill Update / Waivers ➔ Active Due Invoices<br/><i>(/school/bill-process & /school/bill-collection)</i>"]
    P5["<b>PHASE 05: Collections & Receipts</b><br/>Cash Counter OR Multi-Month Advance OR bKash ➔ 2-Copy Money Receipts<br/><i>(/school/bill-collection & /school/multi-month-bill-collection)</i>"]
    P6["<b>PHASE 06: Financial Accounting</b><br/>Cashbook & Bankbook ➔ General Ledger ➔ Trial Balance, P&L, Balance Sheet<br/><i>(/school/cashbook & /school/trial-balance)</i>"]
    P7["<b>PHASE 07: Exams & Admit Cards</b><br/>Exam Components ➔ Mark Entry & GPA ➔ Fee Gatekeeper ➔ Official Admit Card<br/><i>(/school/student-mark-entry & /api/AdmitCard)</i>"]
    P8["<b>PHASE 08: Daily Ops & Security</b><br/>Biometric Device Punch Sync ➔ Absent SMS to Parents ➔ RBAC Permissions<br/><i>(/api/AttendanceDevice & /home/gs)</i>"]

    P1 --> P2
    P2 --> P3
    P3 --> P4
    P4 --> P5
    P5 --> P6
    P6 --> P7
    P7 --> P8
```

---

## 2. Phase 1 & 2: Administrator Login & Master Setup Dependency Workflow

```mermaid
flowchart LR
    subgraph Auth["Phase 1: Login & Context Selection"]
        L1["Admin Credentials<br/>showkat131080@gmail.com"] --> L2["API Issues JWT Token<br/>Resolves Role Menus"]
        L2 --> L3["Select Active Branch<br/>(Locks TenantId & BranchId)"]
    end

    subgraph Master["Phase 2: Master Setup Sequential Order"]
        M1["1. Tenant"] --> M2["2. Institute"]
        M2 --> M3["3. Branch"]
        M3 --> M4["4. Financial Year"]
        M4 --> M5["5. Academic Session"]
        M5 --> M6["6. Classes"]
        M6 --> M7["7. Sections"]
        M7 --> M8["8. Shifts"]
        M8 --> M9["9. Student Groups"]
        M9 --> M10["10. Subjects"]
        M10 --> M11["11. Grading Scale"]
        M11 --> M12["12. Fee Heads"]
        M12 --> M13["13. Fee Templates"]
    end

    L3 --> M1
```

---

## 3. Phase 3: Student Onboarding Flowchart (Single Admission vs. Excel Bulk Import)

```mermaid
flowchart TD
    Start(["Start Student Onboarding"])

    Start --> Form["<b>Path A: Single Admission Form</b><br/>/school/admission<br/>• Select Branch, Session, Class, Section<br/>• Enter Student & Guardian details<br/>• Upload Photo (.jpg/.png)"]
    Start --> Excel["<b>Path B: Excel Bulk Import</b><br/>/school/student-list<br/>• Download Sample Excel (TestStudentUpload.xlsx)<br/>• Fill Name, Roll, Class, Section, Phone<br/>• Click Upload Student Excel"]

    Form --> Validate{"Core Validation Pipeline<br/>• Entity names match Master DB?<br/>• Phone is 11-digit mobile?<br/>• Roll unique within Section?"}
    Excel --> Validate

    Validate -- Error --> Fix["Display Validation Errors<br/>Operator corrects missing data"] --> Validate
    Validate -- Valid --> Generate["<b>Save Student Record</b><br/>• Generate GUID Student ID<br/>• Assign Academic Roll Number<br/>• Save Photo to /uploads/students/"]

    Generate --> Registry["<b>Active Student Registry</b><br/>/school/student-list<br/>• Search, view profile, manage guardians<br/>• Export registry to Excel<br/>• Student Promotions"]
```

---

## 4. Phase 4: Billing Lifecycle, Bill Updates & Collection Channels

```mermaid
flowchart TD
    Tpl["Active Fee Template<br/>Tuition, Sports, Lab fees mapped to Class & Shift"] --> Gen["<b>Step 1: Batch Bill Process</b><br/>/school/bill-process<br/>• Select Session, Month (e.g. November), Class<br/>• Generate BillMaster & BillDetails for all students"]

    Gen --> Adj{"<b>Decision: Need Adjustments?</b><br/>Scholarship, waiver, penalty"}

    Adj -- Yes --> Update["<b>Step 2: Bill Update</b><br/>/school/bill-collection (Update Bill)<br/>• Modify specific fee head amounts<br/>• Apply discount or waiver"] --> Collect
    Adj -- No --> Collect["<b>Step 3: Ready for Collection</b><br/>Invoice active with status 'Unpaid'"]

    Collect --> Ch1["<b>Channel A: Single Collection</b><br/>/school/bill-collection<br/>Search Roll ➔ Collect cash ➔ Pay Now"]
    Collect --> Ch2["<b>Channel B: Multi-Month Advance</b><br/>/school/multi-month-bill-collection<br/>Select 3, 6, 12 months in advance"]
    Collect --> Ch3["<b>Channel C: Online bKash / PayBill</b><br/>Parent pays via bKash / bank gateway<br/>System verifies TxnID automatically"]

    Ch1 --> Post["<b>Automated Transaction Completion</b><br/>1. Print instant 2-Copy Money Receipt (Office & Student Copy)<br/>2. Debit Cashbook (/school/cashbook) or Bankbook (/school/bankbook)<br/>3. Mark Bill as 'Paid' in Student Bill History"]
    Ch2 --> Post
    Ch3 --> Post
```

---

## 5. Phase 5: Financial Accounting Flowchart

```mermaid
flowchart LR
    subgraph Inflows["Revenue Inflows"]
        Cash["Counter Cash Payments"]
        Bank["Online bKash & Bank Deposits"]
    end

    subgraph Journals["Books of Original Entry"]
        Cash --> CB["Daily Cashbook<br/>/school/cashbook"]
        Bank --> BB["Bankbook<br/>/school/bankbook"]
    end

    subgraph Ledger["General Ledger Consolidation"]
        CB --> TB["Real-Time Trial Balance<br/>/school/trial-balance<br/>(Total Debits = Total Credits)"]
        BB --> TB
    end

    subgraph Statements["Fiscal Financial Statements"]
        TB --> IS["Income Statement / P&L<br/>/school/income-statement<br/>(Revenues - Operating Expenses)"]
        TB --> BS["Balance Sheet<br/>/school/balance-sheet<br/>(Assets = Liabilities + Equity)"]
    end
```

---

## 6. Phase 6: Examinations, Marks Entry & Fee-Gated Admit Cards

```mermaid
flowchart TD
    E1["<b>Step 1: Configure Exam Components</b><br/>Define exam type (CT, Term Final) & components (CQ, MCQ, Practical)"] --> E2["<b>Step 2: Batch Student Mark Entry</b><br/>/school/student-mark-entry<br/>Filter Session, Class, Section, Subject ➔ Enter scores"]

    E2 --> E3["<b>Step 3: Automated GPA Engine</b><br/>System dynamically calculates Total Marks, Letter Grade (A+, A, B, F) & GPA (5.0 - 0.0)"]

    E3 --> Req["<b>Step 4: Student Requests Admit Card</b><br/>/api/AdmitCard/download/{studentId}/{examType}"]

    Req --> Gate{"<b>Gatekeeper Decision:</b><br/>Has the student cleared all tuition fees?"}

    Gate -- "YES (Fees Paid)" --> Pass["<b>Issue Official Admit Card</b><br/>• Generated via QuestPDF<br/>• Includes Student Photo, Barcode, Exam Schedule<br/>• Printed and stamped for hall entry"]

    Gate -- "NO (Outstanding Dues)" --> Block["<b>Fee Block Active!</b><br/>• System blocks admit card issuance<br/>• Parent directed to cashier counter<br/>• <i>Admin Override: Check 'IgnoreDueCheck=true'</i>"]
```

---

## 7. Phase 7: Role Permissions (RBAC), Biometric Devices & Automated SMS

```mermaid
flowchart TD
    subgraph Security["1. Role-Based Access Control (/role)"]
        R1["Assign Role: Super Admin, Branch Admin, Accountant, Teacher"]
        R2["Toggle Granular Flags: CanView, CanAdd, CanEdit, CanDelete, CanPreview, CanExport, CanPrint"]
        R1 --> R2
    end

    subgraph Hardware["2. Biometric Attendance Sync"]
        D1["Biometric Hardware Devices on Campus Network"] --> D2["Hardware DB: AttendanceConnection (edugatesdvcn)"]
        D2 --> D3["System Sync Engine: Match Student & Teacher IDs ➔ Daily Roster"]
    end

    subgraph SMS["3. Proactive SMS Broadcast Gateway"]
        D3 -- "Unpunched at 10:00 AM" --> S1["Absent Alert SMS to Parent Mobile"]
        Cashier["Fee Collection Counter"] -- "Payment Received" --> S2["Payment Confirmation Receipt SMS"]
        Monthly["15th of Every Month"] -- "Unpaid Invoices" --> S3["Due Fee Reminder Batch SMS"]
    end
```
