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
        public void ResizeImage_ShouldThrowFileNotFoundException_WhenInputFileDoesNotExist()
        {
            // Arrange
            var imageResizer = new ImageResizer();
            string nonExistentFilePath = "C:\\NonExistentFile.jpg";
            string outputPath = Path.Combine(Path.GetTempPath(), "output.jpg");

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => imageResizer.ResizeImage(nonExistentFilePath, outputPath, 100, 100));
        }

        [Test]
        public void ResizeImage_ShouldCreateResizedImage_WhenInputFileExists()
        {
            // Arrange
            var imageResizer = new ImageResizer();
            string tempDirectory = Path.GetTempPath();
            string inputPath = Path.Combine(tempDirectory, "input.jpg");
            string outputPath = Path.Combine(tempDirectory, "output.jpg");

            // Create a valid dummy image file
            using (Bitmap bitmap = new Bitmap(200, 200))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    g.DrawRectangle(Pens.Black, 10, 10, 180, 180);
                }

                bitmap.Save(inputPath, ImageFormat.Jpeg);
            }

            try
            {
                // Act
                imageResizer.ResizeImage(inputPath, outputPath, 100, 100);

                // Assert
                Assert.IsTrue(File.Exists(outputPath));
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
