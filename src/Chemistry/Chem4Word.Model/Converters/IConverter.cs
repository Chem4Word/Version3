﻿namespace Chem4Word.Model.Converters
{
    public interface IConverter
    {
        /// <summary>
        /// Describes the file format used
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Array of supported file patterns.  Does NOT imply that converter supports mutiple format
        /// First extension in the list is the default
        /// </summary>
        string[] Extensions { get; }

        /// <summary>
        /// Exports the model to another format
        /// </summary>
        /// <returns>CML as string</returns>
        string Export(Chem4Word.Model.Model model);

        /// <summary>
        /// Converts text to Model
        /// </summary>
        /// <returns></returns>
        Chem4Word.Model.Model Import(object data);

        bool CanImport { get; }

        bool CanExport { get; }
    }
}