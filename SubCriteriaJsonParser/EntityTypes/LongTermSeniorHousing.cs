using System;
using System.Collections.Generic;

using Snap.Search2;
using Snap.Util;
using Snap.Geography;

namespace Snap.ForSeniors.Search2 {
    public class LongTermSeniorHousing : ISearch {
        public static readonly string GENERAL_CARE_LEVEL = "general-care-level";

        public static readonly string AL_VIRTUAL = "assisted-living";
        public static readonly string SN_VIRTUAL = "skilled-nursing";
        public static readonly string IL_VIRTUAL = "independent-living";
        public static readonly string CCRC_VIRTUAL = "ccrc";

        #region Expression Tree
        private static readonly Aggregate sExpressionTree = new Intersection(
            // Alzheimer's
            new Union( // any care
                new Criterion("alzheimers", "alzheimers-care", "mixed"),
                new Criterion("alzheimers", "alzheimers-care", "dedicated-area"),
                new Criterion("alzheimers", "alzheimers-care", "dedicated-facility")
                ),
            new Union( // dedicated unit or facility only
                new Criterion("alzheimers-dedicated", "alzheimers-care", "dedicated-area"),
                new Criterion("alzheimers-dedicated", "alzheimers-care", "dedicated-facility")
                ),

            new Union( // Housing
                new Criterion("studio", "studio-apartment-available", Boolean.TrueString),
                new Criterion("one-bedroom", "one-bedroom-apartment-available", Boolean.TrueString),
                new Criterion("two-bedroom", "two-plus-bedroom-apartment-available", Boolean.TrueString),
                new Union(
                    new Criterion("shared-residence", "priv-room-priv-bath-available", Boolean.TrueString),
                    new Criterion("shared-residence", "priv-room-shared-bath-available", Boolean.TrueString),
                    new Criterion("shared-residence", "shared-room-available", Boolean.TrueString)
                    ),
                new Union(
                    new Criterion("house", "one-bedroom-house-available", Boolean.TrueString),
                    new Criterion("house", "two-plus-bedroom-house-available", Boolean.TrueString)
                    ),
                new Criterion("house-one-bedroom", "one-bedroom-house-available", Boolean.TrueString),
                new Criterion("house-two-bedroom", "two-plus-bedroom-house-available", Boolean.TrueString),
                new Criterion("priv-room-priv-bath", "priv-room-priv-bath-available", Boolean.TrueString),
                new Criterion("priv-room-shared-bath", "priv-room-shared-bath-available", Boolean.TrueString),
                new Criterion("shared-room", "shared-room-available", Boolean.TrueString)
                ),
            new Union( // Housekeeping, Laundry
                new Criterion("housekeeping-laundry", "onsite-housekeeping", Boolean.TrueString),
                new Criterion("housekeeping-laundry", "laundry-service", Boolean.TrueString)
                ),

            // Payment
            new Criterion("medicare-payment", "payment-medicare", Boolean.TrueString),
            new Criterion("medicaid-payment", "payment-medicaid", Boolean.TrueString),
            new Criterion("va-benefits", "payment-va-benefits", Boolean.TrueString),
            new Criterion("hud-or-other-subsidized-payment", "payment-subsidized-unit", Boolean.TrueString),
            new Criterion("income-qualified", "variable-rent-for-qualified-residents", Boolean.TrueString),
            new Criterion("private-payment", "payment-private-funds", Boolean.TrueString),
            new Criterion("ltc-insurance", "payment-long-term-care-insurance", Boolean.TrueString),

            // Skilled Nursing Facility-specific
            new Criterion("short-term-rehabilitation", "temporary-rehabilitation", Boolean.TrueString),
            new Criterion("physical-therapy", "on-site-physical-therapy", Boolean.TrueString),
            new Criterion("occupational-therapy", "on-site-occupational-therapy", Boolean.TrueString),
            new Criterion("speech-therapy", "on-site-speech-therapy", Boolean.TrueString),

            // Independent Living-specific
            new Criterion("beautician-or-barber", "onsite-beautician-or-barber", Boolean.TrueString),
            new Criterion("library", "onsite-library", Boolean.TrueString),
            new Criterion("maintenance-director", "onsite-maintenance-director", Boolean.TrueString),
            new Criterion("air-conditioned", "units-are-air-conditioned", Boolean.TrueString),
            new Criterion("may-bring-furniture", "resident-may-bring-furniture", Boolean.TrueString),
            new Criterion("cable-ready", "units-are-cable-ready", Boolean.TrueString),
            new Criterion("climate-control", "per-unit-climate-control", Boolean.TrueString),
            new Criterion("wheelchair-accessible", "units-are-wheelchair-accessible", Boolean.TrueString),
            new Criterion("emergency-call-system", "per-unit-emergency-call-system", Boolean.TrueString),
            new Criterion("washer-dryer-hook-up", "units-have-washer-dryer-hook-up", Boolean.TrueString),

            new Criterion("outside-services", "outside-agency-services-allowed", Boolean.TrueString),
            new Criterion("short-term-respite-care", "temporary-respite", Boolean.TrueString),
            new Criterion("assistance-catheter", "assistance-catheter", Boolean.TrueString),
            new Criterion("colostomy-or-urostomy", "colostomy-urostomy", Boolean.TrueString),
            new Criterion("toileting", "assistance-toileting", Boolean.TrueString),
            new Criterion("mobility", "assistance-mobility", Boolean.TrueString),
            new Criterion("assistance-daily-activities", "assistance-activities-of-daily-living", Boolean.TrueString),
            new Criterion("foot-care", "assistance-foot-care", Boolean.TrueString),
            new Criterion("medication-management", "medication-management", Boolean.TrueString),
            new Criterion("public-transportation-access", "public-transportation-access", Boolean.TrueString),
            new Criterion("facility-provides-transportation", "facility-provides-transportation", Boolean.TrueString),
            new Criterion("resident-parking", "resident-parking", Boolean.TrueString),
            new Criterion("guest-parking", "guest-parking", Boolean.TrueString),
            new Criterion("arthritis", "arthritis", Boolean.TrueString),
            new Criterion("asthma", "asthma", Boolean.TrueString),
            new Criterion("bariatric", "bariatric", Boolean.TrueString),
            new Criterion("psychiatric-conditions", "psychiatric-condition-care", Boolean.TrueString),
            new Criterion("oxygen", "breathing-support-oxygen", Boolean.TrueString),
            new Criterion("ventilator", "breathing-support-ventilator", Boolean.TrueString),
            new Criterion("broken-bones", "broken-bones", Boolean.TrueString),
            new Criterion("cancer", "cancer", Boolean.TrueString),
            new Criterion("depression", "depression", Boolean.TrueString),
            new Criterion("developmental-disabilities", "developmental-disabilities", Boolean.TrueString),
            new Criterion("diabetes-diet", "diabetes-care-diet", Boolean.TrueString),
            new Criterion("diabetes-oral-med", "diabetes-care-oral-medication", Boolean.TrueString),
            new Criterion("diabetes-staff-inject", "diabetes-care-staff-injected", Boolean.TrueString),
            new Criterion("diabetes-self-inject", "diabetes-care-self-injected-insulin", Boolean.TrueString),
            new Criterion("dialysis-on-site", "dialysis-on-site", Boolean.TrueString),
            new Criterion("lung-disease", "lung-disease", Boolean.TrueString),
            new Criterion("heart-disease-or-heart-failure", "heart-disease-or-heart-failure", Boolean.TrueString),
            new Criterion("memory-loss", "memory-loss", Boolean.TrueString),
            new Criterion("food-and-liquid-intake-monitoring", "food-and-liquid-intake-monitoring", Boolean.TrueString),
            new Criterion("multiple-sclerosis", "multiple-sclerosis", Boolean.TrueString),
            new Criterion("osteoporosis", "osteoporosis", Boolean.TrueString),
            new Criterion("fall-tendency", "tendency-to-fall", Boolean.TrueString),
            new Criterion("parkinsons", "parkinsons", Boolean.TrueString),
            new Criterion("stroke", "stroke", Boolean.TrueString),
            new Criterion("less-than-8-hours", "licensed-nursing-care", "8-hours-per-day-or-less"),
            new Criterion("less-than-24-hours", "licensed-nursing-care", "8-to-24-hours-per-day"),
            new Criterion("constant", "licensed-nursing-care", "24-hours-per-day"),
            new Criterion("ethnic-diet", "ethnic-diet-available", Boolean.TrueString),
            new Criterion("low-sugar", "low-sugar-diet-available", Boolean.TrueString),
            new Criterion("vegetarian", "vegetarian-diet-available", Boolean.TrueString),
            new Criterion("low-sodium", "low-sodium-diet-available", Boolean.TrueString),

            // Languages
            new Criterion("arabic", "arabic", Boolean.TrueString),
            new Criterion("cantonese", "cantonese", Boolean.TrueString),
            new Criterion("czech", "czech", Boolean.TrueString),
            new Criterion("dutch", "dutch", Boolean.TrueString),
            new Criterion("english","english",Boolean.TrueString),
            new Criterion("french", "french", Boolean.TrueString),
            new Criterion("german", "german", Boolean.TrueString),
            new Criterion("greek", "greek", Boolean.TrueString),
            new Criterion("hebrew","hebrew",Boolean.TrueString),
            new Criterion("hindi", "hindi", Boolean.TrueString),
            new Criterion("italian","italian",Boolean.TrueString),
            new Criterion("japanese", "japanese", Boolean.TrueString),
            new Criterion("korean", "korean", Boolean.TrueString),
            new Criterion("mandarin", "mandarin", Boolean.TrueString),
            new Criterion("farsi","farsi",Boolean.TrueString),
            new Criterion("polish","polish",Boolean.TrueString),
            new Criterion("portuguese", "portuguese", Boolean.TrueString),
            new Criterion("romanian", "romanian", Boolean.TrueString),
            new Criterion("russian", "russian", Boolean.TrueString),
            new Criterion("ameslan", "american-sign-language", Boolean.TrueString),
            new Criterion("spanish", "spanish", Boolean.TrueString),
            new Criterion("tagalog", "tagalog", Boolean.TrueString),
            new Criterion("thai", "thai", Boolean.TrueString),
            new Criterion("vietnamese", "vietnamese", Boolean.TrueString),

            // Alzheimer's Assoc. Portal-specific
            new Criterion("wander", "behavior-care-wander", Boolean.TrueString),
            new Union( // physically/verbally aggressive (aggro)
                new Criterion("aggro", "behavior-care-verbally-aggressive", Boolean.TrueString),
                new Criterion("aggro", "behavior-care-physically-aggressive", Boolean.TrueString)
                ),
            new Union( // nursing
                new Criterion("nursing", "licensed-nursing-care", "8-to-24-hours-per-day"),
                new Criterion("nursing", "licensed-nursing-care", "24-hours-per-day")
                )
            );

        #endregion Expression Tree

        public Aggregate ExpressionTree {
            get {
                return sExpressionTree;
            }
        }

        public string ProfileTemplateLabel {
            get {
                return Snap.ForSeniors.Constants.LongTermHousingProfileTemplateLabel;
            }
        }

        public GeoSearchType GeoSearch {
            get {
                return GeoSearchType.Point;
            }
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

            return Search(new Params() {
                timerContext                    = timerContext,
                nameSearchNoiseWords            = nameSearchNoiseWords,
                consumerProperties              = consumerProperties,
                sortOrder                       = sortOrder,
                searchLocale                    = searchLocale,
                desiredResultCount              = desiredResultCount,
                maxResultCount                  = maxResultCount,
                externalPartyMembershipTypeIds  = externalPartyMembershipTypeIds
            });
        }

        public ResultSet Search(Params ps) {
            return Search(
                ps,
                Contains(ps.consumerProperties, IL_VIRTUAL),
                Contains(ps.consumerProperties, AL_VIRTUAL),
                Contains(ps.consumerProperties, SN_VIRTUAL),
                Contains(ps.consumerProperties, CCRC_VIRTUAL));
        }

        public ResultSet Search(
            ITimer timerContext,
            IEnumerable<string> nameSearchNoiseWords,
            Dictionary<string, string> consumerProperties,
            Comparison<Result> sortOrder,
            bool showIndependentLiving,
            bool showAssistedLiving,
            bool showSkilledNursing,
            bool showOnlyCCRCs,
            AddressParserResult searchLocale,
            int desiredResultCount,
            int maxResultCount,
            List<int> externalPartyMembershipTypeIds) {

            var ps = new Params() {
                timerContext                    = timerContext,
                nameSearchNoiseWords            = nameSearchNoiseWords,
                consumerProperties              = consumerProperties,
                sortOrder                       = sortOrder,
                searchLocale                    = searchLocale,
                desiredResultCount              = desiredResultCount,
                maxResultCount                  = maxResultCount,
                externalPartyMembershipTypeIds  = externalPartyMembershipTypeIds,
            };

            return Search(ps, 
                showIndependentLiving, 
                showAssistedLiving, 
                showSkilledNursing, 
                showOnlyCCRCs);
        }

        public ResultSet Search(
            Params ps,
            bool showIndependentLiving,
            bool showAssistedLiving,
            bool showSkilledNursing,
            bool showOnlyCCRCs) {

            IntSet<ProfileId> gclMatches = null;
            IntSet<PartyId> ccrcs = null;
            if (showOnlyCCRCs) {
                ccrcs = Snap.Cache.Cache.CCRC;
                gclMatches = IntSet<ProfileId>.Universe;
            } else {
                gclMatches = IntSet<ProfileId>.UnionOfMany(
                    Snap.Search2.Search.UnionTerm<LongTermSeniorHousing>(
                        showAssistedLiving, GENERAL_CARE_LEVEL, AL_VIRTUAL),
                    Snap.Search2.Search.UnionTerm<LongTermSeniorHousing>(
                        showIndependentLiving, GENERAL_CARE_LEVEL, IL_VIRTUAL),
                    Snap.Search2.Search.UnionTerm<LongTermSeniorHousing>(
                        showSkilledNursing, GENERAL_CARE_LEVEL, SN_VIRTUAL));
            }

            ps.profileSpecificParties = ccrcs;
            ps.profileSpecificiProfiles = gclMatches;
            return Snap.Search2.Search.Do<LongTermSeniorHousing>(ps);
        }

        private static bool Contains(
            Dictionary<string, string> properties, string virtualProperty) {
            return DictionaryX.FindAll<string, string>(
                properties,
                delegate(KeyValuePair<string, string> pair) {
                    bool value;
                    return pair.Key == virtualProperty &&
                        Boolean.TryParse(pair.Value, out value) &&
                        value;
                }).Count > 0;
        }
    }
}
