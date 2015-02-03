using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Snap.Util;
using Snap.Geography;

namespace Snap.Search2 {

    /// <summary>
    /// Column indices and names for columns of the SNAP_PartySummary view.
    /// </summary>
    public class Result {

        #region Sort comparison delegates

        public static Comparison<Result> ShowVacanciesFirst(
            Comparison<Result> secondOrderComparison) {

            return delegate(Result lhs, Result rhs) {
                int result = 0; // KLUDGE: necessary for type inference :-(
                bool lhsVacancy = lhs.IsAdvertisingVacancyNow;
                bool rhsVacancy = rhs.IsAdvertisingVacancyNow;

                if (lhsVacancy == rhsVacancy) {
                    result = secondOrderComparison(lhs, rhs);
                } else if (lhsVacancy) {
                    result = -1;
                } else if (rhsVacancy) {
                    result = 1;
                }

                return result;
            };
        }

        public static Comparison<Result> ShowServiceAreaMatchesFirst(
            IntSet<PartyId> serviceAreaMatches,
            Comparison<Result> secondOrderComparison) {

            return delegate(Result lhs, Result rhs) {
                int result = 0; // KLUDGE: necessary for type inference :-(
                bool lhsIsSvcAreaMatch = serviceAreaMatches.Contains(lhs.PartyId);
                bool rhsIsSvcAreaMatch = serviceAreaMatches.Contains(rhs.PartyId);

                if (lhsIsSvcAreaMatch == rhsIsSvcAreaMatch) {
                    result = secondOrderComparison(lhs, rhs);
                } else if (lhsIsSvcAreaMatch) {
                    result = -1;
                } else if (rhsIsSvcAreaMatch) {
                    result = 1;
                }

                return result;
            };
        }

        public static int NameAscending(Result lhs, Result rhs) {
            return String.Compare(lhs.PartyName, rhs.PartyName);
        }

        public static int NameDescending(Result lhs, Result rhs) {
            return -NameAscending(lhs, rhs);
        }

        public static int DistanceAscending(Result lhs, Result rhs) {
            return lhs.DistanceMi.CompareTo(rhs.DistanceMi);
        }

        #endregion

        /// <summary>
        /// A SNAP_PartySummary_SeniorHousing row
        /// </summary>
        private DataRow mSummaryRow;

        /// <summary>
        /// The distance from the geographic search original, when applicable.
        /// May be Double.NaN.
        /// </summary>
        private double mDistanceMi;

        /// <summary>
        /// A list of Profile IDs which match the search criteria, when 
        /// applicable.
        /// </summary>
        private List<int> mMatchingProfiles;

        public Result() {
            mSummaryRow = null;
            mDistanceMi = Double.NaN;
            mMatchingProfiles = null;
        }

        #region Merge two Result instances

        /// <summary>
        /// Merge two results.
        /// </summary>
        public static Result Merge(Result lhs, Result rhs) {
            return new Result(lhs, rhs);
        }

        /// <summary>
        /// Throws an ArgumentException with 'message' if the condition is 
        /// false.  This method should only be used by the Result "merge" 
        /// constructor.
        /// </summary>
        private void Assert(bool condition, string message) {
            if (!condition) {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Merge two results.  Clients should call Result.Merge(...) instead.
        /// </summary>
        private Result(Result lhs, Result rhs)
            : this() {
            Precondition.IsNotNull(lhs, "lhs");
            Precondition.IsNotNull(rhs, "rhs");

            Assert((null == lhs.mSummaryRow) || (null == rhs.mSummaryRow),
                "Both 'lhs' and 'rhs' have non-null mSummaryRow fields.");
            if (null == lhs.mSummaryRow) {
                mSummaryRow = rhs.mSummaryRow;
            } else {
                mSummaryRow = lhs.mSummaryRow;
            }

            Assert(Double.IsNaN(lhs.DistanceMi) || Double.IsNaN(rhs.DistanceMi),
                "Both 'lhs' and 'rhs' have non-NaN mDistanceMi fields.");
            if (Double.IsNaN(lhs.DistanceMi)) {
                mDistanceMi = rhs.DistanceMi;
            } else {
                mDistanceMi = lhs.DistanceMi;
            }

            Assert((0 == lhs.MatchingProfiles.Count) || (0 == rhs.MatchingProfiles.Count),
                "Both 'lhs' and 'rhs' have non-empty mMatchingProfiles fields.");
            if (0 == lhs.MatchingProfiles.Count) {
                mMatchingProfiles = rhs.mMatchingProfiles;
            } else {
                mMatchingProfiles = lhs.mMatchingProfiles;
            }
        }

        #endregion

        /// <summary>
        /// The distance from the geographic search original, when applicable.
        /// May be Double.NaN.
        /// </summary>
        public double DistanceMi {
            get {
                return mDistanceMi;
            }
            set {
                mDistanceMi = value;
            }
        }

        public bool HasMatchingProfiles {
            get {
                return (null != mMatchingProfiles) &&
                    (mMatchingProfiles.Count > 0);
            }
        }

        private static readonly IList<int> sEmptyMatchingProfiles =
            new List<int>(0).AsReadOnly();

        /// <summary>
        /// A list of Profile IDs which match the search criteria, when 
        /// applicable.  May be null.
        /// </summary>
        public IList<int> MatchingProfiles {
            get {
                if (null == mMatchingProfiles) {
                    return sEmptyMatchingProfiles;
                } else {
                    return mMatchingProfiles.AsReadOnly();
                }
            }
        }

        public void AddMatchingProfile(int profileId) {
            if (null == mMatchingProfiles) {
                mMatchingProfiles = new List<int>(2);
            }

            mMatchingProfiles.Add(profileId);
        }


        /// <summary>
        /// Newly introduced rule to exclude party 
        /// in result which have no party address at all
        /// </summary>
        /// <param name="summaryRow"></param>
        /// <returns></returns>
        public bool IsValidAddress(DataRow summaryRow) {
            if (summaryRow[PartyPhysicalAddressCol] == DBNull.Value
                && summaryRow[PartyPhysicalAddressCol] == DBNull.Value
                  && summaryRow[PartyPhysicalCityCol] == DBNull.Value
                  && summaryRow[PartyPhysicalCountyCol] == DBNull.Value
                 && summaryRow[PartyPhysicalStateCol] == DBNull.Value
                  && summaryRow[LatitudeCol] == DBNull.Value
                  && summaryRow[LongitudeCol] == DBNull.Value
                ) {
                return false;
            } else {
                return true;
            }
        }

        internal void SetSummaryRow(DataRow summaryRow) {
            mSummaryRow = summaryRow;
        }

        private void AssertSummaryIsSet() {
            if (null == mSummaryRow) {
                throw new InvalidOperationException(
                    "No SummaryRow is associated with the Result.");
            }
        }

        public T SummaryField<T>(int columnIndex) {
            if (columnIndex < 0) {
                throw new ArgumentOutOfRangeException("columnIndex",
                    "columnIndex must be >= 0");
            }

            AssertSummaryIsSet();
            return (T)mSummaryRow[columnIndex];
        }

        public void SummaryToCsv(System.IO.TextWriter output) {
            for (int i = 0; i < mSummaryRow.Table.Columns.Count; i++) {
                output.Write(mSummaryRow[i]);
                output.Write(", ");
            }

            output.Write(mDistanceMi);
            output.Write(", ");

            if (null != mMatchingProfiles) {
                output.Write(String.Join(" ",
                    mMatchingProfiles.ConvertAll<String>(Convert.ToString).ToArray()));
            }
        }

        private static T As<T>(object dbValue) {
            if (DBNull.Value == dbValue) {
                return default(T);
            }

            return (T)dbValue;
        }

        public void SetDistanceMi(Location searchOrigin) {
            if (null == searchOrigin) {
                // This method is a no-op when there's no search origin.
                // E.g., for an exact match city name search.
                return;
            }

            if (null == mSummaryRow) {
                throw new InvalidOperationException(
                    "Must invoke SetSummaryRow(...) first");
            }

            if (Latitude.HasValue && Longitude.HasValue) {
                mDistanceMi = Location.DistanceMi(
                    new Location(Latitude.Value, Longitude.Value),
                    searchOrigin);
            }
        }

        #region General Party columns

        internal const int PartyIdCol = 0;
        public int PartyId {
            get {
                return (int)mSummaryRow[PartyIdCol];
            }
        }

        internal const int PartyNameCol = 1;
        public string PartyName {
            get {
                return As<string>(mSummaryRow[PartyNameCol]);
            }
        }

        internal const int PartyPhysicalAddressCol = 2;
        public string PartyPhysicalAddress {
            get {
                return As<string>(mSummaryRow[PartyPhysicalAddressCol]);
            }
        }

        internal const int PartyPhysicalPostalCodeCol = 3;
        public string PartyPhysicalPostalCode {
            get {
                return As<string>(mSummaryRow[PartyPhysicalPostalCodeCol]);
            }
        }

        internal const int PartyPhysicalCityCol = 4;
        internal const string PartyPhysicalCityColName = "PartyPhysicalCity";
        public string PartyPhysicalCity {
            get {
                return As<string>(mSummaryRow[PartyPhysicalCityCol]);
            }
        }

        internal const int PartyPhysicalCountyCol = 5;
        internal const string PartyPhysicalCountyColName = "PartyPhysicalCounty";
        public string PartyPhysicalCounty {
            get {
                return As<string>(mSummaryRow[PartyPhysicalCountyCol]);
            }
        }

        internal const int PartyPhysicalStateCol = 6;
        internal const string PartyPhysicalStateColName = "PartyPhysicalState";
        public string PartyPhysicalState {
            get {
                return As<string>(mSummaryRow[PartyPhysicalStateCol]);
            }
        }

        internal const int LatitudeCol = 7;
        internal const string LatitudeColName = "Latitude";
        public double? Latitude {
            get {
                return As<double?>(mSummaryRow[LatitudeCol]);
            }
        }

        internal const int LongitudeCol = 8;
        internal const string LongitudeColName = "Longitude";
        public double? Longitude {
            get {
                return As<double?>(mSummaryRow[LongitudeCol]);
            }
        }

        internal const int PartyPhoneVoiceCol = 9;
        internal const string PartyPhoneVoiceColName = "PartyPhoneVoice";

        /// <summary>
        /// The actual voice phone number of the facility.  This should always
        /// be shown to SNAP personnel.  Consumers should be shown
        /// PartyPhoneDisplayVoice instead (because consumers should always call
        /// through the virtual number).
        /// </summary>
        public string PartyPhoneVoice {
            get {
                return As<string>(mSummaryRow[PartyPhoneVoiceCol]);
            }
        }

        internal const int PartyPhoneFaxCol = 10;
        internal const string PartyPhoneFaxColName = "PartyPhoneFax";
        public string PartyPhoneFax {
            get {
                return As<string>(mSummaryRow[PartyPhoneFaxCol]);
            }
        }

        internal const int PartyPhoneMobileCol = 11;
        internal const string PartyPhoneMobileColName = "PartyPhoneMobile";
        public string PartyPhoneMobile {
            get {
                return As<string>(mSummaryRow[PartyPhoneMobileCol]);
            }
        }

        internal const int PartyPhoneVirtualVoiceCol = 12;
        internal const string PartyPhoneVirtualVoiceColName = "PartyPhoneVirtualVoice";

        /// <summary>
        /// A virtual phone number, if the Party has one.
        /// </summary>
        public string PartyPhoneVirtualVoice {
            get {
                return As<string>(mSummaryRow[PartyPhoneVirtualVoiceCol]);
            }
        }

        internal const int PartyPhoneDisplayVoiceCol = 13;
        internal const string PartyPhoneDisplayVoiceColName = "PartyPhoneDisplayVoice";

        /// <summary>
        /// A virtual phone number or, if the Party has no virtual number, the 
        /// actual voice phone number of the facility.  This should always be
        /// shown to consumers.  SNAP personel should be shown PartyPhoneVoice 
        /// instead (because we shouldn't call through the virtual number).
        /// </summary>
        public string PartyPhoneDisplayVoice {
            get {
                return As<string>(mSummaryRow[PartyPhoneDisplayVoiceCol]);
            }
        }

        internal const int PartyEmailCol = 14;
        internal const string PartyEmailColName = "PartyEmail";
        public string PartyEmail {
            get {
                return As<string>(mSummaryRow[PartyEmailCol]);
            }
        }

        internal const int PartyWebsiteCol = 15;
        internal const string PartyWebsiteColName = "PartyWebsite";
        public string PartyWebsite {
            get {
                return As<string>(mSummaryRow[PartyWebsiteCol]);
            }
        }

        internal const int DefaultImageFileNameCol = 16;
        internal const string DefaultImageFileNameColName = "DefaultImageFileName";
        public string DefaultImageFileName {
            get {
                return As<string>(mSummaryRow[DefaultImageFileNameCol]);
            }
        }

        #endregion

        #region Senior Housing specific columns

        internal const int IsAdvertisingVacancyNowCol = 17;
        internal const string IsAdvertisingVacancyNowColName = "IsAdvertisingVacancyNow";
        public bool IsAdvertisingVacancyNow {
            get {
                bool? b = As<bool?>(mSummaryRow[IsAdvertisingVacancyNowCol]);
                return b.HasValue && b.Value;
            }
        }

        internal const int HighlightsTextFragmentCol = 18;
        internal const string HighlightsTextFragmentColName = "HighlightsTextFragment";
        public string HighlightsTextFragment {
            get {
                return As<string>(mSummaryRow[HighlightsTextFragmentCol]);
            }
        }

        internal const int OffersMemoryCareCol = 19;
        internal const string OffersMemoryCareColName = "OffersMemoryCare";
        public bool OffersMemoryCare {
            get {
                bool? b = As<bool?>(mSummaryRow[OffersMemoryCareCol]);
                return b.HasValue && b.Value;
            }
        }

        internal const int GeneralCareLevelsCol = 20;
        internal const string GeneralCareLevelsColName = "Categories";
        internal enum GclBitMask {
            IndependentLiving = 2,
            AssistedLiving = 4,
            SkilledNursing = 8
        };

        protected int GeneralCareLevels {
            get {
                int? b = As<int?>(mSummaryRow[GeneralCareLevelsCol]);
                return b.HasValue ? b.Value : 0;
            }
        }

        public bool IsIndependentLiving {
            get {
                return 0 != (GeneralCareLevels & (int)GclBitMask.IndependentLiving);
            }
        }

        public bool IsAssistedLiving {
            get {
                return 0 != (GeneralCareLevels & (int)GclBitMask.AssistedLiving);
            }
        }

        public bool IsSkilledNursing {
            get {
                return 0 != (GeneralCareLevels & (int)GclBitMask.SkilledNursing);
            }
        }

        public bool IsCCRC {
            get {
                return IntX.PopulationCount(GeneralCareLevels) > 1;
            }
        }

        // TODO: Add IsProfiled column - see SFS_SearchResults\View.ascx:1276 GetURLtoListingViewer()
        [Obsolete("This method was never implemented. It always returns true.")]
        public bool IsProfiled {
            get {
                return true;
            }
        }

        // TODO: Add HasExternalPartyMembership column - see SFS_SearchResults\View.ascx:1286 GetURLtoListingViewer()
        [Obsolete("This method was never implemented.  It always returns true.")]
        public bool HasExternalPartyMembership {
            get {
                return true;
            }
        }

        #endregion

        internal const int ProfileTemplateLabelsCol = 21;
        internal const string ProfileTemplateLabelsColName = "ProfileTemplateLabels";
        internal static readonly char[] ProfileTemplateLabelSeparator = new char[] { ',' };
        private string[] mProfileTemplateLabels = null;
        internal string[] ProfileTemplateLabels {
            get {
                if (null == mProfileTemplateLabels) {
                    string ptls = As<string>(mSummaryRow[ProfileTemplateLabelsCol]);
                    if (String.IsNullOrEmpty(ptls)) {
                        mProfileTemplateLabels = new string[0];
                    } else {
                        mProfileTemplateLabels = ptls.Split(
                            ProfileTemplateLabelSeparator,
                            StringSplitOptions.RemoveEmptyEntries);
                    }
                }

                return mProfileTemplateLabels;
            }
        }

        public bool Offers(string profileTemplateLabel) {
            return -1 < Array.IndexOf(ProfileTemplateLabels, profileTemplateLabel);
        }

        #region Featured-ness

        /// <summary>
        /// The DateTime since which the Party has been featured.  See 
        /// ResultSet.LoadFeaturedSinceDates() for more information.  If the
        /// Party is not featured, this property returns null.
        /// </summary>
        public DateTime? FeaturedSince { get; internal set; }

        /// <summary>
        /// True iff this Party is featured.
        /// </summary>
        public bool IsFeatured { get { return FeaturedSince.HasValue; } }

        #endregion
    }
}
