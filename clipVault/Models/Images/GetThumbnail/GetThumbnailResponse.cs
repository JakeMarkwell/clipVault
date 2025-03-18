using MediatR;

namespace clipVault.Models.Images.GetThumbnail
{
    public class GetThumbnailResponse 
    {      
        required public byte[] imageData { get; set; } 
        required public string fileType { get; set; }
        required public string title { get; set; }
        
    }
}
