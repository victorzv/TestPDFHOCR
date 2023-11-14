using Aspose.Pdf;

class Program
{
    static void Main(string[] args)
    {
        Document.CallBackGetHocr recognizeText = (Aspose.Pdf.Drawing.PdfImage img) =>
        {
            string outputFolder="/home/tigra/";
            string tmpFile = Path.Combine(outputFolder, Path.GetFileName(Path.GetTempFileName()));
            tmpFile = Path.ChangeExtension(tmpFile, "png");
            try
            {
                using (var fs = File.Create(tmpFile))
                {
                    img.BitmapStream.Seek(0, SeekOrigin.Begin);
                    img.BitmapStream.CopyTo(fs);
                }

                string pathTempFile = $"\"{tmpFile}\"";
                string arguments = $"{pathTempFile} {pathTempFile} --oem 1 -l hocr";
                string tesseractProcessName = @"tesseract";

                System.Diagnostics.ProcessStartInfo psi =
                    new System.Diagnostics.ProcessStartInfo(tesseractProcessName, arguments);

                using (System.Diagnostics.Process p = new System.Diagnostics.Process())
                {
                    p.StartInfo = psi;
                    p.Start();
                    p.PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal;
                    p.WaitForExit();
                }

                return File.ReadAllText($"{tmpFile}.hocr");
            }
            catch (Exception)
            {
                return string.Empty;
            }
            finally
            {
                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);
                if (File.Exists(tmpFile + ".hocr"))
                    File.Delete(tmpFile + ".hocr");
            }
        };
       
        PdfDocument.Convert(recognizeText);        
    }
}