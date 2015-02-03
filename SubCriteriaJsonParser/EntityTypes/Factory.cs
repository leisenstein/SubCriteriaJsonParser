using System;
using System.Collections.Generic;

using Snap.ForSeniors;
using Snap.Search2;

namespace Snap.ForSeniors.Search2 {

    /// <summary>
    /// Creates ISearch instances tailed to searches for Parties having specific
    /// Profile types.
    /// </summary>
    public static class Factory {

        /// <summary>
        /// Given a ProfileTemplateLabel (PTL), create an ISearch that will 
        /// return information about Parties having said PTL.
        /// </summary>
        public static ISearch Create(string profileTemplateLabel) {
            try {
                return sPtlToSearch[profileTemplateLabel];
            } catch (KeyNotFoundException exc) {
                throw new ArgumentException(
                    "Unexpected Profile Template Label value: " +
                    profileTemplateLabel,
                    exc);
            }
        }

        /// <summary>
        /// Maps Profile Template Label to GeoSearchType
        /// </summary>
        public static Dictionary<string, GeoSearchType> GeoSearchType {
            get {
                return new Dictionary<string, GeoSearchType>(sPtlToGeoSearch);
            }
        }

        /// <summary>
        /// These, and only these, ISearches will be available.  If it's not in
        /// this list, the factory can't instantiate it.  If the factory can't
        /// instantiate it, it won't be available to search clients.
        /// </summary>
        private static readonly ISearch[] sSearchConfigs = new ISearch[] {
            new AdultDayCare(),
            new AgingAndDisabilityResourceCenter(),
            new AlzheimersChapter(),
            new AreaAgencyOnAging(),
            new ElderLaw(),
            new Entertainment(),
            new Financial(),
            new GeriatricCareManager(),
            new HomeCare(),
            new HomeHealthCare(),
            new Hospice(),
            new Hospital(),
            new Insurance(),
            new LongTermSeniorHousing(),
            new MedicalEquipment(),
            new Pharmacy(),
            new RealEstate(),
            new Relocation(),
            new Security(),
            new SocialWorker(),
            new StateUnitOnAging(),
            new SupportServices(),
            new TitleVI(),
            new Transportation(),
            new MedicareDialysis(),
            new WoundCareCenter(),
            new HyperbaricTreatmentCenter(),
            new VoluntaryHealthOrganization(),
            new DiabetesEducationCenter(),
            new DiabetesEducator(),
            new SkuMcfBasic(),

            new Everything(), // Shh!  It's a secret!
        };

        /// <summary>
        /// Maps Profile Template Label to ISearch.
        /// </summary>
        private static readonly Dictionary<string, ISearch> sPtlToSearch;

        /// <summary>
        /// Maps Profile Template Label to GeoSearchType
        /// </summary>
        private static readonly Dictionary<string, GeoSearchType> sPtlToGeoSearch;

        static Factory() {
            sPtlToSearch = Snap.Util.DictionaryX.UniqueIndex(
                sSearchConfigs,
                (sc) => sc.ProfileTemplateLabel);
            sPtlToGeoSearch = Snap.Util.DictionaryX.UniqueIndex(
                sSearchConfigs,
                (sc) => sc.ProfileTemplateLabel,
                (sc) => sc.GeoSearch);
        }
    }
}
