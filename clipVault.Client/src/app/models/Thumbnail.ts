// src/app/models/Thumbnail.ts

export interface ThumbnailResponse {
  imageData: string;
  fileType: string;
  title: string;
  friendTags?: string;
  categoryTags?: string;
}