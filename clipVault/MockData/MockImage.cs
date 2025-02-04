namespace clipVault.MockData
{
    public class MockImage
    {
        public string id { get; set; }
        public byte[] imageData { get; set; }
        public string fileType { get; set; }

        // Static array of mock images
        public static MockImage[] MockImageArray = new MockImage[]
        {
        new MockImage { id = "1", imageData = GenerateDummyImage(0), fileType = "image/jpeg" }, 
        new MockImage { id = "2", imageData = GenerateDummyImage(1), fileType = "image/png" },
        new MockImage { id = "3", imageData = GenerateDummyImage(2), fileType = "image/gif" } 
        };

        // Mock image generator
        private static byte[] GenerateDummyImage(int id)
        {
            var random = new Random(id);
            var imageBytes = new byte[100 + random.Next(100)];
            random.NextBytes(imageBytes); 

            return imageBytes;
        }
    }
}
