import { NgModule } from '@angular/core';
import { MyAccountModule as SharedMyAccountModule } from 'riva-shared';
import { MyAccountRoutingModule } from './my-account-routing.module';

@NgModule({
    imports: [SharedMyAccountModule, MyAccountRoutingModule]
})
export class MyAccountModule {}
