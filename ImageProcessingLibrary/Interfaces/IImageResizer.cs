using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLibrary.Interfaces
{
    internal interface IImageResizer
    {
        void ResizeImage(string inputPath, string outputPath, int width, int height);
    }
}
