namespace Upload.Domain.Entities
{
    public class Video
    {
        public string Id { get; private set; }
        public string FileName { get; private set; }
        public string Extension { get; private set; }

        private Video(string fileName, string extension)
        {
            Id = Guid.NewGuid().ToString();
            FileName = fileName;
            Extension = extension.ToLower();
        }

        public static Video Create(string fileName, string extension)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be empty", nameof(fileName));

            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException("Extension cannot be empty", nameof(extension));

            return new Video(fileName, extension);
        }

        public bool HasValidExtension()
        {
            var validExtensions = new[] { ".mp4", ".avi", ".mkv", ".mov", ".webm" };
            return validExtensions.Contains(Extension.ToLower());
        }
    }
}
