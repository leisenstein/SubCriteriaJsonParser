using System;
using System.Collections.Generic;

using Snap.Search2;
using Snap.Util;
using Snap.Geography;

namespace Snap.ForSeniors.Search2 {

    public class HomeHealthCare : ISearch {

        private static readonly Aggregate sExpressionTree = new Intersection(
            new Criterion("nursing-care", "nursing-care", Boolean.TrueString),
            new Criterion("physical-therapy", "physical-therapy", Boolean.TrueString),
            new Criterion("occupational-therapy", "occupational-therapy", Boolean.TrueString),
            new Criterion("speech-therapy", "speech-therapy", Boolean.TrueString),
            new Criterion("medical-social", "medical-social", Boolean.TrueString)
            );

        public Aggregate ExpressionTree {
            get { 
                return sExpressionTree;
            }
        }

        public string ProfileTemplateLabel {
            get {
                return Snap.ForSeniors.Constants.HomeHealthCareProfileTemplateLabel; 
            }
        }

        public GeoSearchType GeoSearch {
            get {
                return GeoSearchType.ServiceArea;
            }
        }

        public ResultSet Search(Params ps) {
            return Snap.Search2.Search.Do<HomeHealthCare>(ps);
        }

        public ResultSet Search(
            ITimer timerContext,
            IEnumerable<string> nameSearchNoiseWords,
            Dictionary<string, string> consumerProperties,
            Comparison<Result> sortOrder,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            List<int> externalPartyMembershipTypeIds) {

            return Snap.Search2.Search.Do<HomeHealthCare>(
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
