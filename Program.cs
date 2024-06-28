using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

class AnyToPng
{
    public static void Main(string[] args)
    {
        AnyToPng anyToPng = new();
        Console.WriteLine("Input path of photos:");

        string _folderPath = Console.ReadLine();
        _folderPath = _folderPath.Trim('"'); 

        anyToPng.MakePNG(_folderPath);
        anyToPng.MakeTransparent(_folderPath, _folderPath + @"\IsConverted");
    }

    private void MakePNG(string _folderPath)
    {
        string[] _files = Directory.GetFiles(_folderPath);
        string _outputFolderPath = Path.Combine(_folderPath, "IsConverted");

        if (!Directory.Exists(_outputFolderPath))
        {
            Directory.CreateDirectory(_outputFolderPath);
        }

        foreach (string file in _files)
        {
            if (IsImageFile(file))
            {
                string _newFileName = Path.Combine(_outputFolderPath, Path.GetFileNameWithoutExtension(file) + ".png");

                try
                {
                    using (Bitmap bitmap = new Bitmap(file))
                    {
                        if (HasTransparentBackground(bitmap))
                        {
                            bitmap.Save(_newFileName, ImageFormat.Png);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"File {file} was copied to {_newFileName}");
                        }
                        else
                        {
                            MakeTransparent(file, _newFileName);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"File {file} was converted and background was made transparent in {_newFileName}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to process {file}: {ex.Message}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"File {file} is not a valid image and was skipped.");
            }
        }

        Console.ResetColor();
        Console.WriteLine("\nAll files have been processed.");
        Console.ReadLine();
    }

    private bool IsImageFile(string filePath)
    {
        string[] validExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff" };
        string fileExtension = Path.GetExtension(filePath).ToLower();
        return Array.Exists(validExtensions, ext => ext == fileExtension);
    }

    private bool HasTransparentBackground(Bitmap bitmap)
    {
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                if (bitmap.GetPixel(x, y).A < 255)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void MakeTransparent(string inputFilePath, string outputFilePath)
    {
        using (Bitmap bitmap = new Bitmap(inputFilePath))
        {
            Bitmap transparentBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);

            Color backgroundColor = bitmap.GetPixel(0, 0);

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    if (pixelColor == backgroundColor)
                    {
                        transparentBitmap.SetPixel(x, y, Color.Transparent);
                    }
                    else
                    {
                        transparentBitmap.SetPixel(x, y, pixelColor);
                    }
                }
            }

            transparentBitmap.Save(outputFilePath, ImageFormat.Png);
        }
    }
}
