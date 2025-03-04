﻿using clipVault.Models.Images.GetThumbnail;
using clipVault.Repositories.Images;
using MediatR;

namespace clipVault.Handlers.Images
{
    public class GetThumbnailHandler : IRequestHandler<GetThumbnailRequest, GetThumbnailResponse>
    {
        private readonly IGetThumbnailRepository _repository;

        public GetThumbnailHandler(IGetThumbnailRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetThumbnailResponse> Handle(GetThumbnailRequest request, CancellationToken cancellationToken)
        {
            var imageData = await _repository.GetThumbnailAsync(request.id, cancellationToken);

            return new GetThumbnailResponse
            {
                imageData = imageData,
                fileType = "image/png"
            };
        }
    }
}
