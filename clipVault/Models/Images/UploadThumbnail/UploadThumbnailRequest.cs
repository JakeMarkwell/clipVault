﻿using MediatR;

namespace clipVault.Models.Images.UploadThumbnail
{
    public class UploadThumbnailRequest : IRequest<UploadThumbnailResponse>
    {
        public IFormFile File { get; set; }
        public byte[] Thumbnail { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
    }
}
