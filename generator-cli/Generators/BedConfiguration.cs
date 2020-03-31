// <copyright file="BedConfiguration.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static generator_cli.Generators.CommonLiterals;

namespace generator_cli.Generators
{
    /// <summary>A bed state.</summary>
    public class BedConfiguration
    {
        private readonly string _availability;
        private readonly string _type;
        private readonly string _status;
        private readonly string _feature;

        /// <summary>
        /// Initializes a new instance of the <see cref="BedConfiguration"/> class.
        /// </summary>
        /// <param name="availability">The availability.</param>
        /// <param name="type">        The type.</param>
        /// <param name="status">      The status.</param>
        /// <param name="feature">     The feature.</param>
        public BedConfiguration(
            string availability,
            string type,
            string status,
            string feature)
        {
            _availability = availability;
            _type = type;
            _status = status;
            _feature = feature;
        }

        /// <summary>Gets the availability.</summary>
        /// <value>The availability.</value>
        public string Availability => _availability;

        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        public string Type => _type;

        /// <summary>Gets the status.</summary>
        /// <value>The status.</value>
        public string Status => _status;

        /// <summary>Gets the feature.</summary>
        /// <value>The feature.</value>
        public string Feature => _feature;

        /// <summary>Gets the fixed key.</summary>
        /// <value>The fixed key.</value>
        public string FixedKey => $"{_type}-{_feature}";

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{_availability}-{_type}-{_status}-{_feature}";
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return $"{_availability}-{_type}-{_status}-{_feature}".GetHashCode(StringComparison.Ordinal);
        }

        /// <summary>States for parameters.</summary>
        /// <param name="availabilities">     The availabilities.</param>
        /// <param name="bedTypes">           List of types of the bed.</param>
        /// <param name="operationalStatuses">The operational statuses.</param>
        /// <param name="features">           The features.</param>
        /// <returns>A List&lt;BedInformation&gt;.</returns>
        public static List<BedConfiguration> StatesForParams(
            string availabilities,
            string bedTypes,
            string operationalStatuses,
            string features)
        {
            List<BedConfiguration> states = new List<BedConfiguration>();

            // parse various strings into lists we can work with
            ItemsInSet(
                out List<string> availabilityList,
                AvailabilityStatuses,
                availabilities);

            ItemsInSet(
                out List<string> typeList,
                BedTypes,
                bedTypes);

            ItemsInSet(
                out List<string> statusList,
                OperationalStatuses,
                operationalStatuses);

            ItemsInSet(
                out List<string> featureList,
                BedFeatures,
                features);

            // nested traverse each list to build the expansion
            foreach (string bedType in typeList)
            {
                foreach (string feature in featureList)
                {
                    foreach (string availability in availabilityList)
                    {
                        switch (availability)
                        {
                            case AvailabilityStatusActive:
                                foreach (string status in statusList)
                                {
                                    states.Add(new BedConfiguration(
                                        availability,
                                        bedType,
                                        status,
                                        feature));
                                }

                                break;

                            case AvailabilityStatusInactive:
                            case AvailabilityStatusSuspended:
                                states.Add(new BedConfiguration(
                                    availability,
                                    bedType,
                                    OperationalStatusClosed,
                                    feature));

                                break;
                        }
                    }
                }
            }

            return states;
        }

        /// <summary>Adds the items in set.</summary>
        /// <param name="list">     [in,out] The list.</param>
        /// <param name="set">      The set.</param>
        /// <param name="possibles">The possibles.</param>
        private static void ItemsInSet(
            out List<string> list,
            string[] set,
            string possibles)
        {
            list = new List<string>();

            if (string.IsNullOrEmpty(possibles))
            {
                list.AddRange(set);
                return;
            }

            string[] split = possibles.Split('|');

            foreach (string item in split)
            {
                // note: this is searching the array every time, but the lists are small and this op is infrequent
                if (set.Contains(item))
                {
                    list.Add(item);
                }
            }
        }
    }
}
