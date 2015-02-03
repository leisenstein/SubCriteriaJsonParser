using System;
using System.Collections.Generic;

using Snap.Util;
using Snap.Geography;

namespace Snap.Search2 {
    public partial class Search {

        //----------------------------------------------------------------------
        private static IntSet<PartyId> SearchForLocations(
            Timer t,
            IEnumerable<string> nameSearchNoiseWords,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            double nonGeoMatchAbsDensity,
            out Location centroid,
            out double? maxRadiusMi) {

            centroid = null;
            maxRadiusMi = null;
            switch (searchLocale.Type) {
                case Snap.Data.Controller.SearchType.City:
                    return SearchByCity(
                       t, searchLocale, desiredResultCount, maxResultCount, nonGeoMatchAbsDensity, out centroid, out maxRadiusMi);
                case Snap.Data.Controller.SearchType.PostalCode:
                    return SearchByPostalCode(
                        t, searchLocale, desiredResultCount, maxResultCount, nonGeoMatchAbsDensity, out centroid, out maxRadiusMi);
                case Snap.Data.Controller.SearchType.StreetAddress:
                    return SearchNearStreetAddress(
                        t, searchLocale, desiredResultCount, maxResultCount, nonGeoMatchAbsDensity, out centroid, out maxRadiusMi);
                case Snap.Data.Controller.SearchType.County:
                    return SearchByCounty(
                        t, searchLocale, desiredResultCount, maxResultCount, nonGeoMatchAbsDensity, out centroid, out maxRadiusMi);
                case Snap.Data.Controller.SearchType.BoundingBox:
                    return SearchByBoundingBox(t, searchLocale, out centroid);
                case Snap.Data.Controller.SearchType.PartyName:
                    return SearchByName(t, searchLocale, nameSearchNoiseWords);
                case Snap.Data.Controller.SearchType.Membership:
                    return IntSet<PartyId>.Universe;
                case Snap.Data.Controller.SearchType.State:
                    return SearchByState(t, searchLocale);
                case Snap.Data.Controller.SearchType.Location:
                    return SearchNearLocation(
                        t, desiredResultCount, searchLocale.Location, nonGeoMatchAbsDensity, out maxRadiusMi);
                default:
                    throw new NotImplementedException();
            }
        }

        //----------------------------------------------------------------------
        private static IntSet<PartyId> SearchByBoundingBox(
            Timer t,
            AddressParserResult searchLocale,
            out Location centroid) {

            centroid = searchLocale.BoundingBox.Centroid;

            // KLUDGE: Justify this initial array size and make it a constant at
            // least or a configuration setting at best.  This is a wild guess.
            List<int> partyIds = new List<int>(1024);
            Snap.Cache.Cache.PartyLocation.Search(searchLocale.BoundingBox,
                delegate(int partyId) {
                    partyIds.Add(partyId);
                });
            return IntSet<PartyId>.CreateFromUnsorted(partyIds);
        }

        //----------------------------------------------------------------------
        private static IntSet<PartyId> SearchNearLocation(
            Timer t,
            int desiredResultCount,
            Location centroid,
            double nonGeoMatchAbsDensity,
            out double? maxRadiusMi) {

            double searchRadiusMi = InitialGeoSearchRadiusMi;
            maxRadiusMi = searchRadiusMi;

            // KLUDGE: Scale the desired result count up by the non-geographic
            // match absolute density in the database.  In doing so, we attempt
            // to return enough results that we'll get close to the desired 
            // result count in the end.
            desiredResultCount = (int)(desiredResultCount / nonGeoMatchAbsDensity);

            // Allocate an initial list large enough that we'll (hopefully) 
            // avoid re-allocating space as we iterate.
            List<int> partyIds = new List<int>(2 * desiredResultCount);
            Tree2D<int> tree2d = Snap.Cache.Cache.PartyLocation;

            int iterationCount = 0;
            Timer tree2dTimer = new Timer(t, String.Format(
                "Expand MBR to fit {0} Parties", desiredResultCount));
            while ((partyIds.Count < desiredResultCount) &&
               (searchRadiusMi < MaxGeoSearchRadiusMi)) {

                iterationCount++;
                maxRadiusMi = searchRadiusMi;
                partyIds.Clear();
                tree2d.Search(
                    new BoundingBox(centroid, searchRadiusMi),
                    (partyId) => partyIds.Add(partyId));
                searchRadiusMi *= GeoSearchRadiusScalar;
            }
            tree2dTimer.Stop(String.Format("in {0} iterations", iterationCount));

            return IntSet<PartyId>.CreateFromUnsorted(partyIds);
        }

        //----------------------------------------------------------------------
        private static IntSet<PartyId> SearchNearStreetAddress(
            Timer t,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            double nonGeoMatchAbsDensity,
            out Location centroid,
            out double? maxRadiusMi) {

            if (null == searchLocale.Location) {
                IGeoCoder geocoder = GeoCoderFactory.CreateGeoCoder();
                Location[] ls = geocoder.GeoCode(
                    searchLocale.StreetAddress,
                    searchLocale.City,
                    searchLocale.StateCode,
                    searchLocale.PostalCode);

                if (ls.Length == 0) {
                    throw new ArgumentException("unknown address");
                }

                if (ls.Length > 1) {
                    throw new ArgumentException("ambiguous address");
                }

                centroid = ls[0];
            } else {
                centroid = searchLocale.Location;
            }

            maxRadiusMi = null;
            return SearchNearLocation(
                t,
                desiredResultCount,
                centroid,
                nonGeoMatchAbsDensity,
                out maxRadiusMi);
        }

        //----------------------------------------------------------------------
        private static IntSet<PartyId> SearchByPostalCode(
            Timer t,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            double nonGeoMatchAbsDensity,
            out Location centroid,
            out double? maxRadiusMi) {
            maxRadiusMi = null;

            if (desiredResultCount == ExactMatchResultCount) {
                IntSet<PartyId> partiesInPostalCode = null;
                t.Measure("Search by PostalCode", delegate() {
                    partiesInPostalCode = Snap.Cache.Cache.PostalCode(searchLocale.PostalCode);
                });
                centroid = null;
                return partiesInPostalCode;
            } else {
                IGeoCoder geocoder = GeoCoderFactory.CreateGeoCoder();
                centroid = geocoder.GeoCodeZip(searchLocale.PostalCode);
                return SearchNearLocation(
                    t,
                    desiredResultCount,
                    centroid,
                    nonGeoMatchAbsDensity,
                    out maxRadiusMi);
            }
        }

        //----------------------------------------------------------------------
        private static IntSet<PartyId> SearchByCounty(
            Timer t,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            double nonGeoMatchAbsDensity,
            out Location centroid,
            out double? maxRadiusMi) {
            maxRadiusMi = null;

            if (desiredResultCount == ExactMatchResultCount) {
                IntSet<PartyId> partiesInCounty = null;
                t.Measure("Search by County", delegate() {
                    partiesInCounty = Snap.Cache.Cache.County(searchLocale);
                });
                centroid = null;
                return partiesInCounty;
            } else {
                IGeoCoder geocoder = GeoCoderFactory.CreateGeoCoder();
                centroid = geocoder.GeoCodeCounty(
                    searchLocale.County,
                    searchLocale.StateCode);
                return SearchNearLocation(
                    t,
                    desiredResultCount,
                    centroid,
                    nonGeoMatchAbsDensity,
                    out maxRadiusMi);
            }
        }

        //----------------------------------------------------------------------
        private static IntSet<PartyId> SearchByCity(
            Timer t,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            double nonGeoMatchAbsDensity,
            out Location centroid,
            out double? maxRadiusMi) {
            maxRadiusMi = null;

            if (desiredResultCount == ExactMatchResultCount) {
                IntSet<PartyId> partiesInCity = null;
                t.Measure("Search by City", delegate() {
                    partiesInCity = Snap.Cache.Cache.City(searchLocale);
                });
                centroid = null;
                return partiesInCity;
            } else {
                IGeoCoder geocoder = GeoCoderFactory.CreateGeoCoder();
                centroid = geocoder.GeoCode(
                    searchLocale.City,
                    searchLocale.StateCode);
                return SearchNearLocation(
                    t,
                    desiredResultCount,
                    centroid,
                    nonGeoMatchAbsDensity,
                    out maxRadiusMi);
            }
        }

        //----------------------------------------------------------------------
        private static IntSet<PartyId> SearchByState(
            Timer t,
            AddressParserResult searchLocale) {

            IntSet<PartyId> partiesInState = null;
            t.Measure("Search by State", delegate() {
                partiesInState = Snap.Cache.Cache.State(searchLocale.StateCode);
            });
            return partiesInState;
        }

        //-----------------------------------------------------------------------
        private static IntSet<PartyId> SearchForServiceArea(
            Timer t,
            AddressParserResult searchLocale) {

            return RegionSearchFactory
                .Create()
                .Search(t, searchLocale);
        }
    }
}
