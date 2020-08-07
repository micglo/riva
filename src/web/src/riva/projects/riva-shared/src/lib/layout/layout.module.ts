import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { SpinnerModule } from 'riva-core';
import { ContentLayoutComponent } from './components/content-layout/content-layout.component';
import { CustomizerComponent } from './components/customizer/customizer.component';
import { FooterComponent } from './components/footer/footer.component';
import { FullLayoutComponent } from './components/full-layout/full-layout.component';
import { HorizontalMenuComponent } from './components/horizontal-menu/horizontal-menu.component';
import { NavbarFullscreenComponent } from './components/navbar-fullscreen/navbar-fullscreen.component';
import { NavbarLanguageComponent } from './components/navbar-language/navbar-language.component';
import { NavbarLogoComponent } from './components/navbar-logo/navbar-logo.component';
import { NavbarUserComponent } from './components/navbar-user/navbar-user.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { VerticalMenuComponent } from './components/vertical-menu/vertical-menu.component';
import { HorizontalMenuAnchorToggleDirective } from './directives/horizontal-menu-anchor-toggle.directive';
import { HorizontalMenuLinkDirective } from './directives/horizontal-menu-link.directive';
import { HorizontalMenuDirective } from './directives/horizontal-menu.directive';
import { ToggleFullscreenDirective } from './directives/toggle-fullscreen.directive';
import { VerticalAnchorToggleDirective } from './directives/vertical-anchor-toggle.directive';
import { VerticalLinkDirective } from './directives/vertical-link.directive';
import { VerticalDirective } from './directives/vertical.directive';
import { LayoutServicesModule } from './layout-services.module';

@NgModule({
    declarations: [
        FullLayoutComponent,
        NavbarComponent,
        NavbarFullscreenComponent,
        NavbarLogoComponent,
        NavbarLanguageComponent,
        NavbarUserComponent,
        HorizontalMenuComponent,
        VerticalMenuComponent,
        FooterComponent,
        CustomizerComponent,
        ToggleFullscreenDirective,
        HorizontalMenuDirective,
        HorizontalMenuLinkDirective,
        HorizontalMenuAnchorToggleDirective,
        VerticalDirective,
        VerticalLinkDirective,
        VerticalAnchorToggleDirective,
        ContentLayoutComponent
    ],
    exports: [
        FullLayoutComponent,
        NavbarComponent,
        NavbarFullscreenComponent,
        NavbarLogoComponent,
        NavbarLanguageComponent,
        NavbarUserComponent,
        HorizontalMenuComponent,
        VerticalMenuComponent,
        FooterComponent,
        CustomizerComponent,
        ToggleFullscreenDirective,
        HorizontalMenuDirective,
        HorizontalMenuLinkDirective,
        HorizontalMenuAnchorToggleDirective,
        VerticalDirective,
        VerticalLinkDirective,
        VerticalAnchorToggleDirective,
        ContentLayoutComponent
    ],
    imports: [
        CommonModule,
        RouterModule,
        PerfectScrollbarModule,
        NgbDropdownModule,
        TranslateModule.forChild(),
        SpinnerModule.forChild(),
        LayoutServicesModule
    ],
    providers: [HorizontalMenuDirective, HorizontalMenuLinkDirective, VerticalDirective, VerticalLinkDirective]
})
export class LayoutModule {}
