// <copyright file="FhirIds.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Hl7.Fhir.Model;
using static covidReportTransformationLib.Utils.CommonLiterals;

namespace covidReportTransformationLib.Utils
{
    /// <summary>A fhir identifier.</summary>
    public abstract class FhirIds
    {
        /// <summary>The random.</summary>
        private static Random _rand = new Random();

        /// <summary>The identifier.</summary>
        private static long _id = 1;

        /// <summary>Gets the identifier of the next.</summary>
        /// <value>The identifier of the next.</value>
        public static string NextId
        {
            get
            {
                return $"{FhirIdPrefix}{Interlocked.Increment(ref _id)}";
            }
        }

        /// <summary>Gets the identifier of the next organisation.</summary>
        /// <value>The identifier of the next organisation.</value>
        public static string NextOrgId
        {
            get
            {
                return $"{OrgPrefix}{Interlocked.Increment(ref _id)}";
            }
        }

        /// <summary>Identifier for identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A List&lt;Hl7.Fhir.Model.Identifier&gt;.</returns>
        public static Hl7.Fhir.Model.Identifier IdentifierForId(string id)
        {
            return new Hl7.Fhir.Model.Identifier(
                    FhirSystems.Internal,
                    id);
        }

        /// <summary>Identifier list for identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A List&lt;Hl7.Fhir.Model.Identifier&gt;.</returns>
        public static List<Hl7.Fhir.Model.Identifier> IdentifierListForId(string id)
        {
            return new List<Identifier>()
            {
                new Hl7.Fhir.Model.Identifier(
                        FhirSystems.Internal,
                        id),
            };
        }

        /// <summary>Concept for organization type.</summary>
        /// <returns>A List&lt;Hl7.Fhir.Model.CodeableConcept&gt;.</returns>
        public static List<Hl7.Fhir.Model.CodeableConcept> ConceptListForOrganizationType() =>
            new List<Hl7.Fhir.Model.CodeableConcept>()
            {
                new Hl7.Fhir.Model.CodeableConcept(
                    FhirSystems.OrganizationType,
                    OrganizationTypeProvider),
            };

        /// <summary>Identifier for organization root location.</summary>
        /// <param name="orgId">Identifier for the organization.</param>
        /// <returns>A string.</returns>
        private static string IdForOrgRootLocation(string orgId)
        {
            if (string.IsNullOrEmpty(orgId))
            {
                return NextId;
            }

            return orgId.Replace(
                OrgPrefix,
                $"{RootLocationPrefix}{OrgPrefix}",
                StringComparison.Ordinal);
        }
    }
}
