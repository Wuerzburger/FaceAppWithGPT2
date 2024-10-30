using Emgu.CV;
using Emgu.CV.CvEnum;
using ImageProcessingLibrary.Interfaces;
using ImageProcessingLibrary.Logging;
using ImageProcessingLibrary.Exceptions;
using System;
using System.IO;

namespace ImageProcessingLibrary.PictureSizeAdaptation
{

        public class ImageResizer : IImageResizer
        {
            public void ResizeImage(string inputPath, string outputPath, string resizeOption, string dimensionType)
            {
                try
                {
                    // Log the start of the resize process
                    Logger.LogInfo($"Starting resizing for image: {inputPath}");

                    // Validate input paths
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

                        if (resizeOption.EndsWith("%"))
                        {
                            int percentage = int.Parse(resizeOption.TrimEnd('%'));
                            using (var resizedImage = ResizeImageByPercentage(image, percentage))
                            {
                                CvInvoke.Imwrite(outputPath, resizedImage);
                            }
                        }
                        else if (int.TryParse(resizeOption, out int fixedSize))
                        {
                            using (var resizedImage = dimensionType == "width"
                                ? ResizeImageKeepingAspectRatio(image, fixedSize, isWidth: true)
                                : ResizeImageKeepingAspectRatio(image, fixedSize, isWidth: false))
                            {
                                CvInvoke.Imwrite(outputPath, resizedImage);
                            }
                        }
                    }

                    // Log the completion of the resize process
                    Logger.LogInfo($"Successfully resized image: {inputPath} -> {outputPath}");
                }
                catch (FileNotFoundException ex)
                {
                    Logger.LogError($"File not found: {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    Logger.LogError($"Invalid argument: {ex.Message}");
                }
                catch (ImageProcessingException ex)
                {
                    Logger.LogError($"Image processing error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Unexpected error resizing image {inputPath}: {ex.Message}");
                }
            }

            public Mat ResizeImageKeepingAspectRatio(Mat image, int fixedSize, bool isWidth)
            {
                try
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
                catch (Exception ex)
                {
                    throw new ImageProcessingException("Error while resizing the image while keeping aspect ratio.", ex);
                }
            }

            public Mat ResizeImageByPercentage(Mat image, int percentage)
            {
                try
                {
                    int newWidth = (int)(image.Width * (percentage / 100.0));
                    int newHeight = (int)(image.Height * (percentage / 100.0));

                    var resizedImage = new Mat();
                    CvInvoke.Resize(image, resizedImage, new System.Drawing.Size(newWidth, newHeight), 0, 0, Inter.Linear);

                    return resizedImage;
                }
                catch (Exception ex)
                {
                    throw new ImageProcessingException("Error while resizing the image by percentage.", ex);
                }
            }
        }
    }
