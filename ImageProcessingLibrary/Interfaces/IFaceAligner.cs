using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLibrary.Interfaces
{
    internal interface IFaceAligner
    {
        void AlignFaces(string inputPath, string outputPath);
        Mat AlignFace(Mat image);
    }
}
