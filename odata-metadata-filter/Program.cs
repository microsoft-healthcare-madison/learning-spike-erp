// <copyright file="Program.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace odata_metadata_filter
{
    /// <summary>A program.</summary>
    public static class Program
    {
        /// <summary>Main entry-point for this application.</summary>
        /// <param name="directory">          Directory to use.</param>
        /// <param name="sourceFilename">     Source file to read.</param>
        /// <param name="destinationFilename">Destination file to write.</param>
        public static void Main(
            string directory,
            string sourceFilename = "microsoftECR.xml",
            string destinationFilename = "msft_ECR.xml")
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            string dir;

            if (Path.IsPathRooted(directory))
            {
                dir = directory;
            }
            else
            {
                dir = Path.Combine(Directory.GetCurrentDirectory(), directory);
            }

            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException($"Could not find xml directory: {dir}");
            }

            string sourceFile = Path.Combine(dir, sourceFilename);

            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException($"Could not find source file: {sourceFile}");
            }

            string destinationFile = Path.Combine(dir, destinationFilename);

            if (File.Exists(destinationFile))
            {
                File.Delete(destinationFile);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(sourceFile);

            XmlNodeList entities;
            XmlNode root = xmlDoc.DocumentElement;

            XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            nsManager.AddNamespace("edmx", "http://docs.oasis-open.org/odata/ns/edmx");
            nsManager.PushScope();
            nsManager.AddNamespace("mscrm", "http://docs.oasis-open.org/odata/ns/edm");
            nsManager.PushScope();

            entities = root.SelectNodes("//mscrm:EntityType", nsManager);

            HashSet<string> requiredTypes = new HashSet<string>();
            Dictionary<string, XmlNode> entityDict = new Dictionary<string, XmlNode>();

            foreach (XmlNode entity in entities)
            {
                string name = (entity.Attributes["Name"] == null) ? string.Empty : entity.Attributes["Name"].Value;

                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                entityDict.Add(name, entity);
            }

            AddRequired(ref requiredTypes, entityDict, "msft_covid");
            AddRequired(ref requiredTypes, entityDict, "msft_department");
            AddRequired(ref requiredTypes, entityDict, "msft_equipmentneeded");
            AddRequired(ref requiredTypes, entityDict, "msft_facility");
            AddRequired(ref requiredTypes, entityDict, "msft_location");
            AddRequired(ref requiredTypes, entityDict, "msft_region");
            AddRequired(ref requiredTypes, entityDict, "msft_supply");

            //foreach (KeyValuePair<string, XmlNode> kvp in entityDict)
            //{
            //    if (kvp.Key.StartsWith("msft_", StringComparison.Ordinal))
            //    {
            //        AddRequired(ref requiredTypes, entityDict, kvp.Key);
            //    }
            //}

            foreach (KeyValuePair<string, XmlNode> kvp in entityDict)
            {
                if (!requiredTypes.Contains(kvp.Key))
                {
                    kvp.Value.ParentNode.RemoveChild(kvp.Value);
                }
            }

            using (FileStream writeStream = new FileStream(destinationFile, FileMode.Create))
            using (XmlWriter writer = XmlWriter.Create(writeStream, null))
            {
                xmlDoc.WriteTo(writer);
            }
        }

        /// <summary>Adds a required to 'entity'.</summary>
        /// <param name="required">  [in,out] The required.</param>
        /// <param name="entityDict">The entity dictionary.</param>
        /// <param name="name">      The entity we are adding.</param>
        private static void AddRequired(
            ref HashSet<string> required,
            Dictionary<string, XmlNode> entityDict,
            string name)
        {
            if (!entityDict.ContainsKey(name))
            {
                return;
            }

            if (required.Contains(name))
            {
                return;
            }

            // add this type
            required.Add(name);

            XmlNode entity = entityDict[name];

            string baseType = (entity.Attributes["BaseType"] == null) ? string.Empty : entity.Attributes["BaseType"].Value;

            if (!string.IsNullOrEmpty(baseType))
            {
                if (baseType.StartsWith("mscrm.", StringComparison.Ordinal))
                {
                    // remove the prefix and add to required types
                    baseType = baseType.Substring(6);
                }

                if (!required.Contains(baseType))
                {
                    AddRequired(ref required, entityDict, baseType);
                }
            }

            if (!entity.HasChildNodes)
            {
                return;
            }

            string typeName;
            foreach (XmlNode child in entity.ChildNodes)
            {
                if ((child.Name == "Property") || (child.Name == "NavigationProperty"))
                {
                    typeName = (child.Attributes["Type"] == null) ? string.Empty : child.Attributes["Type"].Value;

                    if (string.IsNullOrEmpty(typeName))
                    {
                        continue;
                    }

                    if (typeName.StartsWith("Collection", StringComparison.Ordinal))
                    {
                        typeName = typeName.Substring(11, typeName.Length - 12);
                    }

                    // check for base types
                    if (typeName.StartsWith("Edm.", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (typeName.StartsWith("mscrm.", StringComparison.Ordinal))
                    {
                        // remove the prefix and add to required types
                        typeName = typeName.Substring(6);
                        if (!required.Contains(typeName))
                        {
                            AddRequired(ref required, entityDict, typeName);
                        }
                    }
                }
            }
        }
    }
}
