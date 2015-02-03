using System;
using System.Collections.Generic;
using System.Text;

using Snap.Search2;
using Snap.Geography;

namespace Snap.ForSeniors.Search2
{
    public class HyperbaricTreatmentCenter : ISearch
    {
        private static readonly Aggregate sExpressionTree = new NoopAggregate();

        public Aggregate ExpressionTree {
            get {
                return sExpressionTree;
            }
        }

        public string ProfileTemplateLabel {
            get {
                return Snap.ForSeniors.Constants.HyperbaricTreatmentCenterProfileTemplateLabel;
            }
        }

        public GeoSearchType GeoSearch {
            get {
                return GeoSearchType.Point;
            }
        }

        public ResultSet Search(Params ps) {
            return Snap.Search2.Search.Do<HyperbaricTreatmentCenter>(ps);
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


            return Snap.Search2.Search.Do<HyperbaricTreatmentCenter>(
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
