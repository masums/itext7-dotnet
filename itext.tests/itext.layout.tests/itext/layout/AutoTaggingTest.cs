/*
This file is part of the iText (R) project.
Copyright (c) 1998-2017 iText Group NV
Authors: iText Software.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using System.Text;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Utils;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Layout {
    public class AutoTaggingTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/layout/AutoTaggingTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/layout/AutoTaggingTest/";

        public const String imageName = "Desert.jpg";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void TextInParagraphTest01() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "textInParagraphTest01.pdf"));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            Paragraph p = CreateParagraph1();
            document.Add(p);
            for (int i = 0; i < 26; ++i) {
                document.Add(CreateParagraph2());
            }
            document.Close();
            CompareResult("textInParagraphTest01.pdf", "cmp_textInParagraphTest01.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.ELEMENT_DOES_NOT_FIT_AREA)]
        public virtual void ImageTest01() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "imageTest01.pdf"));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            iText.Layout.Element.Image image = new Image(ImageDataFactory.Create(sourceFolder + imageName));
            document.Add(image);
            document.Close();
            CompareResult("imageTest01.pdf", "cmp_imageTest01.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void ImageTest02() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "imageTest02.pdf"));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            Div div = new Div();
            div.Add(new Paragraph("text before"));
            iText.Layout.Element.Image image = new iText.Layout.Element.Image(ImageDataFactory.Create(sourceFolder + imageName
                )).SetWidth(200);
            PdfDictionary imgAttributes = new PdfDictionary();
            imgAttributes.Put(PdfName.O, PdfName.Layout);
            imgAttributes.Put(PdfName.Placement, PdfName.Block);
            image.GetAccessibilityProperties().AddAttributes(imgAttributes);
            div.Add(image);
            div.Add(new Paragraph("text after"));
            document.Add(div);
            document.Close();
            CompareResult("imageTest02.pdf", "cmp_imageTest02.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void DivTest01() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "divTest01.pdf"));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            Div div = new Div();
            div.Add(CreateParagraph1());
            iText.Layout.Element.Image image = new iText.Layout.Element.Image(ImageDataFactory.Create(sourceFolder + imageName
                ));
            image.SetAutoScale(true);
            div.Add(image);
            div.Add(CreateParagraph2());
            div.Add(image);
            div.Add(CreateParagraph2());
            document.Add(div);
            document.Close();
            CompareResult("divTest01.pdf", "cmp_divTest01.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void TableTest01() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "tableTest01.pdf"));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            Table table = new Table(3).SetWidth(UnitValue.CreatePercentValue(100)).SetFixedLayout();
            iText.Layout.Element.Image image = new iText.Layout.Element.Image(ImageDataFactory.Create(sourceFolder + imageName
                )).SetWidth(100).SetAutoScale(true);
            table.AddCell(CreateParagraph1());
            table.AddCell(image);
            table.AddCell(CreateParagraph2());
            table.AddCell(image);
            table.AddCell(new Paragraph("abcdefghijklmnopqrstuvwxyz").SetFontColor(Color.GREEN));
            table.AddCell("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                 + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                 + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                 + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                );
            document.Add(table);
            document.Close();
            CompareResult("tableTest01.pdf", "cmp_tableTest01.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void TableTest02() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "tableTest02.pdf"));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            Table table = new Table(3).SetWidth(UnitValue.CreatePercentValue(100)).SetFixedLayout();
            for (int i = 0; i < 5; ++i) {
                table.AddCell(CreateParagraph2());
            }
            table.AddCell("little text");
            document.Add(table);
            document.Close();
            CompareResult("tableTest02.pdf", "cmp_tableTest02.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void TableTest03() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "tableTest03.pdf"));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            Table table = new Table(3).SetWidth(UnitValue.CreatePercentValue(100)).SetFixedLayout();
            Cell cell = new Cell(1, 3).Add(new Paragraph("full-width header"));
            cell.SetRole(PdfName.TH);
            table.AddHeaderCell(cell);
            for (int i = 0; i < 3; ++i) {
                cell = new Cell().Add(new Paragraph("header " + i));
                cell.SetRole(PdfName.TH);
                table.AddHeaderCell(cell);
            }
            for (int i = 0; i < 3; ++i) {
                table.AddFooterCell("footer " + i);
            }
            cell = new Cell(1, 3).Add(new Paragraph("full-width paragraph"));
            table.AddCell(cell);
            for (int i = 0; i < 5; ++i) {
                table.AddCell(CreateParagraph2());
            }
            table.AddCell(new Paragraph("little text"));
            document.Add(table);
            document.Close();
            CompareResult("tableTest03.pdf", "cmp_tableTest03.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void TableTest04() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "tableTest04.pdf"));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            Table table = new Table(5, true);
            doc.Add(table);
            for (int i = 0; i < 20; i++) {
                for (int j = 0; j < 4; j++) {
                    table.AddCell(new Cell().Add(new Paragraph(String.Format("Cell {0}, {1}", i + 1, j + 1))));
                }
                if (i % 10 == 0) {
                    table.Flush();
                    // This is a deliberate additional flush.
                    table.Flush();
                }
            }
            table.Complete();
            doc.Add(new Table(1).SetBorder(new SolidBorder(Color.ORANGE, 2)).AddCell("Is my occupied area correct?"));
            doc.Close();
            CompareResult("tableTest04.pdf", "cmp_tableTest04.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void TableTest05() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "tableTest05.pdf"));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            Table table = new Table(5, true);
            doc.Add(table);
            Cell cell = new Cell(1, 5).Add(new Paragraph("Table XYZ (Continued)"));
            table.AddHeaderCell(cell);
            for (int i = 0; i < 5; ++i) {
                table.AddHeaderCell(new Cell().Add("Header " + (i + 1)));
            }
            cell = new Cell(1, 5).Add(new Paragraph("Continue on next page"));
            table.AddFooterCell(cell);
            table.SetSkipFirstHeader(true);
            table.SetSkipLastFooter(true);
            for (int i = 0; i < 350; i++) {
                table.AddCell(new Cell().Add(new Paragraph((i + 1).ToString())));
                table.Flush();
            }
            table.Complete();
            doc.Add(new Table(1).SetBorder(new SolidBorder(Color.ORANGE, 2)).AddCell("Is my occupied area correct?"));
            doc.Close();
            CompareResult("tableTest05.pdf", "cmp_tableTest05.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void TableTest06() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "tableTest06.pdf"));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            String textContent = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.\n"
                 + "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.\n" + "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.\n";
            String shortTextContent = "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.";
            String middleTextContent = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.\n"
                 + "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.";
            Table table = new Table(new float[] { 130, 130, 260 }).AddCell(new Cell().Add(new Paragraph("cell 1, 1\n" 
                + shortTextContent))).AddCell(new Cell().Add(new Paragraph("cell 1, 2\n" + shortTextContent))).AddCell
                (new Cell().Add(new Paragraph("cell 1, 3\n" + middleTextContent))).AddCell(new Cell().Add(new Paragraph
                ("cell 2, 1\n" + shortTextContent))).AddCell(new Cell().Add(new Paragraph("cell 2, 2\n" + shortTextContent
                ))).AddCell(new Cell().Add(new Paragraph("cell 2, 3\n" + middleTextContent))).AddCell(new Cell(3, 2).Add
                (new Paragraph("cell 3:2, 1:3\n" + textContent + textContent))).AddCell(new Cell().Add(new Paragraph("cell 3, 3\n"
                 + textContent))).AddCell(new Cell().Add(new Paragraph("cell 4, 3\n" + textContent))).AddCell(new Cell
                ().Add(new Paragraph("cell 5, 3\n" + textContent))).AddCell(new Cell().Add(new Paragraph("cell 6, 1\n"
                 + shortTextContent))).AddCell(new Cell().Add(new Paragraph("cell 6, 2\n" + shortTextContent))).AddCell
                (new Cell().Add(new Paragraph("cell 6, 3\n" + middleTextContent))).AddCell(new Cell().Add(new Paragraph
                ("cell 7, 1\n" + middleTextContent))).AddCell(new Cell().Add(new Paragraph("cell 7, 2\n" + middleTextContent
                ))).AddCell(new Cell().Add(new Paragraph("cell 7, 3\n" + middleTextContent)));
            doc.Add(table);
            doc.Close();
            CompareResult("tableTest06.pdf", "cmp_tableTest06.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void TableTest07() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "tableTest07.pdf"));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            Table table = new Table(new float[] { 130, 130, 260 }).AddHeaderCell(new Cell().Add(new Paragraph("hcell 1, 1"
                ))).AddHeaderCell(new Cell().Add(new Paragraph("hcell 1, 2"))).AddHeaderCell(new Cell().Add(new Paragraph
                ("hcell 1, 3"))).AddCell(new Cell().Add(new Paragraph("cell 2, 1"))).AddCell(new Cell().Add(new Paragraph
                ("cell 2, 2"))).AddCell(new Cell().Add(new Paragraph("cell 2, 3"))).AddCell(new Cell().Add(new Paragraph
                ("cell 3, 1"))).AddCell(new Cell().Add(new Paragraph("cell 3, 2"))).AddCell(new Cell().Add(new Paragraph
                ("cell 3, 3"))).AddFooterCell(new Cell().Add(new Paragraph("fcell 4, 1"))).AddFooterCell(new Cell().Add
                (new Paragraph("fcell 4, 2"))).AddFooterCell(new Cell().Add(new Paragraph("fcell 4, 3")));
            doc.Add(table);
            doc.Close();
            CompareResult("tableTest07.pdf", "cmp_tableTest07.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void TableTest08() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "tableTest08.pdf"));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            Table table = new Table(new UnitValue[5], true);
            doc.Add(table);
            Cell cell = new Cell(1, 5).Add(new Paragraph("Table XYZ (Continued)"));
            table.AddHeaderCell(cell);
            for (int i = 0; i < 5; ++i) {
                table.AddHeaderCell(new Cell().Add("Header " + (i + 1)));
            }
            cell = new Cell(1, 5).Add(new Paragraph("Continue on next page"));
            table.AddFooterCell(cell);
            table.SetSkipFirstHeader(true);
            table.SetSkipLastFooter(true);
            for (int i = 0; i < 350; i++) {
                table.AddCell(new Cell().Add(new Paragraph((i + 1).ToString())));
                table.Flush();
            }
            table.Complete();
            doc.Add(new Table(1).SetBorder(new SolidBorder(Color.ORANGE, 2)).AddCell("Is my occupied area correct?"));
            doc.Close();
            CompareResult("tableTest08.pdf", "cmp_tableTest08.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void ListTest01() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "listTest01.pdf"));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            List list = new List(ListNumberingType.DECIMAL);
            list.Add("item 1");
            list.Add("item 2");
            list.Add("item 3");
            doc.Add(list);
            doc.Close();
            CompareResult("listTest01.pdf", "cmp_listTest01.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void ListTest02() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "listTest02.pdf"));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            doc.SetFont(PdfFontFactory.CreateFont(sourceFolder + "../fonts/NotoSans-Regular.ttf", PdfEncodings.IDENTITY_H
                ));
            PdfDictionary attributesDisc = new PdfDictionary();
            attributesDisc.Put(PdfName.O, PdfName.List);
            attributesDisc.Put(PdfName.ListNumbering, PdfName.Disc);
            PdfDictionary attributesSquare = new PdfDictionary();
            attributesSquare.Put(PdfName.O, PdfName.List);
            attributesSquare.Put(PdfName.ListNumbering, PdfName.Square);
            PdfDictionary attributesCircle = new PdfDictionary();
            attributesCircle.Put(PdfName.O, PdfName.List);
            attributesCircle.Put(PdfName.ListNumbering, PdfName.Circle);
            String discSymbol = "\u2022";
            String squareSymbol = "\u25AA";
            String circleSymbol = "\u25E6";
            List list = new List(ListNumberingType.ROMAN_UPPER);
            // setting numbering type for now
            list.Add("item 1");
            ListItem listItem = new ListItem("item 2");
 {
                List subList = new List().SetListSymbol(discSymbol).SetMarginLeft(30);
                subList.GetAccessibilityProperties().AddAttributes(attributesDisc);
                ListItem subListItem = new ListItem("sub item 1");
 {
                    List subSubList = new List().SetListSymbol(squareSymbol).SetMarginLeft(30);
                    subSubList.GetAccessibilityProperties().AddAttributes(attributesSquare);
                    subSubList.Add("sub sub item 1");
                    subSubList.Add("sub sub item 2");
                    subSubList.Add("sub sub item 3");
                    subListItem.Add(subSubList);
                }
                subList.Add(subListItem);
                subList.Add("sub item 2");
                subList.Add("sub item 3");
                listItem.Add(subList);
            }
            list.Add(listItem);
            list.Add("item 3");
            doc.Add(list);
            doc.Add(new LineSeparator(new SolidLine()));
            doc.Add(list.SetListSymbol(circleSymbol));
            // setting circle symbol, not setting attributes
            doc.Add(new LineSeparator(new SolidLine()));
            list.GetAccessibilityProperties().AddAttributes(attributesCircle);
            doc.Add(list);
            // circle symbol set, setting attributes
            doc.Close();
            CompareResult("listTest02.pdf", "cmp_listTest02.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void ListTest03() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "listTest03.pdf"));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            PdfDictionary attributesSquare = new PdfDictionary();
            attributesSquare.Put(PdfName.O, PdfName.List);
            attributesSquare.Put(PdfName.ListNumbering, PdfName.Square);
            List list = new List(ListNumberingType.DECIMAL);
            // explicitly overriding ListNumbering attribute
            list.GetAccessibilityProperties().AddAttributes(attributesSquare);
            list.Add("item 1");
            list.Add("item 2");
            list.Add("item 3");
            doc.Add(list);
            doc.Close();
            CompareResult("listTest03.pdf", "cmp_listTest03.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void LinkTest01() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "linkTest01.pdf"));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            PdfAction action = PdfAction.CreateURI("http://itextpdf.com/", false);
            Link link = new Link("linked text", action);
            link.SetUnderline();
            link.GetLinkAnnotation().Put(PdfName.Border, new PdfArray(new int[] { 0, 0, 0 }));
            doc.Add(new Paragraph("before ").Add(link).Add(" after"));
            doc.Close();
            CompareResult("linkTest01.pdf", "cmp_linkTest01.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void ArtifactTest01() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "artifactTest01.pdf"));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            String watermarkText = "WATERMARK";
            Paragraph watermark = new Paragraph(watermarkText);
            watermark.SetFontColor(new DeviceGray(0.75f)).SetFontSize(72);
            document.ShowTextAligned(watermark, PageSize.A4.GetWidth() / 2, PageSize.A4.GetHeight() / 2, 1, TextAlignment
                .CENTER, VerticalAlignment.MIDDLE, (float)(Math.PI / 4));
            String textContent = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.\n"
                 + "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.\n" + "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.\n";
            document.Add(new Paragraph(textContent + textContent + textContent));
            document.Add(new Paragraph(textContent + textContent + textContent));
            document.Close();
            CompareResult("artifactTest01.pdf", "cmp_artifactTest01.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void ArtifactTest02() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "artifactTest02.pdf"));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            document.Add(new Paragraph("Hello world"));
            Table table = new Table(5);
            for (int i = 0; i < 25; ++i) {
                table.AddCell(i.ToString());
            }
            table.SetRole(PdfName.Artifact);
            document.Add(table);
            document.Close();
            CompareResult("artifactTest02.pdf", "cmp_artifactTest02.pdf");
        }

        /// <summary>
        /// Document generation and result is the same in this test as in the textInParagraphTest01, except the partial flushing of
        /// tag structure.
        /// </summary>
        /// <remarks>
        /// Document generation and result is the same in this test as in the textInParagraphTest01, except the partial flushing of
        /// tag structure. So you can check the result by comparing resultant document with the one in textInParagraphTest01.
        /// </remarks>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void FlushingTest01() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "flushingTest01.pdf"));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            Paragraph p = CreateParagraph1();
            document.Add(p);
            int pageToFlush = 1;
            for (int i = 0; i < 26; ++i) {
                if (i % 6 == 5) {
                    pdfDocument.GetPage(pageToFlush++).Flush();
                }
                document.Add(CreateParagraph2());
            }
            document.Close();
            CompareResult("flushingTest01.pdf", "cmp_flushingTest01.pdf");
        }

        /// <summary>
        /// Document generation and result is the same in this test as in the tableTest05, except the partial flushing of
        /// tag structure.
        /// </summary>
        /// <remarks>
        /// Document generation and result is the same in this test as in the tableTest05, except the partial flushing of
        /// tag structure. So you can check the result by comparing resultant document with the one in tableTest05.
        /// </remarks>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void FlushingTest02() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "flushingTest02.pdf"));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            Table table = new Table(5, true);
            doc.Add(table);
            //        TODO solve header/footer problems with tagging. Currently, partial flushing when header/footer is used leads to crash.
            Cell cell = new Cell(1, 5).Add(new Paragraph("Table XYZ (Continued)"));
            table.AddHeaderCell(cell);
            for (int i = 0; i < 5; ++i) {
                table.AddHeaderCell(new Cell().Add("Header " + (i + 1)));
            }
            cell = new Cell(1, 5).Add(new Paragraph("Continue on next page"));
            table.AddFooterCell(cell);
            table.SetSkipFirstHeader(true);
            table.SetSkipLastFooter(true);
            for (int i = 0; i < 350; i++) {
                table.AddCell(new Cell().Add(new Paragraph((i + 1).ToString())));
                table.Flush();
            }
            table.Complete();
            doc.Add(new Table(1).SetBorder(new SolidBorder(Color.ORANGE, 2)).AddCell("Is my occupied area correct?"));
            doc.Close();
            CompareResult("flushingTest02.pdf", "cmp_flushingTest02.pdf");
        }

        /// <summary>
        /// Document generation and result is the same in this test as in the tableTest04, except the partial flushing of
        /// tag structure.
        /// </summary>
        /// <remarks>
        /// Document generation and result is the same in this test as in the tableTest04, except the partial flushing of
        /// tag structure. So you can check the result by comparing resultant document with the one in tableTest04.
        /// </remarks>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void FlushingTest03() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "flushingTest03.pdf"));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            Table table = new Table(5, true);
            doc.Add(table);
            for (int i = 0; i < 20; i++) {
                for (int j = 0; j < 4; j++) {
                    table.AddCell(new Cell().Add(new Paragraph(String.Format("Cell {0}, {1}", i + 1, j + 1))));
                }
                if (i % 10 == 0) {
                    table.Flush();
                    pdfDocument.GetTagStructureContext().FlushPageTags(pdfDocument.GetPage(1));
                    // This is a deliberate additional flush.
                    table.Flush();
                }
            }
            table.Complete();
            doc.Add(new Table(1).SetBorder(new SolidBorder(Color.ORANGE, 2)).AddCell("Is my occupied area correct?"));
            doc.Close();
            CompareResult("flushingTest03.pdf", "cmp_tableTest04.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void WordBreaksLineEndingsTest01() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "wordBreaksLineEndingsTest01.pdf"
                , new WriterProperties().SetCompressionLevel(CompressionConstants.NO_COMPRESSION)));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            String s = "Beaver was settled in 1856 by Mormon pioneers traveling this road.";
            StringBuilder text = new StringBuilder();
            for (int i = 0; i < 10; ++i) {
                text.Append(s);
                text.Append(" ");
            }
            Paragraph p = new Paragraph(text.ToString().Trim());
            doc.Add(p);
            doc.Close();
            CompareResult("wordBreaksLineEndingsTest01.pdf", "cmp_wordBreaksLineEndingsTest01.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void WordBreaksLineEndingsTest02() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "wordBreaksLineEndingsTest02.pdf"
                , new WriterProperties().SetCompressionLevel(CompressionConstants.NO_COMPRESSION)));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            String s = "Beaver was settled in 1856 by Mormon pioneers traveling this road.";
            Paragraph p = new Paragraph(s + " Beaver was settled in 1856 by").Add(" Mormon pioneers traveling this road."
                );
            doc.Add(p);
            doc.Close();
            CompareResult("wordBreaksLineEndingsTest02.pdf", "cmp_wordBreaksLineEndingsTest02.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void WordBreaksLineEndingsTest03() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "wordBreaksLineEndingsTest03.pdf"
                , new WriterProperties().SetCompressionLevel(CompressionConstants.NO_COMPRESSION)));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            String s = "Beaver was settled in 1856 by\nMormon pioneers traveling this road.";
            Paragraph p = new Paragraph(s);
            doc.Add(p);
            String s1 = "Beaver was settled in 1856 by \n Mormon pioneers traveling this road.";
            Paragraph p1 = new Paragraph(s1);
            doc.Add(p1);
            String s2 = "\nBeaver was settled in 1856 by Mormon pioneers traveling this road.";
            Paragraph p2 = new Paragraph(s2);
            doc.Add(p2);
            String s3_1 = "Beaver was settled in 1856 by";
            String s3_2 = "\nMormon pioneers traveling this road.";
            Paragraph p3 = new Paragraph(s3_1).Add(s3_2);
            doc.Add(p3);
            doc.Close();
            CompareResult("wordBreaksLineEndingsTest03.pdf", "cmp_wordBreaksLineEndingsTest03.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void WordBreaksLineEndingsTest04() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "wordBreaksLineEndingsTest04.pdf"
                , new WriterProperties().SetCompressionLevel(CompressionConstants.NO_COMPRESSION)));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            String s = "ShortWord Beaverwassettledin1856byMormonpioneerstravelingthisroadBeaverwassettledin1856byMormonpioneerstravelingthisroad.";
            Paragraph p = new Paragraph(s);
            doc.Add(p);
            String s1 = "ShortWord " + "                                                                                          "
                 + "                                                                                          " + "and another short word.";
            Paragraph p1 = new Paragraph(s1);
            doc.Add(p1);
            doc.Close();
            CompareResult("wordBreaksLineEndingsTest04.pdf", "cmp_wordBreaksLineEndingsTest04.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void WordBreaksLineEndingsTest05() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "wordBreaksLineEndingsTest05.pdf"
                , new WriterProperties().SetCompressionLevel(CompressionConstants.NO_COMPRESSION)));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            String s = "t\n";
            Paragraph p = new Paragraph(s).Add("\n").Add(s);
            doc.Add(p);
            Paragraph p1 = new Paragraph(s);
            doc.Add(p1);
            Paragraph p2 = new Paragraph(s).Add("another t");
            doc.Add(p2);
            doc.Close();
            CompareResult("wordBreaksLineEndingsTest05.pdf", "cmp_wordBreaksLineEndingsTest05.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ImageAndTextNoRole01() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "imageAndTextNoRole01.pdf", new 
                WriterProperties().SetCompressionLevel(CompressionConstants.NO_COMPRESSION)));
            pdfDocument.SetTagged();
            Document doc = new Document(pdfDocument);
            doc.Add(new Paragraph("Set Image role to null and add to div with role \"Figure\""));
            iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(sourceFolder + imageName
                )).SetWidth(200);
            img.SetRole(null);
            Div div = new Div();
            div.SetRole(PdfName.Figure);
            div.Add(img);
            Paragraph caption = new Paragraph("Caption");
            caption.SetRole(PdfName.Caption);
            div.Add(caption);
            doc.Add(div);
            doc.Add(new Paragraph("Set Text role to null and add to Paragraph").SetMarginTop(20));
            div = new Div();
            div.SetRole(PdfName.Code);
            iText.Layout.Element.Text txt = new iText.Layout.Element.Text("// Prints Hello world!");
            txt.SetRole(null);
            div.Add(new Paragraph(txt).SetMarginBottom(0));
            txt = new iText.Layout.Element.Text("System.out.println(\"Hello world!\");");
            txt.SetRole(null);
            div.Add(new Paragraph(txt).SetMarginTop(0));
            doc.Add(div);
            doc.Close();
            CompareResult("imageAndTextNoRole01.pdf", "cmp_imageAndTextNoRole01.pdf");
        }

        /// <exception cref="System.IO.IOException"/>
        private Paragraph CreateParagraph1() {
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);
            Paragraph p = new Paragraph().Add("text chunk. ").Add("explicitly added separate text chunk");
            iText.Layout.Element.Text id = new iText.Layout.Element.Text("text chunk with specific font").SetFont(font
                ).SetFontSize(8).SetTextRise(6);
            p.Add(id);
            return p;
        }

        private Paragraph CreateParagraph2() {
            Paragraph p;
            String alphabet = "abcdefghijklmnopqrstuvwxyz";
            StringBuilder longTextBuilder = new StringBuilder();
            for (int i = 0; i < 26; ++i) {
                longTextBuilder.Append(alphabet);
            }
            String longText = longTextBuilder.ToString();
            p = new Paragraph(longText);
            return p;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        private void CompareResult(String outFileName, String cmpFileName) {
            CompareTool compareTool = new CompareTool();
            String outPdf = destinationFolder + outFileName;
            String cmpPdf = sourceFolder + cmpFileName;
            String contentDifferences = compareTool.CompareByContent(outPdf, cmpPdf, destinationFolder, "diff");
            String taggedStructureDifferences = compareTool.CompareTagStructures(outPdf, cmpPdf);
            String errorMessage = "";
            errorMessage += taggedStructureDifferences == null ? "" : taggedStructureDifferences + "\n";
            errorMessage += contentDifferences == null ? "" : contentDifferences;
            if (!String.IsNullOrEmpty(errorMessage)) {
                NUnit.Framework.Assert.Fail(errorMessage);
            }
        }
    }
}
