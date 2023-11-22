using Aspose.Pdf;

class Program
{
    static void Main(string[] args)
    {
        new License().SetLicense("/home/tigra/test.lic");
        
        var pdfDocument = new Document("/home/tigra/MD_TEST.pdf");
        
        Document.CallBackGetHocr recognizeText = (Aspose.Pdf.Drawing.PdfImage img) =>
        {
            string outputFolder="/home/tigra/";
            string tmpFile = "/home/tigra/789.jpg";//Path.Combine(outputFolder, Path.GetFileName(Path.GetTempFileName()));
            tmpFile = Path.ChangeExtension(tmpFile, "jpg");
            try
            {
                using (var fs = File.Create(tmpFile))
                {
                    img.BitmapStream.Seek(0, SeekOrigin.Begin);
                    img.BitmapStream.CopyTo(fs);
                }

                string pathTempFile = $"\"{tmpFile}\"";
                string arguments = $"{pathTempFile} {pathTempFile} --oem 1 -l eng hocr";
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
        
        bool result = pdfDocument.Convert(recognizeText);
        Console.WriteLine(result);
        //pdfDocument.Save();
    }
}