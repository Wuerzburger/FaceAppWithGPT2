using Emgu.CV;
using Emgu.CV.CvEnum;
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
        public void ResizeImage(string inputPath, string outputPath, string resizeOption, string dimensionType)
        {
            // Validate input paths
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException($"Input file not found: {inputPath}");
            }

            using (var image = CvInvoke.Imread(inputPath))
            {
                if (resizeOption.EndsWith("%"))
                {
                    // Resize by percentage
                    int percentage = int.Parse(resizeOption.TrimEnd('%'));
                    using (var resizedImage = ResizeImageByPercentage(image, percentage))
                    {
                        CvInvoke.Imwrite(outputPath, resizedImage);
                    }
                }
                else if (int.TryParse(resizeOption, out int fixedSize))
                {
                    if (string.IsNullOrEmpty(dimensionType))
                    {
                        throw new ArgumentException("Dimension type must be specified when providing a fixed dimension.");
                    }
                    using (var resizedImage = dimensionType == "width"
                        ? ResizeImageKeepingAspectRatio(image, fixedSize, isWidth: true)
                        : ResizeImageKeepingAspectRatio(image, fixedSize, isWidth: false))
                    {
                        CvInvoke.Imwrite(outputPath, resizedImage);
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid resize option. Provide a percentage or a fixed size for width or height.");
                }
            }
        }

        public Mat ResizeImageKeepingAspectRatio(Mat image, int fixedSize, bool isWidth)
        {
            int newWidth, newHeight;

            if (isWidth)
            {
                newWidth = fixedSize;
                newHeight = (int)(image.Height * ((double)fixedSize / image.Width));
            }
            else
            {
                newHeight = fixedSize;
                newWidth = (int)(image.Width * ((double)fixedSize / image.Height));
            }

            var resizedImage = new Mat();
            CvInvoke.Resize(image, resizedImage, new System.Drawing.Size(newWidth, newHeight), 0, 0, Inter.Linear);

            return resizedImage;
        }

        public Mat ResizeImageByPercentage(Mat image, int percentage)
        {
            int newWidth = (int)(image.Width * (percentage / 100.0));
            int newHeight = (int)(image.Height * (percentage / 100.0));

            var resizedImage = new Mat();
            CvInvoke.Resize(image, resizedImage, new System.Drawing.Size(newWidth, newHeight), 0, 0, Inter.Linear);

            return resizedImage;
        }
    }
}
