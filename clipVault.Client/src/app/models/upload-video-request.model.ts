export interface UploadVideoRequest {
  file: File;
  title: string;
  friendTags: string;
  categoryIds: number[];
}
