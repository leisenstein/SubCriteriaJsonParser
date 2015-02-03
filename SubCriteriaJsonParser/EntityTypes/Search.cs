using System;
using System.Collections.Generic;
using Snap.Geography;
using Snap.Util;

namespace Snap.Search2 {
    public partial class Search {
        /// <summary>
        /// Supply this value as the Search() method maxResultCount parameter
        /// to specify that an unlimited number of results is to be returned.
        /// That is, no explicit maximum exists.
        /// </summary>
        public const int NoMaxSearchResultCount = Int32.MaxValue;

        /// <summary>
        /// Supply this value as the Search() method desiredResultCount
        /// parameter to specify that only those facilities which exactly
        /// match the search criteria should be returned.
        /// </summary>
        public const int ExactMatchResultCount = 0;

        /// <summary>
        /// The radius we'll start with for "SearchNear..." searches.  If we 
        /// don't find enough results closer than this distance, we'll expand
        /// the search radius progressively.
        /// </summary>
        private const double InitialGeoSearchRadiusMi = 2.0;

        /// <summary>
        /// The factor we'll use to expand "SearchNear..." searches when the
        /// current radius doesn't produce enough results.  Our intent is to
        /// roughly double the searched area every time.
        /// </summary>
        private const double GeoSearchRadiusScalar = 1.41;

        /// <summary>
        /// The maximum allowed search radius.  Regardless of how many results
        /// we have found, we'll never use a BoundingBox radius greater than
        /// this value.  This is really an arbitrary value; it could probably
        /// be much smaller and never affect normal usage.
        /// </summary>
        private static readonly double MaxGeoSearchRadiusMi = 1000.0;

        //----------------------------------------------------------------------
        /// <summary>
        /// This isn't exactly a search, because the exact Party IDs of the 
        /// desired results, in the order they're expected, are provided.  This
        /// is primarily useful for Search Engine Marketing (SEM) landing pages
        /// where we want to highlight an exlicit set of Parties.
        /// </summary>
        public static ResultSet Do(ITimer timerContext, List<int> partyIds) {
            Timer t = new Timer(timerContext, "Search.Do() - explicit Party ID list");
            try {
                Comparison<Result> explicitOrdering =
                     delegate(Result lhs, Result rhs) {
                         return partyIds
                             .IndexOf(lhs.PartyId)
                             .CompareTo(partyIds.IndexOf(rhs.PartyId));
                     };


                // The comparison delegate above depends on the specific 
                // ordering of 'partyIds'.  Make a copy of that list to pass to
                // our IntSet constructor, since it will "own" the list we give
                // it.  We can't have it altering the list order we use for 
                // sorting!
                List<int> partyIdsCopy = new List<int>(partyIds);
                ResultSet results = ResultSet.From(
                    null, // search origin
                    null, // max radius mi.
                    IntSet<PartyId>.CreateFromUnsorted(partyIdsCopy));

                return FinalizeResultSet(
                    t,
                    explicitOrdering,
                    ExactMatchResultCount,
                    results,
                    null);
            }
            finally {
                t.Stop();
            }
        }

        //----------------------------------------------------------------------
        public static ResultSet Do<ProfileType>(Params ps)
            where ProfileType : ISearch, new() {

            return Do<ProfileType>(
                ps.timerContext,
                ps.nameSearchNoiseWords,
                ps.consumerProperties,
                ps.sortOrder,
                ps.searchLocale,
                ps.desiredResultCount,
                ps.maxResultCount,
                ps.externalPartyMembershipTypeIds,
                ps.profileSpecificParties,
                ps.profileSpecificiProfiles,
                SearchByMembership,
                SearchByParty<ProfileType>,
                SearchByProfile<ProfileType>,
                ps.featuredProfileTemplateLabel);
        }

        //----------------------------------------------------------------------
        // We'd like to make this method obsolete ASAP, but it'll require some
        // refactoring and rework in dependent code. - ESV 2011-12-22
        //[Obsolete("Prefer Do<ProfileType>(Params)")]
        public static ResultSet Do<ProfileType>(
            ITimer timerContext,
            IEnumerable<string> nameSearchNoiseWords,
            Dictionary<string, string> consumerProperties,
            Comparison<Result> sortOrder,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            List<int> externalPartyMembershipTypeIds,
            IntSet<PartyId> profileSpecificParties,
            IntSet<ProfileId> profileSpecificiProfiles)
            where ProfileType : ISearch, new() {

            return Do<ProfileType>(
                timerContext,
                nameSearchNoiseWords,
                consumerProperties,
                sortOrder,
                searchLocale,
                desiredResultCount,
                maxResultCount,
                externalPartyMembershipTypeIds,
                profileSpecificParties,
                profileSpecificiProfiles,
                SearchByMembership,
                SearchByParty<ProfileType>,
                SearchByProfile<ProfileType>,
                null); // featuredProfileTemplateLabel
        }

        //----------------------------------------------------------------------
        /// <summary>
        /// Search the SNAP system for matches to the provided criteria.
        /// </summary>
        public static ResultSet Do<ProfileType>(
            ITimer timerContext,
            IEnumerable<string> nameSearchNoiseWords,
            Dictionary<string, string> consumerProperties,
            Comparison<Result> sortOrder,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            List<int> externalPartyMembershipTypeIds,
            IntSet<PartyId> profileSpecificParties,
            IntSet<ProfileId> profileSpecificiProfiles,
            MembershipSearch membershipSearch,
            PartySearch partySearch,
            ProfileSearch<ProfileType> profileSearch,
            string featuredProfileTemplateLabel)
            where ProfileType : ISearch, new() {

            Timer t = new Timer(timerContext, "Search.Do() - implicit");
            try {
                return InnerSearch<ProfileType>(t,
                    nameSearchNoiseWords,
                    consumerProperties,
                    sortOrder,
                    searchLocale,
                    desiredResultCount,
                    maxResultCount,
                    externalPartyMembershipTypeIds,
                    profileSpecificParties,
                    profileSpecificiProfiles,
                    membershipSearch,
                    partySearch,
                    profileSearch,
                    featuredProfileTemplateLabel);
            }
            finally {
                t.Stop();
            }
        }

        //----------------------------------------------------------------------
        private static ResultSet InnerSearch<ProfileType>(
            Timer t,
            IEnumerable<string> nameSearchNoiseWords,
            Dictionary<string, string> consumerProperties,
            Comparison<Result> sortOrder,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            List<int> externalPartyMembershipTypeIds,
            IntSet<PartyId> profileSpecificParties,
            IntSet<ProfileId> profileSpecificProfiles,
            MembershipSearch membershipSearch,
            PartySearch partySearch,
            ProfileSearch<ProfileType> profileSearch,
            string featuredProfileTemplateLabel)
            where ProfileType : ISearch, new() {

            if (null == profileSpecificProfiles) {
                profileSpecificProfiles = IntSet<ProfileId>.Universe;
            }

            if (null == profileSpecificParties) {
                profileSpecificParties = IntSet<PartyId>.Universe;
            }

            //Console.WriteLine(Snap.Cache.Cache.ProfileTemplate.Count);
            ISearch profileType = new ProfileType();

            IntSet<PartyId> matchesByProfileType = null;
            try {
                if (String.IsNullOrEmpty(profileType.ProfileTemplateLabel)) {
                    matchesByProfileType = IntSet<PartyId>.Universe;
                } else {
                    matchesByProfileType =
                       Snap.Cache.Cache.ProfileTemplate[profileType.ProfileTemplateLabel];
                }
            }
            catch (KeyNotFoundException exc) {
                throw new ApplicationException(
                    "Unknown ProfileTemplateLabel: " + profileType.ProfileTemplateLabel,
                    exc);
            }

            IntSet<PartyId> matchesByMembership = IntSet<PartyId>.Universe;
            t.Measure("Search by membership(s)", delegate() {
                if (!ListX.IsEmpty(externalPartyMembershipTypeIds)) {
                    matchesByMembership = membershipSearch(externalPartyMembershipTypeIds);
                }
            });

            matchesByMembership =
                IntSet<PartyId>.Intersection(matchesByProfileType, matchesByMembership);

            ResultSet matchesByProfile = null;
            t.Measure("Search by Profile ('advanced' search)", delegate() {
                matchesByProfile = profileSearch(
                    t,
                    consumerProperties,
                    profileSpecificProfiles);
            });

            double nonGeoMatchAbsDensity = NonGeoMatchAbsDensity(
                matchesByMembership,
                matchesByProfile);

            ResultSet matchesByParty = ResultSet.Universe;
            t.Measure("Search by Party (geography,name)", delegate() {
                if (searchLocale == null) {
                    matchesByParty = ResultSet.From(null, null, matchesByMembership);
                } else {
                    matchesByParty = partySearch(
                       t,
                       nameSearchNoiseWords,
                       searchLocale,
                       desiredResultCount,
                       maxResultCount,
                       matchesByMembership,
                       profileSpecificParties,
                       nonGeoMatchAbsDensity,
                       ref sortOrder);
                }
            });

            ResultSet matches = null;
            t.Measure("Combine matches-by-party and matches-by-profile", delegate() {
                matches = ResultSet.Intersection(matchesByParty, matchesByProfile);
            });

            return FinalizeResultSet(
                t,
                sortOrder,
                desiredResultCount,
                matches,
                featuredProfileTemplateLabel);
        }

        //----------------------------------------------------------------------
        private static ResultSet FinalizeResultSet(
            Timer t,
            Comparison<Result> sortOrder,
            int desiredResultCount,
            ResultSet matches,
            string featuredProfileTemplateLabel) {

            // KLUDGE: Ed V. is not confident that this is the right place for 
            // this code.  Would it be better to make this a ResultSet method?
            // Would you ever *not* want to call all of these methods in exactly
            // this order?  If so, it should not be a ResultSet method.  If not, 
            // it should be.

            t.Measure("Find summaries for each matching Party", delegate() {
                matches.LoadSummaries();
            });

            if (StringX.IsNotEmpty(featuredProfileTemplateLabel)) {
                t.Measure("Find featured-since dates for each matching Party", delegate() {
                    matches.LoadFeaturedSinceDates(featuredProfileTemplateLabel);
                });
            }

            t.Measure("Sort matching Parties", delegate() {
                matches.OrderBy(sortOrder);
            });

            t.Measure("Trim results to desiredResultCount", delegate() {
                matches.TrimToDesiredResultCount(desiredResultCount);
            });

            t.Measure("Find EPMTs associated with matching Parties", delegate() {
                // BUG: Since the "trim" step above doesn't actually discard the
                // Party IDs from the ResultSet, we may find a EPMT here that 
                // isn't actually represented in the *visible* results.  Arrg!
                // In practice, this is *very* rare, so we'll live with the
                // slight chance that an extra license type will be included for
                // now.
                matches.LoadExternalPartyMembershipTypes();
            });

            return matches;
        }

        //----------------------------------------------------------------------
        public delegate IntSet<PartyId> MembershipSearch(List<int> epmts);
        private static IntSet<PartyId> SearchByMembership(
            List<int> externalPartyMembershipTypeIds) {

            return IntSet<PartyId>.IntersectionOfMany(
                externalPartyMembershipTypeIds
                    .ConvertAll<IntSet<PartyId>>(Cache.Cache.MembershipMappings.PartyIds)
                    .ToArray());
        }

        //----------------------------------------------------------------------
        public delegate ResultSet PartySearch(
            Timer t,
            IEnumerable<string> nameSearchNoiseWords,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            IntSet<PartyId> externalPartyMembershipTypeMatches,
            IntSet<PartyId> profileSpecificParties,
            double nonGeoMatchAbsDensity,
            ref Comparison<Result> sortOrder);

        //----------------------------------------------------------------------
        private static ResultSet SearchByParty<ProfileType>(
            Timer t,
            IEnumerable<string> nameSearchNoiseWords,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            IntSet<PartyId> externalPartyMembershipTypeMatches,
            IntSet<PartyId> profileSpecificParties,
            double nonGeoMatchAbsDensity,
            ref Comparison<Result> sortOrder)
            where ProfileType : ISearch, new() {

            IntSet<PartyId> nameMatches = null;
            IntSet<PartyId> locationMatches = null;
            IntSet<PartyId> geoOrNameMatches = null;
            Location centroid = null;
            double? maxRadiusMi = null;
            ResultSet rs = null;


            if (StringX.IsNotEmpty(searchLocale.PartyName)) {
                nameMatches = SearchByName(t, searchLocale, nameSearchNoiseWords);

            }


            if (searchLocale.Type == Snap.Data.Controller.SearchType.PartyName) {
                geoOrNameMatches = nameMatches;
                sortOrder = Result.NameAscending;


            } else {
                switch (new ProfileType().GeoSearch) {
                    case GeoSearchType.Point:

                        locationMatches = SearchForLocations(
                            t, nameSearchNoiseWords, searchLocale, desiredResultCount,
                            maxResultCount, nonGeoMatchAbsDensity,
                            out centroid, out maxRadiusMi);
                        break;
                    case GeoSearchType.ServiceArea:
                        locationMatches = SearchForServiceArea(t, searchLocale);
                        break;
                    case GeoSearchType.All:
                        var pointMatches = SearchForLocations(
                            t, nameSearchNoiseWords, searchLocale, desiredResultCount,
                            maxResultCount, nonGeoMatchAbsDensity, out centroid, out maxRadiusMi);
                        var svcAreaMatches = SearchForServiceArea(t, searchLocale);
                        locationMatches = IntSet<PartyId>.Union(pointMatches, svcAreaMatches);
                        sortOrder = Result.ShowServiceAreaMatchesFirst(svcAreaMatches, sortOrder);
                        break;
                    default:
                        throw new ArgumentException(String.Format(
                            "Unexpected value of 'ISearch.GeoSearch': {0}",
                            new ProfileType().GeoSearch.ToString()));
                }
                geoOrNameMatches = StringX.IsNotEmpty(searchLocale.PartyName) ? IntSet<PartyId>.Intersection(nameMatches, locationMatches) : locationMatches;

            }



            rs = ResultSet.From(
               centroid,
               maxRadiusMi,
               IntSet<PartyId>.IntersectionOfMany(
                   geoOrNameMatches,
                   externalPartyMembershipTypeMatches,
                   profileSpecificParties));

            if (Snap.Data.Controller.SearchType.Membership == searchLocale.Type) {
                rs.UnfilteredMatchCount = externalPartyMembershipTypeMatches.Count;
            } else if (IntSet<PartyId>.IsEmpty(geoOrNameMatches)) {
                rs.UnfilteredMatchCount = 0;
            } else {
                rs.UnfilteredMatchCount = geoOrNameMatches.Count;
            }


            return rs;
        }

        //----------------------------------------------------------------------
        /// <summary>
        /// Returns a double in the range [0.0, 1.0] indicating the global 
        /// density of Parties that match non-geographic search criteria.
        /// 
        /// If there are no non-geographic criteria, this method always
        /// returns 1.0.
        /// </summary>
        private static double NonGeoMatchAbsDensity(
            IntSet<PartyId> matchesByMembership,
            ResultSet matchesByProfile) {

            bool byMembershipIsEmpty = IntSet<PartyId>.IsEmpty(matchesByMembership);
            bool byMembershipIsUniverse =
                !byMembershipIsEmpty
                && matchesByMembership.IsUniverse;

            bool byProfileIsEmpty = ResultSet.IsEmpty(matchesByProfile);
            bool byProfileIsUniverse =
                !byProfileIsEmpty
                && matchesByProfile.IsUniverse;

            double nonGeoMatchCount = 0.0;
            if (byMembershipIsUniverse && byProfileIsUniverse) {
                return 1.0;
            } else if (byMembershipIsUniverse) {
                nonGeoMatchCount = byProfileIsEmpty ? 0 : matchesByProfile.Count;
            } else if (byProfileIsUniverse) {
                nonGeoMatchCount = byMembershipIsEmpty ? 0 : matchesByMembership.Count;
            } else {
                nonGeoMatchCount = Math.Min(
                    byMembershipIsEmpty ? 0 : matchesByMembership.Count,
                    byProfileIsEmpty ? 0 : matchesByProfile.Count);
            }

            // Ensure that the activePartyCount is never less than the number 
            // of matches.  This guarantees a return value in the promised 
            // range: [0.0, 1.0].
            double activePartyCount = Math.Max(
                nonGeoMatchCount,
                Cache.Cache.ActivePartyCount);
            return nonGeoMatchCount / activePartyCount;
        }
    }
}
