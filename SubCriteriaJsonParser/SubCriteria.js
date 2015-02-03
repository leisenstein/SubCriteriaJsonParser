

var alwaysVisible = [
     ["alzheimers", "Provides Memory Care",
                        "Provides Memory Care"],
     ["alzheimers-dedicated", "Secured Care Unit",
                        "Secured Care Unit"]
];


var careServices = [
            ["physical-therapy", "Physical Therapy",
                "Physical Therapy"],
            ["occupational-therapy", "Occupational Therapy",
                "Occupational Therapy"],
            ["speech-therapy", "Speech Therapy",
                "Speech Therapy"]
];

var shortTermStay = [
            ["short-term-respite-care", "Respite",
                "Respite"],
            ["short-term-rehabilitation", "Rehabilitation",
                "Rehabilitation"]
];

var roomAmenities = [
                ["air-conditioned", "Air Conditioning",
                    "Air Conditioning"],
                ["may-bring-furniture", "Residents may bring own furniture",
                    "Residents may bring own furniture"],
                ["cable-ready", "Cable hook up",
                    "Cable hook up"],
                ["climate-control", "Climate control thermostat",
                    "Climate control thermostat"],
                ["wheelchair-accessible", "Wheelchair accessible",
                    "Wheelchair accessible"],
                ["emergency-call-system", "Emergency call system",
                    "Emergency call system"],
                ["washer-dryer-hook-up", "Washer/dryer hookup",
                    "Washer/dryer hookup"]
];

var communityServices = [
                ["beautician-or-barber", "Beautician/barber",
                    "Beautician/barber"],
                ["library", "Library",
                    "Library"],
                ["maintenance-director", "Maintenance director",
                    "Maintenance director"]
];

var alzheimersCare = [
                    ["alzheimers", "Provides Memory Care",
                        "Provides Memory Care"],
                    ["alzheimers-dedicated", "Dedicated Care Unit",
                        "Dedicated Care Unit"]
];


var personalAssistance = [
                        ["assistance-activities-of-daily-living", "Activities of Daily Living",
                            "Activities of Daily Living (bathing, grooming, dressing, eating, etc.)"],
                        ["assistance-catheter", "Catheter management",
                            "A means to control incontinence; Managing a catheter may entail insertion, dressing changes, flushing, removal, etc."],
                        ["housekeeping-laundry", "Housekeeping/laundry",
                            "May include vacuuming, dusting, dish washing, bed making, etc."],
                        ["colostomy-or-urostomy", "Colostomy or Urostomy",
                            "A procedure that involves incisions in the colon or urinary system, respectively, to allow for drainage when the colon or urinary system is blocked or healing from injury or surgery. This assistance may include changing the collection bag, irrigation, and skin care around the incision."],
                        ["toileting", "Toileting",
                            "Toileting"],
                        ["mobility", "Mobility Assistance",
                            "Mobility Assistance"],
                        ["foot-care", "Foot Care",
                            "Foot Care"],
                        ["medication-management", "Medication management",
                            "May entail following a formalized procedure with a written set of rules, management of timing and dosage, filling prescriptions, reminders to take medication, keeping medications locked up, etc."]
];

var paymentAccepted = [
                        ["private-payment", "Private Pay",
                            "Private Pay"],
                        ["va-benefits", "Veterans Administration Benefits",
                            "Veterans Administration Benefits"],
                        ["hud-or-other-subsidized-payment", "HUD/Other Subsidies",
                            "Programs that accept federal and state money to provide affordable independent housing for the elderly and disabled. Those who qualify generally pay a portion of their monthly income towards rent"],
                        ["medicaid-payment", "Medicaid",
                            "Federally supported, state operated public assistance program that pays for health care services of people with a low income, including elderly or disabled persons, who qualify"],
                        ["ltc-insurance", "Long term care insurance",
                            "Long term care insurance"],
                        ["income-qualified", "Income Qualified",
                            "Programs that accept federal and state money to provide affordable independent housing for the elderly and disabled. Those who qualify generally pay a portion of their monthly income towards rent"]
];

var livingSpace = [
                        ["studio", "Studio Apartment",
                            "Studio Apartment"],
                        ["one-bedroom", "One Bedroom Apartment",
                            "One Bedroom Apartment"],
                        ["two-bedroom", "Two+ Bedroom Apartment",
                            "Two+ Bedroom Apartment"],
                        ["shared-residence", "Private bedroom with private bath",
                            "Private bedroom with private bath"],
                        ["shared-residence", "Private bedroom with shared bath",
                            "Private bedroom with shared bath"],
                        ["shared-room", "Shared Bedroom",
                            "Shared Bedroom"]
];

var specializedCare = [

                    ["arthritis", "Arthritis",
                        "Degenerative joint disease that results in decreased ability and pain associated with movement"],
                    ["developmental-disabilities", "Developmental disabilities",
                        "Developmental disabilities"],
                    ["diabetes-diet", "Diabetes, Diet Controlled",
                        "Disease characterized by high blood sugar; Its health risks include heart and kidney disease, blindness, and nerve damage. It can be treated many ways, including consumption of a proper diet."],
                    ["diabetes-oral-med", "Diabetes, Oral Medication",
                        "Disease characterized by high blood sugar; Its health risks include heart and kidney disease, blindness, and nerve damage. It can be treated many ways, including use of oral medication."]
];


var languages = [
                ["english", "English", "English"],
                ["arabic", "Arabic", "Arabic"],
                ["cantonese", "Cantonese", "Cantonese"],
                ["czech", "Czech", "Czech"],
                ["dutch", "Dutch", "Dutch"],
                ["french", "French", "French"],
                ["german", "German", "German"],
                ["greek", "Greek", "Greek"],
                ["hebrew", "Hebrew", "Hebrew"],
                ["hindi", "Hindi", "Hindi"],
                ["italian", "Italian", "Italian"],
                ["japanese", "Japanese", "Japanese"],
                ["korean", "Korean", "Korean"],
                ["mandarin", "Mandarin", "Mandarin"],
                ["farsi", "Farsi", "Farsi"],
                ["polish", "Polish", "Polish"],
                ["portuguese", "Portuguese", "Portuguese"],
                ["romanian", "Romanian", "Romanian"],
                ["russian", "Russian", "Russian"],
                ["ameslan", "Sign Language", "Sign Language"],
                ["spanish", "Spanish", "Spanish"],
                ["tagalog", "Tagalog", "Tagalog"],
                ["thai", "Thai", "Thai"],
                ["vietnamese", "Vietnamese", "Vietnamese"]];

var alCriteria = {
    'Always Visible': alwaysVisible,
    'Payment Accepted': paymentAccepted,
    'Living Space Options': livingSpace,
    'Personal Assistance Offered': personalAssistance,
    'Specialized Care': specializedCare,
    'Languages': languages
};

var vhoCriteria = {
    'Primary Health Focus': [
            ['addiction', 'Addiction', ''],
            ['alzheimers-dementia-memory', 'Alzheimer\'s Disease, Dementia, Memory', ''],
            ['amputation', 'Amputation', ''],
            ['blood-disorder', 'Blood Disorder', ''],
            ['bone-joint', 'Bone and Joint Disorders', ''],
            ['nervous-system-conditions', 'Brain, Spine, and Nervous System Conditions', ''],
            ['cancer', 'Cancer', ''],
            ['dental', 'Dental Conditions', ''],
            ['digestive', 'Digestive Conditions', ''],
            ['endocrine', 'Endocrine Conditions and Diabetes', ''],
            ['heart-circulatory', 'Heart or Circulatory Conditions', ''],
            ['immune-disorders', 'Immune Disorders', ''],
            ['kidney-urological-ostomy', 'Kidney, Urological, Ostomy', ''],
            ['lung-respiratory', 'Lung and Respiratory Diseases', ''],
            ['mental-health', 'Mental Health Conditions', ''],
            ['muscular', 'Muscular Disorders', ''],
            ['nutrition-food', 'Nutrition and Diet', ''],
            ['pain', 'Pain', ''],
            ['palliative', 'Palliative and Hospice', ''],
            ['reproductive-system', 'Reproductive and Fertility Disorders', ''],
            ['skin', 'Skin Conditions', ''],
            ['speech-language-hearing', 'Speech, Language, Hearing', ''],
            ['vision', 'Vision', '']
    ],
    'Care and Assistance': [
            ['focus-abuse', 'Abuse', ''],
            ['focus-assistive-devices', 'Assistive Devices', ''],
            ['focus-caregiver-support', 'Caregiver Support', ''],
            ['focus-case-management', 'Case, Condition, Utilization Management', ''],
            ['focus-emergency-support', 'Emergency Support', ''],
            ['focus-financial-services', 'Financial Services', ''],
            ['focus-wellness', 'Health Promotion and Wellness', ''],
            ['focus-housing-and-care-services', 'Housing and Care Services', ''],
            ['focus-info-and-referral', 'Information and Referral Services', ''],
            ['focus-insurance', 'Insurance, Medicare, Medicaid', ''],
            ['focus-legal', 'Legal Services', ''],
            ['focus-low-income', 'Low Income and Subsidy Programs', ''],
            ['focus-professionals', 'Providing Resources and Support to Professionals', ''],
            ['focus-spiritual', 'Spiritual Support', '']
    ]
}

var criteria = {
    'assisted-living': alCriteria,
    'adult-family-home': alCriteria,
    'residential-home': alCriteria,
    'boarding-home': alCriteria,

    'independent-living': {
        'Living Space': livingSpace,
        'Payment Accepted': paymentAccepted,
        'Community services/amenities available': communityServices,
        'Room Amenities': roomAmenities,
        'Languages': languages
    },

    'skilled-nursing': {
        'Always Visible': alwaysVisible,
        'Short Term / Temporary Stay': shortTermStay,
        'Payment Accepted': paymentAccepted,
        'Living Space Options': livingSpace,
        'Personal Assistance Offered': personalAssistance,
        'Specialized Care': specializedCare,
        'Languages': languages
    },

    'ccrc': {
        'Always Visible': alwaysVisible,
        'Living Space': [["studio", "Studio Apartment",
                    "Studio Apartment"],
                ["one-bedroom", "One Bedroom Apartment",
                    "One Bedroom Apartment"],
                ["two-bedroom", "Two+ Bedroom Apartment",
                    "Two+ Bedroom Apartment"],
                ["house-one-bedroom", "1 Bedroom house",
                    "1 Bedroom house"],
                ["house-two-bedroom", "2+ Bedroom house",
                    "2+ Bedroom house"],
                ["shared-residence", "Private bedroom with private bath",
                    "Private bedroom with private bath"],
                ["shared-residence", "Private bedroom with shared bath",
                    "Private bedroom with shared bath"],
                ["shared-room", "Shared Bedroom",
                    "Shared Bedroom"]],
        'Payment Accepted': [["private-payment", "Private Pay",
                    "Private Pay"],
                ["ltc-insurance", "Long Term Care Insurance",
                    "Long Term Care Insurance"],
                ["medicare-payment", "Medicare",
                    "Federal program for people 65 and older, some disabled persons and those with end-stage renal failure (note: generally does not cover long-term care other than short-term rehabilitation provided by skilled nursing facilities)"],
                ["va-benefits", "Veterans Administration Benefits",
                    "Veterans Administration Benefits"]],
        'Personal Assistance': personalAssistance,
        'Specialized Care': specializedCare,
        'Languages': languages
    },


    'home-health-care': {
        'Care Options': [
            ["nursing-care", "Skilled Nursing Care", "A level of care that must be given or supervised by Registered Nurses on an intermittent basis. Examples of skilled nursing care are: administration of intravenous medication, injections, tube feeding, oxygen to help you breathe, and changing sterile dressings on a wound."],
            ["physical-therapy", "Physical Therapy", "Treatment of injury and disease by mechanical means, as heat, light, exercise, and massage."],
            ["occupational-therapy", "Occupational Therapy", "Services given to help you return to usual activities (such as bathing, preparing meals, housekeeping) after illness either on an inpatient or outpatient basis."],
            ["speech-therapy", "Speech Therapy", "This is the study of communication problems. Speech therapists assist with problems involving speech, language and swallowing. Communication problems can be present at birth or develop after an injury or illness - such as a stroke."],
            ["medical-social", "Medical Social Services", "Services to help you with social and emotional concerns related to your illness. This might include counseling or help in finding resources in your community."],
            ["home-health-aide", "Home Health Aide", "Services to help you with daily living activities (such as getting up, bathing, getting dressed, and making a meal). If you receive skilled home health services (from a nurse, physical therapist, or occupational therapist) and also require services that can be performed by an aide, those services are covered by Medicare and are to be provided by the home health agency at no charge to you."]
        ]

    },

    'hospital': {
        'Hospital Type': [
            ['acute-care-hospital', 'Acute Care Hospital', ''],
            ['critical-access-hospital', 'Critical Access Hospital', ''],
            ['acute-care-va-medical-center', 'Acute Care VA Medical Center', ''],
            ['childrens-hospital', 'Children\'s Hospital', '']
        ],

        'Emergency Service': [
            ['emergency-service', 'Emergency Service', '']
        ],

        'Registries': [
            ['cardiac-surgery-registry', 'Participates in the Cardiac Surgery Registry', ''],
            ['stroke-care-registry', 'Participates in the Stroke Care Registry', ''],
            ['nursing-care-registry', 'Participates in the Nursing Care Registry', '']
        ],

        'Hospital Ownership': [
            ['ownership-government-hospital-district-or-authority', 'Government Hospital District or Authority', ''],
            ['ownership-government-federal', 'Federal Government', ''],
            ['ownership-government-local', 'Local Government', ''],
            ['ownership-government-state', 'State Government', ''],
            ['ownership-proprietary', 'Proprietary', ''],
            ['ownership-voluntary-non-profit-church', 'Non-Profit - Church', ''],
            ['ownership-voluntary-non-profit-provate', 'Non-Profit - Private', ''],
            ['ownership-voluntary-non-profit-other', 'Non-Profit - Other', '']
        ]
    },

    'voluntary-health-organization': vhoCriteria,
    'national-organization': vhoCriteria,

    'medical-equipment': {
        "Prosthetics":
        [["breast-prostheses-and-accessories", "Breast Prostheses", ""],
       ["cochlear-implants", "Cochlear Implants", ""],
       ["eye-prostheses", "Eye Prostheses", ""],
       ["facial-prostheses", "Facial Prostheses", ""],
       ["limb-prostheses", "Limb Prostheses", ""],
       ["neurostimulators", "Neurostimulators", ""],
       ["ocular-prostheses", "Ocular Prostheses", ""],
       ["ostomy-supplies", "Ostomy Supplies", ""],
       ["somatic-prostheses", "Somatic Prostheses", ""],
       ["tracheostomy-supplies", "Tracheostomy Supplies", ""],
       ["urological-supplies", "Urological Supplies", ""],
       ["voice-prosthetics", "Voice Prosthetics", ""]],

        "Corrective Eyewear": [
        ["contact-lenses", "Contact Lenses", ""],
        ["eyeglasses", "Eyeglasses", ""],
        ["cataract-lenses", "Cataract Lenses", ""]],

        "Durable Medical Equipment":
         [["automatic-external-defibrillators", "Automatic External Defibrillators", ""],
         ["commodes-urinals-and-bedpans", "Commodes, Urinals, and Bedpans", ""],
         ["gastric-suction-pumps", "Gastric Suction Pumps", ""],
         ["pneumatic-compression-devices", "Pneumatic Compression Devices", ""],
         ["speech-generating-devices", "Speech Generating Devices", ""],
         ["transcutaneous-electrical-nerve-stimulators", "Transcutaneous Electrical Nerve Stimulators Units", ""],
         ["ultraviolet-light-devices", "Ultraviolet Light Devices", ""]],

        "Rehabilitation": [
        ["continuous-passive-motion-devices", "Continuous Passive Motion Devices", ""],
        ["dynamic-splints", "Dynamic Splints", ""],
        ["heat-and-cold-applications", "Heat and Cold Applications", ""],
        ["infrared-heating-pad-systems", "Infrared Heating Pad Systems", ""],
        ["negative-pressure-wound-therapy-pumps-and-supplies", "Negative Pressure Wound Therapy Pumps and Supplies", ""],
        ["neuromuscular-electrical-stimulators", "Neuromuscular Electrical Stimulators", ""],
        ["osteogenesis-stimulators", "Osteogenesis Stimulators", ""],
        ["surgical-dressings", "Surgical Dressings", ""],
        ["traction-equipment", "Traction Equipment", ""]],

        "Beds": [
        ["electric-hospital-beds", "Electric Hospital Beds", ""],
        ["manual-hospital-beds", "Manual Hospital Beds", ""],
        ["pressure-reducing-beds-mattresses-overlays-and-pads", "Pressure Reducing Beds, Mattresses, Overlays, and Pads", ""]],

        "Infusion": [
         ["external-infusion-pumps-and-supplies", "Infusion Pumps and Supplies: External Infusion", ""],
         ["implanted-infusion-pumps-and-supplies", "Infusion Pumps and Supplies: Implanted Infusion", ""],
         ["insulin-infusion-pumps-and-supplies", "Infusion Pumps and Supplies: Insulin Infusion", ""]],

        "Diabetic Supplies": [
        ["mail-order-blood-glucose-monitors-and-supplies", "Blood Glucose Monitors and Supplies: Mail Order", ""],
         ["non-mail-order-blood-glucose-monitors-and-supplies", "Blood Glucose Monitors and Supplies: Non-Mail Order", ""],
         ["custom-fabricated-diabetic-shoes-and-inserts", "Custom Fabricated Diabetic Shoes", ""],
         ["prefabricated-diabetic-shoes-and-inserts", "Prefabricated Diabetic Shoes", ""]],

        "Powered Mobility Devices": [
         ["patient-lifts", "Patient Lifts", ""],
         ["power-operated-vehicles", "Scooters", ""],
         ["seat-lift-mechanisms", "Seat Lift Mechanisms", ""],
         ["walkers", "Walkers", ""],
         ["power-wheelchairs-and-accessories", "Powered Wheelchairs", ""],
         ["power-complex-rehabilitative-wheelchairs-and-accessories", "Wheelchairs and Accessories: Complex Rehabilitative Power", ""]],

        "Manual Mobility Devices": [
         ["canes-and-crutches", "Canes and Crutches", ""],
         ["manual-wheelchairs-and-accessories", "Manual Wheelchairs", ""],
         ["wheelchair-seating-and-cushions", "Wheelchair Seating/Cushions", ""],
         ["manual-complex-rehabilitative-wheelchairs-and-accessories", "Wheelchairs and Accessories: Complex Rehabilitative Manual", ""]],

        "Respiratory Devices":
         [["high-frequency-chest-wall-oscillation-devices", "High Frequency Chest Wall Oscillation Devices", ""],
         ["intermittent-positive-pressure-breathing-devices", "Intermittent Positive Pressure Breathing Devices", ""],
         ["intrapulmonary-percussive-ventilation-devices", "Intrapulmonary Percussive Ventilation Devices", ""],
         ["invasive-mechanical-ventilation", "Invasive Mechanical Ventilation", ""],
         ["mechanical-in-exsufflation-devices", "Mechanical In-Exsufflation Devices", ""],
         ["nebulizer-equipment-and-supplies", "Nebulizer Equipment and Supplies", ""],
         ["oxygen-equipment-and-supplies", "Oxygen Equipment and Supplies", ""],
         ["respiratory-suction-pumps", "Respiratory Suction Pumps", ""],
         ["ventilators-accessories-and-supplies", "Ventilators, Accessories, and Supplies", ""],
         ["cpap-rads-and-accessories", "CPAP, RADs, and Related Supplies and Accessories", ""]],

        "Parenteral and Enteral Nutrition":
         [["entereal-nutrientes-equipment-and-supplies", "Enteral Nutrients, Equipment, and Supplies", ""],
         ["parentereal-nutrientes-equipment-and-supplies", "Parenteral Nutrients, Equipment, and Supplies", ""]],

        "Orthotics":
         [["custom-fabricated-orthoses", "Orthoses: Custom Fabricated", ""],
         ["off-the-shelf-orthoses", "Orthoses: Off-the-Shelf", ""],
         ["prefabricated-orthoses", "Orthoses: Prefabricated", ""]],

        "Dialysis Equipment":
         [["hemiodialysis-equipment-and-supplies", "Hemodialysis Equipment", ""],
         ["home-dialysis-equipment-and-supplies", "Home Dialysis Equipment", ""]]
    }

};
