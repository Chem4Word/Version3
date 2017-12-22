// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Windows.Controls;

namespace Chem4Word.Controls.TagControl
{
    /// <summary>
    /// Interaction logic for TagControlItem.xaml
    /// </summary>
    public partial class TagControlItem : UserControl
    {
        public TagControlItem()
        {
            InitializeComponent();
        }

        public string Label
        {
            get { return TagItemLabel.Content.ToString(); }
            set { TagItemLabel.Content = value; }
        }
    }
}