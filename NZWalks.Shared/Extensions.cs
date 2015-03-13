﻿// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Conglomo">
// Copyright 2015 Peter Chapman. Please see LICENCE.md for licence details.
// </copyright>
// -----------------------------------------------------------------------

namespace Conglomo.NZWalks
{
    using System.Collections.Generic;
#if WINDOWS_APP
    using Bing.Maps;
#endif
    using Windows.Devices.Geolocation;

    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
#if WINDOWS_APP

        /// <summary>
        /// Converts to a location collection.
        /// </summary>
        /// <param name="pointList">The list of points.</param>
        /// <returns>The location collection.</returns>
        public static LocationCollection ToLocationCollection(this IList<BasicGeoposition> pointList)
        {
            LocationCollection locs = new LocationCollection();
            if (pointList != null)
            {
                foreach (BasicGeoposition p in pointList)
                {
                    locs.Add(p.ToLocation());
                }
            }

            return locs;
        }

        /// <summary>
        /// Converts to a geo point.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>The geo point.</returns>
        public static Geopoint ToGeopoint(this Location location)
        {
            if (location != default(Location))
            {
                return new Geopoint(new BasicGeoposition() { Latitude = location.Latitude, Longitude = location.Longitude });
            }
            else
            {
                return default(Geopoint);
            }
        }

        /// <summary>
        /// Converts to a location.
        /// </summary>
        /// <param name="location">The geo point.</param>
        /// <returns>The location.</returns>
        public static Location ToLocation(this Geopoint location)
        {
            if (location != default(Geopoint))
            {
                return new Location(location.Position.Latitude, location.Position.Longitude);
            }
            else
            {
                return new Location();
            }
        }

        /// <summary>
        /// Converts to a location.
        /// </summary>
        /// <param name="location">The geo position.</param>
        /// <returns>The location.</returns>
        public static Location ToLocation(this BasicGeoposition location)
        {
            return new Location(location.Latitude, location.Longitude);
        }

#elif WINDOWS_PHONE_APP
        // Add any required Windows Phone Extensions
#endif
    }
}
