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
using ImageProcessingLibrary.Helpers;
using ImageProcessingLibrary.Interfaces;
using Logger = ImageProcessingLibrary.Logging.Logger;

namespace ImageProcessingLibrary.PictureAlignment
{
    internal class FaceAligner : IFaceAligner
    {
        private readonly ShapePredictor _shapePredictor;

        public FaceAligner()
        {
            // Load the pretrained shape predictor model from Dlib
            try
            {
                _shapePredictor = ShapePredictor.Deserialize("shape_predictor_68_face_landmarks.dat");
            }
            catch (Exception ex)
            {
                throw new ImageProcessingException("Failed to load shape predictor model.", ex);
            }
        }

        public void AlignFaces(string inputPath, string outputPath)
        {
            try
            {
                Logger.LogInfo($"Starting face alignment for image: {inputPath}");

                if (!File.Exists(inputPath))
                {
                    throw new FileNotFoundException($"Input file not found: {inputPath}");
                }

                using (var image = CvInvoke.Imread(inputPath))
                {
                    if (image == null || image.IsEmpty)
                    {
                        throw new ImageProcessingException($"Failed to load image: {inputPath}");
                    }

                    using (var alignedImage = AlignFace(image))
                    {
                        CvInvoke.Imwrite(outputPath, alignedImage);
                    }
                }

                Logger.LogInfo($"Successfully aligned face for image: {inputPath} -> {outputPath}");
            }
            catch (FileNotFoundException ex)
            {
                Logger.LogError(ex.Message);
            }
            catch (ImageProcessingException ex)
            {
                Logger.LogError($"Image processing error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Unexpected error aligning image {inputPath}: {ex.Message}");
            }
        }

        public Mat AlignFace(Mat image)
        {
            try
            {
                // Convert Emgu.CV Mat to Bitmap to use with Dlib
                using (var bitmap = image.ToBitmap())
                {
                    // Detect facial landmarks using the helper method
                    var landmarks = AlignmentHelper.DetectFacialLandmarks(bitmap, _shapePredictor);

                    // Define the desired facial points for alignment
                    var desiredPoints = new[]
                    {
                        new PointF(30.0f, 30.0f), // Left eye
                        new PointF(70.0f, 30.0f), // Right eye
                        new PointF(50.0f, 70.0f)  // Mouth center
                    };

                    // Compute affine transformation using the helper method
                    var transformation = AlignmentHelper.ComputeAffineTransform(landmarks, new List<PointF>(desiredPoints));

                    // Apply affine transformation using the helper method
                    var alignedImage = AlignmentHelper.ApplyAffineTransformation(image, transformation, image.Size);

                    return alignedImage;
                }
            }
            catch (Exception ex)
            {
                throw new ImageProcessingException("Error during face alignment.", ex);
            }
        }
    }
}
