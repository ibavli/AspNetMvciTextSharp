using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;

namespace AspNetMvciTextSharp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GettingStarted()
        {
            //string path = Server.MapPath("PDFs");

            var document1 = new Document();//Dökümanı oluşturalım
            var document1Path = Path.Combine(Server.MapPath("~/PDFs/document1.pdf"));
            PdfWriter.GetInstance(document1, new FileStream(document1Path, FileMode.Create));
            document1.Open();
            document1.Add(new Paragraph("My first PDF"));
            document1.Close();

            //A5 boyutunda oluşturmak için.
            var documentA5 = new Document(PageSize.A5);
            var documentA5Path = Path.Combine(Server.MapPath("~/PDFs/documentA5.pdf"));
            PdfWriter.GetInstance(documentA5, new FileStream(documentA5Path, FileMode.Create));
            documentA5.Open();
            documentA5.Add(new Paragraph("This is a A5 size"));
            documentA5.Close();


            //Kendi boyutumuzu ayarlamak
            var documentCustomSize = new Document(new Rectangle(120f, 330f));
            var documentCustomSizePath = Path.Combine(Server.MapPath("~/PDFs/documentCustomSize.pdf"));
            PdfWriter.GetInstance(documentCustomSize, new FileStream(documentCustomSizePath, FileMode.Create));
            documentCustomSize.Open();
            documentCustomSize.Add(new Paragraph("This is a custom size"));
            documentCustomSize.Close();

            TempData["url"] = "/PDFs/document1.pdf";
            return RedirectToAction("Index");
        }

        public ActionResult WorkingWithFonts()
        {
            //Desteklenen fontlar
            //Courier,
            //Courier Bold,
            //Courier Italic,
            //Courier Bold and Italic,
            //Helvetica,
            //Helvetica Bold,
            //Helvetica Italic,
            //Helvetica Bold and Italic,
            //Times Roman,
            //Times Roman Bold,
            //Times Roman Italic,
            //Times Roman Bold and Italic,
            //Symbol,
            //ZapfDingBats®

            //Varsayılan font => Helvetica, 12pt, black color, normal

            //Fontlar ile çalışmanın üç yöntemi vardır.
            //1. => BaseFont.CreateFont()
            //2. => FontFactory.GetFont()
            //3. => Font

            //BaseFont.CreateFont() => Yalnızca bir font tanımlamamızı sağlar.
            //FontFactory.GetFont() => Doğrudan çalışabileceğiniz geçerli ve yeni bir Font nesnesi döndürür. 14 adet override vardır. Bu nedenle, büyük olasılıkla FontFactory.GetFont() yöntemini kullanacaksınız.

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 25);

            string path = Server.MapPath("PDFs");
            Document document = new Document();
            var documentPath = Path.Combine(Server.MapPath("~/PDFs/documentFonts.pdf"));
            PdfWriter.GetInstance(document, new FileStream(documentPath, FileMode.Create));
            document.Open();
            document.Add(new Paragraph("This is a Test Font", times));


            int totalfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
            StringBuilder sb = new StringBuilder();
            foreach (string fontname in FontFactory.RegisteredFonts)
            {
                sb.Append(fontname + "\n");
            }
            document.Add(new Paragraph("All Fonts:\n" + sb.ToString()));
            document.Close();

            TempData["url"] = "/PDFs/documentFonts.pdf";
            return RedirectToAction("Index");

        }


        public ActionResult ChunksPhrasesParagraphs()
        {
            var chunksPhrasesParagraphsPath = Path.Combine(Server.MapPath("~/PDFs/ChunksPhrasesParagraphs.pdf"));

            Rectangle r = new Rectangle(400, 300);

            Document doc = new Document(r);
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(chunksPhrasesParagraphsPath, FileMode.Create));
                doc.Open();

                string text = @"The result can be seen below, which shows the text
                  having been written to the document but it looks a
                  mess. Chunks have no concept of how to force a new
                   line when the length exceeds the available width in
                  the document. Really, all they should be used for is
                  to change or set the style of a word or phrase inline. ";

                text = text.Replace(Environment.NewLine, String.Empty).Replace("  ", String.Empty);
                Font georgia = FontFactory.GetFont("georgia", 10f);


                Chunk beginning = new Chunk(text, georgia);

                Phrase p1 = new Phrase(beginning);
                Chunk c1 = new Chunk("You can of course force a newline using \"", georgia);
                Chunk c2 = new Chunk(@"\n", georgia);
                Chunk c3 = new Chunk("\" or ", georgia);
                Chunk c4 = new Chunk("Environment", georgia);
                Chunk c5 = new Chunk(".NewLine", georgia);
                Chunk c6 = new Chunk(", or even ", georgia);
                Chunk c7 = new Chunk("Chunk", georgia);
                Chunk c8 = new Chunk(".NEWLINE", georgia);
                Chunk c9 = new Chunk(" as part of the string you give a chunk.", georgia);

                Phrase p2 = new Phrase();
                p2.Add(c1);
                p2.Add(c2);
                p2.Add(c3);
                p2.Add(c4);
                p2.Add(c5);
                p2.Add(c6);
                p2.Add(c7);
                p2.Add(c8);
                p2.Add(c9);

                Paragraph p = new Paragraph();
                p.Add(p1);
                p.Add(p2);
                doc.Add(p);
            }
            catch (DocumentException dex)
            {
                throw (dex);
            }
            catch (IOException ioex)
            {
                throw (ioex);
            }
            finally
            {
                doc.Close();
            }
            TempData["url"] = "/PDFs/ChunksPhrasesParagraphs.pdf";
            return RedirectToAction("Index");
        }

        public ActionResult ListWithiTextSharp()
        {
            var path = Path.Combine(Server.MapPath("~/PDFs/Lists.pdf"));
            iTextSharp.text.ListItem li = new iTextSharp.text.ListItem();
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));

                doc.Open();
                iTextSharp.text.List list = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED, 10f);
                list.Add(new iTextSharp.text.ListItem("One"));
                list.Add("Two");
                list.Add("Three");
                list.Add("Four");
                list.Add("Five");
                iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph();
                string text = "Lists";
                paragraph.Add(text);
                doc.Add(paragraph);
                doc.Add(list);


                RomanList romanlist = new RomanList(true, 20);
                romanlist.IndentationLeft = 30f;
                romanlist.Add("One");
                romanlist.Add("Two");
                romanlist.Add("Three");
                romanlist.Add("Four");
                romanlist.Add("Five");
                doc.Add(romanlist);



                ZapfDingbatsList zlist = new iTextSharp.text.ZapfDingbatsList(49, 15);
                zlist.Add("One");
                zlist.Add("Two");
                zlist.Add("Three");
                zlist.Add("Four");
                zlist.Add("Five");
                doc.Add(zlist);


                RomanList romanlist2 = new RomanList(true, 20);
                romanlist2.IndentationLeft = 10f;
                romanlist2.Add("One");
                romanlist2.Add("Two");
                romanlist2.Add("Three");
                romanlist2.Add("Four");
                romanlist2.Add("Five");



                List list2 = new List(List.ORDERED, 20f);

                list2.SetListSymbol("\u2022");
                list2.IndentationLeft = 20f;
                list2.Add("One");
                list2.Add("Two");
                list2.Add("Three");
                list2.Add("Roman List");
                list2.Add(romanlist);
                list2.Add("Four");
                list2.Add("Five");
                doc.Add(paragraph);
                doc.Add(list2);



            }
            catch (iTextSharp.text.DocumentException dex)
            {
                Response.Write(dex.Message);
            }
            catch (IOException ioex)
            {
                Response.Write(ioex.Message);
            }
            finally
            {
                doc.Close();
            }
            TempData["url"] = "/PDFs/Lists.pdf";
            return RedirectToAction("Index");

        }

        public ActionResult Links()
        {
            var path = Path.Combine(Server.MapPath("~/PDFs/Links.pdf"));

            Document doc = new Document();
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                doc.Open();
                Font link = FontFactory.GetFont("Arial", 12, Font.UNDERLINE);
                Anchor anchor = new Anchor("www.google.com", link);
                anchor.Reference = "https://www.google.com";
                doc.Add(anchor);


                Anchor click = new Anchor("Click to go to Target");
                click.Reference = "#target";
                Paragraph p1 = new Paragraph();
                p1.Add(click);
                doc.Add(p1);

                Paragraph p4 = new Paragraph();
                p4.Add(new Chunk("Click "));
                p4.Add(new Chunk("here", link).SetLocalGoto("GOTO"));
                p4.Add(new Chunk(" to find local goto"));
                p4.Add(new Chunk("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"));
                Paragraph p5 = new Paragraph();
                p5.Add(new Chunk("Local Goto Destination").SetLocalDestination("GOTO"));


                doc.Add(p4);

                doc.Add(p5);
                Paragraph p2 = new Paragraph();
                p2.Add(new Chunk("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"));
                doc.Add(p2);

                Anchor target = new Anchor("This is the Target");
                target.Name = "target";
                Paragraph p3 = new Paragraph();
                p3.Add(target);
                doc.Add(p3);

            }
            catch (DocumentException dex)
            {
                Response.Write(dex.Message);
            }
            catch (IOException ioex)
            {
                Response.Write(ioex.Message);
            }
            finally
            {
                doc.Close();
            }
            TempData["url"] = "/PDFs/Links.pdf";
            return RedirectToAction("Index");
        }

        public ActionResult Table1()
        {
            var path = Path.Combine(Server.MapPath("~/PDFs/Table1.pdf"));

            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();
            PdfPTable table = new PdfPTable(3);
            PdfPCell cell = new PdfPCell(new Phrase("Header spanning 3 columns"));
            cell.Colspan = 3;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);
            table.AddCell("Col 1 Row 1");
            table.AddCell("Col 2 Row 1");
            table.AddCell("Col 3 Row 1");
            table.AddCell("Col 1 Row 2");
            table.AddCell("Col 2 Row 2");
            table.AddCell("Col 3 Row 2");
            doc.Add(table);
            doc.Close();
            TempData["url"] = "/PDFs/Table1.pdf";
            return RedirectToAction("Index");
        }

        public ActionResult Table2()
        {
            var path = Path.Combine(Server.MapPath("~/PDFs/Table2.pdf"));

            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();
            PdfPTable table = new PdfPTable(2);
            //actual width of table in points
            table.TotalWidth = 216f;
            //fix the absolute width of the table
            table.LockedWidth = true;

            //relative col widths in proportions - 1/3 and 2/3
            //float[] widths = new float[] { 1f, 2f };
            float[] widths = new float[] { 100f, 116f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 0;
            //leave a gap before and after the table
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            PdfPCell cell = new PdfPCell(new Phrase("Products"));
            cell.Colspan = 2;
            cell.Border = 0;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            for (int i = 1; i <= 15; i++)
            {
                table.AddCell(i.ToString());
                table.AddCell($"Ürün_{i}");
            }
            


            doc.Add(table);
            doc.Close();
            TempData["url"] = "/PDFs/Table2.pdf";
            return RedirectToAction("Index");

        }

        public ActionResult Table3()
        {
            var path = Path.Combine(Server.MapPath("~/PDFs/Table3.pdf"));

            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = 400f;
            table.LockedWidth = true;
            PdfPCell header = new PdfPCell(new Phrase("Header"));
            header.Colspan = 4;
            table.AddCell(header);
            table.AddCell("Cell 1");
            table.AddCell("Cell 2");
            table.AddCell("Cell 3");
            table.AddCell("Cell 4");
            PdfPTable nested = new PdfPTable(1);
            nested.AddCell("Nested Row 1");
            nested.AddCell("Nested Row 2");
            nested.AddCell("Nested Row 3");
            PdfPCell nesthousing = new PdfPCell(nested);
            nesthousing.Padding = 0f;
            table.AddCell(nesthousing);
            PdfPCell bottom = new PdfPCell(new Phrase("bottom"));
            bottom.Colspan = 3;
            table.AddCell(bottom);
            doc.Add(table);
            doc.Close();
            TempData["url"] = "/PDFs/Table2.pdf";
            return RedirectToAction("Index");
        }


        public ActionResult Table4()
        {
            var path = Path.Combine(Server.MapPath("~/PDFs/Table4.pdf"));

            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = 144f;
            table.LockedWidth = true;
            table.HorizontalAlignment = 0;
            PdfPCell left = new PdfPCell(new Paragraph("Rotated"));
            left.Rotation = 90;
            table.AddCell(left);
            PdfPCell middle = new PdfPCell(new Paragraph("Rotated"));
            middle.Rotation = -90;
            table.AddCell(middle);
            table.AddCell("Not Rotated");
            doc.Add(table);
            doc.Close();

            TempData["url"] = "/PDFs/Table3.pdf";
            return RedirectToAction("Index");
        }
    }
}