using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Snap.Util;
using Snap.Geography;

namespace Snap.Search2 {

    /// <summary>
    /// A mapping, Party ID --> Result, which may include an ordering of the
    /// values (which makes this a set-and-maybe-sequence).
    /// </summary>
    public class ResultSet : IEnumerable<Result>, ICollection, IList {

        /// <summary>
        /// A ResultSet instance that represents the result of a search which
        /// matched every possible candidate Party.
        /// </summary>
        public static readonly ResultSet Universe = new ResultSet(0);

        /// <summary>
        /// A mapping from SNAP Party ID --> Result.  One such pair exists for
        /// each result item contained in this ResultSet.
        /// </summary>
        Dictionary<int, Result> mPartyIdToResult;

        public Dictionary<int, Result>.KeyCollection AllPartyIds {
            get {
                return mPartyIdToResult.Keys;
            }
        }

        /// <summary>
        /// An ordering over (a subset of) the results in mPartyIdToResult.
        /// For inexact searches which are truncated at a specific number of 
        /// results, mOrdering.Length may be less than mPartyIdToResult.Count.
        /// </summary>
        Result[] mOrdering;

        /// <summary>
        /// A list of all ExternalPartyMembershipTypes that were associated with
        /// at least one facility in the ResultSet.
        /// </summary>
        List<Snap.Data.ExternalPartyMembershipTypeInfo> mEPMTs;

        /// <summary>
        /// The total, unfiltered count of geographic or facility name matches.
        /// When this value is less than zero, it is considered to be undefined.
        /// </summary>
        int mUnfilteredCount;

        /// <summary>
        /// Centroid of the searched area for non-exact searches; may be null
        /// </summary>
        Location mSearchOrigin;

        /// <summary>
        /// For non-exact search results, returns a value at least as large as
        /// the distance (in statute miles) from the search origin to the most
        /// distant facility considered.  The value may be larger, but will not
        /// be smaller.  Otherwise, null.
        /// </summary>
        double? mMaxRadiusMi;

        /// <summary>
        /// Construct a ResultSet instance from a set of Party IDs and some
        /// (optional) supplemental information about the Party-based search
        /// that was performed.
        /// </summary>
        /// <param name="searchOrigin">
        /// the centroid of the searched area; may be null
        /// </param>
        /// <param name="maxRadiusMi">
        /// the maximum distance from the origin, in statute miles, at which
        /// facilities were considered for inclusion in the Party ID set; may
        /// be null
        /// </param>
        /// <param name="partyIds">
        /// the set of Party IDs which satisfied some criteria
        /// </param>
        /// <returns></returns>
        public static ResultSet From(
            Location searchOrigin,
            double? maxRadiusMi,
            IntSet<PartyId> partyIds) {

            if (IntSet<PartyId>.IsEmpty(partyIds)) {
                return new ResultSet(0);
            }

            if (partyIds.IsUniverse) {
                throw new ArgumentException(
                    "partyIds may not be IntSet<PartyId>.Universe");
            }

            ResultSet resultSet = new ResultSet(partyIds.Count, searchOrigin, maxRadiusMi);
            foreach (int partyId in partyIds) {
                resultSet.mPartyIdToResult[partyId] = new Result();
            }

            return resultSet;
        }

        public static ResultSet From(IntSet<ProfileId> profileIds) {
            if (IntSet<ProfileId>.IsEmpty(profileIds)) {
                return new ResultSet(0);
            }

            if (profileIds.IsUniverse) {
                return Universe;
            }

            ResultSet resultSet = new ResultSet(profileIds.Count);
            Dictionary<int, int> profileToPartyMap = Snap.Cache.Cache.ProfileToPartyMap;

            foreach (int profileId in profileIds) {
                int partyId = Snap.Data.PartyInfo.INVALID_PARTY_ID;

                if (profileToPartyMap.TryGetValue(profileId, out partyId)) {
                    Result r = null;

                    if (resultSet.mPartyIdToResult.TryGetValue(partyId, out r)) {
                        r.AddMatchingProfile(profileId);
                    } else {
                        Result newResult = new Result();
                        newResult.AddMatchingProfile(profileId);
                        resultSet.mPartyIdToResult.Add(
                            partyId,
                            newResult);
                    }
                }
            }

            return resultSet;
        }

        /// <summary>
        /// Returns an empty ResultSet instance which preserves the 
        /// </summary>
        private static ResultSet EmptySetBasedOn(ResultSet original) {
            ResultSet empty = new ResultSet(0, original.mSearchOrigin, original.mMaxRadiusMi);

            if (original.mUnfilteredCount >= 0) {
                empty.UnfilteredMatchCount = original.UnfilteredMatchCount;
            }

            return empty;
        }

        /// <summary>
        /// Returns the intersection of two Result sets.
        /// </summary>
        public static ResultSet Intersection(ResultSet lhs, ResultSet rhs) {
            if (IsEmpty(lhs) && IsEmpty(rhs)) {
                ResultSet rs = new ResultSet(0, null, null);
                rs.UnfilteredMatchCount = 0;
                return rs;
            } else if (IsEmpty(lhs)) {
                return EmptySetBasedOn(rhs);
            } else if (IsEmpty(rhs)) {
                return EmptySetBasedOn(lhs);
            } else if (lhs.IsUniverse) {
                return rhs;
            } else if (rhs.IsUniverse) {
                return lhs;
            }

            // Ensure that we pass to NormalIntersection an lhs parameter that
            // is the smaller of the two sets.
            if (lhs.Count <= rhs.Count) {
                return InnerIntersection(lhs, rhs);
            } else {
                return InnerIntersection(rhs, lhs);
            }


        }

        /// <summary>
        /// Returns the maximum of two double values, taking into account that
        /// one or both may be null.
        /// </summary>
        private static double? Max(double? lhs, double? rhs) {
            if (!lhs.HasValue && !rhs.HasValue) {
                return null;
            } else if (!lhs.HasValue) {
                return rhs.Value;
            } else if (!rhs.HasValue) {
                return lhs.Value;
            } else {
                return Math.Max(lhs.Value, rhs.Value);
            }
        }

        /// <summary>
        /// Returns the intersection of two non-empty, non-universe sets.
        /// </summary>
        private static ResultSet InnerIntersection(ResultSet smaller, ResultSet larger) {
            if (smaller.Count > larger.Count) {
                throw new InvalidOperationException("smaller.Count > larger.Count!");
            }

            ResultSet intersection = new ResultSet(0);

            if ((null != smaller.mSearchOrigin) && (null != larger.mSearchOrigin)) {
                throw new ArgumentException("Both sets have a search origin!");
            } else if (null != smaller.mSearchOrigin) {
                intersection.mSearchOrigin = smaller.mSearchOrigin;
            } else {
                intersection.mSearchOrigin = larger.mSearchOrigin;
            }

            intersection.mPartyIdToResult = new Dictionary<int, Result>(smaller.Count);
            intersection.mUnfilteredCount = Math.Max(
                smaller.mUnfilteredCount,
                larger.mUnfilteredCount);
            intersection.mMaxRadiusMi = Max(
                smaller.MaxDistanceMi,
                larger.MaxDistanceMi);



            foreach (KeyValuePair<int, Result> pair in smaller.mPartyIdToResult) {
                int partyId = pair.Key;
                Result smResult = pair.Value;
                Result lgResult = null;

                if (larger.mPartyIdToResult.TryGetValue(partyId, out lgResult)) {
                    intersection.mPartyIdToResult.Add(partyId,
                        Result.Merge(smResult, lgResult));
                }
            }


            return intersection;
        }

        private ResultSet(int capacity)
            : this(capacity, null, null) {
        }

        private ResultSet(int capacity, Location searchOrigin, double? maxRadiusMi) {
            mPartyIdToResult = new Dictionary<int, Result>(capacity);
            mOrdering = null;
            mEPMTs = null;
            mUnfilteredCount = Int32.MinValue;
            mSearchOrigin = searchOrigin;
            mMaxRadiusMi = maxRadiusMi;
        }

        /// <summary>
        /// The total, unfiltered count of geographic or facility name matches.
        /// </summary>
        [Obsolete("Use ResultSet.UnfilteredMatchCount instead.")]
        public int GeographicOrNameMatchCount {
            get {
                return UnfilteredMatchCount;
            }
            set {
                UnfilteredMatchCount = value;
            }
        }

        /// <summary>
        /// The total, unfiltered count of geographic or facility name matches.
        /// </summary>
        public int UnfilteredMatchCount {
            get {
                if (mUnfilteredCount < 0) {
                    throw new InvalidOperationException(
                        "UnfilteredMatchCount has not been set");
                }

                return mUnfilteredCount;
            }
            set {
                if (value < 0) {
                    throw new ArgumentException("value must be >= 0");
                }

                mUnfilteredCount = value;
            }
        }

        /// <summary>
        /// The cardinality of the result set.
        /// </summary>
        public int Count {
            get {
                if (IsUniverse) {
                    throw new InvalidOperationException(
                        "ResultSet.Universe.Count is not a meaningful " +
                        "operation; client code must always check IsUniverse");
                } else if (null == mPartyIdToResult) {
                    return 0;
                } else if (null != mOrdering) {
                    return mOrdering.Length;
                } else {
                    return mPartyIdToResult.Count;
                }
            }
        }

        /// <summary>
        /// Returns true iff this ResultSet instance represents the Universe.
        /// I.e., it contains all possible results.  I.e., the criteria used to 
        /// perform the search matched every possible item in the domain.
        /// </summary>
        public bool IsUniverse {
            get {
                return Object.ReferenceEquals(this, Universe);
            }
        }

        /// <summary>
        /// Returns true iff this ResultSet instance represents the empty set.
        /// I.e., it contains zero results.
        /// </summary>
        public static bool IsEmpty(ResultSet resultSet) {
            return (null == resultSet) ||
                (!resultSet.IsUniverse && (0 == resultSet.Count));
        }

        /// <summary>
        /// For non-exact search results, returns a value at least as large as
        /// the distance (in statute miles) from the search origin to the most
        /// distant facility considered.  The value may be larger, but will not
        /// be smaller.  Otherwise, null.
        /// </summary>
        public double? MaxDistanceMi {
            get {
                return mMaxRadiusMi;
            }
        }

        /// <summary>
        /// A list of all ExternalPartyMembershipTypes that were associated with
        /// at least one facility in the ResultSet.
        /// </summary>
        public List<Snap.Data.ExternalPartyMembershipTypeInfo> MembershipTypes {
            get {
                return mEPMTs;
            }
        }

        #region Ordering

        /// <summary>
        /// Throws an exception if mOrdering has not been set.
        /// </summary>
        private void AssertOrderingIsSet() {
            if (null == mOrdering) {
                throw new InvalidOperationException(
                    "ResultSet enumerators are available only AFTER OrderBy(...) has been invoked.");
            }
        }

        /// <summary>
        /// Iterate over the contained Results in the order specified in the
        /// call to OrderBy().
        /// </summary>
        public IEnumerator<Result> GetEnumerator() {
            AssertOrderingIsSet();
            return ((IEnumerable<Result>)mOrdering).GetEnumerator();
        }

        /// <summary>
        /// Iterate over the contained Results in the order specified in the
        /// call to OrderBy().
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            AssertOrderingIsSet();
            return mOrdering.GetEnumerator();
        }

        /// <summary>
        /// Assign an ordering to over the set of search results (e.g., 
        /// alphabetic, distance).
        /// </summary>
        public void OrderBy(Comparison<Result> comparison) {
            Precondition.IsNotNull(comparison, "comparison");

            if (null == mOrdering) {
                mOrdering = new Result[mPartyIdToResult.Count];
                mPartyIdToResult.Values.CopyTo(mOrdering, 0);
            }

            Array.Sort(mOrdering, comparison);
        }

        /// <summary>
        /// Retrieve the i-th item in the ResultSet based on the order supplied
        /// in the call to OrderBy().
        /// </summary>
        public Result this[int index] {
            get {
                AssertOrderingIsSet();
                return mOrdering[index];
            }
        }

        #endregion

        /// <summary>
        /// Attach a PartySummary row to each Result in the set.
        /// </summary>
        public void LoadSummaries() {
            Dictionary<int, DataRow> partySummaries =
                Snap.Cache.Cache.PartySummary2(mPartyIdToResult.Keys);
            List<int> partiesWithNoSummary = null;
            List<int> partiesWithNoValidAddress = null;

            foreach (KeyValuePair<int, Result> pair in mPartyIdToResult) {
                int partyId = pair.Key;
                Result result = pair.Value;
                DataRow summaryRow = null;

                if (partySummaries.TryGetValue(partyId, out summaryRow)) {

                    // newly introduced ruled to exclude party which have no address at all
                    if (result.IsValidAddress(summaryRow)) {
                        result.SetSummaryRow(summaryRow);
                        result.SetDistanceMi(mSearchOrigin);
                    } else {
                        if (null == partiesWithNoValidAddress) {
                            partiesWithNoValidAddress = new List<int>();
                        }
                        partiesWithNoValidAddress.Add(partyId);
                    }
                } else {
                    // This Party's information isn't yet included in the 
                    // PartySummary cache.

                    if (null == partiesWithNoSummary) {
                        partiesWithNoSummary = new List<int>(10);
                    }

                    partiesWithNoSummary.Add(partyId);
                }
            }

            // Remove Parties which does not have valid address

            if (partiesWithNoValidAddress != null) {
                foreach (int partyId in partiesWithNoValidAddress) {
                    mPartyIdToResult.Remove(partyId);
                }
            }




            if (null == partiesWithNoSummary) {
                return;
            }

            // Remove Parties that 
            //   - don't yet have information in the PartySummary cache (new)
            //   - won't ever have info in the PartySummary cache (expired)
            foreach (int partyId in partiesWithNoSummary) {
                mPartyIdToResult.Remove(partyId);
            }



        }

        /// <summary>
        /// Given a Profile that defines a featured Party in the current 
        /// ResultSet, set the FeaturedSince properties of all featured
        /// Parties.
        /// </summary>
        public void LoadFeaturedSinceDates(string featuredProfileTemplateLabel) {
            Dictionary<int, DateTime> profileEffectiveDates =
                Snap.Cache.Cache.ActiveProfileEffectiveDate(featuredProfileTemplateLabel);

            foreach (KeyValuePair<int, DateTime> pair in profileEffectiveDates) {
                int partyId = pair.Key;
                DateTime effectiveDate = pair.Value;
                Result result = null;

                if (mPartyIdToResult.TryGetValue(partyId, out result)) {
                    result.FeaturedSince = effectiveDate;
                }
            }
        }

        /// <summary>
        /// Like List(of T).ConvertAll(...).  This maps integer Party IDs to T.
        /// </summary>
        public List<T> ConvertAllPartyIds<T>(Converter<int, T> convert) {
            List<T> output = new List<T>(mPartyIdToResult.Count);
            foreach (int partyId in mPartyIdToResult.Keys) {
                output.Add(convert(partyId));
            }
            return output;
        }

        /// <summary>
        /// Attach a list of all ExternalPartyMembershipTypes assocaited with at
        /// least one of the Reslts.
        /// </summary>
        public void LoadExternalPartyMembershipTypes() {
            mEPMTs = Cache.Cache.MembershipMappings.EPMTs(mPartyIdToResult.Keys);
        }

        /// <summary>
        /// Throw away all but the first n results in the ordering.  This 
        /// doesn't actually modify the complete result set mapping from Party
        /// ID to Result.  It just chops off end of the ordered array.
        /// </summary>
        public void TrimToDesiredResultCount(int desiredResultCount) {
            if (null == mOrdering) {
                throw new Exception("You must apply an ordering first!");
            }

            if ((desiredResultCount == Search.ExactMatchResultCount) ||
                (desiredResultCount >= mPartyIdToResult.Count)) {
                return;
            }

            Result[] trimmedOrder = new Result[desiredResultCount];
            Array.Copy(mOrdering, 0, trimmedOrder, 0, desiredResultCount);
            mOrdering = trimmedOrder;
            mUnfilteredCount = desiredResultCount;
        }

        #region IList Members

        public int Add(object value) {
            throw new InvalidOperationException();
        }

        public void Clear() {
            throw new InvalidOperationException();
        }

        public bool Contains(object value) {
            return -1 < IndexOf(value);
        }

        public int IndexOf(object value) {
            AssertOrderingIsSet();

            for (int i = 0; i < mOrdering.Length; i++) {
                if (mOrdering[i] == value) {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, object value) {
            throw new InvalidOperationException();
        }

        public bool IsFixedSize {
            get {
                return true;
            }
        }

        public bool IsReadOnly {
            get {
                return true;
            }
        }

        public void Remove(object value) {
            throw new InvalidOperationException();
        }

        public void RemoveAt(int index) {
            throw new InvalidOperationException();
        }

        object IList.this[int index] {
            get {
                AssertOrderingIsSet();
                return mOrdering[index];
            }
            set {
                throw new InvalidOperationException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index) {
            AssertOrderingIsSet();
            mOrdering.CopyTo(array, index);
        }

        public bool IsSynchronized {
            get {
                AssertOrderingIsSet();
                return mOrdering.IsSynchronized;
            }
        }

        public object SyncRoot {
            get {
                AssertOrderingIsSet();
                return mOrdering.SyncRoot;
            }
        }

        #endregion
    }
}
