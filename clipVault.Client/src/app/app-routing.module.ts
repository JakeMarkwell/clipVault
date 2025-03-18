import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApiTestComponent } from './api-test/api-test.component';

const routes: Routes = [ { path: 'api-test', component: ApiTestComponent },
  { path: '', redirectTo: '/api-test', pathMatch: 'full' },
  { path: '**', redirectTo: '/api-test'}]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
