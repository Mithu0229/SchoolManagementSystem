import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';

// Define a type for permission types
export type PermissionType =
  | 'canView'
  | 'canAdd'
  | 'canEdit'
  | 'canDelete'
  | 'canPreview'
  | 'canExport'
  | 'canPrint';

@Injectable({
  providedIn: 'root',
})
export class PermissionService {
  constructor(private authService: AuthService) {}
  /**
   * Checks if the user has a specific permission for a given module
   * @param moduleName The name of the module to check permissions for (e.g., 'Disposition Type')
   * @param permissionType The type of permission to check (canView, canAdd, canEdit, canDelete)
   * @param routerLinkPattern Optional pattern to match in the routerLink (e.g., '/disposition')
   * @returns boolean indicating if the user has the specified permission
   */
  public checkPermission(
    moduleName: string,
    permissionType: PermissionType,
    routerLinkPattern?: string
  ): boolean {
    try {
      const menuListString = localStorage.getItem('menuList');
      if (!menuListString) {
        return false;
      }

      const menuList = JSON.parse(menuListString);

      // Navigate through the menu structure to find the module
      const findModulePermission = (items: any[]): boolean => {
        for (const item of items) {
          // Check if this is the item we're looking for by name
          if (item.label === moduleName) {
            // If routerLinkPattern is provided, also check that
            if (routerLinkPattern) {
              if (
                !item.routerLink ||
                !(Array.isArray(item.routerLink)
                  ? item.routerLink.some((link: string) =>
                      link.includes(routerLinkPattern)
                    )
                  : item.routerLink.includes(routerLinkPattern))
              ) {
                continue; // Skip if routerLink doesn't match the pattern
              }
            }

            // Check if user has the required permission type
            return item[permissionType] === true;
          }

          // Check children if they exist
          if (item.items && item.items.length > 0) {
            const hasPermission = findModulePermission(item.items);
            if (hasPermission) {
              return true;
            }
          }
        }
        return false;
      };

      return Array.isArray(menuList) ? findModulePermission(menuList) : false;
    } catch (error) {
      console.error(
        `Error checking ${permissionType} permission for ${moduleName}:`,
        error
      );
      return false;
    }
  }

  /**
   * Checks if the user has permission to view a specific module
   * @param moduleName The name of the module
   * @param routerLinkPattern Optional pattern to match in the routerLink
   */
  public canView(moduleName: string, routerLinkPattern?: string): boolean {
    return this.checkPermission(moduleName, 'canView', routerLinkPattern);
  }

  /**
   * Checks if the user has permission to add items to a specific module
   * @param moduleName The name of the module
   * @param routerLinkPattern Optional pattern to match in the routerLink
   */
  public canAdd(moduleName: string, routerLinkPattern?: string): boolean {
    return this.checkPermission(moduleName, 'canAdd', routerLinkPattern);
  }

  /**
   * Checks if the user has permission to edit items in a specific module
   * @param moduleName The name of the module
   * @param routerLinkPattern Optional pattern to match in the routerLink
   */
  public canEdit(moduleName: string, routerLinkPattern?: string): boolean {
    return this.checkPermission(moduleName, 'canEdit', routerLinkPattern);
  }

  /**
   * Checks if the user has permission to delete items from a specific module
   * @param moduleName The name of the module
   * @param routerLinkPattern Optional pattern to match in the routerLink
   */
  public canDelete(moduleName: string, routerLinkPattern?: string): boolean {
    return this.checkPermission(moduleName, 'canDelete', routerLinkPattern);
  }

  /**
   * Checks if the user has permission to preview items in a specific module
   * @param moduleName The name of the module
   * @param routerLinkPattern Optional pattern to match in the routerLink
   */
  public canPreview(moduleName: string, routerLinkPattern?: string): boolean {
    return this.checkPermission(moduleName, 'canPreview', routerLinkPattern);
  }

  /**
   * Checks if the user has permission to export items from a specific module
   * @param moduleName The name of the module
   * @param routerLinkPattern Optional pattern to match in the routerLink
   */
  public canExport(moduleName: string, routerLinkPattern?: string): boolean {
    return this.checkPermission(moduleName, 'canExport', routerLinkPattern);
  }

  /**
   * Checks if the user has permission to print items from a specific module
   * @param moduleName The name of the module
   * @param routerLinkPattern Optional pattern to match in the routerLink
   */
  public canPrint(moduleName: string, routerLinkPattern?: string): boolean {
    return this.checkPermission(moduleName, 'canPrint', routerLinkPattern);
  }

  public async getSitemapID(routerLinkPattern: string) {
    try {
      const menuList = await this.loadMenuList();

      const findSitemapID = (items: any[]): string | null => {
        for (const item of items) {
          if (item.routerLink?.includes(routerLinkPattern)) {
            return item.sitemapId || null; // Return the ID if it exists
          }

          if (item.items && item.items.length > 0) {
            const id = findSitemapID(item.items);
            if (id) {
              return id;
            }
          }
        }
        return null;
      };

      return Array.isArray(menuList) ? findSitemapID(menuList) : null;
    } catch (error) {
      console.error('Error fetching sitemap ID:', error);
      return null;
    }
  }

  private loadMenuList(): Promise<any[]> {
    return new Promise((resolve, reject) => {
      const cached = localStorage.getItem('menuList');
      if (cached) {
        resolve(JSON.parse(cached));
        return;
      }

      // If no cache → call API
      this.authService.getMenuList().subscribe({
        next: (response: any) => {
          if (response.isSuccess) {
            const data = response.data ?? [];
            localStorage.setItem('menuList', JSON.stringify(data));
            resolve(data);
          } else {
            reject(response.errors);
          }
        },
        error: reject,
      });
    });
  }
}
