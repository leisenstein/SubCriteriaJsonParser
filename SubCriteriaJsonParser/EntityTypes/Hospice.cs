using System;
using System.Collections.Generic;
using System.Text;

using Snap.Search2;

namespace Snap.ForSeniors.Search2 {
    public class Hospice : ISearch {
        private static readonly Aggregate sExpressionTree = new NoopAggregate();

        public Aggregate ExpressionTree {
            get {
                return sExpressionTree;
            }
        }

        public string ProfileTemplateLabel {
            get {
                return Snap.ForSeniors.Constants.HospiceProfileTemplateLabel;
            }
        }

        public GeoSearchType GeoSearch {
            get {
                return GeoSearchType.ServiceArea;
            }
        }

        public ResultSet Search(Params ps) {
            return Snap.Search2.Search.Do<Hospice>(ps);
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

            return Snap.Search2.Search.Do<Hospice>(
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
