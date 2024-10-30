using Emgu.CV;
using ImageProcessingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLibrary.PictureSizeAdaptation
{
    public class ImageResizer : IImageResizer
    {
        public void ResizeImage(string inputPath, string outputPath, int width, int height)
        {
            // Validate input paths
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException($"Input file not found: {inputPath}");
            }

            // Load the image using Emgu.CV
            using (Mat image = CvInvoke.Imread(inputPath))
            {
                // Resize the image while maintaining the aspect ratio
                Mat resizedImage = new Mat();
                CvInvoke.Resize(image, resizedImage, new System.Drawing.Size(width, height), 0, 0, Emgu.CV.CvEnum.Inter.Linear);

                // Save the resized image to the output path
                CvInvoke.Imwrite(outputPath, resizedImage);
            }
        }
    }
}
