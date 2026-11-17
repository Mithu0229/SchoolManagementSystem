
--SELECT * FROM tb_gs_Users tgu
---step 1
INSERT INTO [tb_gs_Users]
(
    Id, IsActive, IsDeleted, DeletedById, DeletedDate,
    CreatedById, CreatedDate, ModifiedById, ModifiedDate,
    FirstName, LastName, Email, PhoneNumber, Password,
    UserType, Address, TenantId, StudentId
)
VALUES
(
    'b54c6a36-8684-47d1-b9c9-5a63cd9c41bf',
    1,
    0,
    NULL,
    NULL,
    'c0e863bf-7bf3-4fe6-8206-0523467d46b9',
    '2026-11-16 15:06:20.6666699',
    'c0e863bf-7bf3-4fe6-8206-0523467d46b9',
    '2026-11-16 15:08:13.9412686',
    N'Showkat',
    N'Akbar',
    N'showkat131080@gmail.com',
    N'01817676673',
    N'$2a$11$jQEf4nucW6VlYt/ka1rQLuQAqZSle7f80lX8HWmLf3JBnlG7.9LSi',
    1,
    N'Baridhara J block,dhaka',
    NULL,
    NULL
);
GO

---step 2
INSERT INTO [dbo].[tb_gs_Roles] (Id, Description, CreatedDate, ModifiedDate, CreatedById, DeleteRequestedOn, DeletedById, DeletedDate, IsActive, IsDeleted, ModifiedById, RoleName, RoleType, TenantId) VALUES
('480dc803-3176-4f1e-9a4f-9ea9e8cb25c8', N'All Power', CONVERT(DATETIME2, '2025-11-02 17:22:00.3035751', 121), CONVERT(DATETIME2, '2026-08-03 15:22:13.0855727', 121), '184a4281-1484-4ded-b978-79bb89a67c81', NULL, NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), 'ba4060c9-35ba-498f-9105-52d466fa3bb3', N'Super Admin', 2, NULL)
GO

INSERT INTO [tb_gs_Roles] (Id, Description, CreatedDate, ModifiedDate, CreatedById, DeleteRequestedOn, DeletedById, DeletedDate, IsActive, IsDeleted, ModifiedById, RoleName, RoleType, TenantId) VALUES
('990fdd19-959f-4986-a889-70faac5d7cd4', N'Branch Admin', CONVERT(DATETIME2, '2025-10-27 09:56:15.7733333', 121), CONVERT(DATETIME2, '2025-11-02 17:23:03.2318323', 121), '00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), '184a4281-1484-4ded-b978-79bb89a67c81', N'Branch', 1, NULL)
GO

---step 3
INSERT INTO [tb_gs_UserRoles] (Id, IsActive, IsDeleted, DeletedById, DeletedDate, CreatedById, CreatedDate, ModifiedById, ModifiedDate, TenantId, UserId, RoleId) VALUES
('5edb4f3c-8fbf-4658-9335-c99a461602c6', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'c0e863bf-7bf3-4fe6-8206-0523467d46b9', CONVERT(DATETIME2, '2026-11-16 15:08:13.8974077', 121), NULL, NULL, NULL, 'b54c6a36-8684-47d1-b9c9-5a63cd9c41bf', '480dc803-3176-4f1e-9a4f-9ea9e8cb25c8')
GO


INSERT INTO tb_gs_Sitemaps (Id, IsActive, IsDeleted, DeletedById, DeletedDate, CreatedById, CreatedDate, ModifiedById, ModifiedDate, Name, FavIcon, PageUrl, SortingOrder, ParentId, IsFeature, IsSidebarmenu, MenuType, MenuAccessType) VALUES
('7e0a08b0-ac66-44be-bb43-4f23157d1d78', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '184a4281-1484-4ded-b978-79bb89a67c81', CONVERT(DATETIME2, '2025-11-02 17:29:53.8928635', 121), 'c0e863bf-7bf3-4fe6-8206-0523467d46b9', CONVERT(DATETIME2, '2026-08-02 17:46:01.2973411', 121), N'Tenant', N'pi pi-circle', N'/tenant', 1, '8afee2b8-2870-4f36-90e3-7349e62427f1', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 2)
GO

---step 5
INSERT INTO [tb_gs_RoleMenus] (Id, IsActive, IsDeleted, DeletedById, DeletedDate, CreatedById, CreatedDate, ModifiedById, ModifiedDate, TenantId, RoleId, SitemapId, CanView, CanAdd, CanEdit, CanDelete, CanPreview, CanExport, CanPrint) VALUES
('833cfbdb-57af-4e7d-80af-c5035efef0b9', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'ba4060c9-35ba-498f-9105-52d466fa3bb3', CONVERT(DATETIME2, '2026-08-03 15:22:13.1359832', 121), NULL, NULL, NULL, '480dc803-3176-4f1e-9a4f-9ea9e8cb25c8', '7e0a08b0-ac66-44be-bb43-4f23157d1d78', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'))
GO

INSERT INTO tb_gs_RoleMenus (Id, IsActive, IsDeleted, DeletedById, DeletedDate, CreatedById, CreatedDate, ModifiedById, ModifiedDate, TenantId, RoleId, SitemapId, CanView, CanAdd, CanEdit, CanDelete, CanPreview, CanExport, CanPrint) VALUES
('8c5b261c-2f97-4919-9da4-4f55ae53f372', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'ba4060c9-35ba-498f-9105-52d466fa3bb3', CONVERT(DATETIME2, '2026-08-03 15:22:13.1359799', 121), NULL, NULL, NULL, '480dc803-3176-4f1e-9a4f-9ea9e8cb25c8', '3cbb3270-9527-4015-bb14-23b6e7fcb5a4', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'))
GO

INSERT INTO [tb_gs_RoleMenus] (Id, IsActive, IsDeleted, DeletedById, DeletedDate, CreatedById, CreatedDate, ModifiedById, ModifiedDate, TenantId, RoleId, SitemapId, CanView, CanAdd, CanEdit, CanDelete, CanPreview, CanExport, CanPrint) VALUES
('ed876582-d751-47ad-aafd-6152fc6e1211', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'ba4060c9-35ba-498f-9105-52d466fa3bb3', CONVERT(DATETIME2, '2026-08-03 15:22:13.1359812', 121), NULL, NULL, NULL, '480dc803-3176-4f1e-9a4f-9ea9e8cb25c8', 'f556e836-9139-45f4-b541-100f23489cc9', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'True'))
GO

--select * FROM tb_gs_Sitemaps tgs

---step 4
INSERT INTO [tb_gs_Sitemaps] (Id, IsActive, IsDeleted, DeletedById, DeletedDate, CreatedById, CreatedDate, ModifiedById, ModifiedDate, Name, FavIcon, PageUrl, SortingOrder, ParentId, IsFeature, IsSidebarmenu, MenuType, MenuAccessType, TenantId) VALUES
('3d74250b-b950-457d-909c-0689f4b0f0ab', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '00000000-0000-0000-0000-000000000000', CONVERT(DATETIME2, '2025-10-29 00:21:41.6739540', 121), NULL, NULL, N'Sitemaps', N'pi pi-circle', N'/sitemap', 3, 'b2c3d4e5-6f7a-8b9c-0d1e-f2a3b4c5d6e8', CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 2, NULL),
('23099701-d78c-4d85-83b5-0a81293c55ec', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 06:12:51.8348312', 121), NULL, NULL, N'Fee Head', N'pi pi-circle', N'/school/fee-head', 9, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('c7cbda9e-9341-41ed-bf87-0cac9e611250', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 05:29:27.7198942', 121), NULL, NULL, N'Class', N'pi pi-circle', N'/school/academic-class', 5, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('f556e836-9139-45f4-b541-100f23489cc9', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '0ff409c1-4045-4083-b7f4-53e8c5166cda', CONVERT(DATETIME2, '2025-11-01 18:29:17.0454312', 121), 'fa8ec5d5-51c6-407a-9bef-3976badf80ef', CONVERT(DATETIME2, '2025-11-18 10:27:00.9006353', 121), N'Roles', N'pi pi-circle', N'/role', 4, 'b2c3d4e5-6f7a-8b9c-0d1e-f2a3b4c5d6e8', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 2, NULL),
('6b4434bd-fa41-46dc-8988-14dced11ca11', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 05:30:41.5115175', 121), NULL, NULL, N'Section', N'pi pi-circle', N'/school/section', 6, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('ee661595-9177-4797-919d-180d79833b01', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 05:21:04.8489436', 121), '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 05:23:04.1689713', 121), N'Institue', N'pi pi-circle', N'/school/institute', 1, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('beabff1a-e339-416b-a3c5-1bc69e690226', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'c0e863bf-7bf3-4fe6-8206-0523467d46b9', CONVERT(DATETIME2, '2026-08-31 14:42:09.0127339', 121), 'ba4060c9-35ba-498f-9105-52d466fa3bb3', CONVERT(DATETIME2, '2026-09-16 00:21:00.4542756', 121), N'Bill Collection', N'pi pi-circle', N'/school/multi-month-bill-collection', 4, 'cfeb78df-5cc6-46fb-a1d9-c7868e4d1f5c', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('3cbb3270-9527-4015-bb14-23b6e7fcb5a4', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '0ff409c1-4045-4083-b7f4-53e8c5166cda', CONVERT(DATETIME2, '2025-11-01 13:24:10.2971574', 121), NULL, NULL, N'Members', N'pi pi-circle', N'/admin/member-list', 3, 'b2c3d4e5-6f7a-8b9c-0d1e-f2a3b4c5d6e8', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 2, NULL),
('fb4d5595-693e-40eb-a2dc-25f52b5a8f10', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 05:01:33.4789500', 121), 'ba4060c9-35ba-498f-9105-52d466fa3bb3', CONVERT(DATETIME2, '2026-09-16 00:25:07.8483689', 121), N'School Management System', N'pi pi-building', N'/school', 2, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), 5, 3, NULL),
('9bf4c4aa-9317-4bd3-844c-2f7626a3c915', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '00000000-0000-0000-0000-000000000000', CONVERT(DATETIME2, '2025-10-26 12:35:30.0818108', 121), NULL, NULL, N'Settings', N'pi pi-cog', N'/home/gs', 1, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'True'), 5, 3, NULL),
('7e0a08b0-ac66-44be-bb43-4f23157d1d78', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '184a4281-1484-4ded-b978-79bb89a67c81', CONVERT(DATETIME2, '2025-11-02 17:29:53.8928635', 121), 'c0e863bf-7bf3-4fe6-8206-0523467d46b9', CONVERT(DATETIME2, '2026-08-02 17:46:01.2973411', 121), N'Tenant', N'pi pi-circle', N'/tenant', 1, '8afee2b8-2870-4f36-90e3-7349e62427f1', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 2, NULL),
('808c608d-d384-4725-9247-51a2a3a48bbf', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 06:11:28.4294912', 121), NULL, NULL, N'Student Group', N'pi pi-circle', N'/school/student-group', 8, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('227edf50-923b-4d7d-8719-5321e991e8b4', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'ba4060c9-35ba-498f-9105-52d466fa3bb3', CONVERT(DATETIME2, '2026-08-12 17:37:57.0337113', 121), NULL, NULL, N'Paid Bill', N'pi pi-circle', N'/school/paid-bill-list', 15, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 2, NULL),
('d1111111-2222-3333-4444-555555555501', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-10-17 23:53:52.7100000', 121), NULL, NULL, N'Grading Scale', N'pi pi-chart-bar', N'/school/grading-scale', 15, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('d1111111-2222-3333-4444-555555555502', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-10-17 23:53:52.7400000', 121), NULL, NULL, N'Subject', N'pi pi-book', N'/school/subject', 16, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('d1111111-2222-3333-4444-555555555503', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-10-17 23:53:52.7566667', 121), NULL, NULL, N'Exam Marks', N'pi pi-file-edit', N'/school/exam-marks', 10, 'cfeb78df-5cc6-46fb-a1d9-c7868e4d1f5c', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('828262d9-77b2-4a1e-91dc-58448d71024e', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 06:18:33.1549681', 121), 'ba4060c9-35ba-498f-9105-52d466fa3bb3', CONVERT(DATETIME2, '2026-09-16 00:19:15.4342902', 121), N'Admission', N'pi pi-circle', N'/school/admission', 2, 'cfeb78df-5cc6-46fb-a1d9-c7868e4d1f5c', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('2240783e-662f-4424-8775-68ed4b6d10d1', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 06:16:59.1197922', 121), NULL, NULL, N'Student', N'pi pi-circle', N'/school/student', 14, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('8afee2b8-2870-4f36-90e3-7349e62427f1', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '184a4281-1484-4ded-b978-79bb89a67c81', CONVERT(DATETIME2, '2025-11-02 17:27:59.1360524', 121), NULL, NULL, N'Branch Management', N'pi pi-cog', N'/branch/home', 2, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'True'), 5, 1, NULL),
('43729442-b181-48af-819e-7b653f86b903', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'fa8ec5d5-51c6-407a-9bef-3976badf80ef', CONVERT(DATETIME2, '2025-12-11 14:49:57.2014647', 121), NULL, NULL, N'Student Application', N'pi pi-circle', N'/application', 5, 'b2c3d4e5-6f7a-8b9c-0d1e-f2a3b4c5d6e8', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 2, NULL),
('2f0c44c9-c0fa-4f72-9f20-96b169d22894', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 06:16:02.9080434', 121), NULL, NULL, N'Student Fee Ledger', N'pi pi-circle', N'/school/student-fee-ledger', 13, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('df8f5d00-3a7f-4384-88b3-975e84715c36', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 06:10:16.5916617', 121), NULL, NULL, N'Shift', N'pi pi-circle', N'/school/shift', 7, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 05:09:17.7628336', 121), NULL, NULL, N'Master', N'pi pi-cog', N'/school/', 1, 'fb4d5595-693e-40eb-a2dc-25f52b5a8f10', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 5, 3, NULL),
('cc2e49ac-5c06-4527-a139-a80911fac840', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'c0e863bf-7bf3-4fe6-8206-0523467d46b9', CONVERT(DATETIME2, '2026-08-07 15:29:16.4213849', 121), 'ba4060c9-35ba-498f-9105-52d466fa3bb3', CONVERT(DATETIME2, '2026-09-16 00:22:46.5901756', 121), N'Student list', N'pi pi-circle', N'/school/student-list', 1, 'cfeb78df-5cc6-46fb-a1d9-c7868e4d1f5c', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 2, NULL),
('42a97d51-33b9-4229-9b19-b3e081e06ca8', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-07-04 10:13:44.3866886', 121), 'ba4060c9-35ba-498f-9105-52d466fa3bb3', CONVERT(DATETIME2, '2026-09-16 00:21:23.8388804', 121), N'Update Bill', N'pi pi-circle', N'/school/bill-collection', 3, 'cfeb78df-5cc6-46fb-a1d9-c7868e4d1f5c', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('28073bb8-bf14-4dc3-a13f-bb02b1da4d68', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 05:26:59.0632897', 121), NULL, NULL, N'Financial Year', N'pi pi-circle', N'/school/financial-year', 3, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('cfeb78df-5cc6-46fb-a1d9-c7868e4d1f5c', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'ba4060c9-35ba-498f-9105-52d466fa3bb3', CONVERT(DATETIME2, '2026-09-16 00:15:35.9830593', 121), NULL, NULL, N'Process', N'pi pi-cog', N'/school/process/', 2, 'fb4d5595-693e-40eb-a2dc-25f52b5a8f10', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 5, 3, NULL),
('9155a984-25c9-49dc-8049-c88a1f634268', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'a88ef7b5-7cd8-479a-8bd3-3786c7820afd', CONVERT(DATETIME2, '2026-03-14 07:46:56.5581743', 121), 'a88ef7b5-7cd8-479a-8bd3-3786c7820afd', CONVERT(DATETIME2, '2026-03-14 07:53:53.7962490', 121), N'Profile', N'pi pi-circle', N'/application/update-student-info/8d76923f-c852-4b7c-9f60-df68420d76f8', 6, '43729442-b181-48af-819e-7b653f86b903', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 2, NULL),
('d4396f73-f487-404a-8ae3-ccee40d14b23', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 05:24:51.1971329', 121), NULL, NULL, N'Branch', N'pi pi-circle', N'/school/branch', 2, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('d4544948-ef33-4e7e-9f9b-e19f49b03507', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 06:14:52.8691262', 121), NULL, NULL, N'Fee Collection', N'pi pi-circle', N'/school/fee-collection', 12, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('0f58a5e1-f7cb-47b3-aff1-e2ffe64f75f5', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 06:14:02.8760164', 121), NULL, NULL, N'Fee Template', N'pi pi-circle', N'/school/fee-template', 10, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('b2c3d4e5-6f7a-8b9c-0d1e-f2a3b4c5d6e8', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '00000000-0000-0000-0000-000000000000', CONVERT(DATETIME2, '2025-10-26 13:00:42.5561505', 121), NULL, NULL, N'User Management', N'pi pi-shield', N'home/usr-management', 1, '9bf4c4aa-9317-4bd3-844c-2f7626a3c915', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 5, 3, NULL),
('ce55f8bd-ed21-4680-a591-fe762e248d68', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, '14311103-4501-4f5e-95f6-c6153c530de5', CONVERT(DATETIME2, '2026-05-15 05:27:59.0558476', 121), NULL, NULL, N'Session', N'pi pi-circle', N'/school/academic-session', 4, 'ece96d01-3fe1-43f1-9c8f-9e08daa64397', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL),
('5d087518-e58e-46b2-991e-ff8960252b1c', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'ba4060c9-35ba-498f-9105-52d466fa3bb3', CONVERT(DATETIME2, '2026-10-01 01:00:39.6046404', 121), NULL, NULL, N'Bill History', N'pi pi-circle', N'/school/student-bill-history', 7, 'cfeb78df-5cc6-46fb-a1d9-c7868e4d1f5c', CONVERT(bit, 'False'), CONVERT(bit, 'True'), 1, 3, NULL)
GO

--SELECT * FROM tb_gs_Tenants tgt

--INSERT INTO [tb_gs_Tenants] (Id, IsActive, IsDeleted, DeletedById, DeletedDate, CreatedById, CreatedDate, ModifiedById, ModifiedDate, TenantName, BinNo, TenantEmail, PhoneNumber, Domain, Street, City, Province, PostCode, Reason, tenantId) VALUES
--('1d01de60-178b-476b-bc14-84515136894a', CONVERT(bit, 'True'), CONVERT(bit, 'False'), NULL, NULL, 'c0e863bf-7bf3-4fe6-8206-0523467d46b9', CONVERT(DATETIME2, '2026-11-15 15:36:25.3490860', 121), NULL, NULL, N'AK', N'bb solution', N'akschool@gmail.com', N'01817676673', N'bb solution', N'Satkania', N'Chittagong', N'', N'4386', NULL, NULL)
--GO



--SELECT * FROM dbo.tb_gs_Roles tgr

---view---

CREATE OR ALTER VIEW [dbo].[vGetTreeViewSitemap]
AS
WITH tree_view AS
(
    -- Root nodes (ParentId IS NULL)
    SELECT 
        s.Id,
        s.Name,
        s.FavIcon,
        s.PageUrl,
        s.SortingOrder,
        s.ParentId,
        0 AS [Level],
        CAST(ROW_NUMBER() OVER (ORDER BY s.SortingOrder) AS VARCHAR(100)) AS OrderSequence,
        CAST(s.Name AS VARCHAR(500)) AS NameWithParent,
        s.IsActive,
        s.IsFeature,
        s.IsSidebarmenu,
        s.MenuType,
        s.MenuAccessType
    FROM dbo.tb_gs_Sitemaps AS s
    WHERE s.ParentId IS NULL 
      AND s.IsDeleted = 0

    UNION ALL

    -- Child nodes (recursive)
    SELECT 
        c.Id,
        c.Name,
        c.FavIcon,
        c.PageUrl,
        c.SortingOrder,
        c.ParentId,
        (p.[Level] + 1) AS [Level],
        CAST(p.OrderSequence + '_' + CAST(ROW_NUMBER() OVER (ORDER BY c.SortingOrder) AS VARCHAR(10)) AS VARCHAR(100)) AS OrderSequence,
        CAST(p.NameWithParent + ' >> ' + REPLACE(c.Name, ' ', '-') AS VARCHAR(500)) AS NameWithParent,
        c.IsActive,
        c.IsFeature,
        c.IsSidebarmenu,
        c.MenuType,
        c.MenuAccessType
    FROM dbo.tb_gs_Sitemaps AS c
    INNER JOIN tree_view AS p ON c.ParentId = p.Id
    WHERE c.IsDeleted = 0
)
SELECT 
    Id,
    Name,
    FavIcon,
    PageUrl,
    SortingOrder,
    ParentId,
    [Level],
    OrderSequence,
    NameWithParent,
    IsActive,
    IsFeature,
    IsSidebarmenu,
    MenuType,
    MenuAccessType
FROM tree_view;
GO


GO

CREATE OR ALTER   VIEW [dbo].[vGetParentMenus]
AS
SELECT 
    Id,
    NameWithParent,
    OrderSequence
FROM dbo.vGetTreeViewSitemap;
GO


CREATE OR ALTER VIEW [dbo].[vw_leaf_sitemaps]
        AS
        SELECT 
            Id,
            Name,
            FavIcon,
            PageUrl,
            SortingOrder,
            ParentId,
            IsFeature,
            CreatedDate,
            MenuType,
            MenuAccessType
        FROM dbo.tb_gs_Sitemaps sm
        WHERE IsActive = 1 AND IsDeleted = 0;
GO





----function-----------
---step 6
CREATE FUNCTION [dbo].[fn_generate_order_by]
(
    @Sort NVARCHAR(255),
    @Direction NVARCHAR(10),
    @p_allowed_columns NVARCHAR(MAX) -- comma-separated list of allowed columns
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @v_final_direction NVARCHAR(4) = 'ASC';
    DECLARE @v_col NVARCHAR(255);
    DECLARE @result NVARCHAR(MAX) = NULL;

    -- Set direction
    IF LOWER(@Direction) = 'desc'
        SET @v_final_direction = 'DESC';

    -- If Sort is NULL or empty, default to ORDER BY 1 ASC
    IF (@Sort IS NULL OR LTRIM(RTRIM(@Sort)) = '')
    BEGIN
        RETURN 'ORDER BY 1 ASC';
    END

    -- Loop through allowed columns
    DECLARE cur CURSOR FOR
        SELECT LTRIM(RTRIM(value))
        FROM STRING_SPLIT(@p_allowed_columns, ',');

    OPEN cur;
    FETCH NEXT FROM cur INTO @v_col;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF LOWER(@v_col) = LOWER(@Sort)
        BEGIN
            SET @result = 'ORDER BY [' + @v_col + '] ' + @v_final_direction;
            CLOSE cur;
            DEALLOCATE cur;
            RETURN @result;
        END
        FETCH NEXT FROM cur INTO @v_col;
    END

    CLOSE cur;
    DEALLOCATE cur;

    -- Default if no match found
    RETURN 'ORDER BY 1 ASC';
END;

GO
CREATE FUNCTION [dbo].[fn_generate_search_filter]
(
    @Search NVARCHAR(MAX),
    @Columns NVARCHAR(MAX) -- comma-separated column names: 'Name,Email,Status'
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Col NVARCHAR(255)
    DECLARE @Pos INT
    DECLARE @PrevPos INT = 0
    DECLARE @Len INT = LEN(@Columns)
    DECLARE @Conditions NVARCHAR(MAX) = ''

    IF @Search IS NULL OR LTRIM(RTRIM(@Search)) = ''
        RETURN '1=1'

    -- Parse comma-separated column list
    WHILE CHARINDEX(',', @Columns, @PrevPos + 1) > 0
    BEGIN
        SET @Pos = CHARINDEX(',', @Columns, @PrevPos + 1)
        SET @Col = LTRIM(RTRIM(SUBSTRING(@Columns, @PrevPos + 1, @Pos - @PrevPos - 1)))

        IF @Conditions <> ''
            SET @Conditions += ' OR '

        SET @Conditions += 'LOWER(' + QUOTENAME(@Col) + ') LIKE ''%' + LOWER(@Search) + '%'''

        SET @PrevPos = @Pos
    END

    -- Last column (or only one if no commas)
    SET @Col = LTRIM(RTRIM(SUBSTRING(@Columns, @PrevPos + 1, @Len)))
    IF @Col <> ''
    BEGIN
        IF @Conditions <> ''
            SET @Conditions += ' OR '
        SET @Conditions += 'LOWER(' + QUOTENAME(@Col) + ') LIKE ''%' + LOWER(@Search) + '%'''
    END

    -- Additional condition for "active"/"inactive"
    IF @Search LIKE '%active%' AND @Search NOT LIKE '%inactive%'
        SET @Conditions += ' OR [IsActive] = 1'
    ELSE IF @Search LIKE '%inactive%'
        SET @Conditions += ' OR [IsActive] = 0'

    RETURN '(' + @Conditions + ')'
END

GO


---sp-----------------
---step 7

CREATE OR ALTER   PROCEDURE [dbo].[CreateOrReplace_vw_leaf_sitemaps]
AS
BEGIN
    SET NOCOUNT ON;

    -- If the view already exists, drop it first
    IF OBJECT_ID('dbo.vw_leaf_sitemaps', 'V') IS NOT NULL
        DROP VIEW dbo.vw_leaf_sitemaps;
    
    -- Create the view again
    EXEC('
        CREATE VIEW dbo.vw_leaf_sitemaps
        AS
        SELECT 
            Id,
            Name,
            FavIcon,
            PageUrl,
            SortingOrder,
            ParentId,
            IsFeature,
            CreatedDate,
            MenuType,
            MenuAccessType
        FROM dbo.tb_gs_Sitemaps sm
        WHERE IsActive = 1 AND IsDeleted = 0;
    ');
END;

go

CREATE OR ALTER PROCEDURE [dbo].[sp_get_users]
    @Page INT,
    @Length INT,
    @Sort NVARCHAR(50),
    @Direction NVARCHAR(4),
    @Search NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Sql NVARCHAR(MAX);
    DECLARE @Where NVARCHAR(MAX) = '1=1';
    DECLARE @OrderBy NVARCHAR(MAX);
    DECLARE @Offset INT = @Page * @Length;

    -- Add search filter using custom function
    DECLARE @Filter NVARCHAR(MAX) = dbo.fn_generate_search_filter(@Search, 'FirstName,LastName,Email,PhoneNumber,UserType,Status');
    IF @Filter IS NOT NULL AND LEN(@Filter) > 0
        SET @Where += N' AND (' + @Filter + ')';

    -- Always generate ORDER BY (function will default to ORDER BY 1 ASC if empty)
    SET @OrderBy = dbo.fn_generate_order_by(@Sort, @Direction, 'FirstName,LastName,Email,PhoneNumber,UserTypeName,UserType');

    -- Build final SQL
    SET @Sql = '
        SELECT 
            d.Id,
            d.FirstName,
            d.LastName,
			d.Email,
			d.PhoneNumber,
			d.UserType,
			CASE 
            WHEN d.UserType = 2 THEN ''Admin''
            WHEN d.UserType = 3 THEN ''Member''
            ELSE ''Other''
        END AS UserTypeName,
        d.Address,
        CASE 
            WHEN d.IsActive = 1 THEN ''Authorized''
            ELSE ''Pending''
        END AS Status
        FROM dbo.tb_gs_Users d
        WHERE ' + @Where + '
        ' + @OrderBy + '
        OFFSET ' + CAST(@Offset AS NVARCHAR) + ' ROWS
        FETCH NEXT ' + CAST(@Length AS NVARCHAR) + ' ROWS ONLY;
    ';

    EXEC sp_executesql @Sql;
END;

GO
CREATE OR ALTER PROCEDURE [dbo].[sp_get_users_count]
    @Search NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @Where NVARCHAR(MAX)= '1=1';
    DECLARE @SearchFilter NVARCHAR(MAX);

    -- Get search filter from reusable function
    SET @SearchFilter = dbo.fn_generate_search_filter(@Search,'FirstName,LastName,Email,PhoneNumber,UserType,Address');

    -- Append to WHERE if search filter is present
    IF @SearchFilter IS NOT NULL AND LTRIM(RTRIM(@SearchFilter)) <> ''
    BEGIN
        SET @Where += N' AND (' + @SearchFilter + ')';
    END

    -- Final SQL
    SET @SQL = N'
        SELECT COUNT(1) AS totalrecord
        FROM dbo.tb_gs_Users d
        WHERE ' + @Where;

    EXEC sp_executesql @SQL;
END

GO

CREATE   PROCEDURE [dbo].[sp_GetRoles]  
(  
    @Page INT,  
    @Length INT,  
    @Sort NVARCHAR(100) = NULL,  
    @Direction NVARCHAR(10) = 'ASC',  
    @Search NVARCHAR(200) = NULL,  
    @FiltersText NVARCHAR(MAX) = NULL,  -- optional JSON filters  
    @TenantId UNIQUEIDENTIFIER = NULL  
)  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    DECLARE   
        @Sql NVARCHAR(MAX),  
        @Where NVARCHAR(MAX) = N' r.IsDeleted = 0 ',  
        @OrderBy NVARCHAR(MAX),  
        @Offset INT = @Page * @Length;  
  
    -----------------------------------------------------  
    -- Tenant filtering  
    -----------------------------------------------------  
    IF @TenantId IS NOT NULL  
        SET @Where = @Where + N' AND r.RoleType IN (2,3) AND r.TenantId = ''' + CAST(@TenantId AS NVARCHAR(36)) + '''';  
  
    -----------------------------------------------------  
    -- Search filter  
    -----------------------------------------------------  
    IF @Search IS NOT NULL AND LTRIM(RTRIM(@Search)) <> ''  
        SET @Where = @Where + N' AND (  
            r.RoleName LIKE ''%' + @Search + '%'' OR  
            t.TenantName LIKE ''%' + @Search + '%''  
        )';  
  
    -----------------------------------------------------  
    -- (Optional) dynamic JSON filters placeholder  
    -- You can extend this if you parse @FiltersText (e.g., using OPENJSON)  
    -----------------------------------------------------  
    -- Example:  
    -- IF @FiltersText IS NOT NULL AND ISJSON(@FiltersText) = 1  
    -- BEGIN  
    --     -- parse custom filters here using OPENJSON  
    -- END  
  
    -----------------------------------------------------  
    -- ORDER BY clause  
    -----------------------------------------------------  
    SET @OrderBy = CASE   
        WHEN @Sort IS NULL OR @Sort = '' THEN N' ORDER BY r.CreatedDate DESC '  
        ELSE N' ORDER BY ' + QUOTENAME(@Sort) + N' ' +   
             CASE WHEN UPPER(@Direction) = 'DESC' THEN 'DESC' ELSE 'ASC' END  
    END;  
  
    -----------------------------------------------------  
    -- Final SQL Query with pagination  
    -----------------------------------------------------  
    SET @Sql = N'  
        SELECT   
            r.Id,  
            r.RoleName,  
            r.Description,  
            r.TenantId,  
            t.TenantName,  
            r.IsActive,  
            CASE WHEN r.TenantId IS NULL THEN ''System'' ELSE ''Tenant'' END AS ManagedBy  
        FROM dbo.tb_gs_Roles r  
        LEFT JOIN dbo.tb_gs_Tenants t ON r.TenantId = t.Id  
        WHERE ' + @Where +   
        @OrderBy + '  
        OFFSET ' + CAST(@Offset AS NVARCHAR(20)) +   
        ' ROWS FETCH NEXT ' + CAST(@Length AS NVARCHAR(20)) + ' ROWS ONLY;';  
  
    -----------------------------------------------------  
    -- Execute Dynamic SQL  
    -----------------------------------------------------  
    EXEC sp_executesql @Sql;  
END  
GO

CREATE   PROCEDURE [dbo].[sp_GetRolesCount]  
(  
    @Search NVARCHAR(200) = NULL,  
    @FiltersText NVARCHAR(MAX) = NULL,  -- Optional JSON filters  
    @TenantId UNIQUEIDENTIFIER = NULL  
)  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    DECLARE   
        @Sql NVARCHAR(MAX),  
        @Where NVARCHAR(MAX) = N' r.IsDeleted = 0 ';  
  
    -----------------------------------------------------  
    -- Tenant filtering  
    -----------------------------------------------------  
    IF @TenantId IS NOT NULL  
        SET @Where = @Where + N' AND r.RoleType IN (2,3) AND r.TenantId = ''' + CAST(@TenantId AS NVARCHAR(36)) + '''';  
  
    -----------------------------------------------------  
    -- Search filter  
    -----------------------------------------------------  
    IF @Search IS NOT NULL AND LTRIM(RTRIM(@Search)) <> ''  
        SET @Where = @Where + N' AND (  
            r.RoleName LIKE ''%' + @Search + '%'' OR  
            t.TenantName LIKE ''%' + @Search + '%''  
        )';  
  
    -----------------------------------------------------  
    -- (Optional) Dynamic JSON filters (placeholder)  
    -- You can extend using OPENJSON if you want  
    -----------------------------------------------------  
    -- Example:  
    -- IF @FiltersText IS NOT NULL AND ISJSON(@FiltersText) = 1  
    -- BEGIN  
    --     -- parse and append to @Where dynamically  
    -- END  
  
    -----------------------------------------------------  
    -- Final Count Query  
    -----------------------------------------------------  
    SET @Sql = N'  
        SELECT COUNT(r.Id) AS TotalRecord  
        FROM dbo.tb_gs_Roles r  
        LEFT JOIN dbo.tb_gs_Tenants t ON r.TenantId = t.Id  
        WHERE ' + @Where + ';';  
  
    -----------------------------------------------------  
    -- Execute SQL  
    -----------------------------------------------------  
    EXEC sp_executesql @Sql;  
END  
GO

CREATE   PROCEDURE [dbo].[sp_GetRoleWisePermission]  
    @RoleId UNIQUEIDENTIFIER  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    SELECT   
        gm.Id,  
        gm.RoleId,  
        grp.RoleName,  
        gm.SitemapId,  
        gm.CanView,  
        gm.CanAdd,  
        gm.CanEdit,  
        gm.CanDelete,  
        gm.CanPreview,  
        gm.CanExport,  
        gm.CanPrint,  
        sm.PageUrl,  
        sm.Name AS SiteMapName,  
        sm.NameWithParent  
    FROM dbo.tb_gs_RoleMenus AS gm  
    INNER JOIN dbo.vGetTreeViewSitemap AS sm   
        ON gm.SitemapId = sm.Id  
    INNER JOIN dbo.tb_gs_Roles AS grp   
        ON gm.RoleId = grp.Id  
    WHERE gm.RoleId = @RoleId  
      AND (gm.IsDeleted IS NULL OR gm.IsDeleted = 0)  
    ORDER BY sm.OrderSequence;  
END;  

GO
CREATE   PROCEDURE dbo.sp_GetSitemapCount  
    @Search NVARCHAR(200) = NULL  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    DECLARE   
        @Sql NVARCHAR(MAX),  
        @Where NVARCHAR(MAX) = N'1=1';  -- Initialize WHERE condition  
  
    -- ✅ Generate search filter dynamically  
    IF @Search IS NOT NULL AND LTRIM(RTRIM(@Search)) <> ''  
    BEGIN  
        SET @Where = @Where + N' AND (Name LIKE ''%' + @Search + '%''   
                                 OR PageUrl LIKE ''%' + @Search + '%''   
                                 OR NameWithParent LIKE ''%' + @Search + '%'')';  
    END;  
  
    -- ✅ Build dynamic SQL for counting  
    SET @Sql = N'  
        SELECT COUNT(*) AS TotalRecord  
        FROM dbo.vGetTreeViewSitemap  
        WHERE ' + @Where + N';';  
  
    -- Debug (optional)  
    -- PRINT @Sql;  
  
    -- ✅ Execute dynamic SQL  
    EXEC sp_executesql @Sql;  
END;  

GO
CREATE   PROCEDURE [dbo].[sp_GetSitemapList]  
    @Page INT,  
    @Length INT,  
    @Sort NVARCHAR(100) = 'OrderSequence',  
    @Direction NVARCHAR(4) = 'ASC',  
    @Search NVARCHAR(200) = NULL  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    DECLARE   
        @Sql NVARCHAR(MAX),  
        @Where NVARCHAR(MAX) = N'1=1',  
        @OrderBy NVARCHAR(MAX),  
        @Offset INT;  
  
    -- ✅ Generate search filter dynamically  
    IF @Search IS NOT NULL AND LTRIM(RTRIM(@Search)) <> ''  
    BEGIN  
        SET @Where = @Where + N' AND ([Name] LIKE ''%' + @Search + '%''   
                                 OR [PageUrl] LIKE ''%' + @Search + '%''   
                                 OR [NameWithParent] LIKE ''%' + @Search + '%'')';  
    END;  
  
    -- ✅ Build ORDER BY clause with safe column name  
    SET @OrderBy = N'ORDER BY 1'   
 --+ QUOTENAME(@Sort) + N' ' +   
 --                  CASE WHEN UPPER(@Direction) IN ('ASC', 'DESC') THEN @Direction ELSE 'ASC' END;  
  
    -- ✅ Calculate OFFSET for pagination  
    SET @Offset = @Page * @Length;  
  
    -- ✅ Final dynamic SQL with bracketed reserved words  
    SET @Sql = N'  
        SELECT   
            [Id],  
            [Name],  
            [FavIcon],  
            [PageUrl],  
            [SortingOrder],  
            [ParentId],  
            [Level],  
            [OrderSequence],  
            [NameWithParent],  
            [IsActive],  
            [IsFeature],  
            [IsSidebarmenu],  
            [MenuType],  
            [MenuAccessType]  
        FROM dbo.vGetTreeViewSitemap  
        WHERE ' + @Where + N'  
        ' + @OrderBy + N'  
        OFFSET ' + CAST(@Offset AS NVARCHAR(10)) + N' ROWS   
        FETCH NEXT ' + CAST(@Length AS NVARCHAR(10)) + N' ROWS ONLY;';  
  
    -- Execute dynamic SQL  
    EXEC sp_executesql @Sql;  
END;  
GO

CREATE   PROCEDURE [dbo].[sp_GetStudentList]  
(  
    @Page INT,  
    @Length INT,  
    @Sort NVARCHAR(100) = NULL,  
    @Direction NVARCHAR(10) = 'ASC',  
    @Search NVARCHAR(200) = NULL,  
    @FiltersText NVARCHAR(MAX) = NULL,  -- optional JSON filters  
    @TenantId UNIQUEIDENTIFIER = NULL  
)  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    DECLARE   
        @Sql NVARCHAR(MAX),  
        @Where NVARCHAR(MAX) = N' st.IsDeleted = 0 ',  
        @OrderBy NVARCHAR(MAX),  
        @Offset INT = @Page * @Length;  
  
    -----------------------------------------------------  
    -- Tenant filtering  
    -----------------------------------------------------  
    IF @TenantId IS NOT NULL  
        SET @Where = @Where + N' AND st.TenantId = ''' + CAST(@TenantId AS NVARCHAR(36)) + '''';  
  
    -----------------------------------------------------  
    -- Search filter  
    -----------------------------------------------------  
    IF @Search IS NOT NULL AND LTRIM(RTRIM(@Search)) <> ''  
        SET @Where = @Where + N' AND (  
            st.FullName LIKE ''%' + @Search + '%'' OR  
            st.ApplicationForClass LIKE ''%' + @Search + '%''  
        )';  
  
    -----------------------------------------------------  
    -- (Optional) dynamic JSON filters placeholder  
    -- You can extend this if you parse @FiltersText (e.g., using OPENJSON)  
    -----------------------------------------------------  
    -- Example:  
    -- IF @FiltersText IS NOT NULL AND ISJSON(@FiltersText) = 1  
    -- BEGIN  
    --     -- parse custom filters here using OPENJSON  
    -- END  
  
    -----------------------------------------------------  
    -- ORDER BY clause  
    -----------------------------------------------------  
    SET @OrderBy = CASE   
        WHEN @Sort IS NULL OR @Sort = '' THEN N' ORDER BY st.CreatedDate DESC '  
        ELSE N' ORDER BY ' + QUOTENAME(@Sort) + N' ' +   
             CASE WHEN UPPER(@Direction) = 'DESC' THEN 'DESC' ELSE 'ASC' END  
    END;  
  
    -----------------------------------------------------  
    -- Final SQL Query with pagination  
    -----------------------------------------------------  
    SET @Sql = N'  
        SELECT   
   st.id,  
   st.FullName,  
   gd.FatherName,  
   gd.MotherName,  
   tsac.ClassName AS ApplicationForClass,  
   lg.Name,  
   st.StdCID  
   FROM tb_sl_StudentInfo st   
   LEFT JOIN tb_sl_GuardianInfo gd  ON st.Id = gd.StudentInfoId  
   LEFT JOIN tb_sl_LocalGuardianInfo lg ON lg.StudentInfoId = st.Id  
   LEFT JOIN tb_sch_AcademicClasses tsac ON tsac.Id = TRY_CONVERT(UNIQUEIDENTIFIER, st.ApplicationForClass)  
        WHERE ' + @Where +   
        @OrderBy + '  
        OFFSET ' + CAST(@Offset AS NVARCHAR(20)) +   
        ' ROWS FETCH NEXT ' + CAST(@Length AS NVARCHAR(20)) + ' ROWS ONLY;';  
  
    -----------------------------------------------------  
    -- Execute Dynamic SQL  
    -----------------------------------------------------  
    EXEC sp_executesql @Sql;  
END  

GO
CREATE   PROCEDURE [dbo].[sp_GetStudentListCount]  
(  
    @Search NVARCHAR(200) = NULL,  
    @FiltersText NVARCHAR(MAX) = NULL,  -- Optional JSON filters  
    @TenantId UNIQUEIDENTIFIER = NULL  
)  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    DECLARE   
        @Sql NVARCHAR(MAX),  
        @Where NVARCHAR(MAX) = N' st.IsDeleted = 0 ';  
  
    -----------------------------------------------------  
    -- Tenant filtering  
    -----------------------------------------------------  
    IF @TenantId IS NOT NULL  
        SET @Where = @Where + N' AND st.TenantId = ''' + CAST(@TenantId AS NVARCHAR(36)) + '''';  
  
    -----------------------------------------------------  
    -- Search filter  
    -----------------------------------------------------  
    IF @Search IS NOT NULL AND LTRIM(RTRIM(@Search)) <> ''  
        SET @Where = @Where + N' AND (  
            st.FullName LIKE ''%' + @Search + '%'' OR  
            st.ApplicationForClass LIKE ''%' + @Search + '%''  
        )';  
  
    -----------------------------------------------------  
    -- (Optional) Dynamic JSON filters (placeholder)  
    -- You can extend using OPENJSON if you want  
    -----------------------------------------------------  
    -- Example:  
    -- IF @FiltersText IS NOT NULL AND ISJSON(@FiltersText) = 1  
    -- BEGIN  
    --     -- parse and append to @Where dynamically  
    -- END  
  
    -----------------------------------------------------  
    -- Final Count Query  
    -----------------------------------------------------  
    SET @Sql = N'  
  SELECT   
   COUNT(st.Id) AS TotalRecord  
   FROM tb_sl_StudentInfo st   
   LEFT JOIN tb_sl_GuardianInfo gd  ON st.Id = gd.StudentInfoId  
   LEFT JOIN tb_sl_LocalGuardianInfo lg ON lg.StudentInfoId = st.Id  
  
        WHERE ' + @Where + ';';  
  
    -----------------------------------------------------  
    -- Execute SQL  
    -----------------------------------------------------  
    EXEC sp_executesql @Sql;  
END  

GO
CREATE   PROCEDURE [dbo].[sp_GetTenants]  
    @Page INT,  
    @Length INT,  
    @Sort NVARCHAR(100) = 'CreatedDate',  
    @Direction NVARCHAR(4) = 'ASC',  
    @Search NVARCHAR(200) = NULL,  
    @Filters_text NVARCHAR(MAX) = NULL  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    DECLARE   
        @Sql NVARCHAR(MAX),  
        @Where NVARCHAR(MAX) = N't.IsDeleted = 0',  
        @OrderBy NVARCHAR(MAX),  
        @Offset INT;  
  
    --------------------------------------------------------------------------------  
    -- 🔍 Search filter  
    --------------------------------------------------------------------------------  
    IF @Search IS NOT NULL AND LTRIM(RTRIM(@Search)) <> ''  
    BEGIN  
        SET @Where = @Where + N' AND (  
            t.TenantName LIKE ''%' + @Search + '%'' OR  
            t.TenantEmail LIKE ''%' + @Search + '%'' OR  
            t.Domain LIKE ''%' + @Search + '%'' OR  
            t.PhoneNumber LIKE ''%' + @Search + '%'' OR  
            t.City LIKE ''%' + @Search + '%'' OR  
            t.Street LIKE ''%' + @Search + '%'' OR  
            t.Province LIKE ''%' + @Search + '%'' OR  
            t.PostCode LIKE ''%' + @Search + '%''  
        )';  
    END;  
  
    --------------------------------------------------------------------------------  
    -- 🧩 Dynamic Filters (optional)  
    --------------------------------------------------------------------------------  
    --IF @Filters_text IS NOT NULL AND LTRIM(RTRIM(@Filters_text)) <> ''  
    --BEGIN  
    --    -- You can send ready-to-append SQL conditions here, for example:  
    --    -- "t.IsActive = 1 AND t.CountryId IS NOT NULL"  
    --    SET @Where = @Where + N' AND (' + @Filters_text + N')';  
    --END;  
  
    --------------------------------------------------------------------------------  
    -- ⚙️ Sorting  
    --------------------------------------------------------------------------------  
    IF @Sort IS NULL OR @Sort = ''  
        SET @Sort = 'CreatedDate';  
  
    IF @Direction IS NULL OR @Direction = ''  
        SET @Direction = 'ASC';  
  
    SET @OrderBy = N'ORDER BY 1' ;  
 --+ QUOTENAME(@Sort) + N' ' +  
 --                  CASE WHEN UPPER(@Direction) IN ('ASC', 'DESC') THEN @Direction ELSE 'ASC' END;  
  
    --------------------------------------------------------------------------------  
    -- 📄 Pagination  
    --------------------------------------------------------------------------------  
    SET @Offset = @Page * @Length;  
  
    --------------------------------------------------------------------------------  
    -- 🧱 Final SQL  
    --------------------------------------------------------------------------------  
    SET @Sql = N'  
        SELECT   
             t.Id,  
            t.TenantName,  
            t.TenantEmail,  
            t.IsActive,  
            t.CreatedById,  
            t.CreatedDate,  
            t.ModifiedById,  
            t.ModifiedDate,  
            t.IsDeleted,  
            t.DeletedById,  
            t.DeletedDate,  
            t.BinNo,  
            t.Domain,  
            t.PhoneNumber,  
            t.PostCode,  
            t.Street,  
            t.City,  
            t.Province  
        FROM dbo.tb_gs_Tenants t  
        WHERE ' + @Where + N'  
        ' + @OrderBy + N'  
        OFFSET ' + CAST(@Offset AS NVARCHAR(10)) + N' ROWS   
        FETCH NEXT ' + CAST(@Length AS NVARCHAR(10)) + N' ROWS ONLY;';  
  
    --------------------------------------------------------------------------------  
    -- 🏃 Execute SQL  
    --------------------------------------------------------------------------------  
    PRINT @Sql; -- Debug only, remove in production  
    EXEC sp_executesql @Sql;  
END;  
GO
CREATE   PROCEDURE [dbo].[sp_GetTenantsCount]  
    @Search NVARCHAR(200) = NULL,  
    @Filters_text NVARCHAR(MAX) = NULL  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    DECLARE   
        @Sql NVARCHAR(MAX),  
        @Where NVARCHAR(MAX) = N't.IsDeleted = 0';  
  
    --------------------------------------------------------------------------------  
    -- 🔍 Apply search filter  
    --------------------------------------------------------------------------------  
    IF @Search IS NOT NULL AND LTRIM(RTRIM(@Search)) <> ''  
    BEGIN  
        SET @Where = @Where + N' AND (  
            t.TenantName LIKE ''%' + @Search + '%'' OR  
            t.TenantEmail LIKE ''%' + @Search + '%'' OR  
            t.Domain LIKE ''%' + @Search + '%'' OR  
            t.PhoneNumber LIKE ''%' + @Search + '%'' OR  
            t.City LIKE ''%' + @Search + '%'' OR  
            t.Street LIKE ''%' + @Search + '%'' OR  
            t.Province LIKE ''%' + @Search + '%'' OR  
            t.PostCode LIKE ''%' + @Search + '%''  
        )';  
    END;  
  
    --------------------------------------------------------------------------------  
    -- 🧩 Dynamic filters (optional)  
    --------------------------------------------------------------------------------  
    --IF @Filters_text IS NOT NULL AND LTRIM(RTRIM(@Filters_text)) <> ''  
    --BEGIN  
    --    -- Example usage: "t.IsActive = 1 AND t.CountryId IS NOT NULL"  
    --    SET @Where = @Where + N' AND (' + @Filters_text + N')';  
    --END;  
  
    --------------------------------------------------------------------------------  
    -- 📄 Build final SQL  
    --------------------------------------------------------------------------------  
    SET @Sql = N'  
        SELECT COUNT(*) AS totalrecord  
        FROM dbo.tb_gs_Tenants t  
        WHERE ' + @Where + N';';  
  
    --------------------------------------------------------------------------------  
    -- 🏃 Execute  
    --------------------------------------------------------------------------------  
    PRINT @Sql; -- For debugging only, remove in production  
    EXEC sp_executesql @Sql;  
END;  
GO
CREATE   PROCEDURE dbo.sp_GetUserWiseMenuPermission  
    @UserId UNIQUEIDENTIFIER  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    ;WITH permitted_menus AS  
    (  
        -- Step 1: Direct permission items  
        SELECT   
            sm.Id,  
            sm.Name,  
            sm.FavIcon,  
            sm.PageUrl,  
            sm.SortingOrder,  
            sm.ParentId,  
            sm.IsFeature,  
            sm.IsActive,  
            rm.CanView,  
            rm.CanAdd,  
            rm.CanEdit,  
            rm.CanDelete,  
            rm.CanPreview,  
            rm.CanExport,  
            rm.CanPrint,  
            sm.IsSidebarmenu  
        FROM dbo.tb_gs_UserRoles ur  
        INNER JOIN dbo.tb_gs_RoleMenus rm   
            ON ur.RoleId = rm.RoleId   
            AND rm.IsActive = 1   
            AND rm.IsDeleted = 0  
        INNER JOIN dbo.tb_gs_Sitemaps sm   
            ON rm.SitemapId = sm.Id  
        WHERE   
            ur.UserId = @UserId  
            AND ur.IsActive = 1  
            AND ur.IsDeleted = 0  
  
        UNION ALL  
  
        -- Step 2: Recursively bring in parents (without permission fields)  
        SELECT   
            p.Id,  
            p.Name,  
            p.FavIcon,  
            p.PageUrl,  
            p.SortingOrder,  
            p.ParentId,  
            p.IsFeature,  
            p.IsActive,  
            CAST(1 AS BIT) AS CanView,   
            CAST(NULL AS BIT) AS CanAdd,  
            CAST(NULL AS BIT) AS CanEdit,  
            CAST(NULL AS BIT) AS CanDelete,  
            CAST(NULL AS BIT) AS CanPreview,  
            CAST(NULL AS BIT) AS CanExport,  
            CAST(NULL AS BIT) AS CanPrint,  
            p.IsSidebarmenu  
        FROM dbo.tb_gs_Sitemaps p  
        INNER JOIN permitted_menus c   
            ON c.ParentId = p.Id  
        WHERE   
            p.IsActive = 1  
            AND p.IsDeleted = 0  
    )  
    SELECT DISTINCT  
        pm.Id AS SitemapId,  
        pm.Name,  
        pm.FavIcon,  
        pm.PageUrl,  
        pm.SortingOrder,  
        pm.ParentId,  
        pm.IsFeature,  
        pm.IsActive,  
        pm.CanView,  
        pm.CanAdd,  
        pm.CanEdit,  
        pm.CanDelete,  
        pm.CanPreview,  
        pm.CanExport,  
        pm.CanPrint,  
        ISNULL(pm.ParentId, pm.Id) AS SortParent,  
        pm.SortingOrder AS SortOrder,  
        pm.IsSidebarmenu  
    FROM permitted_menus pm  
    ORDER BY pm.SortingOrder;  
END;  



