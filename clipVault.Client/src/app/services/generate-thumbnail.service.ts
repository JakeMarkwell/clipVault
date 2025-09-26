import { Injectable } from '@angular/core';
import { ApiService } from '../api.service';
import { AzureBlobService } from './azure-blob.service';

@Injectable({
  providedIn: 'root'
})
export class GenerateThumbnailService {
  async generateAndUploadThumbnail(
    videoFile: File,
    title: string,
    friendTags: string,
    categoryIds: number[],
    apiService: ApiService,
    azureBlobService: AzureBlobService
  ): Promise<void> {
    const thumbnailBlob = await new Promise<Blob>((resolve, reject) => {
      const video = document.createElement('video');
      video.src = URL.createObjectURL(videoFile);
      video.currentTime = 5;
      video.muted = true;
      video.playsInline = true;

      video.addEventListener('loadeddata', () => {
        const canvas = document.createElement('canvas');
        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;
        const ctx = canvas.getContext('2d');
        ctx?.drawImage(video, 0, 0, canvas.width, canvas.height);
        canvas.toBlob(blob => {
          if (blob) resolve(blob);
          else reject(new Error('Failed to create thumbnail blob'));
        }, 'image/png');
      });

      video.addEventListener('error', () => reject(new Error('Failed to load video')));
    });

    const thumbnailFileName = videoFile.name.replace(/\.[^/.]+$/, '') + '.png';
    const sasResult = await apiService.getImageSasToken(thumbnailFileName).toPromise();
    const sasUrl = sasResult?.sasUrl;

    await azureBlobService.uploadFileToBlob(
      sasUrl || '',
      new File([thumbnailBlob], thumbnailFileName, { type: 'image/png' }),
      {
        title,
        friendTags,
        categoryIds: categoryIds.join(','),
        id: crypto.randomUUID()
      }
    );
  }
}

