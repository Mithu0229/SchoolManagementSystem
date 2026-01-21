import { inject } from '@angular/core';
import { Router, type CanActivateFn } from '@angular/router';
import { ToastService } from '../services/toast.service';
import { Severity } from '../enums/toast-severity.enum';
import { PermissionService, PermissionType } from '../services/permission.service';

export const permissionGuard = (requiredPermission: string): CanActivateFn => {
    return (route, state) => {
        const router = inject(Router);
        const toastService = inject(ToastService);
        const permissionService = inject(PermissionService);

        try {
            // Check if user has the required permission
            const permissionType = getPermissionTypeFromUrl(state.url);

            // We need to determine the module name from the route
            // For now, we'll use a simple approach based on the URL path segments
            const urlSegments = state.url.split('/').filter((segment) => segment);
            const moduleName = getModuleNameFromUrl(urlSegments);

            const userHasPermission = permissionService.checkPermission(moduleName, permissionType, requiredPermission);

            console.log(`Permission check for ${moduleName} with ${permissionType}: ${userHasPermission}`);
            if (userHasPermission) {
                return true;
            } else {
                const permissionMessages: Record<PermissionType, string> = {
                    canView: 'You do not have permission to view this page.',
                    canAdd: 'You do not have permission to add new items.',
                    canEdit: 'You do not have permission to edit items.',
                    canDelete: 'You do not have permission to delete items.',
                    canPreview: 'You do not have permission to preview items.',
                    canExport: 'You do not have permission to export items.',
                    canPrint: 'You do not have permission to print items.'
                };

                const errorMessage = permissionMessages[permissionType] || 'You do not have the required permission.';
                toastService.error(errorMessage, Severity.ERROR);
                return router.createUrlTree(['/not-found']);
            }
        } catch (error) {
            console.error('Error checking permission:', error);
            toastService.error('An error occurred while checking permissions.', Severity.ERROR);
            return router.createUrlTree(['/']);
        }
    };
};

/**
 * Determines the permission type needed based on the URL
 */
function getPermissionTypeFromUrl(url: string): PermissionType {
    if (url.includes('/create')) {
        return 'canAdd';
    } else if (url.includes('/edit')) {
        return 'canEdit';
    } else if (url.includes('/delete')) {
        return 'canDelete';
    } else if (url.includes('/preview')) {
        return 'canPreview';
    } else if (url.includes('/export')) {
        return 'canExport';
    } else if (url.includes('/print')) {
        return 'canPrint';
    } else {
        return 'canView';
    }
}

/**
 * Maps URL segments to module names
 * This is a simple implementation that can be expanded as needed
 */
// function getModuleNameFromUrl(urlSegments: string[]): string {
//     // Map of URL segments to module names
//     const moduleMap: Record<string, string> = {
//         disposition: 'Disposition Type',
//         carType: 'Car Type',
//         role: 'Roles',
//         user: 'Users',
//         standard: 'Industry Standards',
//         documentCategory: 'Document Category',
//         division: 'Divisions',
//         project: 'Projects',
//         department: 'Departments',
//         supplier: 'Supplier',
//         employee: 'Employee',
//         policy: 'Policy Management',
//         document: 'Document Management',
//         subscription: 'Plan',
//         initiateNCR: 'Initiate NCR',
//         qualityManagerReview: 'QM Review',
//         dispositionPlanning: 'Disposition Planning',
//         qmApprovals: 'QM Approval',
//         ncrApproverAssignmentList: 'NCR Approval Assignment',
//         dispositionCompletion: 'Disposition Completion',
//         verificationClosure: 'Verification and Closure',
//         tenant: 'Tenant',
//         documentApproval: 'Document Approval',
//         'car-source': 'Car Source',
//         'cause-option': 'Preliminary Cause',
//         'cause-method': 'Root Cause Method',
//         'car-management': 'View CAR',
//         'car-type': 'Car Type',
//         initiateCAR: 'Initiate CAR',
//         casReview: 'CAS Review',
//         immediateAction: 'Immediate Action',
//         causeAnalysis: 'Cause Analysis',
//         carApproverAssignmentList: 'CAR Approval Assignment',
//         carTabDocument: 'Car tab document',
//         carReports: 'Car Reports',
//         'Sncr-management': 'View SNCR',
//         initiateSNCR: 'Initiate SNCR'
//         // Add more mappings as needed
//     };

//     // First, check for specific nested routes (more specific paths should be checked first)
//     // Check all possible combinations of segments to handle nested routes
//     for (let i = urlSegments.length - 1; i >= 0; i--) {
//         const segment = urlSegments[i];
//         if (moduleMap[segment]) {
//             return moduleMap[segment];
//         }
//     }

//     // Also check for compound paths (e.g., "parent-child" combinations)
//     if (urlSegments.length >= 2) {
//         for (let i = 0; i < urlSegments.length - 1; i++) {
//             const compoundKey = `${urlSegments[i]}-${urlSegments[i + 1]}`;
//             if (moduleMap[compoundKey]) {
//                 return moduleMap[compoundKey];
//             }
//         }
//     }

//     // Check for full path combinations
//     if (urlSegments.length >= 2) {
//         const fullPath = urlSegments.join('/');
//         if (moduleMap[fullPath]) {
//             return moduleMap[fullPath];
//         }
//     }

//     // Default to the first segment with capitalization if no mapping found
//     if (urlSegments.length > 0) {
//         const segment = urlSegments[0];
//         return segment.charAt(0).toUpperCase() + segment.slice(1);
//     }

//     return 'Unknown Module';
// }

function getModuleNameFromUrl(urlSegments: string[]): string {
    try {
        const menuListString = localStorage.getItem('menuList');
        if (!menuListString) {
            console.warn('Menu list not found in localStorage.');
            return 'Unknown Module';
        }

        const menuList = JSON.parse(menuListString);
        const normalizedUrl = '/' + urlSegments.join('/').toLowerCase();

        const allMatches: { label: string; link: string }[] = [];

        const findModuleByUrl = (items: any[]) => {
            for (const item of items) {
                if (item.routerLink) {
                    const links = Array.isArray(item.routerLink) ? item.routerLink : [item.routerLink];
                    for (const link of links) {
                        const normalizedLink = link.toLowerCase().trim();
                        if (normalizedUrl.startsWith(normalizedLink)) {
                            allMatches.push({ label: item.label, link: normalizedLink });
                        }
                    }
                }
                if (item.items && Array.isArray(item.items)) {
                    findModuleByUrl(item.items);
                }
            }
        };

        findModuleByUrl(menuList);

        if (allMatches.length > 0) {
            // ✅ Choose the longest (most specific) path match
            const bestMatch = allMatches.sort((a, b) => b.link.length - a.link.length)[0];
            console.log('✅ Matched Module:', bestMatch.label, 'for URL:', normalizedUrl);
            return bestMatch.label;
        }

        console.warn('⚠️ No module found for URL:', normalizedUrl);
        return urlSegments.length > 0 ? urlSegments[urlSegments.length - 1].charAt(0).toUpperCase() + urlSegments[urlSegments.length - 1].slice(1) : 'Unknown Module';
    } catch (err) {
        console.error('Error resolving module name from URL:', err);
        return 'Unknown Module';
    }
}
