﻿// ---------------------------------------------------------------------------
//  Copyright (c) 2021, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Windows;

namespace Chem4Word.Core.Helpers
{
    public static class PointHelper
    {
        public static bool PointIsEmpty(Point point)
        {
            return point.Equals(new Point(0, 0));
        }
    }
}