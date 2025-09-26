import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule } from '@angular/forms';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatTreeModule } from '@angular/material/tree';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { MatIconModule } from '@angular/material/icon';
import { GetThumbnailToolComponent } from './get-thumbnail-tool/get-thumbnail-tool.component';
import { UploadVideoToolComponent } from './upload-video-tool/upload-video-tool.component';
import { GetCategoriesToolComponent } from './get-categories-tool/get-categories-tool.component';
import { AddVideoCategoryToolComponent } from './add-video-category-tool/add-video-category-tool.component';

interface ApiTreeNode {
  name: string;
  api?: string;
  children?: ApiTreeNode[];
}

interface FlatNode {
  expandable: boolean;
  name: string;
  api: string | undefined;
  level: number;
}

const TREE_DATA: ApiTreeNode[] = [
  {
    name: 'Video',
    children: [
      { name: 'UploadVideo', api: 'uploadVideo' }
    ]
  },
  {
    name: 'Thumbnail',
    children: [
      { name: 'GetThumbnail', api: 'getThumbnail' }
    ]
  },
  {
    name: 'Categories',
    children: [
      { name: 'GetVideoCategories', api: 'getVideoCategories' },
      { name: 'AddVideoCategory', api: 'addVideoCategory' }
    ]
  }
];

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    FormsModule,
    MatSidenavModule,
    MatTableModule,
    MatSelectModule,
    MatTreeModule,
    MatIconModule,
    GetThumbnailToolComponent,
    UploadVideoToolComponent,
    GetCategoriesToolComponent,
    AddVideoCategoryToolComponent
  ],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.scss',
})
export class AdminPanelComponent {
  selectedApi: string = 'getThumbnail';

  private _transformer = (node: ApiTreeNode, level: number) => ({
    expandable: !!node.children && node.children.length > 0,
    name: node.name,
    api: node.api,
    level: level,
  });

  apiTreeControl = new FlatTreeControl<FlatNode>(
    node => node.level,
    node => node.expandable
  );

  hasChild = (_: number, node: FlatNode) => node.expandable;

  apiTreeFlattener = new MatTreeFlattener(
    this._transformer,
    node => node.level,
    node => node.expandable,
    node => node.children
  );

  apiTreeDataSource = new MatTreeFlatDataSource(this.apiTreeControl, this.apiTreeFlattener);

  constructor() {
    this.apiTreeDataSource.data = TREE_DATA;
  }

  selectApi(api: string): void {
    this.selectedApi = api;
  }
}
