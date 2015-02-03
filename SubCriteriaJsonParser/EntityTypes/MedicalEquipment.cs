using System;
using System.Collections.Generic;

using Snap.Search2;

namespace Snap.ForSeniors.Search2 {
    public class MedicalEquipment : ISearch {
        private static readonly Aggregate sExpressionTree = new Intersection(
            new Criterion("medicare", "medicare", Boolean.TrueString),
            new Criterion("breast-prostheses-and-accessories", "breast-prostheses-and-accessories", Boolean.TrueString),
            new Criterion("cochlear-implants", "cochlear-implants", Boolean.TrueString),
            new Criterion("eye-prostheses", "eye-prostheses", Boolean.TrueString),
            new Criterion("facial-prostheses", "facial-prostheses", Boolean.TrueString),
            new Criterion("limb-prostheses", "limb-prostheses", Boolean.TrueString),
            new Criterion("neurostimulators", "neurostimulators", Boolean.TrueString),
            new Criterion("ocular-prostheses", "ocular-prostheses", Boolean.TrueString),
            new Criterion("ostomy-supplies", "ostomy-supplies", Boolean.TrueString),
            new Criterion("contact-lenses", "contact-lenses", Boolean.TrueString),
            new Criterion("eyeglasses", "eyeglasses", Boolean.TrueString),
            new Criterion("cataract-lenses", "cataract-lenses", Boolean.TrueString),
            new Criterion("somatic-prostheses", "somatic-prostheses", Boolean.TrueString),
            new Criterion("tracheostomy-supplies", "tracheostomy-supplies", Boolean.TrueString),
            new Criterion("urological-supplies", "urological-supplies", Boolean.TrueString),
            new Criterion("voice-prosthetics", "voice-prosthetics", Boolean.TrueString),
            new Criterion("automatic-external-defibrillators", "automatic-external-defibrillators", Boolean.TrueString),
            new Criterion("mail-order-blood-glucose-monitors-and-supplies", "mail-order-blood-glucose-monitors-and-supplies", Boolean.TrueString),
            new Criterion("non-mail-order-blood-glucose-monitors-and-supplies", "non-mail-order-blood-glucose-monitors-and-supplies", Boolean.TrueString),
            new Criterion("commodes-urinals-and-bedpans", "commodes-urinals-and-bedpans", Boolean.TrueString),
            new Criterion("continuous-passive-motion-devices", "continuous-passive-motion-devices", Boolean.TrueString),
            new Criterion("dynamic-splints", "dynamic-splints", Boolean.TrueString),
            new Criterion("gastric-suction-pumps", "gastric-suction-pumps", Boolean.TrueString),
            new Criterion("heat-and-cold-applications", "heat-and-cold-applications", Boolean.TrueString),
            new Criterion("electric-hospital-beds", "electric-hospital-beds", Boolean.TrueString),
            new Criterion("manual-hospital-beds", "manual-hospital-beds", Boolean.TrueString),
            new Criterion("infrared-heating-pad-systems", "infrared-heating-pad-systems", Boolean.TrueString),
            new Criterion("external-infusion-pumps-and-supplies", "external-infusion-pumps-and-supplies", Boolean.TrueString),
            new Criterion("implanted-infusion-pumps-and-supplies", "implanted-infusion-pumps-and-supplies", Boolean.TrueString),
            new Criterion("insulin-infusion-pumps-and-supplies", "insulin-infusion-pumps-and-supplies", Boolean.TrueString),
            new Criterion("negative-pressure-wound-therapy-pumps-and-supplies", "negative-pressure-wound-therapy-pumps-and-supplies", Boolean.TrueString),
            new Criterion("neuromuscular-electrical-stimulators", "neuromuscular-electrical-stimulators", Boolean.TrueString),
            new Criterion("osteogenesis-stimulators", "osteogenesis-stimulators", Boolean.TrueString),
            new Criterion("pneumatic-compression-devices", "pneumatic-compression-devices", Boolean.TrueString),
            new Criterion("speech-generating-devices", "speech-generating-devices", Boolean.TrueString),
            new Criterion("pressure-reducing-beds-mattresses-overlays-and-pads", "pressure-reducing-beds-mattresses-overlays-and-pads", Boolean.TrueString),
            new Criterion("traction-equipment", "traction-equipment", Boolean.TrueString),
            new Criterion("transcutaneous-electrical-nerve-stimulators", "transcutaneous-electrical-nerve-stimulators", Boolean.TrueString),
            new Criterion("ultraviolet-light-devices", "ultraviolet-light-devices", Boolean.TrueString),
            new Criterion("canes-and-crutches", "canes-and-crutches", Boolean.TrueString),
            new Criterion("patient-lifts", "patient-lifts", Boolean.TrueString),
            new Criterion("power-operated-vehicles", "power-operated-vehicles", Boolean.TrueString),
            new Criterion("seat-lift-mechanisms", "seat-lift-mechanisms", Boolean.TrueString),
            new Criterion("walkers", "walkers", Boolean.TrueString),
            new Criterion("wheelchair-seating-and-cushions", "wheelchair-seating-and-cushions", Boolean.TrueString),
            new Criterion("manual-wheelchairs-and-accessories", "manual-wheelchairs-and-accessories", Boolean.TrueString),
            new Criterion("power-wheelchairs-and-accessories", "power-wheelchairs-and-accessories", Boolean.TrueString),
            new Criterion("manual-complex-rehabilitative-wheelchairs-and-accessories", "manual-complex-rehabilitative-wheelchairs-and-accessories", Boolean.TrueString),
            new Criterion("power-complex-rehabilitative-wheelchairs-and-accessories", "power-complex-rehabilitative-wheelchairs-and-accessories", Boolean.TrueString),
            new Criterion("custom-fabricated-diabetic-shoes-and-inserts", "custom-fabricated-diabetic-shoes-and-inserts", Boolean.TrueString),
            new Criterion("prefabricated-diabetic-shoes-and-inserts", "prefabricated-diabetic-shoes-and-inserts", Boolean.TrueString),
            new Criterion("surgical-dressings", "surgical-dressings", Boolean.TrueString),
            new Criterion("high-frequency-chest-wall-oscillation-devices", "high-frequency-chest-wall-oscillation-devices", Boolean.TrueString),
            new Criterion("intermittent-positive-pressure-breathing-devices", "intermittent-positive-pressure-breathing-devices", Boolean.TrueString),
            new Criterion("intrapulmonary-percussive-ventilation-devices", "intrapulmonary-percussive-ventilation-devices", Boolean.TrueString),
            new Criterion("invasive-mechanical-ventilation", "invasive-mechanical-ventilation", Boolean.TrueString),
            new Criterion("mechanical-in-exsufflation-devices", "mechanical-in-exsufflation-devices", Boolean.TrueString),
            new Criterion("nebulizer-equipment-and-supplies", "nebulizer-equipment-and-supplies", Boolean.TrueString),
            new Criterion("oxygen-equipment-and-supplies", "oxygen-equipment-and-supplies", Boolean.TrueString),
            new Criterion("respiratory-suction-pumps", "respiratory-suction-pumps", Boolean.TrueString),
            new Criterion("ventilators-accessories-and-supplies", "ventilators-accessories-and-supplies", Boolean.TrueString),
            new Criterion("cpap-rads-and-accessories", "cpap-rads-and-accessories", Boolean.TrueString),
            new Criterion("entereal-nutrientes-equipment-and-supplies", "entereal-nutrientes-equipment-and-supplies", Boolean.TrueString),
            new Criterion("parentereal-nutrientes-equipment-and-supplies", "parentereal-nutrientes-equipment-and-supplies", Boolean.TrueString),
            new Criterion("custom-fabricated-orthoses", "custom-fabricated-orthoses", Boolean.TrueString),
            new Criterion("off-the-shelf-orthoses", "off-the-shelf-orthoses", Boolean.TrueString),
            new Criterion("prefabricated-orthoses", "prefabricated-orthoses", Boolean.TrueString),
            new Criterion("hemiodialysis-equipment-and-supplies", "hemiodialysis-equipment-and-supplies", Boolean.TrueString),
            new Criterion("home-dialysis-equipment-and-supplies", "home-dialysis-equipment-and-supplies", Boolean.TrueString)
       );

        public Aggregate ExpressionTree {
            get {
                return sExpressionTree;
            }
        }

        public string ProfileTemplateLabel {
            get {
                return Snap.ForSeniors.Constants.MedicalEquipmentProfileTemplateLabel;
            }
        }

        public GeoSearchType GeoSearch {
            get {
                return GeoSearchType.Point;
            }
        }

        public ResultSet Search(Params ps) {
            return Snap.Search2.Search.Do<MedicalEquipment>(ps);
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

            return Snap.Search2.Search.Do<MedicalEquipment>(
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
