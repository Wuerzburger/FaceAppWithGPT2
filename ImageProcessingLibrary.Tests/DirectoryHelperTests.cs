using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessingLibrary.Helpers;
using NUnit.Framework;

namespace ImageProcessingLibrary.Tests
{
    [TestFixture]
    public class DirectoryHelperTests
    {
        [Test]
        public void ValidateDirectory_ShouldThrowArgumentNullException_WhenPathIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => DirectoryHelper.ValidateDirectory(null));
        }

        [Test]
        public void ValidateDirectory_ShouldThrowArgumentException_WhenPathIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => DirectoryHelper.ValidateDirectory(""));
        }

        [Test]
        public void ValidateDirectory_ShouldThrowDirectoryNotFoundException_WhenDirectoryDoesNotExist()
        {
            // Arrange
            string nonExistentDirectory = "C:\\NonExistentDirectory";

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelper.ValidateDirectory(nonExistentDirectory));
        }

        [Test]
        public void ValidateDirectory_ShouldNotThrowException_WhenDirectoryExists()
        {
            // Arrange
            string existingDirectory = Path.GetTempPath();

            // Act & Assert
            Assert.DoesNotThrow(() => DirectoryHelper.ValidateDirectory(existingDirectory));
        }

        [Test]
        public void GetImageFiles_ShouldReturnEmptyList_WhenNoImagesArePresent()
        {
            // Arrange
            string tempDirectory = Path.Combine(Path.GetTempPath(), "EmptyDirectory");
            Directory.CreateDirectory(tempDirectory);

            try
            {
                // Act
                List<string> imageFiles = DirectoryHelper.GetImageFiles(tempDirectory);

                // Assert
                Assert.AreEqual(0, imageFiles.Count);
            }
            finally
            {
                // Cleanup
                Directory.Delete(tempDirectory);
            }
        }

        [Test]
        public void GetImageFiles_ShouldReturnImageFiles_WhenImagesArePresent()
        {
            // Arrange
            string tempDirectory = Path.Combine(Path.GetTempPath(), "ImageDirectory");
            Directory.CreateDirectory(tempDirectory);

            string imagePath1 = Path.Combine(tempDirectory, "image1.jpg");
            string imagePath2 = Path.Combine(tempDirectory, "image2.png");
            File.Create(imagePath1).Dispose();
            File.Create(imagePath2).Dispose();

            try
            {
                // Act
                List<string> imageFiles = DirectoryHelper.GetImageFiles(tempDirectory);

                // Assert
                Assert.AreEqual(2, imageFiles.Count);
                Assert.Contains(imagePath1, imageFiles);
                Assert.Contains(imagePath2, imageFiles);
            }
            finally
            {
                // Cleanup
                Directory.Delete(tempDirectory, true);
            }
        }
    }
}
