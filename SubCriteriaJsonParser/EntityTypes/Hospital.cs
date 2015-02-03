using System;
using System.Collections.Generic;

using Snap.Search2;

namespace Snap.ForSeniors.Search2 {
    public class Hospital : ISearch {
        private static readonly Aggregate sExpressionTree = new Intersection(

            // Hospital Type
            new Criterion("acute-care-hospital",          "type", "acute-care-hospital"),
            new Criterion("critical-access-hospital",     "type", "critical-access-hospital"),
            new Criterion("acute-care-va-medical-center", "type", "acute-care-va-medical-center"),
            new Criterion("childrens-hospital",           "type", "childrens-hospital"),

            // Emergency Service
            new Criterion("emergency-service","emergency-service",Boolean.TrueString),

            // Registries
            new Criterion("cardiac-surgery-registry", "cardiac-surgery-registry", Boolean.TrueString),
            new Criterion("stroke-care-registry",     "stroke-care-registry",     Boolean.TrueString),
            new Criterion("nursing-care-registry",    "nursing-care-registry",    Boolean.TrueString),

            // Hospital Ownership
            new Criterion("ownership-government-federal",                        "ownership", "government-federal"),
            new Criterion("ownership-government-hospital-district-or-authority", "ownership", "government-hospital-district-or-authority"),
            new Criterion("ownership-government-local",                          "ownership", "government-local"),
            new Criterion("ownership-government-state",                          "ownership", "government-state"),
            new Criterion("ownership-proprietary",                               "ownership", "proprietary"),
            new Criterion("ownership-voluntary-non-profit-church",               "ownership", "voluntary-non-profit-church"),
            new Criterion("ownership-voluntary-non-profit-other",                "ownership", "voluntary-non-profit-other"),
            new Criterion("ownership-voluntary-non-profit-private",              "ownership", "voluntary-non-profit-private")

            );

        public Aggregate ExpressionTree {
            get {
                return sExpressionTree;
            }
        }

        public string ProfileTemplateLabel {
            get {
                return Snap.ForSeniors.Constants.HospitalProfileTemplateLabel;
            }
        }

        public GeoSearchType GeoSearch {
            get {
                return GeoSearchType.Point;
            }
        }

        public ResultSet Search(Params ps) {
            return Snap.Search2.Search.Do<Hospital>(ps);
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

            return Snap.Search2.Search.Do<Hospital>(
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
