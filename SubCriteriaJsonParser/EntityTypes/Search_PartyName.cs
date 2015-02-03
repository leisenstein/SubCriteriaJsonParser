using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Snap.Geography;
using Snap.Util;

namespace Snap.Search2 {
    public partial class Search {

        //-------------------------------------------------------------------------
        private static IntSet<PartyId> SearchByName(
            Timer t,
            AddressParserResult apr,
            IEnumerable<string> noiseWords) {

            Precondition.IsNotNull(apr, "apr");

            // this is not used becasue now name search can happen even with SearchType=postcode,city,zip etc.

            //if (Snap.Data.Controller.SearchType.PartyName != apr.Type) {
            //    throw new ArgumentException("apr.Type must be PartyName");
            //}

            string searchTerms =
                Snap.Util.StringX.CollapseWhitespace(
                    RemoveNameSearchNoiseTerms(apr.PartyName, noiseWords));

            IntSet<PartyId> partiesByName = null;
            t.Measure("Search by Name", delegate() {
                //partiesByName = Snap.Cache.Cache.PartyName(searchTerms);
                partiesByName = new Snap.Data.LuceneController().LuceneSearchIds(searchTerms, Snap.ForSeniors.Constants.SnapPartyLuceneTableName, Snap.ForSeniors.Constants.SnapPartyLuceneSearchField, Snap.ForSeniors.Constants.SnapPartyLuceneResultField);

            });
            if (partiesByName.Count == 0) {
                throw new InvalidPartyNameException(apr.PartyName);
            }

            return partiesByName;
        }

        private static readonly Regex InvalidSearchTermPattern =
            new Regex(@"[^A-Za-z0-9-'\ ]");

        //-------------------------------------------------------------------------
        /// <summary>
        /// Returns a full-text search-ready string with most punctuation and 
        /// the words and phrases listed in the module settings removed.
        /// </summary>
        internal static string RemoveNameSearchNoiseTerms(
            string facilityNameSearchTerms,
            IEnumerable<string> nameSearchNoiseTerms) {

            Precondition.IsNotNull<string>(
                facilityNameSearchTerms, "facilityNameSearchTerms");

            List<string> noiseWords =
                new List<string>(nameSearchNoiseTerms);

            List<string> cleanTerms = new List<string>();
            string[] terms = StringX.SplitOnWhitespace(
                facilityNameSearchTerms.ToLower());

            foreach (string term in terms) {
                string cleanTerm =
                    InvalidSearchTermPattern.Replace(term, String.Empty);

                if ((cleanTerm.Length > 0) &&
                    !cleanTerms.Contains(cleanTerm) &&
                    !noiseWords.Contains(cleanTerm)) {

                    cleanTerms.Add(cleanTerm);
                }
            }

            return String.Join(" ", cleanTerms.ToArray());
        }
    }
}
