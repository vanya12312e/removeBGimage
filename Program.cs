using System;
using System.IO;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using System.Drawing.Imaging;

class AnyToPng
{
    private static void Main(string[] args)
    {
        AnyToPng anyToPng = new();
        Console.WriteLine("Input path of photos");
        
        string _filePath = Console.ReadLine();

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
        _filePath = _filePath.Replace("\"", "");
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

        anyToPng.MakePNG(_filePath);
    }

    private void MakePNG(string _folderPath)

    {
        string[] _files = Directory.GetFiles(_folderPath);

        foreach (string file in _files)
        {
            string _newFileName = Path.ChangeExtension(file, ".png");

            File.Move(file, _newFileName);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"File {file} was renamed to {_newFileName}");
        }


        Console.WriteLine("\nAll files have been renamed.");

        Console.ResetColor();

        Console.ReadLine();
    }

  
    private void MakeTranspent()
    {
        
    }
}
