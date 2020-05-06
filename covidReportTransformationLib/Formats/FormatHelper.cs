// <copyright file="FormatHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>A format helper.</summary>
    public abstract class FormatHelper
    {
        private static Dictionary<string, IReportingFormat> _formatsByName = new Dictionary<string, IReportingFormat>();

        /// <summary>True to needs initialize.</summary>
        private static bool _needsInit = true;

        /// <summary>Initializes this object.</summary>
        private static void Init()
        {
            if (_needsInit)
            {
                IEnumerable<Type> localTypes = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.GetInterfaces().Contains(typeof(IReportingFormat)));

                foreach (Type localType in localTypes)
                {
                    IReportingFormat format = (IReportingFormat)localType.GetProperty("Current").GetValue(null);
                    _formatsByName.Add(format.Name, format);
                }

                _needsInit = false;
            }
        }

        /// <summary>Gets format list.</summary>
        /// <returns>The format list.</returns>
        public static List<IReportingFormat> GetFormatList()
        {
            if (_needsInit)
            {
                Init();
            }

            return _formatsByName.Values.ToList<IReportingFormat>();
        }
    }
}
