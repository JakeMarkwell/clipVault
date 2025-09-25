import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-home-video-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home-video-card.component.html',
  styleUrls: ['./home-video-card.component.scss']
})
export class HomeVideoCardComponent {
  @Input() imageData: string = '';
  @Input() title: string = '';
  @Input() friendTags: string = '';
  @Input() categoryIds: number[] = [];
}
