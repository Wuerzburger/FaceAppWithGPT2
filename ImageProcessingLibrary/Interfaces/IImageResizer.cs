using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLibrary.Interfaces
{

    
        public interface IImageResizer
        {
            /// <summary>
            /// Resizes the image while maintaining the aspect ratio, based on a given fixed size for width or height.
            /// </summary>
            /// <param name="inputPath">The path of the input image.</param>
            /// <param name="outputPath">The path where the resized image will be saved.</param>
            /// <param name="resizeOption">The resize option, either a fixed size or percentage.</param>
            /// <param name="dimensionType">Indicates whether the fixed size is for width ("width") or height ("height").</param>
            void ResizeImage(string inputPath, string outputPath, string resizeOption, string dimensionType);

            /// <summary>
            /// Resizes the image while maintaining the aspect ratio, based on a given fixed size for width or height.
            /// </summary>
            /// <param name="image">The input image as a Mat object.</param>
            /// <param name="fixedSize">The fixed size for either width or height.</param>
            /// <param name="isWidth">Indicates whether the fixed size is for width (true) or height (false).</param>
            Mat ResizeImageKeepingAspectRatio(Mat image, int fixedSize, bool isWidth);

            /// <summary>
            /// Resizes the image by a given percentage, maintaining the original aspect ratio.
            /// </summary>
            /// <param name="image">The input image as a Mat object.</param>
            /// <param name="percentage">The percentage by which the image should be resized.</param>
            Mat ResizeImageByPercentage(Mat image, int percentage);
        }
}
