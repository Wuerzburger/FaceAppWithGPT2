using Emgu.CV;
using ImageProcessingLibrary.Helpers;
using ImageProcessingLibrary.PictureSizeAdaptation;

namespace FaceAppWithGPT2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: FaceAppWithGPT2 <inputDirectory> <outputDirectory> <width|height|percentage> [dimensionType]");
                Console.WriteLine("dimensionType: 'width' or 'height' (only required if providing a fixed dimension)");
                return;
            }

            string inputDirectory = args[0];
            string outputDirectory = args[1];
            string resizeOption = args[2];
            string dimensionType = args.Length > 3 ? args[3].ToLower() : string.Empty;

            try
            {
                // Validate directories
                DirectoryHelper.ValidateDirectory(inputDirectory);
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Get image files from the input directory
                var imageFiles = DirectoryHelper.GetImageFiles(inputDirectory);

                // Instantiate the ImageResizer
                var imageResizer = new ImageResizer();

                // Resize each image and save it to the output directory
                foreach (var imagePath in imageFiles)
                {
                    string outputPath = Path.Combine(outputDirectory, Path.GetFileName(imagePath));
                    imageResizer.ResizeImage(imagePath, outputPath, resizeOption, dimensionType);
                    Console.WriteLine($"Resized image saved to: {outputPath}");
                }

                Console.WriteLine("Image resizing completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    
    }
}
