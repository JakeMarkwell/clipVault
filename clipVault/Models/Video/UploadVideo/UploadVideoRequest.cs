﻿using MediatR;
using Microsoft.AspNetCore.Http;

namespace clipVault.Models.Videos.UploadVideo
{
    public class UploadVideoRequest : IRequest<UploadVideoResponse>
    {
        public IFormFile File { get; set; }

        public string Title { get; set; }

        public string? Tags { get; set; }
    }
}
