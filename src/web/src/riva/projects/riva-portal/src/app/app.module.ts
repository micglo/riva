import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateModule } from '@ngx-translate/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { OAuthModule } from 'angular-oauth2-oidc';
import { DeviceDetectorModule } from 'ngx-device-detector';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { ToastrModule } from 'ngx-toastr';
import { UiSwitchModule } from 'ngx-ui-switch';
import {
    AuthModule,
    ConfigurationModule,
    NotificationModule,
    PerfectScrollbarConfigModule,
    SignalRModule,
    SpinnerModule,
    StorageModule,
    TranslationModule,
    UserModule,
    WindowModule
} from 'riva-core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { TRANSLATION_LOADER_PROVIDER } from './core/translation/translation-loader.provider';

@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserAnimationsModule,
        AppRoutingModule,
        HttpClientModule,
        TranslateModule.forRoot({ loader: TRANSLATION_LOADER_PROVIDER }),
        OAuthModule.forRoot(),
        ToastrModule.forRoot(),
        DeviceDetectorModule.forRoot(),
        PerfectScrollbarModule,
        UiSwitchModule.forRoot({
            size: 'medium',
            color: '#975aff',
            switchColor: '#fff'
        }),
        SweetAlert2Module.forRoot(),
        ConfigurationModule.forRoot(),
        PerfectScrollbarConfigModule.forRoot(),
        WindowModule.forRoot(),
        StorageModule.forRoot(),
        TranslationModule.forRoot(),
        AuthModule.forRoot(),
        SpinnerModule.forRoot(),
        NotificationModule.forRoot(),
        SignalRModule.forRoot(),
        UserModule.forRoot(),
        CoreModule.forRoot()
    ],
    bootstrap: [AppComponent]
})
export class AppModule {}
