using System;
using System.Collections.Generic;

using Snap.Util;

namespace Snap.Search2 {

    /// <summary>
    /// Collection of parameters supplied to the Search.Search() method.
    /// </summary>
    public class Params {
        private static readonly string[] NoNoiseWords = new string[0];
        private static readonly Dictionary<string, string> NoConsumerProperties =
            new Dictionary<string, string>(0);
        private static readonly Comparison<Result> DefaultSort =
            Result.NameAscending;
        private const int DefaultMaxResultCount = 1000;

        // TODO: documentation
        public ITimer timerContext = null;
        public IEnumerable<string> nameSearchNoiseWords = NoNoiseWords;
        public Dictionary<string, string> consumerProperties = NoConsumerProperties;
        public Comparison<Result> sortOrder = DefaultSort;
        public Snap.Geography.AddressParserResult searchLocale = null;
        public int desiredResultCount = DefaultMaxResultCount;
        public int maxResultCount = DefaultMaxResultCount;
        public List<int> externalPartyMembershipTypeIds = null;
        public IntSet<PartyId> profileSpecificParties = IntSet<PartyId>.Empty;
        public IntSet<ProfileId> profileSpecificiProfiles = IntSet<ProfileId>.Empty;
        public string featuredProfileTemplateLabel = null;
    }
}
