// <copyright file="OrgWorkerData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Models
{
    /// <summary>An organization worker data.</summary>
    public class OrgWorkerData
    {
        private static Random _rand = new Random();

        private string _shortHCPGroupsNextWeek;
        private bool _isShortOtherHCPNextWeek;
        private bool _isShortTempNextWeek;
        private bool _isShortOtherLicensedNextWeek;
        private bool _isShortPhysiciansNextWeek;
        private bool _isShortPharmNextWeek;
        private bool _isShortRTsNextWeek;
        private bool _isShortNursesNextWeek;
        private bool _isShortEnvironmentalNextWeek;
        private string _shortHCPGroupsToday;
        private bool _isShortOtherHCPToday;
        private bool _isShortTempToday;
        private bool _isShortOtherLicensedToday;
        private bool _isShortPhysiciansToday;
        private bool _isShortPharmToday;
        private bool _isShortRTsToday;
        private bool _isShortNursesToday;
        private bool _isShortEnvironmentalToday;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrgWorkerData"/> class.
        /// </summary>
        /// <param name="isShortEnvironmentalToday">   True if this object is short environmental today,
        ///  false if not.</param>
        /// <param name="isShortNursesToday">          True if this object is short nurses today, false
        ///  if not.</param>
        /// <param name="isShortRTsToday">             True if this object is short r ts today, false if
        ///  not.</param>
        /// <param name="isShortPharmToday">           True if this object is short pharm today, false
        ///  if not.</param>
        /// <param name="isShortPhysiciansToday">      True if this object is short physicians today,
        ///  false if not.</param>
        /// <param name="isShortOtherLicensedToday">   True if this object is short other licensed today,
        ///  false if not.</param>
        /// <param name="isShortTempToday">            True if this object is short temporary today,
        ///  false if not.</param>
        /// <param name="isShortOtherHCPToday">        True if this object is short other hcp today,
        ///  false if not.</param>
        /// <param name="shortHCPGroupsToday">         The short hcp groups today.</param>
        /// <param name="isShortEnvironmentalNextWeek">True if this object is short environmental next
        ///  week, false if not.</param>
        /// <param name="isShortNursesNextWeek">       True if this object is short nurses next week,
        ///  false if not.</param>
        /// <param name="isShortRTsNextWeek">          True if this object is short r ts next week,
        ///  false if not.</param>
        /// <param name="isShortPharmNextWeek">        True if this object is short pharm next week,
        ///  false if not.</param>
        /// <param name="isShortPhysiciansNextWeek">   True if this object is short physicians next week,
        ///  false if not.</param>
        /// <param name="isShortOtherLicensedNextWeek">True if this object is short other licensed next
        ///  week, false if not.</param>
        /// <param name="isShortTempNextWeek">         True if this object is short temporary next week,
        ///  false if not.</param>
        /// <param name="isShortOtherHCPNextWeek">     True if this object is short other hcp next week,
        ///  false if not.</param>
        /// <param name="shortHCPGroupsNextWeek">      The short hcp groups next week.</param>
        public OrgWorkerData(
            bool isShortEnvironmentalToday,
            bool isShortNursesToday,
            bool isShortRTsToday,
            bool isShortPharmToday,
            bool isShortPhysiciansToday,
            bool isShortOtherLicensedToday,
            bool isShortTempToday,
            bool isShortOtherHCPToday,
            string shortHCPGroupsToday,
            bool isShortEnvironmentalNextWeek,
            bool isShortNursesNextWeek,
            bool isShortRTsNextWeek,
            bool isShortPharmNextWeek,
            bool isShortPhysiciansNextWeek,
            bool isShortOtherLicensedNextWeek,
            bool isShortTempNextWeek,
            bool isShortOtherHCPNextWeek,
            string shortHCPGroupsNextWeek)
        {
            _isShortEnvironmentalToday = isShortEnvironmentalToday;
            _isShortNursesToday = isShortNursesToday;
            _isShortRTsToday = isShortRTsToday;
            _isShortPharmToday = isShortPharmToday;
            _isShortPhysiciansToday = isShortPhysiciansToday;
            _isShortOtherLicensedToday = isShortOtherLicensedToday;
            _isShortTempToday = isShortTempToday;
            _isShortOtherHCPToday = isShortOtherHCPToday;
            _shortHCPGroupsToday = shortHCPGroupsToday;
            _isShortEnvironmentalNextWeek = isShortEnvironmentalNextWeek;
            _isShortNursesNextWeek = isShortNursesNextWeek;
            _isShortRTsNextWeek = isShortRTsNextWeek;
            _isShortPharmNextWeek = isShortPharmNextWeek;
            _isShortPhysiciansNextWeek = isShortPhysiciansNextWeek;
            _isShortOtherLicensedNextWeek = isShortOtherLicensedNextWeek;
            _isShortTempNextWeek = isShortTempNextWeek;
            _isShortOtherHCPNextWeek = isShortOtherHCPNextWeek;
            _shortHCPGroupsNextWeek = shortHCPGroupsNextWeek;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrgWorkerData"/> class.
        /// </summary>
        public OrgWorkerData()
        {
            _isShortEnvironmentalToday = _rand.NextDouble() > 0.1;
            _isShortNursesToday = _rand.NextDouble() > 0.1;
            _isShortRTsToday = _rand.NextDouble() > 0.1;
            _isShortPharmToday = _rand.NextDouble() > 0.1;
            _isShortPhysiciansToday = _rand.NextDouble() > 0.1;
            _isShortOtherLicensedToday = _rand.NextDouble() > 0.1;
            _isShortTempToday = _rand.NextDouble() > 0.1;
            _isShortOtherHCPToday = _rand.NextDouble() > 0.1;
            _shortHCPGroupsToday = IsShortOtherHCPToday ? $"Random Current Shortage: {_rand.Next(0, 100)}" : string.Empty;
            _isShortEnvironmentalNextWeek = _rand.NextDouble() > 0.3;
            _isShortNursesNextWeek = _rand.NextDouble() > 0.3;
            _isShortRTsNextWeek = _rand.NextDouble() > 0.3;
            _isShortPharmNextWeek = _rand.NextDouble() > 0.3;
            _isShortPhysiciansNextWeek = _rand.NextDouble() > 0.3;
            _isShortOtherLicensedNextWeek = _rand.NextDouble() > 0.3;
            _isShortTempNextWeek = _rand.NextDouble() > 0.3;
            _isShortOtherHCPNextWeek = _rand.NextDouble() > 0.3;
            _shortHCPGroupsNextWeek = IsShortOtherHCPNextWeek ? $"Random Future Shortage: {_rand.Next(0, 100)}" : string.Empty;
        }

        /// <summary>
        /// Gets a value indicating whether this object is short environmental today.
        /// </summary>
        /// <value>True if this object is short environmental today, false if not.</value>
        public bool IsShortEnvironmentalToday => _isShortEnvironmentalToday;

        /// <summary>Gets a value indicating whether this object is short nurses today.</summary>
        /// <value>True if this object is short nurses today, false if not.</value>
        public bool IsShortNursesToday => _isShortNursesToday;

        /// <summary>Gets a value indicating whether this object is short r ts today.</summary>
        /// <value>True if this object is short r ts today, false if not.</value>
        public bool IsShortRTsToday => _isShortRTsToday;

        /// <summary>Gets a value indicating whether this object is short pharm today.</summary>
        /// <value>True if this object is short pharm today, false if not.</value>
        public bool IsShortPharmToday => _isShortPharmToday;

        /// <summary>Gets a value indicating whether this object is short physicians today.</summary>
        /// <value>True if this object is short physicians today, false if not.</value>
        public bool IsShortPhysiciansToday => _isShortPhysiciansToday;

        /// <summary>
        /// Gets a value indicating whether this object is short other licensed today.
        /// </summary>
        /// <value>True if this object is short other licensed today, false if not.</value>
        public bool IsShortOtherLicensedToday => _isShortOtherLicensedToday;

        /// <summary>Gets a value indicating whether this object is short temporary today.</summary>
        /// <value>True if this object is short temporary today, false if not.</value>
        public bool IsShortTempToday => _isShortTempToday;

        /// <summary>Gets a value indicating whether this object is short other hcp today.</summary>
        /// <value>True if this object is short other hcp today, false if not.</value>
        public bool IsShortOtherHCPToday => _isShortOtherHCPToday;

        /// <summary>Gets the short hcp groups today.</summary>
        /// <value>The short hcp groups today.</value>
        public string ShortHCPGroupsToday => _shortHCPGroupsToday;

        /// <summary>
        /// Gets a value indicating whether this object is short environmental next week.
        /// </summary>
        /// <value>True if this object is short environmental next week, false if not.</value>
        public bool IsShortEnvironmentalNextWeek => _isShortEnvironmentalNextWeek;

        /// <summary>Gets a value indicating whether this object is short nurses next week.</summary>
        /// <value>True if this object is short nurses next week, false if not.</value>
        public bool IsShortNursesNextWeek => _isShortNursesNextWeek;

        /// <summary>Gets a value indicating whether this object is short r ts next week.</summary>
        /// <value>True if this object is short r ts next week, false if not.</value>
        public bool IsShortRTsNextWeek => _isShortRTsNextWeek;

        /// <summary>Gets a value indicating whether this object is short pharm next week.</summary>
        /// <value>True if this object is short pharm next week, false if not.</value>
        public bool IsShortPharmNextWeek => _isShortPharmNextWeek;

        /// <summary>
        /// Gets a value indicating whether this object is short physicians next week.
        /// </summary>
        /// <value>True if this object is short physicians next week, false if not.</value>
        public bool IsShortPhysiciansNextWeek => _isShortPhysiciansNextWeek;

        /// <summary>
        /// Gets a value indicating whether this object is short other licensed next week.
        /// </summary>
        /// <value>True if this object is short other licensed next week, false if not.</value>
        public bool IsShortOtherLicensedNextWeek => _isShortOtherLicensedNextWeek;

        /// <summary>
        /// Gets a value indicating whether this object is short temporary next week.
        /// </summary>
        /// <value>True if this object is short temporary next week, false if not.</value>
        public bool IsShortTempNextWeek => _isShortTempNextWeek;

        /// <summary>
        /// Gets a value indicating whether this object is short other hcp next week.
        /// </summary>
        /// <value>True if this object is short other hcp next week, false if not.</value>
        public bool IsShortOtherHCPNextWeek => _isShortOtherHCPNextWeek;

        /// <summary>Gets the short hcp groups next week.</summary>
        /// <value>The short hcp groups next week.</value>
        public string ShortHCPGroupsNextWeek => _shortHCPGroupsNextWeek;

        /// <summary>Initializes this object.</summary>
        /// <param name="seed">The seed.</param>
        public static void Init(int seed)
        {
            _rand = new Random(seed);
        }

        /// <summary>Updates this object.</summary>
        public void Update()
        {
            _isShortEnvironmentalToday = _rand.NextDouble() > 0.1 ? !_isShortEnvironmentalToday : _isShortEnvironmentalToday;
            _isShortNursesToday = _rand.NextDouble() > 0.1 ? !_isShortNursesToday : _isShortNursesToday;
            _isShortRTsToday = _rand.NextDouble() > 0.1 ? !_isShortRTsToday : _isShortRTsToday;
            _isShortPharmToday = _rand.NextDouble() > 0.1 ? !_isShortPharmToday : _isShortPharmToday;
            _isShortPhysiciansToday = _rand.NextDouble() > 0.1 ? !_isShortPhysiciansToday : _isShortPhysiciansToday;
            _isShortOtherLicensedToday = _rand.NextDouble() > 0.1 ? !_isShortOtherLicensedToday : _isShortOtherLicensedToday;
            _isShortTempToday = _rand.NextDouble() > 0.1 ? !_isShortTempToday : _isShortTempToday;
            _isShortOtherHCPToday = _rand.NextDouble() > 0.1 ? !_isShortOtherHCPToday : _isShortOtherHCPToday;

            if (_isShortOtherHCPToday)
            {
                if (string.IsNullOrEmpty(_shortHCPGroupsNextWeek))
                {
                    _shortHCPGroupsToday = $"Random Current Shortage: {_rand.Next(0, 100)}";
                }
            }
            else
            {
                _shortHCPGroupsToday = string.Empty;
            }

            _isShortEnvironmentalNextWeek = _rand.NextDouble() > 0.1 ? !_isShortEnvironmentalNextWeek : _isShortEnvironmentalNextWeek;
            _isShortNursesNextWeek = _rand.NextDouble() > 0.1 ? !_isShortNursesNextWeek : _isShortNursesNextWeek;
            _isShortRTsNextWeek = _rand.NextDouble() > 0.1 ? !_isShortRTsNextWeek : _isShortRTsNextWeek;
            _isShortPharmNextWeek = _rand.NextDouble() > 0.1 ? !_isShortPharmNextWeek : _isShortPharmNextWeek;
            _isShortPhysiciansNextWeek = _rand.NextDouble() > 0.1 ? !_isShortPhysiciansNextWeek : _isShortPhysiciansNextWeek;
            _isShortOtherLicensedNextWeek = _rand.NextDouble() > 0.1 ? !_isShortOtherLicensedNextWeek : _isShortOtherLicensedNextWeek;
            _isShortTempNextWeek = _rand.NextDouble() > 0.1 ? !_isShortTempNextWeek : _isShortTempNextWeek;
            _isShortOtherHCPNextWeek = _rand.NextDouble() > 0.1 ? !_isShortOtherHCPNextWeek : _isShortOtherHCPNextWeek;

            if (_isShortOtherHCPNextWeek)
            {
                if (string.IsNullOrEmpty(_shortHCPGroupsNextWeek))
                {
                    _shortHCPGroupsNextWeek = $"Random Future Shortage: {_rand.Next(0, 100)}";
                }
            }
            else
            {
                _shortHCPGroupsNextWeek = string.Empty;
            }
        }
    }
}
