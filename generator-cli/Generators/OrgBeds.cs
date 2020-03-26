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

        /// <summary>The allowed configs.</summary>
        private static List<BedConfiguration> _allowedBedConfigs = null;

        /// <summary>The bed statuses by fixed.</summary>
        private static Dictionary<string, List<BedConfiguration>> _bedStatusesByFixed = null;

        /// <summary>Initializes a new instance of the <see cref="OrgBeds"/> class.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="org">            The organization.</param>
        /// <param name="parentLocation"> The parent location.</param>
        /// <param name="initialBedCount">Number of initial bed.</param>
        public OrgBeds(
            Organization org,
            Location parentLocation,
            int initialBedCount)
        {
            if (org == null)
            {
                throw new ArgumentNullException(nameof(org));
            }

            if (parentLocation == null)
            {
                throw new ArgumentNullException(nameof(parentLocation));
            }

            OrgId = org.Id;
            _bedsByConfig = new Dictionary<BedConfiguration, List<Location>>();

            foreach (BedConfiguration config in _allowedBedConfigs)
            {
                _bedsByConfig.Add(config, new List<Location>());
            }

            // create initial set of beds
            for (int i = 0; i < initialBedCount; i++)
            {
                // pick a configuration
                BedConfiguration config = _allowedBedConfigs[_rand.Next(0, _allowedBedConfigs.Count)];

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

        /// <summary>Gets the beds by configuration.</summary>
        /// <value>The beds by configuration.</value>
        public Dictionary<BedConfiguration, List<Location>> BedsByConfig => _bedsByConfig;

        /// <summary>Updates the beds for time step described by changeFactor.</summary>
        /// <param name="changeFactor">(Optional) The change factor.</param>
        public void UpdateBedStatusForTimeStep(
            double changeFactor = 0.1)
        {
            foreach (BedConfiguration config in _bedsByConfig.Keys)
            {
                int bedCount = _bedsByConfig[config].Count;
                if (bedCount == 0)
                {
                    continue;
                }

                for (int bedIndex = bedCount - 1; bedIndex >= 0; bedIndex--)
                {
                    if (_rand.NextDouble() > changeFactor)
                    {
                        continue;
                    }

                    // pull a bed out of this set
                    Location bed = _bedsByConfig[config][bedIndex];
                    _bedsByConfig[config].RemoveAt(bedIndex);

                    // pick a configuration from the matching fixed config parts
                    int nextConfigIndex = _rand.Next(0, _bedStatusesByFixed[config.FixedKey].Count);
                    BedConfiguration nextConfig = _bedStatusesByFixed[config.FixedKey][nextConfigIndex];

                    // change this bed
                    FhirGenerator.UpdateBedConfiguration(ref bed, nextConfig);

                    // add to the destination config
                    _bedsByConfig[nextConfig].Add(bed);
                }
            }
        }

        /// <summary>Initializes this object.</summary>
        /// <param name="seed">                    The seed.</param>
        /// <param name="allowedBedConfigurations">The allowed bed configurations.</param>
        public static void Init(
            int seed,
            List<BedConfiguration> allowedBedConfigurations)
        {
            if (seed == 0)
            {
                _rand = new Random();
            }
            else
            {
                _rand = new Random(seed);
            }

            if ((allowedBedConfigurations == null) || (allowedBedConfigurations.Count == 0))
            {
                throw new ArgumentNullException(nameof(allowedBedConfigurations));
            }

            // copy main configs
            _allowedBedConfigs = allowedBedConfigurations;

            _bedStatusesByFixed = new Dictionary<string, List<BedConfiguration>>();
            foreach (BedConfiguration config in allowedBedConfigurations)
            {
                if (!_bedStatusesByFixed.ContainsKey(config.FixedKey))
                {
                    _bedStatusesByFixed.Add(config.FixedKey, new List<BedConfiguration>());
                }

                _bedStatusesByFixed[config.FixedKey].Add(config);
            }
        }

        /// <summary>Gets the beds.</summary>
        /// <param name="config">(Optional) The configuration.</param>
        /// <returns>A List&lt;Location&gt;</returns>
        public List<Location> Beds(
            BedConfiguration config = null)
        {
            List<Location> beds = new List<Location>();

            if ((config != null) && _bedsByConfig.ContainsKey(config))
            {
                return _bedsByConfig[config];
            }

            foreach (KeyValuePair<BedConfiguration, List<Location>> kvp in _bedsByConfig)
            {
                beds.AddRange(kvp.Value);
            }

            // sort beds on id for sanity
            beds.Sort((a, b) => string.Compare(a.Id, b.Id, StringComparison.Ordinal));

            return beds;
        }

        /// <summary>Bed count.</summary>
        /// <param name="config">(Optional) The configuration.</param>
        /// <returns>An int.</returns>
        public int BedCount(
            BedConfiguration config = null)
        {
            if ((config != null) && _bedsByConfig.ContainsKey(config))
            {
                return _bedsByConfig[config].Count;
            }

            int count = 0;

            foreach (KeyValuePair<BedConfiguration, List<Location>> kvp in _bedsByConfig)
            {
                count += kvp.Value.Count;
            }

            return count;
        }
    }
}
