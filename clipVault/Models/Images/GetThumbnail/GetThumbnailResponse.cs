using MediatR;

namespace clipVault.Models.Images.GetThumbnail
{
    public class GetThumbnailResponse 
    {      
        public byte[] imageData { get; set; } 
        public string fileType { get; set; }
        
    }
}
