// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Forms;
using Chem4Word.Model;
using Chem4Word.Model.Enums;
using Chem4Word.Renderer.OoXmlV3.OOXML.Atoms;
using Chem4Word.Renderer.OoXmlV3.OOXML.Bonds;
using Chem4Word.Renderer.OoXmlV3.TTF;
using DocumentFormat.OpenXml;
using IChem4Word.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using A = DocumentFormat.OpenXml.Drawing;
using Drawing = DocumentFormat.OpenXml.Wordprocessing.Drawing;
using Point = System.Windows.Point;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Wp = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using Wpg = DocumentFormat.OpenXml.Office2010.Word.DrawingGroup;
using Wps = DocumentFormat.OpenXml.Office2010.Word.DrawingShape;

namespace Chem4Word.Renderer.OoXmlV3.OOXML
{
    public class OoXmlRenderer
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private Options _options;
        private IChem4WordTelemetry _telemetry;
        private Point _topLeft;

        private Model.Model _chemistryModel;
        private Dictionary<char, TtfCharacter> _TtfCharacterSet;

        private long _ooxmlId = 1;
        private Rect _canvasExtents;

        private double _medianBondLength;
        private Rect _moleculeExtents;

        private const double EPSILON = 1e-4;

        private List<AtomLabelCharacter> _atomLabelCharacters;
        private List<BondLine> _bondLines;

        private Dictionary<string, Ring> _rings = new Dictionary<string, Ring>();

        private PeriodicTable _pt = new PeriodicTable();

        public OoXmlRenderer(Model.Model model, Options options, IChem4WordTelemetry telemetry, Point topLeft)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            _options = options;
            _telemetry = telemetry;
            _topLeft = topLeft;

            _telemetry.Write(module, "Verbose", "Called");

            LoadFont();

            _chemistryModel = model;

            DetermineInitialExtents();
        }

        private void DetermineInitialExtents()
        {
            var xMax = _chemistryModel.AllAtoms.Select(a => a.Position.X).Max();
            var xMin = _chemistryModel.AllAtoms.Select(a => a.Position.X).Min();

            var yMax = _chemistryModel.AllAtoms.Select(a => a.Position.Y).Max();
            var yMin = _chemistryModel.AllAtoms.Select(a => a.Position.Y).Min();

            _moleculeExtents = new Rect(new Point(xMin, yMin), new Point(xMax, yMax));
        }

        public Run GenerateRun()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            _telemetry.Write(module, "Verbose", "Called");

            //start off progress monitoring
            Progress pb = new Progress();
            pb.TopLeft = _topLeft;

            //lists of objects for drawing
            _atomLabelCharacters = new List<AtomLabelCharacter>();
            _bondLines = new List<BondLine>();

            Stopwatch swr = new Stopwatch();
            Stopwatch sw = new Stopwatch();

            //Create a run
            Run run = new Run();

            sw.Start();
            swr.Start();

            //set the median bond length
            _medianBondLength = GeometryTool.GetMedianBondLength2D(_chemistryModel.AllBonds);

            int moleculeNo = 0;
            foreach (Molecule mol in _chemistryModel.Molecules)
            {
                moleculeNo++;
                // Step 1- gather the atom information together
                Debug.WriteLine($"{module} Starting Step 1");
                //_telemetry.Write(module, "Verbose", $"Starting Step 1 for molecule {moleculeNo}");

                ProcessAtoms(mol, pb, moleculeNo, _pt);

                Debug.WriteLine("Elapsed time " + sw.ElapsedMilliseconds.ToString("##,##0") + "ms");
                //_telemetry.Write(module, "Timing", $"Step 1 for molecule {moleculeNo} took " + sw.ElapsedMilliseconds.ToString("#,##0", CultureInfo.InvariantCulture) + "ms");
                sw.Reset();
                sw.Start();

                // Step 2- gather the bond information together

                Debug.WriteLine($"{module} Starting Step 2");
                //_telemetry.Write(module, "Verbose", $"Starting Step 2 for molecule {moleculeNo}");
                ProcessBonds(mol, pb, moleculeNo);

                Debug.WriteLine("Elapsed time " + sw.ElapsedMilliseconds.ToString("##,##0") + "ms");
                //_telemetry.Write(module, "Timing", $"Step 2 for molecule {moleculeNo} took " + sw.ElapsedMilliseconds.ToString("#,##0", CultureInfo.InvariantCulture) + "ms");
                sw.Reset();
                sw.Start();

                if (_options.ShowRingCentres)
                {
                    // Save Rings for later
                    foreach (Ring ring in mol.Rings)
                    {
                        _rings.Add(ring.UniqueID, ring);
                    }
                }
            }

            Debug.WriteLine($"{module} Starting Step 3");
            //_telemetry.Write(module, "Verbose", "Starting Step 3");

            IncreaseCanvasSize();

            Debug.WriteLine("Elapsed time " + sw.ElapsedMilliseconds.ToString("##,##0") + "ms");
            //_telemetry.Write(module, "Timing", "Step 3 took " + sw.ElapsedMilliseconds.ToString("#,##0", CultureInfo.InvariantCulture) + "ms");
            sw.Reset();
            sw.Start();

            if (_options.ClipLines)
            {
                Debug.WriteLine($"{module} Starting Step 4");
                //_telemetry.Write(module, "Verbose", "Starting Step 4");

                #region Step 4 - Shrink bond lines

                ShrinkBondLines(pb);

                #endregion Step 4 - Shrink bond lines

                Debug.WriteLine("Elapsed time " + sw.ElapsedMilliseconds.ToString("##,##0") + "ms");
                //_telemetry.Write(module, "Timing", "Step 4 took " + sw.ElapsedMilliseconds.ToString("#,##0", CultureInfo.InvariantCulture) + "ms");
                sw.Reset();
                sw.Start();
            }

            Debug.WriteLine($"{module} Starting Step 5");
            //_telemetry.Write(module, "Verbose", "Starting Step 5");

            #region Step 5 - Create main OoXml drawing objects

            Drawing drawing1 = new Drawing();
            A.Graphic graphic1 = CreateGraphic();
            A.GraphicData graphicData1 = CreateGraphicData();
            Wpg.WordprocessingGroup wordprocessingGroup1 = new Wpg.WordprocessingGroup();

            // Create Inline Drawing using canvas extents
            Wp.Inline inline1 = CreateInline(graphicData1, wordprocessingGroup1);

            // Right-Click Issues - Step 1 of hopefull fix - Add background to force whole object select
            //DrawShape(wordprocessingGroup1, _canvasExtents, A.ShapeTypeValues.Rectangle, "eeeeee");

            #endregion Step 5 - Create main OoXml drawing objects

            Debug.WriteLine("Elapsed time " + sw.ElapsedMilliseconds.ToString("##,##0") + "ms");
            //_telemetry.Write(module, "Timing", "Step 5 took " + sw.ElapsedMilliseconds.ToString("#,##0", CultureInfo.InvariantCulture) + "ms");
            sw.Reset();
            sw.Start();

            #region Step 5a - Diagnostics

            if (_options.ShowMoleculeBoundingBoxes)
            {
                DrawBox(wordprocessingGroup1, _moleculeExtents, "0000ff", 1);
                foreach (Molecule mol in _chemistryModel.Molecules)
                {
                    DrawBox(wordprocessingGroup1, mol.BoundingBox, "ff000", 1);
                }
                DrawBox(wordprocessingGroup1, _canvasExtents, "000000", 1);
            }

            if (_options.ShowRingCentres)
            {
                ShowRingCentres(wordprocessingGroup1);
            }

            if (_options.ShowAtomPositions)
            {
                ShowAtomCentres(wordprocessingGroup1);
            }

            #endregion Step 5a - Diagnostics

            Debug.WriteLine($"{module} Starting Step 6");
            //_telemetry.Write(module, "Verbose", "Starting Step 6");

            #region Step 6 - Create and append OoXml objects for all Bond Lines

            AppendBondOoxml(pb, wordprocessingGroup1);

            #endregion Step 6 - Create and append OoXml objects for all Bond Lines

            Debug.WriteLine("Elapsed time " + sw.ElapsedMilliseconds.ToString("##,##0") + "ms");
            //_telemetry.Write(module, "Timing", "Step 6 took " + sw.ElapsedMilliseconds.ToString("#,##0", CultureInfo.InvariantCulture) + "ms");
            sw.Reset();
            sw.Start();

            Debug.WriteLine($"{module} Starting Step 7");
            //_telemetry.Write(module, "Verbose", "Starting Step 7");

            #region Step 7 - Create and append OoXml objects for Atom Labels

            AppendAtomLabelOoxml(pb, wordprocessingGroup1);

            #endregion Step 7 - Create and append OoXml objects for Atom Labels

            Debug.WriteLine("Elapsed time " + sw.ElapsedMilliseconds.ToString("##,##0") + "ms");
            //_telemetry.Write(module, "Timing", "Step 7 took " + sw.ElapsedMilliseconds.ToString("#,##0", CultureInfo.InvariantCulture) + "ms");
            sw.Reset();
            sw.Start();

            Debug.WriteLine($"{module} Starting Step 8");
            //_telemetry.Write(module, "Verbose", "Starting Step 8");

            #region Step 8 - Append OoXml drawing objects to OoXml run object

            AppendAllOoXml(graphicData1, wordprocessingGroup1, graphic1, inline1, drawing1, run);

            #endregion Step 8 - Append OoXml drawing objects to OoXml run object

            Debug.WriteLine("Elapsed time " + sw.ElapsedMilliseconds.ToString("##,##0") + "ms");
            //_telemetry.Write(module, "Timing", "Step 8 took " + sw.ElapsedMilliseconds.ToString("#,##0", CultureInfo.InvariantCulture) + "ms");
            sw.Reset();
            sw.Start();

            double abl = _chemistryModel.MeanBondLength;
            Debug.WriteLine("Elapsed time for GenerateRun " + swr.ElapsedMilliseconds.ToString("#,##0", CultureInfo.InvariantCulture) + "ms");
            _telemetry.Write(module, "Timing", $"Rendering {_chemistryModel.Molecules.Count} molecules with {_chemistryModel.AllAtoms.Count} atoms and {_chemistryModel.AllBonds.Count} bonds took {swr.ElapsedMilliseconds.ToString("##,##0")} ms; Average Bond Length: {abl.ToString("#0.00")}");

            ShutDownProgress(pb);

            return run;
        }

        private void ShowAtomCentres(Wpg.WordprocessingGroup wordprocessingGroup1)
        {
            double xx = _medianBondLength * OoXmlHelper.MULTIPLE_BOND_OFFSET_PERCENTAGE / 3;

            foreach (var atom in _chemistryModel.AllAtoms)
            {
                Rect bb = new Rect(new Point(atom.Position.X - xx, atom.Position.Y - xx), new Point(atom.Position.X + xx, atom.Position.Y + xx));
                DrawShape(wordprocessingGroup1, bb, A.ShapeTypeValues.Ellipse, "ff0000");
            }
        }

        private static void ShutDownProgress(Progress pb)
        {
            pb.Value = 0;
            pb.Hide();
            pb.Close();
        }

        private static void AppendAllOoXml(A.GraphicData graphicData, Wpg.WordprocessingGroup wordprocessingGroup, A.Graphic graphic,
            Wp.Inline inline, Drawing drawing, Run run)
        {
            graphicData.Append(wordprocessingGroup);
            graphic.Append(graphicData);
            inline.Append(graphic);
            drawing.Append(inline);
            run.Append(drawing);
        }

        private void AppendAtomLabelOoxml(Progress pb, Wpg.WordprocessingGroup wordprocessingGroup1)
        {
            AtomLabelRenderer alr = new AtomLabelRenderer(_canvasExtents, ref _ooxmlId, _options);

            if (_chemistryModel.AllAtoms.Count() > 1)
            {
                pb.Show();
            }
            pb.Message = "Rendering Atoms";
            pb.Value = 0;
            pb.Maximum = _atomLabelCharacters.Count;

            foreach (AtomLabelCharacter alc in _atomLabelCharacters)
            {
                pb.Increment(1);
                alr.DrawCharacter(wordprocessingGroup1, alc);
            }
        }

        private void AppendBondOoxml(Progress pb, Wpg.WordprocessingGroup wordprocessingGroup1)
        {
            BondLineRenderer blr = new BondLineRenderer(_canvasExtents, ref _ooxmlId, _medianBondLength);

            if (_chemistryModel.AllBonds.Count > 1)
            {
                pb.Show();
            }
            pb.Message = "Rendering Bonds";
            pb.Value = 0;
            pb.Maximum = _bondLines.Count;

            foreach (BondLine bl in _bondLines)
            {
                pb.Increment(1);
                switch (bl.Type)
                {
                    case BondLineStyle.Wedge:
                    case BondLineStyle.Hash:
                        blr.DrawWedgeBond(wordprocessingGroup1, bl);
                        break;

                    default:
                        blr.DrawBondLine(wordprocessingGroup1, bl);
                        break;
                }
            }
        }

        private void ShowRingCentres(Wpg.WordprocessingGroup wordprocessingGroup1)
        {
            double xx = _medianBondLength * OoXmlHelper.MULTIPLE_BOND_OFFSET_PERCENTAGE / 3;

            //Debug.WriteLine("Ring List :-");
            foreach (KeyValuePair<string, Ring> kvp in _rings)
            {
                Ring r = (Ring)kvp.Value;
                //Debug.WriteLine("Ring " + kvp.Key + " " + r.BondIdSet.Count);
                Debug.Assert(r.Centroid != null);
                Point c = r.Centroid.Value;
                //Debug.WriteLine(" Centroid at " + c);
                Rect bb = new Rect(new Point(c.X - xx, c.Y - xx), new Point(c.X + xx, c.Y + xx));
                DrawShape(wordprocessingGroup1, bb, A.ShapeTypeValues.Ellipse, "0000ff");
            }
        }

        private void ShrinkBondLines(Progress pb)
        {
            // so that they do not overlap label characters

            if (_chemistryModel.AllAtoms.Count > 1)
            {
                pb.Show();
            }
            pb.Message = "Clipping Bond Lines";
            pb.Value = 0;
            //pb.Maximum = (m_AtomLabelCharacters.Count * m_BondLines.Count);
            pb.Maximum = _atomLabelCharacters.Count;

            foreach (AtomLabelCharacter alc in _atomLabelCharacters)
            {
                pb.Increment(1);

                double width = OoXmlHelper.ScaleCsTtfToCml(alc.Character.Width);
                double height = OoXmlHelper.ScaleCsTtfToCml(alc.Character.Height);

                if (alc.IsSubScript)
                {
                    // Shrink bounding box
                    width = width * OoXmlHelper.SUBSCRIPT_SCALE_FACTOR;
                    height = height * OoXmlHelper.SUBSCRIPT_SCALE_FACTOR;
                }

                // Create rectangle of the bounding box with a suitable clipping margin
                Rect cbb = new Rect(alc.Position.X - OoXmlHelper.CHARACTER_CLIPPING_MARGIN,
                    alc.Position.Y - OoXmlHelper.CHARACTER_CLIPPING_MARGIN,
                    width + (OoXmlHelper.CHARACTER_CLIPPING_MARGIN * 2),
                    height + (OoXmlHelper.CHARACTER_CLIPPING_MARGIN * 2));

                //Debug.WriteLine("Character: " + alc.Ascii + " Rectangle: " + a);

                // Just in case we end up splitting a line into two
                List<BondLine> extraBondLines = new List<BondLine>();

                // Select Lines which may require trimming
                // By using LINQ to implement the following SQL
                // Where (L.Right Between Cbb.Left And Cbb.Right)
                //    Or (L.Left Between Cbb.Left And Cbb.Right)
                //    Or (L.Top Between Cbb.Top And Cbb.Botton)
                //    Or (L.Bottom Between Cbb.Top And Cbb.Botton)

                var targeted = from l in _bondLines
                               where (cbb.Left <= l.BoundingBox.Right & l.BoundingBox.Right <= cbb.Right)
                                     | (cbb.Left <= l.BoundingBox.Left & l.BoundingBox.Left <= cbb.Right)
                                     | (cbb.Top <= l.BoundingBox.Top & l.BoundingBox.Top <= cbb.Bottom)
                                     | (cbb.Top <= l.BoundingBox.Bottom & l.BoundingBox.Bottom <= cbb.Bottom)
                               select l;

                foreach (BondLine bl in targeted)
                {
                    //pb.Increment(1);

                    Point start = new Point(bl.Start.X, bl.Start.Y);
                    Point end = new Point(bl.End.X, bl.End.Y);

                    //Debug.WriteLine("  Line From: " + start + " To: " + end);

                    int attempts = 0;
                    if (CohenSutherland.ClipLine(cbb, ref start, ref end, out attempts))
                    {
                        //Debug.WriteLine("    Clipped Line Start Point: " + start);
                        //Debug.WriteLine("    Clipped Line   End Point: " + end);

                        bool bClipped = false;

                        if (Math.Abs(bl.Start.X - start.X) < EPSILON && Math.Abs(bl.Start.Y - start.Y) < EPSILON)
                        {
                            bl.Start = new Point(end.X, end.Y);
                            bClipped = true;
                        }
                        if (Math.Abs(bl.End.X - end.X) < EPSILON && Math.Abs(bl.End.Y - end.Y) < EPSILON)
                        {
                            bl.End = new Point(start.X, start.Y);
                            bClipped = true;
                        }

                        if (!bClipped)
                        {
                            // Line was clipped at both ends;
                            // 1. Generate new line
                            BondLine extraLine = new BondLine(new Point(end.X, end.Y), new Point(bl.End.X, bl.End.Y), bl.Type, bl.Parent);
                            extraBondLines.Add(extraLine);
                            // 2. Trim existing line
                            bl.End = new Point(start.X, start.Y);
                        }
                    }
                    if (attempts >= 15)
                    {
                        Debug.WriteLine("Clipping failed !");
                    }
                }

                // Add any extra lines generated by this character into the List of Bond Lines
                foreach (BondLine bl in extraBondLines)
                {
                    _bondLines.Add(bl);
                }
            }
        }

        private void IncreaseCanvasSize()
        {
            // to accomodate extra space required by label characters

            //Debug.WriteLine(m_canvasExtents);

            double xMin = _moleculeExtents.Left;
            double xMax = _moleculeExtents.Right;
            double yMin = _moleculeExtents.Top;
            double yMax = _moleculeExtents.Bottom;

            foreach (AtomLabelCharacter alc in _atomLabelCharacters)
            {
                xMin = Math.Min(xMin, alc.Position.X);
                xMax = Math.Max(xMax, alc.Position.X + OoXmlHelper.ScaleCsTtfToCml(alc.Character.Width));
                yMin = Math.Min(yMin, alc.Position.Y);
                yMax = Math.Max(yMax, alc.Position.Y + OoXmlHelper.ScaleCsTtfToCml(alc.Character.Height));
            }

            // Create new canvas extents
            _canvasExtents = new Rect(xMin - OoXmlHelper.DRAWING_MARGIN,
                yMin - OoXmlHelper.DRAWING_MARGIN,
                xMax - xMin + (2 * OoXmlHelper.DRAWING_MARGIN),
                yMax - yMin + (2 * OoXmlHelper.DRAWING_MARGIN));

            //Debug.WriteLine(m_canvasExtents);
        }

        private void ProcessBonds(Molecule mol, Progress pb, int moleculeNo)
        {
            BondLinePositioner br = new BondLinePositioner(_bondLines, _medianBondLength);

            if (mol.AllBonds.Count > 0)
            {
                pb.Show();
            }
            pb.Message = $"Processing Bonds in Molecule {moleculeNo}";
            pb.Value = 0;
            pb.Maximum = mol.Bonds.Count;

            foreach (Bond bond in mol.AllBonds)
            {
                pb.Increment(1);
                br.CreateLines(bond);
            }

            // Rendering molecular sketches for publication quality output
            // Alex M Clark
            // Implement beautification of semi open double bonds and double bonds touching rings

            // Obtain list of Double Bonds with Placement of BondDirection.None
            List<Bond> doubleBonds = mol.AllBonds.Where(b => b.OrderValue.Value == 2 && b.Placement == BondDirection.None).ToList();
            if (doubleBonds.Count > 0)
            {
                pb.Message = $"Processing Double Bonds in Molecule {moleculeNo}";
                pb.Value = 0;
                pb.Maximum = doubleBonds.Count;

                foreach (Bond bond in doubleBonds)
                {
                    BeautifyLines(bond.StartAtom, bond.Id);
                    BeautifyLines(bond.EndAtom, bond.Id);
                }
            }
        }

        private void BeautifyLines(Atom atom, string bondId)
        {
            if ((Element)atom.Element == Globals.PeriodicTable.C)
            {
                if (atom.Bonds.Count == 3)
                {
                    bool isInRing = atom.Rings.Count != 0;
                    List<BondLine> lines = _bondLines.Where(bl => bl.Parent.Equals(bondId)).ToList();
                    List<Bond> otherBonds;
                    if (isInRing)
                    {
                        otherBonds = atom.Bonds.Where(b => !b.Id.Equals(bondId)).ToList();
                    }
                    else
                    {
                        otherBonds = atom.Bonds.Where(b => !b.Id.Equals(bondId) && b.OrderValue == 1).ToList();
                    }
                    if (otherBonds.Count == 2)
                    {
                        BondLine line1 = _bondLines.First(bl => bl.Parent.Equals(otherBonds[0].Id));
                        BondLine line2 = _bondLines.First(bl => bl.Parent.Equals(otherBonds[1].Id));
                        TrimLines(lines, line1, line2, isInRing);
                    }
                }
            }
        }

        private void TrimLines(List<BondLine> mainPair, BondLine line1, BondLine line2, bool isInRing)
        {
            // Only two of these calls are expected to do anything
            if (!TrimLine(mainPair[0], line1, isInRing))
            {
                TrimLine(mainPair[0], line2, isInRing);
            }
            if (!TrimLine(mainPair[1], line1, isInRing))
            {
                TrimLine(mainPair[1], line2, isInRing);
            }
        }

        private bool TrimLine(BondLine leftOrRight, BondLine line, bool isInRing)
        {
            bool dummy;
            bool intersect;
            Point intersection;

            // Make a longer version of the line
            Point startLonger = new Point(leftOrRight.Start.X, leftOrRight.Start.Y);
            Point endLonger = new Point(leftOrRight.End.X, leftOrRight.End.Y);
            CoordinateTool.AdjustLineAboutMidpoint(ref startLonger, ref endLonger, _medianBondLength / 5);

            // See if they intersect at one end
            CoordinateTool.FindIntersection(startLonger, endLonger, line.Start, line.End,
                out dummy, out intersect, out intersection);

            // If they intersect update the main line
            if (intersect)
            {
                double l1 = CoordinateTool.DistanceBetween(intersection, leftOrRight.Start);
                double l2 = CoordinateTool.DistanceBetween(intersection, leftOrRight.End);
                if (l1 > l2)
                {
                    leftOrRight.End = new Point(intersection.X, intersection.Y);
                }
                else
                {
                    leftOrRight.Start = new Point(intersection.X, intersection.Y);
                }
                if (!isInRing)
                {
                    l1 = CoordinateTool.DistanceBetween(intersection, line.Start);
                    l2 = CoordinateTool.DistanceBetween(intersection, line.End);
                    if (l1 > l2)
                    {
                        line.End = new Point(intersection.X, intersection.Y);
                    }
                    else
                    {
                        line.Start = new Point(intersection.X, intersection.Y);
                    }
                }
            }

            return intersect;
        }

        private void ProcessAtoms(Molecule mol, Progress pb, int moleculeNo, PeriodicTable pt)
        {
            AtomLabelPositioner ar = new AtomLabelPositioner(_atomLabelCharacters, _TtfCharacterSet, pt, _telemetry);

            // Create Characters
            if (mol.AllAtoms.Count > 1)
            {
                pb.Show();
            }
            pb.Message = $"Processing Atoms in Molecule {moleculeNo}";
            pb.Value = 0;
            pb.Maximum = mol.AllAtoms.Count;

            foreach (Atom atom in mol.AllAtoms)
            {
                //Debug.WriteLine("Atom: " + atom.Id + " " + atom.ElementType);
                pb.Increment(1);
                ar.CreateCharacters(atom, _options);
            }
        }

        private void LoadFont()
        {
            string json = ResourceHelper.GetStringResource(Assembly.GetExecutingAssembly(), "Arial.json");
            _TtfCharacterSet = JsonConvert.DeserializeObject<Dictionary<char, TtfCharacter>>(json);
        }

        private A.Graphic CreateGraphic()
        {
            A.Graphic graphic1 = new A.Graphic();
            graphic1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
            return graphic1;
        }

        private A.GraphicData CreateGraphicData()
        {
            return new A.GraphicData() { Uri = "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup" };
        }

        private Wp.Inline CreateInline(A.GraphicData graphicData, Wpg.WordprocessingGroup wordprocessingGroup1)
        {
            UInt32Value inlineId = UInt32Value.FromUInt32((uint)_ooxmlId++);

            Int64Value width = OoXmlHelper.ScaleCmlToEmu(_canvasExtents.Width);
            Int64Value height = OoXmlHelper.ScaleCmlToEmu(_canvasExtents.Height);

            Wp.Inline inline1 = new Wp.Inline() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)0U, DistanceFromRight = (UInt32Value)0U };
            Wp.Extent extent1 = new Wp.Extent() { Cx = width, Cy = height };

            Wp.EffectExtent effectExtent1 = new Wp.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L };
            Wp.DocProperties docProperties1 = new Wp.DocProperties() { Id = inlineId, Name = "moleculeGroup" };

            Wpg.NonVisualGroupDrawingShapeProperties nonVisualGroupDrawingShapeProperties1 = new Wpg.NonVisualGroupDrawingShapeProperties();
            Wpg.NonVisualGraphicFrameProperties nonVisualGraphicFrameProperties = new Wpg.NonVisualGraphicFrameProperties();
            A.GraphicFrameLocks gfl = new A.GraphicFrameLocks()
            {
                NoMove = true,
                NoSelection = true
            };

            Wpg.GroupShapeProperties groupShapeProperties1 = new Wpg.GroupShapeProperties();

            A.TransformGroup transformGroup1 = new A.TransformGroup();
            A.Offset offset1 = new A.Offset() { X = 0L, Y = 0L };
            A.Extents extents1 = new A.Extents() { Cx = width, Cy = height };
            A.ChildOffset childOffset1 = new A.ChildOffset() { X = 0L, Y = 0L };
            A.ChildExtents childExtents1 = new A.ChildExtents() { Cx = width, Cy = height };

            transformGroup1.Append(offset1);
            transformGroup1.Append(extents1);
            transformGroup1.Append(childOffset1);
            transformGroup1.Append(childExtents1);

            groupShapeProperties1.Append(transformGroup1);
            wordprocessingGroup1.Append(nonVisualGroupDrawingShapeProperties1);
            wordprocessingGroup1.Append(groupShapeProperties1);

            inline1.Append(extent1);
            inline1.Append(effectExtent1);
            inline1.Append(docProperties1);
            inline1.Append(gfl);

            return inline1;
        }

        private void DrawBox(Wpg.WordprocessingGroup wordprocessingGroup1, Rect extents, string colour, int thick)
        {
            UInt32Value bondLineId = UInt32Value.FromUInt32((uint)_ooxmlId++);
            string bondLineName = "box" + bondLineId;

            Int64Value width1 = OoXmlHelper.ScaleCmlToEmu(extents.Width);
            Int64Value height1 = OoXmlHelper.ScaleCmlToEmu(extents.Height);
            Int64Value top1 = OoXmlHelper.ScaleCmlToEmu(extents.Top);
            Int64Value left1 = OoXmlHelper.ScaleCmlToEmu(extents.Left);
            Point pp1 = new Point(left1, top1);
            Size ss2 = new Size(width1, height1);
            pp1.Offset(OoXmlHelper.ScaleCmlToEmu(-_canvasExtents.Left), OoXmlHelper.ScaleCmlToEmu(-_canvasExtents.Top));
            Rect boundingBox = new Rect(pp1, ss2);

            Int64Value width = (Int64Value)boundingBox.Width;
            Int64Value height = (Int64Value)boundingBox.Height;
            Int64Value top = (Int64Value)boundingBox.Top;
            Int64Value left = (Int64Value)boundingBox.Left;

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
            A.Point point2 = new A.Point() { X = boundingBox.Width.ToString("0"), Y = "0" };
            lineTo1.Append(point2);

            // Mid Point
            A.LineTo lineTo2 = new A.LineTo();
            A.Point point3 = new A.Point() { X = boundingBox.Width.ToString("0"), Y = boundingBox.Height.ToString("0") };
            lineTo2.Append(point3);

            // Last Point
            A.LineTo lineTo3 = new A.LineTo();
            A.Point point4 = new A.Point() { X = "0", Y = boundingBox.Height.ToString("0") };
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

        private void DrawShape(Wpg.WordprocessingGroup wordprocessingGroup1, Rect extents, A.ShapeTypeValues shape, string colour)
        {
            UInt32Value bondLineId = UInt32Value.FromUInt32((uint)_ooxmlId++);
            string bondLineName = "shape" + bondLineId;

            Int64Value width1 = OoXmlHelper.ScaleCmlToEmu(extents.Width);
            Int64Value height1 = OoXmlHelper.ScaleCmlToEmu(extents.Height);
            Int64Value top1 = OoXmlHelper.ScaleCmlToEmu(extents.Top);
            Int64Value left1 = OoXmlHelper.ScaleCmlToEmu(extents.Left);
            Point pp1 = new Point(left1, top1);
            Size ss2 = new Size(width1, height1);
            pp1.Offset(OoXmlHelper.ScaleCmlToEmu(-_canvasExtents.Left), OoXmlHelper.ScaleCmlToEmu(-_canvasExtents.Top));
            Rect boundingBox = new Rect(pp1, ss2);

            Int64Value width = (Int64Value)boundingBox.Width;
            Int64Value height = (Int64Value)boundingBox.Height;
            Int64Value top = (Int64Value)boundingBox.Top;
            Int64Value left = (Int64Value)boundingBox.Left;

            A.Extents extents2 = null;
            A.PresetGeometry presetGeometry1 = null;
            extents2 = new A.Extents() { Cx = width, Cy = height };
            presetGeometry1 = new A.PresetGeometry() { Preset = shape };

            Wps.WordprocessingShape wordprocessingShape1 = new Wps.WordprocessingShape();
            Wps.NonVisualDrawingProperties nonVisualDrawingProperties1 = new Wps.NonVisualDrawingProperties()
            {
                Id = bondLineId,
                Name = bondLineName
            };

            Wps.NonVisualDrawingShapeProperties nonVisualDrawingShapeProperties1 = new Wps.NonVisualDrawingShapeProperties();
            //A.ShapeLocks shapeLocks = new A.ShapeLocks()
            //{
            //    NoMove = true,
            //    NoSelection = true
            //};
            //nonVisualDrawingShapeProperties1.Append(shapeLocks);

            Wps.ShapeProperties shapeProperties1 = new Wps.ShapeProperties();

            A.Transform2D transform2D1 = new A.Transform2D();
            A.Offset offset2 = new A.Offset() { X = left, Y = top };

            transform2D1.Append(offset2);
            transform2D1.Append(extents2);

            A.AdjustValueList adjustValueList1 = new A.AdjustValueList();

            presetGeometry1.Append(adjustValueList1);
            A.SolidFill solidFill1 = new A.SolidFill();

            A.RgbColorModelHex rgbColorModelHex1 = new A.RgbColorModelHex() { Val = colour };
            A.Alpha alpha1 = new A.Alpha() { Val = new Int32Value() { InnerText = "100%" } };
            solidFill1.Append(rgbColorModelHex1);

            shapeProperties1.Append(transform2D1);
            shapeProperties1.Append(presetGeometry1);
            shapeProperties1.Append(solidFill1);

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