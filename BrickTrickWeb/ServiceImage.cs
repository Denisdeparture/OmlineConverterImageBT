using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
namespace BrickTrickWeb
{
    public sealed class ServiceImage
    {
        public string? pathDirectory {  get; set; }
        public void ChangeImage(string path,string NewFormat,string filename)
        {
            Image image = Image.Load(path);
            var clone = image.Clone(p => p.GaussianBlur(15));
            image.Save(pathDirectory + filename + NewFormat);
            image.Dispose();
        }
       
    }
}
