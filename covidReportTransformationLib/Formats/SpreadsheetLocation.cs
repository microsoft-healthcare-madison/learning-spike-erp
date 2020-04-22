// <copyright file="SpreadsheetLocation.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>A spreadsheet location.</summary>
    public class SpreadsheetLocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetLocation"/>
        /// class.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="row">   The row.</param>
        public SpreadsheetLocation(string column, int row)
        {
            Column = column;
            Row = row;
        }

        /// <summary>Gets the column.</summary>
        /// <value>The column.</value>
        public string Column { get; }

        /// <summary>Gets the row.</summary>
        /// <value>The row.</value>
        public int Row { get; }
    }
}
