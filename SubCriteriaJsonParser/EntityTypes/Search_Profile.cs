using System;
using System.Collections.Generic;

using Snap.Util;
using Snap.Geography;

namespace Snap.Search2 {

    //----------------------------------------------------------------------
    public class NoopAggregate : Aggregate {
        public NoopAggregate() 
            : base(IntSet<ProfileId>.UnionOfMany) {
        }
    }

    //----------------------------------------------------------------------
    public class Union : Aggregate {
        public Union(params ISetExpressionTerm[] subExpressions)
            : base(IntSet<ProfileId>.UnionOfMany, subExpressions) {
        }
    }

    //----------------------------------------------------------------------
    public class Intersection : Aggregate {
        public Intersection(params ISetExpressionTerm[] subExpression)
            : base(IntSet<ProfileId>.IntersectionOfMany, subExpression) {
        }
    }

    //----------------------------------------------------------------------
    /// <summary>
    /// An expression node that combines 
    /// </summary>
    public abstract class Aggregate : ISetExpressionTerm {
        private ISetExpressionTerm[] mSubExpressions;
        private CombinationFunction mCombine;

        public Aggregate(
            CombinationFunction combine,
            params ISetExpressionTerm[] subExpressions) {

            mCombine = combine;
            mSubExpressions = subExpressions;
        }

        public bool TryEvaluate(
            Dictionary<string, string> consumerProperties,
            out IntSet<ProfileId> result,
            string profileTemplateLabel) {

            if ((null == consumerProperties) || (0 == consumerProperties.Count)) {
                result = null;
                return false;
            }

            List<IntSet<ProfileId>> activeSubExpressions =
                new List<IntSet<ProfileId>>(mSubExpressions.Length);

            foreach (ISetExpressionTerm expr in mSubExpressions) {
                IntSet<ProfileId> s;

                if (expr.TryEvaluate(consumerProperties, out s, profileTemplateLabel)) {
                    activeSubExpressions.Add(s);
                }
            }

            if (0 == activeSubExpressions.Count) {
                result = null;
                return false;
            } else {
                result = mCombine(activeSubExpressions.ToArray());
                return true;
            }
        }
    }

    //----------------------------------------------------------------------
    public class Criterion : ISetExpressionTerm {
        private string mConsumerPropertyName;
        private string mProviderPropertyName;
        private string mProviderPropertyValue;

        public Criterion(
            string consumerPropertyName,
            string providerPropertyName,
            string providerPropertyValue) {

            mConsumerPropertyName = consumerPropertyName;
            mProviderPropertyName = providerPropertyName;
            mProviderPropertyValue = providerPropertyValue;
        }

        public bool TryEvaluate(
            Dictionary<string, string> consumerProperties, 
            out IntSet<ProfileId> result, 
            string profileTemplateLabel) {

            string criteriaActiveStr;
            bool criteriaActive;

            if (consumerProperties.TryGetValue(mConsumerPropertyName, out criteriaActiveStr) &&
                Boolean.TryParse(criteriaActiveStr, out criteriaActive) &&
                criteriaActive) {

                result = Snap.Cache.Cache.Profile(
                    profileTemplateLabel,
                    mProviderPropertyName,
                    mProviderPropertyValue);
                return true;
            } else {
                result = null;
                return false;
            }
        }
    }

    //----------------------------------------------------------------------
    /// <summary>
    /// A node of a set operation expression tree which represents a Profile
    /// search.
    /// </summary>
    public interface ISetExpressionTerm {
        bool TryEvaluate(
            Dictionary<string, string> consumerProperties,
            out IntSet<ProfileId> result,
            string profileTemplateLabel);
    }

    //----------------------------------------------------------------------
    /// <summary>
    /// A function that combines multiple IntSets into one.  E.g., an 
    /// implementation of set intersection, union, or difference.
    /// </summary>
    public delegate IntSet<ProfileId> CombinationFunction(params IntSet<ProfileId>[] sets);

    //----------------------------------------------------------------------
    public partial class Search {
        //----------------------------------------------------------------------
        /// <summary>
        /// If 'condition' is true, returns the set of Profile IDs which have
        /// the specified Profile Property value in the Search.LTSH profile.  
        /// Otherwise, returns the empty set.
        /// </summary>
        public static IntSet<ProfileId> UnionTerm(
            bool condition,
            string propertyName,
            string propertyValue,
            ISearch options) {

            if (condition) {
                return Snap.Cache.Cache.Profile(options.ProfileTemplateLabel, propertyName, propertyValue);
            } else {
                return IntSet<ProfileId>.Empty;
            }
        }

        //----------------------------------------------------------------------
        public static IntSet<ProfileId> UnionTerm<ProfileType>(
            bool condition,
            string propertyName,
            string propertyValue) where ProfileType : ISearch,new() {
            return UnionTerm(
                condition,
                propertyName,
                propertyValue,
                new ProfileType());
        }

        //----------------------------------------------------------------------
        public delegate ResultSet ProfileSearch<ProfileType>(
            Timer t,
            Dictionary<string, string> consumerProperties,
            IntSet<ProfileId> profileSpecificProfiles) where ProfileType : ISearch, new();

        //----------------------------------------------------------------------
        protected static ResultSet SearchByProfile<ProfileType>(
            Timer t,
            Dictionary<string, string> consumerProperties,
            IntSet<ProfileId> profileSpecificProfiles) where ProfileType : ISearch,new(){

            ISearch profile = new ProfileType();      
            IntSet<ProfileId> moreOptionsMatches;
            if (!profile.ExpressionTree.TryEvaluate(
                consumerProperties,
                out moreOptionsMatches,
                profile.ProfileTemplateLabel)) {

                moreOptionsMatches = IntSet<ProfileId>.Universe;
            }

            return ResultSet.From(
                IntSet<ProfileId>.Intersection(
                profileSpecificProfiles,
                moreOptionsMatches));
        }
    }
}
