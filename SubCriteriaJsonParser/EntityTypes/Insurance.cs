using System;
using System.Collections.Generic;

using Snap.Search2;

namespace Snap.ForSeniors.Search2 {
    public class Insurance : ISearch {
        private static readonly Aggregate sExpressionTree = new NoopAggregate();

        public Aggregate ExpressionTree {
            get {
                return sExpressionTree;
            }
        }

        public string ProfileTemplateLabel {
            get {
                return Snap.ForSeniors.Constants.InsuranceProfileTemplateLabel;
            }
        }

        public GeoSearchType GeoSearch {
            get {
                return GeoSearchType.ServiceArea;
            }
        }

        public ResultSet Search(Params ps) {
            return Snap.Search2.Search.Do<Insurance>(ps);
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

            return Snap.Search2.Search.Do<Insurance>(
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
