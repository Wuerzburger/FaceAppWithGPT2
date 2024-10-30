using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessingLibrary.PictureSizeAdaptation;
using NUnit.Framework;

namespace ImageProcessingLibrary.Tests
{
    [TestFixture]
    public class ImageResizerTests
    {
        [Test]
        public void ResizeImageKeepingAspectRatio_ShouldResizeBasedOnWidth_WhenWidthIsProvided()
        {
            // Arrange
            var imageResizer = new ImageResizer();
            string tempDirectory = Path.GetTempPath();
            string inputPath = Path.Combine(tempDirectory, "input.jpg");
            string outputPath = Path.Combine(tempDirectory, "output.jpg");

            // Create a valid dummy image file
            using (Bitmap bitmap = new Bitmap(200, 100))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    g.DrawRectangle(Pens.Black, 10, 10, 180, 80);
                }
                bitmap.Save(inputPath, ImageFormat.Jpeg);
            }

            try
            {
                // Act
                imageResizer.ResizeImage(inputPath, outputPath, "100", "width");

                // Assert
                Assert.IsTrue(File.Exists(outputPath));
                using (var outputImage = Image.FromFile(outputPath))
                {
                    Assert.AreEqual(100, outputImage.Width);
                    Assert.AreEqual(50, outputImage.Height); // Aspect ratio maintained
                }
            }
            finally
            {
                // Cleanup
                File.Delete(inputPath);
                File.Delete(outputPath);
            }
        }

        [Test]
        public void ResizeImageKeepingAspectRatio_ShouldResizeBasedOnHeight_WhenHeightIsProvided()
        {
            // Arrange
            var imageResizer = new ImageResizer();
            string tempDirectory = Path.GetTempPath();
            string inputPath = Path.Combine(tempDirectory, "input.jpg");
            string outputPath = Path.Combine(tempDirectory, "output.jpg");

            // Create a valid dummy image file
            using (Bitmap bitmap = new Bitmap(200, 100))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    g.DrawRectangle(Pens.Black, 10, 10, 180, 80);
                }
                bitmap.Save(inputPath, ImageFormat.Jpeg);
            }

            try
            {
                // Act
                imageResizer.ResizeImage(inputPath, outputPath, "50", "height");

                // Assert
                Assert.IsTrue(File.Exists(outputPath));
                using (var outputImage = Image.FromFile(outputPath))
                {
                    Assert.AreEqual(100, outputImage.Width); // Aspect ratio maintained
                    Assert.AreEqual(50, outputImage.Height);
                }
            }
            finally
            {
                // Cleanup
                File.Delete(inputPath);
                File.Delete(outputPath);
            }
        }

        [Test]
        public void ResizeImageByPercentage_ShouldResizeImageCorrectly_WhenPercentageIsProvided()
        {
            // Arrange
            var imageResizer = new ImageResizer();
            string tempDirectory = Path.GetTempPath();
            string inputPath = Path.Combine(tempDirectory, "input.jpg");
            string outputPath = Path.Combine(tempDirectory, "output.jpg");

            // Create a valid dummy image file
            using (Bitmap bitmap = new Bitmap(200, 100))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    g.DrawRectangle(Pens.Black, 10, 10, 180, 80);
                }
                bitmap.Save(inputPath, ImageFormat.Jpeg);
            }

            try
            {
                // Act
                imageResizer.ResizeImage(inputPath, outputPath, "50%", "");

                // Assert
                Assert.IsTrue(File.Exists(outputPath));
                using (var outputImage = Image.FromFile(outputPath))
                {
                    Assert.AreEqual(100, outputImage.Width); // 50% of original width
                    Assert.AreEqual(50, outputImage.Height);  // 50% of original height
                }
            }
            finally
            {
                // Cleanup
                File.Delete(inputPath);
                File.Delete(outputPath);
            }
        }
    }
}
