using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PdfiumViewer;

namespace QuikFormatDesktop.ViewModels.PdfToPng
{
    public static class PdfToPngConverter
    {
        private const int DPI = 300;

        public static List<string> Convert(string pdfPath, string outputFolder)
        {
            List<string> pictures = new List<string>();
            using (var document = PdfDocument.Load(pdfPath))
            {
                for (int i = 0; i < document.PageCount; i++)
                {
                    using (var image = document.Render(i, DPI, DPI, true)) 
                    {
                        string path = Path.Combine(outputFolder, $"page_{i + 1}.png");
                        image.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                        pictures.Add(path);
                    }
                }
            }
            return pictures;
        }
    }
}
