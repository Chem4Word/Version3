// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using A = DocumentFormat.OpenXml.Drawing;
using Point = System.Windows.Point;
using Wpg = DocumentFormat.OpenXml.Office2010.Word.DrawingGroup;
using Wps = DocumentFormat.OpenXml.Office2010.Word.DrawingShape;

namespace Chem4Word.Renderer.OoXmlV3.OOXML.Bonds
{
    public class BondLineRenderer
    {
        private Rect m_canvasExtents;
        private long m_ooxmlId;
        private double m_medianBondLength;

        public BondLineRenderer(Rect canvasExtents, ref long ooxmlId, double medianBondLength)
        {
            m_canvasExtents = canvasExtents;
            m_ooxmlId = ooxmlId;
            m_medianBondLength = medianBondLength;
        }

        public void DrawWedgeBond(Wpg.WordprocessingGroup wordprocessingGroup1, BondLine bl)
        {
            BondLine wedgeLeft = bl.GetParallel(BondOffset());
            BondLine wedgeRight = bl.GetParallel(-BondOffset());

            List<Point> points = new List<Point>();
            points.Add(new Point(bl.Start.X, bl.Start.Y));
            points.Add(new Point(wedgeLeft.End.X, wedgeLeft.End.Y));
            points.Add(new Point(wedgeRight.End.X, wedgeRight.End.Y));

            switch (bl.Type)
            {
                case BondLineStyle.Wedge:
                    DrawFilledTriangle(wordprocessingGroup1, points);
                    break;

                case BondLineStyle.Hash:
                    DrawHashBondLines(wordprocessingGroup1, points);
                    break;

                default:
                    DrawBondLine(wordprocessingGroup1, bl);
                    break;
            }
        }

        public void DrawBondLine(Wpg.WordprocessingGroup wordprocessingGroup1, BondLine bl)
        {
            Point startPoint = new Point(bl.Start.X, bl.Start.Y);
            Point endPoint = new Point(bl.End.X, bl.End.Y);
            Rect extents = bl.BoundingBox;

            // Move Bond Line Extents and Points to have 0,0 Top Left Reference
            startPoint.Offset(-m_canvasExtents.Left, -m_canvasExtents.Top);
            endPoint.Offset(-m_canvasExtents.Left, -m_canvasExtents.Top);
            extents.Offset(-m_canvasExtents.Left, -m_canvasExtents.Top);

            // Move points into New Bond Line Extents
            startPoint.Offset(-extents.Left, -extents.Top);
            endPoint.Offset(-extents.Left, -extents.Top);

            switch (bl.Type)
            {
                case BondLineStyle.Solid:
                    DrawSolidLine(wordprocessingGroup1, extents, startPoint, endPoint);
                    break;

                case BondLineStyle.Dotted:
                    DrawDottedLine(wordprocessingGroup1, extents, startPoint, endPoint);
                    break;

                case BondLineStyle.Dashed:
                    DrawDashedLine(wordprocessingGroup1, extents, startPoint, endPoint);
                    break;

                case BondLineStyle.Wavy:
                    DrawWavyLine(wordprocessingGroup1, extents, startPoint, endPoint);
                    break;

                default:
                    DrawSolidLine(wordprocessingGroup1, extents, startPoint, endPoint);
                    break;
            }
        }

        private void DrawWavyLine(Wpg.WordprocessingGroup wordprocessingGroup1, Rect extents, Point bondStart, Point bondEnd)
        {
            UInt32Value bondLineId = UInt32Value.FromUInt32((uint)m_ooxmlId++);
            string bondLineName = "WavyLine" + bondLineId;

            Vector bondVector = bondEnd - bondStart;
            int noOfWiggles = (int)Math.Ceiling(bondVector.Length / BondOffset());
            if (noOfWiggles < 1)
            {
                noOfWiggles = 1;
            }

            double wiggleLength = bondVector.Length / noOfWiggles;
            Debug.WriteLine($"v.Length: {bondVector.Length} noOfWiggles: {noOfWiggles}");

            Vector originalWigglePortion = bondVector;
            originalWigglePortion.Normalize();
            originalWigglePortion *= wiggleLength / 2;

            Matrix toLeft = new Matrix();
            toLeft.Rotate(-60);
            Matrix toRight = new Matrix();
            toRight.Rotate(60);
            Vector leftVector = originalWigglePortion * toLeft;
            Vector rightVector = originalWigglePortion * toRight;

            List<Point> allpoints = new List<Point>();
            List<List<Point>> allTriangles = new List<List<Point>>();
            List<Point> triangle = new List<Point>();

            Point lastPoint = bondStart;
            allpoints.Add(lastPoint);
            triangle.Add(lastPoint);
            for (int i = 0; i < noOfWiggles; i++)
            {
                Point leftPoint = lastPoint + leftVector;
                allpoints.Add(leftPoint);
                triangle.Add(leftPoint);

                Point midPoint = lastPoint + originalWigglePortion;
                allpoints.Add(midPoint);
                triangle.Add(midPoint);
                allTriangles.Add(triangle);
                triangle = new List<Point>();
                triangle.Add(midPoint);

                Point rightPoint = lastPoint + originalWigglePortion + rightVector;
                allpoints.Add(rightPoint);
                triangle.Add(rightPoint);

                lastPoint += originalWigglePortion * 2;
                allpoints.Add(lastPoint);
                triangle.Add(lastPoint);
                allTriangles.Add(triangle);
                triangle = new List<Point>();
                triangle.Add(lastPoint);
            }

            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            foreach (Point p in allpoints)
            {
                maxX = Math.Max(p.X + extents.Left, maxX);
                minX = Math.Min(p.X + extents.Left, minX);
                maxY = Math.Max(p.Y + extents.Top, maxY);
                minY = Math.Min(p.Y + extents.Top, minY);
            }

            Rect newExtents = new Rect(minX, minY, maxX - minX, maxY - minY);
            double xOffset = extents.Left - newExtents.Left;
            double yOffset = extents.Top - newExtents.Top;

            Int64Value width = OoXmlHelper.ScaleCmlToEmu(newExtents.Width);
            Int64Value height = OoXmlHelper.ScaleCmlToEmu(newExtents.Height);
            Int64Value top = OoXmlHelper.ScaleCmlToEmu(newExtents.Top);
            Int64Value left = OoXmlHelper.ScaleCmlToEmu(newExtents.Left);

            Wps.WordprocessingShape wordprocessingShape1 = new Wps.WordprocessingShape();
            Wps.NonVisualDrawingProperties nonVisualDrawingProperties1 = new Wps.NonVisualDrawingProperties() { Id = bondLineId, Name = bondLineName };
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

            A.MoveTo moveTo1 = new A.MoveTo();
            A.Point point1 = new A.Point() { X = OoXmlHelper.ScaleCmlToEmu(bondStart.X + xOffset).ToString(), Y = OoXmlHelper.ScaleCmlToEmu(bondStart.Y + yOffset).ToString() };
            moveTo1.Append(point1);
            path1.Append(moveTo1);

            //Curves
            foreach (var tri in allTriangles)
            {
                A.CubicBezierCurveTo cubicBezierCurveTo = new A.CubicBezierCurveTo();
                foreach (var p in tri)
                {
                    A.Point point = new A.Point() { X = OoXmlHelper.ScaleCmlToEmu(p.X + xOffset).ToString(), Y = OoXmlHelper.ScaleCmlToEmu(p.Y + yOffset).ToString() };
                    cubicBezierCurveTo.Append(point);
                }
                path1.Append(cubicBezierCurveTo);
            }

            //// Straight Lines
            //foreach (var p in allpoints)
            //{
            //    A.LineTo lineTo = new A.LineTo();
            //    A.Point point = new A.Point() { X = OoXmlHelper.ScaleCmlToEmu(p.X + xOffset).ToString(), Y = OoXmlHelper.ScaleCmlToEmu(p.Y + yOffset).ToString() };
            //    lineTo.Append(point);
            //    path1.Append(lineTo);
            //}

            pathList1.Append(path1);

            customGeometry1.Append(adjustValueList1);
            customGeometry1.Append(rectangle1);
            customGeometry1.Append(pathList1);

            A.Outline outline1 = new A.Outline() { Width = 9525, CapType = A.LineCapValues.Round };

            A.SolidFill solidFill1 = new A.SolidFill();

            A.RgbColorModelHex rgbColorModelHex1 = new A.RgbColorModelHex() { Val = "000000" };
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

        private void DrawFilledTriangle(Wpg.WordprocessingGroup wordprocessingGroup1, List<Point> points)
        {
            UInt32Value atomLabelId = UInt32Value.FromUInt32((uint)m_ooxmlId++);
            string atomLabelName = "WedgeBond" + atomLabelId;

            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            foreach (Point p in points)
            {
                maxX = Math.Max(p.X, maxX);
                minX = Math.Min(p.X, minX);
                maxY = Math.Max(p.Y, maxY);
                minY = Math.Min(p.Y, minY);
            }

            Rect extents = new Rect(minX, minY, maxX - minX, maxY - minY);

            // Create modifyable Points
            Point p0 = new Point(points[0].X, points[0].Y);
            Point p1 = new Point(points[1].X, points[1].Y);
            Point p2 = new Point(points[2].X, points[2].Y);

            // Move Points to have 0,0 Top Left Reference within the drawing canvas
            p0.Offset(-m_canvasExtents.Left, -m_canvasExtents.Top);
            p1.Offset(-m_canvasExtents.Left, -m_canvasExtents.Top);
            p2.Offset(-m_canvasExtents.Left, -m_canvasExtents.Top);

            // Move shape's extents to correct place in drawing canvas
            extents.Offset(-m_canvasExtents.Left, -m_canvasExtents.Top);

            // Move points again to put them inside the shape's extents
            p0.Offset(-extents.Left, -extents.Top);
            p1.Offset(-extents.Left, -extents.Top);
            p2.Offset(-extents.Left, -extents.Top);

            Int64Value top = OoXmlHelper.ScaleCmlToEmu(extents.Y);
            Int64Value left = OoXmlHelper.ScaleCmlToEmu(extents.X);
            Int64Value width = OoXmlHelper.ScaleCmlToEmu(extents.Width);
            Int64Value height = OoXmlHelper.ScaleCmlToEmu(extents.Height);

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

            string xCoOrdinate = OoXmlHelper.ScaleCmlToEmu(p0.X).ToString();
            string yCoOrdinate = OoXmlHelper.ScaleCmlToEmu(p0.Y).ToString();

            A.MoveTo moveTo10 = new A.MoveTo();
            A.Point point19 = new A.Point() { X = xCoOrdinate, Y = yCoOrdinate };
            moveTo10.Append(point19);
            path10.Append(moveTo10);

            xCoOrdinate = OoXmlHelper.ScaleCmlToEmu(p1.X).ToString();
            yCoOrdinate = OoXmlHelper.ScaleCmlToEmu(p1.Y).ToString();

            A.LineTo lineTo10 = new A.LineTo();
            A.Point point20 = new A.Point() { X = xCoOrdinate, Y = yCoOrdinate };
            lineTo10.Append(point20);
            path10.Append(lineTo10);

            xCoOrdinate = OoXmlHelper.ScaleCmlToEmu(p2.X).ToString();
            yCoOrdinate = OoXmlHelper.ScaleCmlToEmu(p2.Y).ToString();

            A.LineTo lineTo19 = new A.LineTo();
            A.Point point29 = new A.Point() { X = xCoOrdinate, Y = yCoOrdinate };
            lineTo19.Append(point29);
            path10.Append(lineTo19);
            A.CloseShapePath closeShapePath1 = new A.CloseShapePath();
            path10.Append(closeShapePath1);

            pathList10.Append(path10);

            customGeometry10.Append(adjustValueList10);
            customGeometry10.Append(rectangle10);
            customGeometry10.Append(pathList10);

            A.SolidFill solidFill10 = new A.SolidFill();

            A.RgbColorModelHex rgbColorModelHex10 = new A.RgbColorModelHex() { Val = "000000" };
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

        private void DrawHashBondLines(Wpg.WordprocessingGroup wordprocessingGroup1, List<Point> points)
        {
            // Create modifyable Points
            Point p0 = new Point(points[0].X, points[0].Y);
            Point p1 = new Point(points[1].X, points[1].Y);
            Point p2 = new Point(points[2].X, points[2].Y);

            // Draw Tail Line
            BondLine blTail = new BondLine(points[1], points[2], BondLineStyle.Solid, null, null);
            DrawBondLine(wordprocessingGroup1, blTail);

            // Prep for intermediate lines
            Point midPoint = CoordinateTool.GetMidPoint(points[1], points[2]);
            Vector v = points[0] - midPoint;
            double ticks = v.Length / 2;
            v = v / ticks;

            BondLine blNext;

            Point outIntersectP1;
            Point outIntersectP2;

            Point leftTailEnd = p1;
            Point rightTailEnd = p2;

            bool linesIntersect;
            bool segmentsIntersect;

            // Draw itermediate lines
            for (int i = 1; i <= (int)ticks; i++)
            {
                p1.Offset(v.X, v.Y);
                p2.Offset(v.X, v.Y);
                CoordinateTool.FindIntersection(leftTailEnd, p0, p1, p2,
                    out linesIntersect, out segmentsIntersect, out outIntersectP1);
                CoordinateTool.FindIntersection(rightTailEnd, p0, p1, p2,
                    out linesIntersect, out segmentsIntersect, out outIntersectP2);
                blNext = new BondLine(outIntersectP1, outIntersectP2, BondLineStyle.Solid, null, null);
                DrawBondLine(wordprocessingGroup1, blNext);
            }

            // Draw small line for nose
            Point n1 = new Point(points[0].X - 0.025, points[0].Y - 0.025);
            Point n2 = new Point(points[0].X + 0.025, points[0].Y + 0.025);

            BondLine blNose = new BondLine(n1, n2, BondLineStyle.Solid, null, null);
            DrawBondLine(wordprocessingGroup1, blNose);
        }

        private void DrawSolidLine(Wpg.WordprocessingGroup wordprocessingGroup1, Rect extents, Point bondStart, Point bondEnd)
        {
            UInt32Value bondLineId = UInt32Value.FromUInt32((uint)m_ooxmlId++);
            string bondLineName = "BondLine" + bondLineId;

            Int64Value width = OoXmlHelper.ScaleCmlToEmu(extents.Width);
            Int64Value height = OoXmlHelper.ScaleCmlToEmu(extents.Height);
            Int64Value top = OoXmlHelper.ScaleCmlToEmu(extents.Top);
            Int64Value left = OoXmlHelper.ScaleCmlToEmu(extents.Left);

            Wps.WordprocessingShape wordprocessingShape1 = new Wps.WordprocessingShape();
            Wps.NonVisualDrawingProperties nonVisualDrawingProperties1 = new Wps.NonVisualDrawingProperties() { Id = bondLineId, Name = bondLineName };
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

            A.MoveTo moveTo1 = new A.MoveTo();
            A.Point point1 = new A.Point() { X = OoXmlHelper.ScaleCmlToEmu(bondStart.X).ToString(), Y = OoXmlHelper.ScaleCmlToEmu(bondStart.Y).ToString() };

            moveTo1.Append(point1);

            A.LineTo lineTo1 = new A.LineTo();
            A.Point point2 = new A.Point() { X = OoXmlHelper.ScaleCmlToEmu(bondEnd.X).ToString(), Y = OoXmlHelper.ScaleCmlToEmu(bondEnd.Y).ToString() };

            lineTo1.Append(point2);

            path1.Append(moveTo1);
            path1.Append(lineTo1);

            pathList1.Append(path1);

            customGeometry1.Append(adjustValueList1);
            customGeometry1.Append(rectangle1);
            customGeometry1.Append(pathList1);

            A.Outline outline1 = new A.Outline() { Width = 9525, CapType = A.LineCapValues.Round };

            A.SolidFill solidFill1 = new A.SolidFill();

            A.RgbColorModelHex rgbColorModelHex1 = new A.RgbColorModelHex() { Val = "000000" };
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

        private void DrawDottedLine(Wpg.WordprocessingGroup wordprocessingGroup1, Rect extents, Point bondStart, Point bondEnd)
        {
            UInt32Value bondLineId = UInt32Value.FromUInt32((uint)m_ooxmlId++);
            string bondLineName = "DottedBondLine" + bondLineId;

            Int64Value width = OoXmlHelper.ScaleCmlToEmu(extents.Width);
            Int64Value height = OoXmlHelper.ScaleCmlToEmu(extents.Height);
            Int64Value top = OoXmlHelper.ScaleCmlToEmu(extents.Top);
            Int64Value left = OoXmlHelper.ScaleCmlToEmu(extents.Left);

            Wps.WordprocessingShape wordprocessingShape1 = new Wps.WordprocessingShape();
            Wps.NonVisualDrawingProperties nonVisualDrawingProperties1 = new Wps.NonVisualDrawingProperties() { Id = bondLineId, Name = bondLineName };
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

            A.MoveTo moveTo1 = new A.MoveTo();
            A.Point point1 = new A.Point() { X = OoXmlHelper.ScaleCmlToEmu(bondStart.X).ToString(), Y = OoXmlHelper.ScaleCmlToEmu(bondStart.Y).ToString() };

            moveTo1.Append(point1);

            A.LineTo lineTo1 = new A.LineTo();
            A.Point point2 = new A.Point() { X = OoXmlHelper.ScaleCmlToEmu(bondEnd.X).ToString(), Y = OoXmlHelper.ScaleCmlToEmu(bondEnd.Y).ToString() };

            lineTo1.Append(point2);

            path1.Append(moveTo1);
            path1.Append(lineTo1);

            pathList1.Append(path1);

            customGeometry1.Append(adjustValueList1);
            customGeometry1.Append(rectangle1);
            customGeometry1.Append(pathList1);

            A.Outline outline1 = new A.Outline() { Width = 9525, CapType = A.LineCapValues.Round };

            A.SolidFill solidFill1 = new A.SolidFill();
            A.PresetDash presetDash1 = new A.PresetDash() { Val = A.PresetLineDashValues.Dash };

            A.RgbColorModelHex rgbColorModelHex1 = new A.RgbColorModelHex() { Val = "000000" };
            A.Alpha alpha1 = new A.Alpha() { Val = new Int32Value() { InnerText = "100%" } };

            rgbColorModelHex1.Append(alpha1);

            solidFill1.Append(rgbColorModelHex1);

            outline1.Append(solidFill1);
            outline1.Append(presetDash1);

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

        private void DrawDashedLine(Wpg.WordprocessingGroup wordprocessingGroup1, Rect extents, Point bondStart, Point bondEnd)
        {
            UInt32Value bondLineId = UInt32Value.FromUInt32((uint)m_ooxmlId++);
            string bondLineName = "DashedBondLine" + bondLineId;

            Int64Value width = OoXmlHelper.ScaleCmlToEmu(extents.Width);
            Int64Value height = OoXmlHelper.ScaleCmlToEmu(extents.Height);
            Int64Value top = OoXmlHelper.ScaleCmlToEmu(extents.Top);
            Int64Value left = OoXmlHelper.ScaleCmlToEmu(extents.Left);

            Wps.WordprocessingShape wordprocessingShape1 = new Wps.WordprocessingShape();
            Wps.NonVisualDrawingProperties nonVisualDrawingProperties1 = new Wps.NonVisualDrawingProperties() { Id = bondLineId, Name = bondLineName };
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

            A.MoveTo moveTo1 = new A.MoveTo();
            A.Point point1 = new A.Point() { X = OoXmlHelper.ScaleCmlToEmu(bondStart.X).ToString(), Y = OoXmlHelper.ScaleCmlToEmu(bondStart.Y).ToString() };

            moveTo1.Append(point1);

            A.LineTo lineTo1 = new A.LineTo();
            A.Point point2 = new A.Point() { X = OoXmlHelper.ScaleCmlToEmu(bondEnd.X).ToString(), Y = OoXmlHelper.ScaleCmlToEmu(bondEnd.Y).ToString() };

            lineTo1.Append(point2);

            path1.Append(moveTo1);
            path1.Append(lineTo1);

            pathList1.Append(path1);

            customGeometry1.Append(adjustValueList1);
            customGeometry1.Append(rectangle1);
            customGeometry1.Append(pathList1);

            A.Outline outline1 = new A.Outline() { Width = 9525, CapType = A.LineCapValues.Round };

            A.SolidFill solidFill1 = new A.SolidFill();
            A.PresetDash presetDash1 = new A.PresetDash() { Val = A.PresetLineDashValues.LargeDash };

            A.RgbColorModelHex rgbColorModelHex1 = new A.RgbColorModelHex() { Val = "000000" };
            A.Alpha alpha1 = new A.Alpha() { Val = new Int32Value() { InnerText = "100%" } };

            rgbColorModelHex1.Append(alpha1);

            solidFill1.Append(rgbColorModelHex1);

            outline1.Append(solidFill1);
            outline1.Append(presetDash1);

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

        private double BondOffset()
        {
            return (m_medianBondLength * OoXmlHelper.MULTIPLE_BOND_OFFSET_PERCENTAGE);
        }
    }
}