// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using DocumentFormat.OpenXml;

namespace Chem4Word.Renderer.OoXmlV3.OOXML
{
    public static class OoXmlHelper
    {
        // https://startbigthinksmall.wordpress.com/2010/02/05/unit-converter-and-specification-search-for-ooxmlwordml-development/
        // http://lcorneliussen.de/raw/dashboards/ooxml/

        // Margins are in CML Points
        public const double DRAWING_MARGIN = 5; // 5 is a good value to use (Use 0 to compare with AMC diagrams)

        public const double CHARACTER_CLIPPING_MARGIN = 1.25;

        // Percentage of average (median) bond length
        public const double MULTIPLE_BOND_OFFSET_PERCENTAGE = 0.2;

        public const double SUBSCRIPT_SCALE_FACTOR = 0.6;
        public const double SUBSCRIPT_DROP_FACTOR = 0.75;
        public const double CS_SUPERSCRIPT_RAISE_FACTOR = 0.3;

        private const double EMUS_PER_CML_POINT = 9500;

        private const double EMUS_PER_CS_TTF_POINT = 9.852;
        private const double EMUS_PER_CS_TTF_POINT_SUBSCRIPT = EMUS_PER_CS_TTF_POINT * SUBSCRIPT_SCALE_FACTOR;
        private const double CS_TTF_TO_CML = EMUS_PER_CML_POINT / EMUS_PER_CS_TTF_POINT;

        /// <summary>
        /// Scales a CML X or Y co-ordinate to DrawingML Units (EMU)
        /// </summary>
        /// <param name="XorY"></param>
        /// <returns></returns>
        public static Int64Value ScaleCmlToEmu(double XorY)
        {
            double scaled = XorY * EMUS_PER_CML_POINT;
            return Int64Value.FromInt64((long)scaled);
        }

        #region C# TTF

        /// <summary>
        /// Scales a C# TTF X or Y co-ordinate to DrawingML Units (EMU)
        /// </summary>
        /// <param name="XorY"></param>
        /// <returns></returns>
        public static Int64Value ScaleCsTtfToEmu(double XorY)
        {
            double scaled = XorY * EMUS_PER_CS_TTF_POINT;
            return Int64Value.FromInt64((long)scaled);
        }

        /// <summary>
        /// Scales a CS TTF SubScript X or Y co-ordinate to DrawingML Units (EMU)
        /// </summary>
        public static Int64Value ScaleCsTtfSubScriptToEmu(double XorY)
        {
            double scaled = XorY * EMUS_PER_CS_TTF_POINT_SUBSCRIPT;
            return Int64Value.FromInt64((long)scaled);
        }

        /// <summary>
        /// Scales a C# TTF X or Y co-ordinate to CML Units
        /// </summary>
        /// <param name="XorY"></param>
        /// <returns></returns>
        public static double ScaleCsTtfToCml(double XorY)
        {
            return XorY / CS_TTF_TO_CML;
        }

        #endregion C# TTF
    }
}