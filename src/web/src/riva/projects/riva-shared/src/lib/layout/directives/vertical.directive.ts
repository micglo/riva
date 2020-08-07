import { ChangeDetectorRef, Directive, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { LayoutTemplate } from './../models/layout-template.model';
import { LayoutTemplateStore } from './../stores/layout-template.store';
import { VerticalLinkDirective } from './vertical-link.directive';

@Directive({
    selector: '[libVertical]'
})
export class VerticalDirective implements OnInit, OnDestroy {
    private _navlinks = new Array<VerticalLinkDirective>();
    private _subscriptions = new Subscription();
    private _mouseEnter = false;
    private _layoutTemplate: LayoutTemplate;

    constructor(private layoutTemplateStore: LayoutTemplateStore, private cdr: ChangeDetectorRef, private router: Router) {}

    public ngOnInit(): void {
        const layoutTemplateSubscription = this.layoutTemplateStore.state$.subscribe((state: LayoutTemplate) => {
            this._layoutTemplate = state;
            this.loadLayout(state, this._mouseEnter, this._navlinks);
            this.cdr.markForCheck();
        });
        this._subscriptions.add(layoutTemplateSubscription);
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }

    public addLink(link: VerticalLinkDirective): void {
        this._navlinks.push(link);
    }

    public closeOtherLinks(openLink: VerticalLinkDirective): void {
        this._navlinks.forEach((link: VerticalLinkDirective) => {
            if (link !== openLink && (openLink.level.toString() === '1' || link.level === openLink.level)) {
                link.open = false;
            } else if (link === openLink && openLink.level.toString() === '1' && link.hasSub === false) {
                link.open = false;
            } else if (link === openLink && openLink.level.toString() !== '1' && link.hasSub === false) {
                link.open = false;
                return;
            }
        });
    }

    @HostListener('mouseenter', ['$event'])
    public onMouseEnter(event: MouseEvent): void {
        this._mouseEnter = true;
        if (this._layoutTemplate.sidebar.collapsed) {
            this.setSidebarGroup(this._navlinks, true);
        }
    }

    @HostListener('mouseleave', ['$event'])
    public onMouseLeave(event: MouseEvent): void {
        this._mouseEnter = false;
        if (this._layoutTemplate.sidebar.collapsed) {
            this.closeOtherLinks(this._navlinks.find(link => link.path === this.router.url));
            this.setSidebarGroup(this._navlinks, false);
        }
    }

    private loadLayout(layoutTemplate: LayoutTemplate, mouseEnter: boolean, navlinks: VerticalLinkDirective[]): void {
        if (layoutTemplate.sidebar.collapsed && !mouseEnter) {
            this.closeOtherLinks(navlinks.find(link => link.path === this.router.url));
            this.setSidebarGroup(navlinks, false);
        } else {
            this.setSidebarGroup(navlinks, true);
        }
    }

    private setSidebarGroup(navlinks: VerticalLinkDirective[], open: boolean): void {
        if (navlinks.length > 0) {
            const matched = navlinks.find(link => link.path === this.router.url);
            if (matched) {
                const parent = navlinks.find(
                    link => link.parent === matched.parent && link.level.toString() === '1' && link.hasSub === true
                );
                if (parent) {
                    parent.open = open;
                }
            }
        }
    }
}
