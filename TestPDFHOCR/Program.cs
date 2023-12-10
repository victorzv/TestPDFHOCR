using Aspose.Pdf;
using Aspose.Pdf.Text;

class Program
{
    static void Main(string[] args)
    {
        new License().SetLicense("./test.lic");

        var pdfDocument = new Document("./test.pdf");

        Document.CallBackGetHocr recognizeText = (Aspose.Pdf.Drawing.PdfImage img) =>
        {
            string outputFolder = "./";
            string tmpFile = "./789.jpg";//Path.Combine(outputFolder, Path.GetFileName(Path.GetTempFileName()));
            tmpFile = Path.ChangeExtension(tmpFile, "jpg");
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
        };

        bool result = pdfDocument.Convert(recognizeText);
        //var page = pdfDocument.Pages[1];
        //page.Resources.Images.Replace(1, File.OpenRead("./blank.png"));
        pdfDocument.Save("./searchable_test.pdf");
        pdfDocument.Dispose();

        var doc2 = new Document("./searchable_test.pdf");
        doc2.Pages[1].Resources.Images.Delete(1);
        doc2.Save("./searchable_test.pdf");


        //Console.WriteLine(pdfDocument.Pages[1].Resources.Images.Count);

        //pdfDocument.Pages[1].Resources.Images.Delete(1);
        /*
        ParagraphAbsorber absorb = new ParagraphAbsorber();
        absorb.Visit(pdfDocument);

        var pdfNewDoc = new Document();
        Page page = pdfNewDoc.Pages.Add();

        foreach (PageMarkup markup in absorb.PageMarkups)
        {
            foreach (MarkupSection section in markup.Sections)
            {
                foreach (MarkupParagraph paragraph in section.Paragraphs)
                {
                    foreach (List<TextFragment> line in paragraph.Lines)
                    {
                        foreach (TextFragment textFragment in line)
                        {
                            textFragment.TextState.ForegroundColor =
                                Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Green);
                            textFragment.TextState.BackgroundColor =
                                Aspose.Pdf.Color.FromRgb(System.Drawing.Color.White);
                            page.Paragraphs.Add(textFragment);
                        }
                    }
                }
            }
        }

        pdfNewDoc.Save("/home/tigra/MD_TEST_NEW.pdf");*/

    }
}