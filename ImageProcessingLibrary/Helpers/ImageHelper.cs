using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using DlibDotNet;
using DlibDotNet.Extensions;
using System.Drawing;
using System.Collections.Generic;
using System;
using ImageProcessingLibrary.Exceptions;
using ImageProcessingLibrary.Logging;

namespace ImageProcessingLibrary.Helpers
{
    public static class AlignmentHelper
    {
        /// <summary>
        /// Detects facial landmarks for a given image using a shape predictor.
        /// </summary>
        /// <param name="image">The input image as a Bitmap.</param>
        /// <param name="shapePredictor">The Dlib shape predictor model to use for detection.</param>
        /// <returns>A list of detected facial landmarks as PointFs.</returns>
        public static List<PointF> DetectFacialLandmarks(Bitmap image, ShapePredictor shapePredictor)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "Input image cannot be null.");

            if (shapePredictor == null)
                throw new ArgumentNullException(nameof(shapePredictor), "Shape predictor cannot be null.");

            using (var dlibImage = image.ToArray2D<RgbPixel>())
            using (var detector = Dlib.GetFrontalFaceDetector())
            {
                var faces = detector.Operator(dlibImage);
                if (faces.Length == 0)
                    throw new ImageProcessingException("No face detected in the image.");

                var face = faces[0]; // Assuming only one face for simplicity
                var landmarks = shapePredictor.Detect(dlibImage, face);

                var points = new List<PointF>();
                for (uint i = 0; i < landmarks.Parts; i++)
                {
                    points.Add(new PointF((float)landmarks.GetPart(i).X, (float)landmarks.GetPart(i).Y));
                }

                return points;
            }
        }

        /// <summary>
        /// Computes the affine transformation matrix required to align facial landmarks to desired points.
        /// </summary>
        /// <param name="sourcePoints">The current facial landmarks as a list of PointF.</param>
        /// <param name="destinationPoints">The desired facial points for alignment.</param>
        /// <returns>The affine transformation matrix as a Mat.</returns>
        public static Mat ComputeAffineTransform(List<PointF> sourcePoints, List<PointF> destinationPoints)
        {
            if (sourcePoints == null || sourcePoints.Count != 3)
                throw new ArgumentException("Source points must contain exactly 3 points.", nameof(sourcePoints));

            if (destinationPoints == null || destinationPoints.Count != 3)
                throw new ArgumentException("Destination points must contain exactly 3 points.", nameof(destinationPoints));

            return CvInvoke.GetAffineTransform(sourcePoints.ToArray(), destinationPoints.ToArray());
        }

        /// <summary>
        /// Applies an affine transformation to an image to align it based on a given transformation matrix.
        /// </summary>
        /// <param name="image">The input image as a Mat.</param>
        /// <param name="transformationMatrix">The affine transformation matrix.</param>
        /// <param name="outputSize">The desired size of the output image.</param>
        /// <returns>The aligned image as a Mat.</returns>
        public static Mat ApplyAffineTransformation(Mat image, Mat transformationMatrix, Size outputSize)
        {
            if (image == null || image.IsEmpty)
                throw new ArgumentNullException(nameof(image), "Input image cannot be null or empty.");

            if (transformationMatrix == null || transformationMatrix.IsEmpty)
                throw new ArgumentNullException(nameof(transformationMatrix), "Transformation matrix cannot be null or empty.");

            var alignedImage = new Mat();
            CvInvoke.WarpAffine(image, alignedImage, transformationMatrix, outputSize, Inter.Linear, Warp.Default, BorderType.Constant, new MCvScalar(0, 0, 0));

            return alignedImage;
        }
    }
}
