// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Renderer.OoXmlV3.TTF;
using DocumentFormat.OpenXml;
using System.Windows;
using A = DocumentFormat.OpenXml.Drawing;
using Point = System.Windows.Point;
using Wpg = DocumentFormat.OpenXml.Office2010.Word.DrawingGroup;
using Wps = DocumentFormat.OpenXml.Office2010.Word.DrawingShape;

namespace Chem4Word.Renderer.OoXmlV3.OOXML.Atoms
{
    public class AtomLabelRenderer
    {
        private Rect m_canvasExtents;
        private long m_ooxmlId;
        private Options m_options;

        public AtomLabelRenderer(Rect canvasExtents, ref long ooxmlId, Options opts)
        {
            m_canvasExtents = canvasExtents;
            m_ooxmlId = ooxmlId;
            m_options = opts;
        }

        public void DrawCharacter(Wpg.WordprocessingGroup wordprocessingGroup1, AtomLabelCharacter alc)
        {
            Point characterPosition = new Point(alc.Position.X, alc.Position.Y);
            characterPosition.Offset(-m_canvasExtents.Left, -m_canvasExtents.Top);

            UInt32Value atomLabelId = UInt32Value.FromUInt32((uint)m_ooxmlId++);
            string atomLabelName = "AtomLabel" + atomLabelId;

            Int64Value width = OoXmlHelper.ScaleCsTtfToEmu(alc.Character.Width);
            Int64Value height = OoXmlHelper.ScaleCsTtfToEmu(alc.Character.Height);
            if (alc.IsSubScript)
            {
                width = OoXmlHelper.ScaleCsTtfSubScriptToEmu(alc.Character.Width);
                height = OoXmlHelper.ScaleCsTtfSubScriptToEmu(alc.Character.Height);
            }
            Int64Value top = OoXmlHelper.ScaleCmlToEmu(characterPosition.Y);
            Int64Value left = OoXmlHelper.ScaleCmlToEmu(characterPosition.X);

            // Set variable true to show bounding box of (every) character
            if (m_options.ShowCharacterBoundingBoxes)
            {
                Rect boundingBox = new Rect(new Point(left, top), new Size(width, height));
                DrawCharacterBox(wordprocessingGroup1, boundingBox, "00ff00", 10);
            }

            Wps.WordprocessingShape wordprocessingShape10 = new Wps.WordprocessingShape();
            Wps.NonVisualDrawingProperties nonVisualDrawingProperties10 = new Wps.NonVisualDrawingProperties() { Id = atomLabelId, Name = atomLabelName };
            Wps.NonVisualDrawingShapeProperties nonVisualDrawingShapeProperties10 = new Wps.NonVisualDrawingShapeProperties();

            Wps.ShapeProperties shapeProperties10 = new Wps.ShapeProperties();

            A.Transform2D transform2D10 = new A.Transform2D();
            A.Offset offset11 = new A.Offset() { X = left, Y = top };
            A.Extents extents11 = new A.Extents() { Cx = width, Cy = height };

            transform2D10.Append(offset11);
            transform2D10.Append(extents11);

            A.CustomGeometry customGeometry10 = new A.CustomGeometry();
            A.AdjustValueList adjustValueList10 = new A.AdjustValueList();
            A.Rectangle rectangle10 = new A.Rectangle() { Left = "l", Top = "t", Right = "r", Bottom = "b" };

            A.PathList pathList10 = new A.PathList();

            A.Path path10 = new A.Path() { Width = width, Height = height };

            foreach (TtfContour contour in alc.Character.Contours)
            {
                int i = 0;
                while (i < contour.Points.Count)
                {
                    TtfPoint thisPoint = contour.Points[i];
                    TtfPoint nextPoint = null;
                    if (i < contour.Points.Count - 1)
                    {
                        nextPoint = contour.Points[i + 1];
                    }

                    switch (thisPoint.Type)
                    {
                        case TtfPoint.PointType.Start:
                            A.MoveTo moveTo1 = new A.MoveTo();
                            if (alc.IsSubScript)
                            {
                                A.Point point1 = new A.Point()
                                {
                                    X = OoXmlHelper.ScaleCsTtfSubScriptToEmu(thisPoint.X - alc.Character.OriginX).ToString(),
                                    Y = OoXmlHelper.ScaleCsTtfSubScriptToEmu(alc.Character.Height + thisPoint.Y - (alc.Character.Height + alc.Character.OriginY)).ToString()
                                };
                                moveTo1.Append(point1);
                                path10.Append(moveTo1);
                            }
                            else
                            {
                                A.Point point1 = new A.Point()
                                {
                                    X = OoXmlHelper.ScaleCsTtfToEmu(thisPoint.X - alc.Character.OriginX).ToString(),
                                    Y = OoXmlHelper.ScaleCsTtfToEmu(alc.Character.Height + thisPoint.Y - (alc.Character.Height + alc.Character.OriginY)).ToString()
                                };
                                moveTo1.Append(point1);
                                path10.Append(moveTo1);
                            }
                            i++;
                            break;

                        case TtfPoint.PointType.Line:
                            A.LineTo lineTo1 = new A.LineTo();
                            if (alc.IsSubScript)
                            {
                                A.Point point2 = new A.Point()
                                {
                                    X = OoXmlHelper.ScaleCsTtfSubScriptToEmu(thisPoint.X - alc.Character.OriginX).ToString(),
                                    Y = OoXmlHelper.ScaleCsTtfSubScriptToEmu(alc.Character.Height + thisPoint.Y - (alc.Character.Height + alc.Character.OriginY)).ToString()
                                };
                                lineTo1.Append(point2);
                                path10.Append(lineTo1);
                            }
                            else
                            {
                                A.Point point2 = new A.Point()
                                {
                                    X = OoXmlHelper.ScaleCsTtfToEmu(thisPoint.X - alc.Character.OriginX).ToString(),
                                    Y = OoXmlHelper.ScaleCsTtfToEmu(alc.Character.Height + thisPoint.Y - (alc.Character.Height + alc.Character.OriginY)).ToString()
                                };
                                lineTo1.Append(point2);
                                path10.Append(lineTo1);
                            }
                            i++;
                            break;

                        case TtfPoint.PointType.CurveOff:
                            A.QuadraticBezierCurveTo quadraticBezierCurveTo13 = new A.QuadraticBezierCurveTo();
                            if (alc.IsSubScript)
                            {
                                A.Point point3 = new A.Point()
                                {
                                    X = OoXmlHelper.ScaleCsTtfSubScriptToEmu(thisPoint.X - alc.Character.OriginX).ToString(),
                                    Y = OoXmlHelper.ScaleCsTtfSubScriptToEmu(alc.Character.Height + thisPoint.Y - (alc.Character.Height + alc.Character.OriginY)).ToString()
                                };
                                A.Point point4 = new A.Point()
                                {
                                    X = OoXmlHelper.ScaleCsTtfSubScriptToEmu(nextPoint.X - alc.Character.OriginX).ToString(),
                                    Y = OoXmlHelper.ScaleCsTtfSubScriptToEmu(alc.Character.Height + nextPoint.Y - (alc.Character.Height + alc.Character.OriginY)).ToString()
                                };
                                quadraticBezierCurveTo13.Append(point3);
                                quadraticBezierCurveTo13.Append(point4);
                                path10.Append(quadraticBezierCurveTo13);
                            }
                            else
                            {
                                A.Point point3 = new A.Point()
                                {
                                    X = OoXmlHelper.ScaleCsTtfToEmu(thisPoint.X - alc.Character.OriginX).ToString(),
                                    Y = OoXmlHelper.ScaleCsTtfToEmu(alc.Character.Height + thisPoint.Y - (alc.Character.Height + alc.Character.OriginY)).ToString()
                                };
                                A.Point point4 = new A.Point()
                                {
                                    X = OoXmlHelper.ScaleCsTtfToEmu(nextPoint.X - alc.Character.OriginX).ToString(),
                                    Y = OoXmlHelper.ScaleCsTtfToEmu(alc.Character.Height + nextPoint.Y - (alc.Character.Height + alc.Character.OriginY)).ToString()
                                };
                                quadraticBezierCurveTo13.Append(point3);
                                quadraticBezierCurveTo13.Append(point4);
                                path10.Append(quadraticBezierCurveTo13);
                            }
                            i++;
                            i++;
                            break;

                        case TtfPoint.PointType.CurveOn:
                            // Should never get here !
                            i++;
                            break;
                    }
                }

                A.CloseShapePath closeShapePath1 = new A.CloseShapePath();
                path10.Append(closeShapePath1);
            }

            pathList10.Append(path10);

            customGeometry10.Append(adjustValueList10);
            customGeometry10.Append(rectangle10);
            customGeometry10.Append(pathList10);

            A.SolidFill solidFill10 = new A.SolidFill();

            // Set Colour
            A.RgbColorModelHex rgbColorModelHex10 = new A.RgbColorModelHex() { Val = alc.Colour };
            A.Alpha alpha10 = new A.Alpha() { Val = new Int32Value() { InnerText = "100%" } };

            rgbColorModelHex10.Append(alpha10);

            solidFill10.Append(rgbColorModelHex10);

            shapeProperties10.Append(transform2D10);
            shapeProperties10.Append(customGeometry10);
            shapeProperties10.Append(solidFill10);

            Wps.ShapeStyle shapeStyle10 = new Wps.ShapeStyle();
            A.LineReference lineReference10 = new A.LineReference() { Index = (UInt32Value)0U };
            A.FillReference fillReference10 = new A.FillReference() { Index = (UInt32Value)0U };
            A.EffectReference effectReference10 = new A.EffectReference() { Index = (UInt32Value)0U };
            A.FontReference fontReference10 = new A.FontReference() { Index = A.FontCollectionIndexValues.Minor };

            shapeStyle10.Append(lineReference10);
            shapeStyle10.Append(fillReference10);
            shapeStyle10.Append(effectReference10);
            shapeStyle10.Append(fontReference10);
            Wps.TextBodyProperties textBodyProperties10 = new Wps.TextBodyProperties();

            wordprocessingShape10.Append(nonVisualDrawingProperties10);
            wordprocessingShape10.Append(nonVisualDrawingShapeProperties10);
            wordprocessingShape10.Append(shapeProperties10);
            wordprocessingShape10.Append(shapeStyle10);
            wordprocessingShape10.Append(textBodyProperties10);

            wordprocessingGroup1.Append(wordprocessingShape10);
        }

        private void DrawCharacterBox(Wpg.WordprocessingGroup wordprocessingGroup1, Rect extents, string colour, int thick)
        {
            UInt32Value bondLineId = UInt32Value.FromUInt32((uint)m_ooxmlId++);
            string bondLineName = "box" + bondLineId;

            Int64Value width = (Int64Value)extents.Width;
            Int64Value height = (Int64Value)extents.Height;
            Int64Value top = (Int64Value)extents.Top;
            Int64Value left = (Int64Value)extents.Left;

            Wps.WordprocessingShape wordprocessingShape1 = new Wps.WordprocessingShape();
            Wps.NonVisualDrawingProperties nonVisualDrawingProperties1 = new Wps.NonVisualDrawingProperties()
            {
                Id = bondLineId,
                Name = bondLineName
            };
            Wps.NonVisualDrawingShapeProperties nonVisualDrawingShapeProperties1 = new Wps.NonVisualDrawingShapeProperties();

            Wps.ShapeProperties shapeProperties1 = new Wps.ShapeProperties();

            A.Transform2D transform2D1 = new A.Transform2D();
            A.Offset offset2 = new A.Offset() { X = left, Y = top };
            A.Extents extents2 = new A.Extents() { Cx = width, Cy = height };

            transform2D1.Append(offset2);
            transform2D1.Append(extents2);

            A.CustomGeometry customGeometry1 = new A.CustomGeometry();
            A.AdjustValueList adjustValueList1 = new A.AdjustValueList();
            A.Rectangle rectangle1 = new A.Rectangle() { Left = "l", Top = "t", Right = "r", Bottom = "b" };

            A.PathList pathList1 = new A.PathList();

            A.Path path1 = new A.Path() { Width = width, Height = height };

            // Starting Point
            A.MoveTo moveTo1 = new A.MoveTo();
            A.Point point1 = new A.Point() { X = "0", Y = "0" };
            moveTo1.Append(point1);

            // Mid Point
            A.LineTo lineTo1 = new A.LineTo();
            A.Point point2 = new A.Point() { X = extents.Width.ToString("0"), Y = "0" };
            lineTo1.Append(point2);

            // Mid Point
            A.LineTo lineTo2 = new A.LineTo();
            A.Point point3 = new A.Point() { X = extents.Width.ToString("0"), Y = extents.Height.ToString("0") };
            lineTo2.Append(point3);

            // Last Point
            A.LineTo lineTo3 = new A.LineTo();
            A.Point point4 = new A.Point() { X = "0", Y = extents.Height.ToString("0") };
            lineTo3.Append(point4);

            // Back to Start Point
            A.LineTo lineTo4 = new A.LineTo();
            A.Point point5 = new A.Point() { X = "0", Y = "0" };
            lineTo4.Append(point5);

            path1.Append(moveTo1);
            path1.Append(lineTo1);
            path1.Append(lineTo2);
            path1.Append(lineTo3);
            path1.Append(lineTo4);

            pathList1.Append(path1);

            customGeometry1.Append(adjustValueList1);
            customGeometry1.Append(rectangle1);
            customGeometry1.Append(pathList1);

            A.Outline outline1 = new A.Outline() { Width = thick, CapType = A.LineCapValues.Round };

            A.SolidFill solidFill1 = new A.SolidFill();

            A.RgbColorModelHex rgbColorModelHex1 = new A.RgbColorModelHex() { Val = colour };
            A.Alpha alpha1 = new A.Alpha() { Val = new Int32Value() { InnerText = "100%" } };

            rgbColorModelHex1.Append(alpha1);

            solidFill1.Append(rgbColorModelHex1);

            outline1.Append(solidFill1);

            shapeProperties1.Append(transform2D1);
            shapeProperties1.Append(customGeometry1);
            shapeProperties1.Append(outline1);

            Wps.ShapeStyle shapeStyle1 = new Wps.ShapeStyle();
            A.LineReference lineReference1 = new A.LineReference() { Index = (UInt32Value)0U };
            A.FillReference fillReference1 = new A.FillReference() { Index = (UInt32Value)0U };
            A.EffectReference effectReference1 = new A.EffectReference() { Index = (UInt32Value)0U };
            A.FontReference fontReference1 = new A.FontReference() { Index = A.FontCollectionIndexValues.Minor };

            shapeStyle1.Append(lineReference1);
            shapeStyle1.Append(fillReference1);
            shapeStyle1.Append(effectReference1);
            shapeStyle1.Append(fontReference1);
            Wps.TextBodyProperties textBodyProperties1 = new Wps.TextBodyProperties();

            wordprocessingShape1.Append(nonVisualDrawingProperties1);
            wordprocessingShape1.Append(nonVisualDrawingShapeProperties1);
            wordprocessingShape1.Append(shapeProperties1);
            wordprocessingShape1.Append(shapeStyle1);
            wordprocessingShape1.Append(textBodyProperties1);

            wordprocessingGroup1.Append(wordprocessingShape1);
        }
    }
}