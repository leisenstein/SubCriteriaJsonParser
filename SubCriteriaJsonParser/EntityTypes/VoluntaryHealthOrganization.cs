using System;
using System.Collections.Generic;
using System.Text;

using Snap.Search2;
using Snap.Geography;

namespace Snap.ForSeniors.Search2
{
    public class VoluntaryHealthOrganization : ISearch
    {
        private static readonly Aggregate sExpressionTree = new Intersection(

            // Health-related Focus
            new Criterion("addiction", "addiction", Boolean.TrueString),
            new Criterion("alzheimers-dementia-memory", "alzheimers-dementia-memory", Boolean.TrueString),
            new Criterion("amputation", "amputation", Boolean.TrueString),
            new Criterion("blood-disorder", "blood-disorder", Boolean.TrueString),
            new Criterion("bone-joint", "bone-joint", Boolean.TrueString),
            new Criterion("nervous-system-conditions", "nervous-system-conditions", Boolean.TrueString),
            new Criterion("cancer", "cancer", Boolean.TrueString),
            new Criterion("dental", "dental", Boolean.TrueString),
            new Criterion("digestive", "digestive", Boolean.TrueString),
            new Criterion("endocrine", "endocrine", Boolean.TrueString),
            new Criterion("heart-circulatory", "heart-circulatory", Boolean.TrueString),
            new Criterion("immune-disorders", "immune-disorders", Boolean.TrueString),
            new Criterion("kidney-urological-ostomy", "kidney-urological-ostomy", Boolean.TrueString),
            new Criterion("lung-respiratory", "lung-respiratory", Boolean.TrueString),
            new Criterion("mental-health", "mental-health", Boolean.TrueString),
            new Criterion("muscular", "muscular", Boolean.TrueString),
            new Criterion("nutrition-food", "nutrition-food", Boolean.TrueString),
            new Criterion("pain", "pain", Boolean.TrueString),
            new Criterion("palliative", "palliative", Boolean.TrueString),
            new Criterion("reproductive-system", "reproductive-system", Boolean.TrueString),
            new Criterion("skin", "skin", Boolean.TrueString),
            new Criterion("speech-language-hearing", "speech-language-hearing", Boolean.TrueString),
            new Criterion("vision", "vision", Boolean.TrueString),

            // Care and Assistance
            new Criterion("focus-abuse", "focus-abuse", Boolean.TrueString),
            new Criterion("focus-assistive-devices", "focus-assistive-devices", Boolean.TrueString),
            new Criterion("focus-caregiver-support", "focus-caregiver-support", Boolean.TrueString),
            new Criterion("focus-case-management", "focus-case-management", Boolean.TrueString),
            new Criterion("focus-emergency-support", "focus-emergency-support", Boolean.TrueString),
            new Criterion("focus-financial-services", "focus-financial-services", Boolean.TrueString),
            new Criterion("focus-wellness", "focus-wellness", Boolean.TrueString),
            new Criterion("focus-housing-and-care-services", "focus-housing-and-care-services", Boolean.TrueString),
            new Criterion("focus-info-and-referral", "focus-info-and-referral", Boolean.TrueString),
            new Criterion("focus-insurance", "focus-insurance", Boolean.TrueString),
            new Criterion("focus-legal", "focus-legal", Boolean.TrueString),
            new Criterion("focus-low-income", "focus-low-income", Boolean.TrueString),
            new Criterion("focus-professionals", "focus-professionals", Boolean.TrueString),
            new Criterion("focus-spiritual", "focus-spiritual", Boolean.TrueString)
        );

        public Aggregate ExpressionTree {
            get {
                return sExpressionTree;
            }
        }

        public string ProfileTemplateLabel {
            get {
                return Snap.ForSeniors.Constants.VoluntaryHealthOrganizationProfileTemplateLabel;
            }
        }

        public GeoSearchType GeoSearch {
            get {
                return GeoSearchType.ServiceArea;
            }
        }

        public ResultSet Search(Params ps) {
            return Snap.Search2.Search.Do<VoluntaryHealthOrganization>(ps);
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


            return Snap.Search2.Search.Do<VoluntaryHealthOrganization>(
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
