import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LayoutTemplate } from './../../models/layout-template.model';
import { BodyClassesUpdateService } from './../../services/body-classes-update.service';
import { LayoutService } from './../../services/layout.service';
import { LayoutTemplateStore } from './../../stores/layout-template.store';

@Component({
    selector: 'lib-content-layout',
    templateUrl: './content-layout.component.html',
    providers: [BodyClassesUpdateService, LayoutService],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ContentLayoutComponent implements OnInit, OnDestroy {
    private _subscriptions = new Subscription();

    constructor(
        private layoutTemplateStore: LayoutTemplateStore,
        private layoutService: LayoutService,
        private bodyClassesUpdateService: BodyClassesUpdateService,
        private cdr: ChangeDetectorRef
    ) {}

    public ngOnInit(): void {
        this.bodyClassesUpdateService.addAuthPageClass();
        this.bodyClassesUpdateService.removeTransparentBodyClasses();
        this.bodyClassesUpdateService.removeMenuExpandedClass();
        this.bodyClassesUpdateService.removeNavbarStaticClass();
        this.bodyClassesUpdateService.removeMenuOpenClass();
        this.bodyClassesUpdateService.addBlankPageClass();
        this.subscribeToLayoutTemplateStateChanges();
    }

    public ngOnDestroy(): void {
        this.bodyClassesUpdateService.removeAuthPageClass();
        this._subscriptions.unsubscribe();
    }

    private subscribeToLayoutTemplateStateChanges(): void {
        const subscription = this.layoutTemplateStore.state$.subscribe((layoutTemplate: LayoutTemplate) => {
            this.layoutService.setLayoutOnVariant(layoutTemplate.variant, layoutTemplate.sidebar.backgroundColor);
            this.cdr.markForCheck();
        });
        this._subscriptions.add(subscription);
    }
}
