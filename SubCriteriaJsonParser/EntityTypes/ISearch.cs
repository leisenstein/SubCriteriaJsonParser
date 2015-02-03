using System;
using System.Collections.Generic;

using Snap.Util;
using Snap.Geography;

namespace Snap.Search2 {

    public enum GeoSearchType {
        Point,
        ServiceArea,
        All
    }

    /// <summary>
    /// A type argument to Search that specifies the ProfileTemplateLabel and
    /// Profile-specific ExpressionTree that will be used to find Parties that
    /// satisfy the searcher's Profile-related criteria.
    /// </summary>
    public interface ISearch {

        /// <summary>
        /// A static ExpressionTree.  Typically, you would define a static 
        /// readonly member in an implementing class and it would be returned by
        /// this property getter every time.  It's important to note that you
        /// SHOULD NOT change this expression at runtime to effect clever 
        /// results; Search2.Search.Do() will instantiate this class using the 
        /// default constructor for the class, then access this property through
        /// the new instance.
        /// </summary>
        Aggregate ExpressionTree { get; }

        GeoSearchType GeoSearch { get; }

        /// <summary>
        /// The name of an existing ProfileTemplate.  Typically, you would 
        /// define a static readonly member in an implementing class and it
        /// would be returned by this property getter every time.  It's
        /// important to note that you SHOULD NOT change this expression at
        /// runtime to effect clever results; Search2.Search.Do() will
        /// instantiate this class using the default constructor for the class,
        /// then access this property through the new instance.
        /// </summary>
        string ProfileTemplateLabel { get; }

        /// <summary>
        /// A convenient wrapper around Search2.Search.Do().
        /// </summary>
        [Obsolete("Prefer Search(Snap.Search2.Params)")]
        ResultSet Search(
            ITimer timerContext,
            IEnumerable<string> nameSearchNoiseWords,
            Dictionary<string, string> consumerProperties,
            Comparison<Result> sortOrder,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            List<int> externalPartyMembershipTypeIds);

        ResultSet Search(Params ps);
    }
}
