// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chem4Word.Controls.TagControl
{
    public class TagControlModel : DependencyObject
    {
        // Constants
        private const int ControlHeight = 25;

        /// <summary>
        /// Create a TagControl with no existing tags
        /// </summary>
        public TagControlModel()
        {
            Tags = new ObservableCollection<object>();
            KnownTags = new ObservableCollection<string>();
            // Add the textBox
            CreateTextBox();
        }

        /// <summary>
        /// Create a tag collection with a collection of strings
        /// </summary>
        /// <param name="collectionOfTags"></param>
        public TagControlModel(ObservableCollection<string> collectionOfTags)
        {
            Tags = new ObservableCollection<object>();
            KnownTags = new ObservableCollection<string>();
            foreach (var tag in collectionOfTags)
            {
                AddTag(tag);
            }
            // Now add the textBox
            CreateTextBox();
        }

        public TagControlModel(ObservableCollection<string> collectionOfKnownTags, ObservableCollection<string> collectionOfTags)
        {
            Tags = new ObservableCollection<object>();
            KnownTags = collectionOfKnownTags;
            foreach (var tag in collectionOfTags)
            {
                AddTag(tag);
            }

            // Now add the textBox
            CreateTextBox();
        }

        public ObservableCollection<object> Tags { get; set; }
        public ObservableCollection<string> KnownTags { get; set; }

        #region Tag Management

        public void AddTag(string newTagText)
        {
            // ToDo:  Check to see if the tag exists

            // Create a TagControlItem
            TagControlItem tagControlItem = new TagControlItem();
            tagControlItem.Label = newTagText;

            // Add the remove event to this tag
            tagControlItem.MouseUp += TagControlItem_OnMouseUp;

            // Add the tag to the collection
            // in DataBinding, it is possible that there may be no tags
            if (Tags.Count == 0)
            {
                Tags.Insert(Tags.Count, tagControlItem);
            }
            else
            {
                Tags.Insert(Tags.Count - 1, tagControlItem);
            }

            // Add to the collection of known tags
            KnownTags.Add(newTagText);
        }

        /// <summary>
        /// Add multiple tags from a collection of strings
        /// </summary>
        /// <param name="collectionOfTags"></param>
        public void AddTags(ObservableCollection<string> collectionOfTags)
        {
            // Hope the collection isn't empty
            foreach (var tag in collectionOfTags)
            {
                AddTag(tag);
            }
        }

        public void AddKnownTags(ObservableCollection<string> collectionOfTags)
        {
            foreach (var tag in collectionOfTags)
            {
                KnownTags.Add(tag);
            }
        }

        private void CreateTextBox()
        {
            /*
            // New textbox
            var textBoxTab = new TextBox();
            // At least 70 px wide
            textBoxTab.MinWidth = 70;
            // Same height as other controls in this ItemsControl
            textBoxTab.Height = ControlHeight;
            // Auto width
            textBoxTab.Width = double.NaN;
            // Align text left
            textBoxTab.TextAlignment = TextAlignment.Left;
            // Center the text, just like label control
            textBoxTab.VerticalContentAlignment = VerticalAlignment.Center;
            // Make some space around the control
            textBoxTab.Margin = new Thickness(2.0);
            // Add KeyUp event handler
            textBoxTab.KeyUp += TextBoxTab_KeyUp;

            // Add it to the collection at the end
            Tags.Add(textBoxTab);
            */
            CreateAutoCompleteBox();
        }

        private void CreateAutoCompleteBox()
        {
            var autoCompleteBox = new AutoCompleteBox();
            autoCompleteBox.ItemsSource = KnownTags;
            autoCompleteBox.MinWidth = 70;
            autoCompleteBox.Height = ControlHeight;
            autoCompleteBox.Width = double.NaN;
            autoCompleteBox.VerticalAlignment = VerticalAlignment.Center;
            autoCompleteBox.Margin = new Thickness(2.0);
            autoCompleteBox.KeyUp += TextBoxTab_KeyUp;
            Tags.Add(autoCompleteBox);
        }

        #endregion Tag Management

        #region Event handlers

        private void TextBoxTab_KeyUp(object sender, KeyEventArgs e)
        {
            // working with a TextBox
            //var tagInputBox = sender as TextBox;

            // Working with autocompletebox
            var tagInputBox = sender as AutoCompleteBox;

            // Make sure that we don't have an empty text box, or just a single character
            if (tagInputBox == null || tagInputBox.Text.Length <= 1) return;

            // Grab the last character of the textbox
            var lastChar = tagInputBox.Text[tagInputBox.Text.Length - 1];

            // Any punctuation mark is acceptable
            if (!char.IsPunctuation(lastChar)) return;

            // Add a tag using the content of the textbox
            AddTag(tagInputBox.Text.Substring(0, tagInputBox.Text.Length - 1));

            //Clear the text from the textbox
            tagInputBox.Text = "";
        }

        private void TagControlItem_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            // We're woring with a label here
            var thisTagControlItem = sender as TagControlItem;
            var point = e.GetPosition(thisTagControlItem);
            // Remove this label from the collection
            Tags.Remove(thisTagControlItem);
            thisTagControlItem = null;
        }

        #endregion Event handlers
    }
}