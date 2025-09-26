import { Injectable } from '@angular/core';
import { BlockBlobClient } from '@azure/storage-blob';

@Injectable({
  providedIn: 'root'
})
export class AzureBlobService {
  async uploadFileToBlob(sasUrl: string, file: File, metadata?: { [key: string]: string }): Promise<void> {
    const blockBlobClient = new BlockBlobClient(sasUrl);
    await blockBlobClient.uploadBrowserData(file, {
      blobHTTPHeaders: { blobContentType: file.type },
      metadata: metadata
    });
  }
}
