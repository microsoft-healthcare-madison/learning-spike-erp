// <copyright file="OrgBeds.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;

namespace generator_cli.Generators
{
    /// <summary>An organisation beds.</summary>
    public class OrgBeds
    {
        /// <summary>The random.</summary>
        private static Random _rand = null;

        /// <summary>The beds by configuration.</summary>
        private Dictionary<BedConfiguration, List<Location>> _bedsByConfig;

        /// <summary>Initializes a new instance of the <see cref="OrgBeds"/> class.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="org">                     The organization.</param>
        /// <param name="parentLocation">          The parent location.</param>
        /// <param name="initialBedCount">         Number of initial bed.</param>
        /// <param name="allowedBedConfigurations">The allowed bed configurations.</param>
        public OrgBeds(
            Organization org,
            Location parentLocation,
            int initialBedCount,
            List<BedConfiguration> allowedBedConfigurations)
        {
            if (org == null)
            {
                throw new ArgumentNullException(nameof(org));
            }

            if (parentLocation == null)
            {
                throw new ArgumentNullException(nameof(parentLocation));
            }

            if ((allowedBedConfigurations == null) || (allowedBedConfigurations.Count == 0))
            {
                throw new ArgumentNullException(nameof(allowedBedConfigurations));
            }

            OrgId = org.Id;
            _bedsByConfig = new Dictionary<BedConfiguration, List<Location>>();

            foreach (BedConfiguration config in allowedBedConfigurations)
            {
                _bedsByConfig.Add(config, new List<Location>());
            }

            // create initial set of beds
            for (int i = 0; i < initialBedCount; i++)
            {
                // pick a configuration
                BedConfiguration config = allowedBedConfigurations[_rand.Next(0, allowedBedConfigurations.Count)];

                _bedsByConfig[config].Add(
                    FhirGenerator.GenerateBed(
                        config.Availability,
                        config.Status,
                        new List<string>() { config.Type },
                        org,
                        parentLocation));
            }
        }

        /// <summary>Gets the identifier of the organization.</summary>
        /// <value>The identifier of the organization.</value>
        public string OrgId { get; }

        /// <summary>Initializes this object.</summary>
        /// <param name="seed">(Optional) The seed.</param>
        public static void Init(int seed = 0)
        {
            if (seed == 0)
            {
                _rand = new Random();
            }
            else
            {
                _rand = new Random(seed);
            }
        }

        /// <summary>Gets the beds.</summary>
        /// <returns>A List&lt;Location&gt;</returns>
        public List<Location> Beds()
        {
            List<Location> beds = new List<Location>();

            foreach (KeyValuePair<BedConfiguration, List<Location>> kvp in _bedsByConfig)
            {
                beds.AddRange(kvp.Value);
            }

            return beds;
        }
    }
}
