export interface ThumbnailResponse {
    imageData: Blob;
    fileType: string;
    title: string;
    friendTags: string[];
    categoryTags: string[];
}