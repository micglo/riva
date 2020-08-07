import { Injectable } from '@angular/core';
import { DeviceDetectorService } from 'ngx-device-detector';
import { LayoutMenuPosition } from './../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../enums/layout-navbar-type.enum';
import { LayoutSidebarSize } from './../enums/layout-sidebar-size.enum';
import { BodyClassesUpdateService } from './body-classes-update.service';

@Injectable()
export class FullLayoutService {
    constructor(private bodyClassesUpdateService: BodyClassesUpdateService, private deviceDetectorService: DeviceDetectorService) {}

    public setFullLayoutOnSidebarSize(size: LayoutSidebarSize): void {
        switch (size) {
            case LayoutSidebarSize.Small: {
                this.bodyClassesUpdateService.updateBodyClassesWhenSidebarSizeIsSmall();
                break;
            }
            case LayoutSidebarSize.Medium: {
                this.bodyClassesUpdateService.updateBodyClassesWhenSidebarSizeIsMedium();
                break;
            }
            default: {
                this.bodyClassesUpdateService.updateBodyClassesWhenSidebarSizeIsLarge();
                break;
            }
        }
    }

    public setFullLayoutOnNavbarType(navbarType: LayoutNavbarType): void {
        switch (navbarType) {
            case LayoutNavbarType.Static: {
                this.bodyClassesUpdateService.updateBodyClassesWhenNavbarTypeIsStatic();
                break;
            }
            default: {
                this.bodyClassesUpdateService.updateBodyClassesWhenNavbarTypeIsFixed();
                break;
            }
        }
    }

    public setMenuLayout(
        menuPosition: LayoutMenuPosition,
        isSidebarCollapsed: boolean,
        isSmallScreen: boolean,
        isSmallScreenMenuShown: boolean
    ): void {
        this.bodyClassesUpdateService.removeBlankPageClass();
        this.bodyClassesUpdateService.removeLayoutMenuClasses();

        if (menuPosition === LayoutMenuPosition.Top) {
            this.bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsTop();
            if (isSmallScreen) {
                this.bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsSmall(isSmallScreenMenuShown);
            } else {
                this.bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsLarge();
            }
        } else {
            this.bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsSide();
            if (isSmallScreen) {
                this.bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsSmall(isSmallScreenMenuShown);
            } else {
                this.bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsLarge(isSidebarCollapsed);
            }
        }

        if (this.isMobileOrTabletDevice() && isSmallScreenMenuShown) {
            this.bodyClassesUpdateService.addOverflowHiddenClass();
        }
    }

    private isMobileOrTabletDevice(): boolean {
        return this.deviceDetectorService.isMobile() || this.deviceDetectorService.isTablet();
    }
}
