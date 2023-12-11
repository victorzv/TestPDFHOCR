using Aspose.Pdf;
using Aspose.Pdf.Text;

namespace TestPDFHOCR;

public class TextColorChanger
{
    public static void SetColor(Document document)
    {
        ParagraphAbsorber absorb = new ParagraphAbsorber();
        absorb.Visit(document);
        
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
                                Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Black);
                        }
                    }
                }
            }
        }
    }
}