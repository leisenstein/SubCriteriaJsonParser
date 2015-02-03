using System;
using System.Collections.Generic;

using Snap.Search2;

namespace Snap.ForSeniors.Search2 {
    public class Entertainment : ISearch {
        private static readonly Aggregate sExpressionTree = new NoopAggregate();

        public Aggregate ExpressionTree {
            get {
                return sExpressionTree;
            }
        }

        public string ProfileTemplateLabel {
            get {
                return Snap.ForSeniors.Constants.EntertainmentProfileTemplateLabel;
            }
        }

        public GeoSearchType GeoSearch {
            get {
                return GeoSearchType.ServiceArea;
            }
        }

        public ResultSet Search(Params ps) {
            return Snap.Search2.Search.Do<Entertainment>(ps);
        }

        public ResultSet Search(
            Snap.Util.ITimer timerContext,
            IEnumerable<string> nameSearchNoiseWords,
            Dictionary<string, string> consumerProperties,
            Comparison<Result> sortOrder,
            Snap.Geography.AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            List<int> externalPartyMembershipTypeIds) {

            return Snap.Search2.Search.Do<Entertainment>(
                timerContext,
                nameSearchNoiseWords,
                consumerProperties,
                sortOrder,
                searchLocale,
                desiredResultCount,
                maxResultCount,
                externalPartyMembershipTypeIds,
                null,
                null);
        }
    }
}
