using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace SAMS.Common.Helpers
{
    public static class HtmlToPdfGenerateHelper
    {
        public static Byte[] PdfConvertToHtml(String html)
        {

            var pageWidth = 595;
            var ml = 20;
            var mr = 20;
            var mt = 20;
            var mb = 20;
            Document document = new Document(PageSize.A4, ml, mr, mt, mb);            
            MemoryStream memStream = new MemoryStream();
            PdfWriter wri = PdfWriter.GetInstance(document, memStream);
            document.Open();
            string startupPath = Environment.CurrentDirectory;
            string fontPath = Path.Combine(startupPath, "Contents", "Fonts", "arial.ttf").Replace("Common.API", "Hsys.Common");
            FontFactory.Register(fontPath, "RArial");

            string fontPathTimes = Path.Combine(startupPath, "Contents", "Fonts", "TTimesb.ttf").Replace("Common.API", "Hsys.Common");
            FontFactory.Register(fontPath, "RTTimesb");

            StyleSheet css = new StyleSheet();
            css.LoadTagStyle("body", "face", "RArial");
            css.LoadTagStyle("body", "face", "RTTimesb");
            css.LoadTagStyle("body", "encoding", "Identity-H");
            css.LoadTagStyle("body", "size", "10pt");
            PdfPCell pdfCell = new PdfPCell
            {
                Border = 0,
                RunDirection = PdfWriter.RUN_DIRECTION_LTR
            };
            using (var reader = new StringReader("<html><body>"+html+ "</html></body>"))
            {
                var parsedHtmlElements = HtmlWorker.ParseToList(reader, css);

                foreach (IElement htmlElement in parsedHtmlElements)
                {
                    pdfCell.AddElement(htmlElement);
                }
            }
            var table1 = new PdfPTable(1);

            table1.HorizontalAlignment = 0;
            table1.TotalWidth = pageWidth - (ml + mr);
            table1.LockedWidth = true;           
            table1.AddCell(pdfCell);

            document.Add(table1);
            document.Close();
            memStream.ToArray();
            var bytePdf = memStream.ToArray();
            File.WriteAllBytes(@"C:\Ark\testpdf.pdf", bytePdf);

            return bytePdf;

        }
    }
}





