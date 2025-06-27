import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApiTestComponent } from './api-test/api-test.component';
import { HomeComponent } from './home/home.component';
import { VideoComponent } from './video/video.component';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'api-test', component: ApiTestComponent },
  { path: 'video/:id', component: VideoComponent }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
