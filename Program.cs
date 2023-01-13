
using System;
using System.IO;

using System.Diagnostics;
using System.Drawing;
using Bytescout.PDF;

using Brush = Bytescout.PDF.Brush;
using Font = Bytescout.PDF.Font;
using Pen = Bytescout.PDF.Pen;
using SolidBrush = Bytescout.PDF.SolidBrush;
using StringFormat = Bytescout.PDF.StringFormat;
using TagParser;

namespace DrawString
{
    /// <summary>
    /// This example demonstrates how to draw a text.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Create new document
            Document pdfDocument = new Document();
            pdfDocument.RegistrationName = "demo";
            pdfDocument.RegistrationKey = "demo";

            // Add page
            Page page = new Page(PaperFormat.A4);
            pdfDocument.Pages.Add(page);

            Font font = null;
            Brush brush = null;

            int left, top;
            left = top = 20;


            // Parse html tags
            TagParser.TagParser parser = new TagParser.TagParser();
            parser.ParseText("<!DOCTYPE html>\r\n\r\n<html>\r\n\r\n<body>\r\n\r\n<h1>Heading 1</h1>\r\n\r\n<h2>Heading 2</h2>\r\n\r\n<h3>Heading 3</h3>\r\n\r\n<h4>Heading 4</h4>\r\n\r\n<h5>Heading 5</h5>\r\n\r\n<h6>Heading 6</h6>\r\n\r\n<a href=\"https://www.w3schools.com\">Visit W3Schools</a>\r\n\r\n<p><b>This text is bold</b></p>\r\n\r\n<p><i>This text is italic</i></p>\r\n\r\n<p><strong>This text is important!</strong></p>\r\n\r\n<p><em>This text is emphasized</em></p>\r\n\r\n<p><small>This is some smaller text.</small></p>\r\n\r\n</body>\r\n\r\n</html>");
            Console.WriteLine(parser.plainText);

            for (int i = 0; i < parser.tags.Count; i++)
            {
                TagObject tag = parser.tags[i];
                Console.WriteLine(tag.Name); // print the name of the tag
                string tagText = parser.plainText.Substring(tag.startIndex, tag.endIndex - tag.startIndex+1);
                Console.WriteLine(tagText);
                for (int j = 0; j < tag.Properties.Count; j++)
                {
                    Console.WriteLine(tag.Properties[j].key); // would print "amount"
                    Console.WriteLine(tag.Properties[j].value); // would print "3"
                }

                switch (tag.Name)
                {
                     case "h0":
                        font = new Font("Arial", 40.0f);
                        brush = new SolidBrush();
                        break;
                    case "h1":
                        font = new Font("Arial", 32.0f);
                        brush = new SolidBrush();
                        break;
                    case "h2":
                        font = new Font("Arial", 24.0f);
                        brush = new SolidBrush();
                        break;
                    case "h3":
                        font = new Font("Arial", 18.72f);
                        brush = new SolidBrush();
                        break;
                    case "h4":
                        font = new Font("Arial", 16.0f);
                        brush = new SolidBrush();
                        break;
                    case "h5":
                        font = new Font("Arial", 13.28f);
                        brush = new SolidBrush();
                        break;
                    case "h6":
                        font = new Font("Arial", 10.72f);
                        brush = new SolidBrush();
                        break;
                    case "a":
                        for (int j = 0; j < tag.Properties.Count; j++)
                        {
                            if (tag.Properties[j].key == "href")
                            {
                                string uriStr = tag.Properties[j].value.Substring(1, tag.Properties[j].value.Length - 2);
                                URIAction action = new URIAction(new Uri(uriStr));
                                LinkAnnotation linkAnnotation = new LinkAnnotation(action, left, top, tagText.Length*6+5, 15);
                                linkAnnotation.HighlightingMode = LinkAnnotationHighlightingMode.None;
                                linkAnnotation.Color = new ColorRGB(255, 255, 255);
                                page.Annotations.Add(linkAnnotation);
                            }
                        }
                        font = new Font(StandardFonts.Times, 13.28f, true, false);
                        brush = new SolidBrush(new ColorRGB(0, 0, 255));
                        break;
                    case "b":
                        font = new Font("Arial", 13.28f, true, false, false, false);
                        brush = new SolidBrush(new ColorRGB(0, 0, 0));
                        break;
                    case "i":
                        font = new Font("Arial", 13.28f, false, true, false, false);
                        brush = new SolidBrush(new ColorRGB(0, 0, 0));
                        break;
                    case "strong":
                        font = new Font("Impact", 13.28f, false, false, false, false);
                        brush = new SolidBrush(new ColorRGB(0, 0, 0));
                        break;
                    case "em":
                        font = new Font("Verdana", 13.28f, false, false, false, false);
                        brush = new SolidBrush(new ColorRGB(0, 0, 0));
                        break;
                    case "small":
                        font = new Font("Arial", 9.0f, false, false, false, false);
                        brush = new SolidBrush(new ColorRGB(0, 0, 0));
                        break;
                    default:
                        font = null;
                        brush = null;
                        break;
                }

                if ((font != null) && (brush != null))
                {
                    page.Canvas.DrawString(tagText, font, brush, left, top);
                    top += 40;
                }
            }

            // Save document to file
            pdfDocument.Save("result.pdf");

            return;
        }
    }
}
