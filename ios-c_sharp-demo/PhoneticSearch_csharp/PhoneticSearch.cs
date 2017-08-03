//
//  NSObject+StringUtils_m.h
//  SiemonaDemo
//
//  Created by Dominik Pich on 24.04.17.
//  Copyright © 2017 Dominik Pich. All rights reserved.
//
using System;
using System.Collections.Generic;

namespace PhoneticSearch
{
    public enum PhoneticMatchAlgorithm
    {
        KeepOriginal = 0,
        Metaphone2,
        Soundex,
    }

    public enum PhoneticSearchAlgorithm
    {
        //DirectComparison = 0,
        LevensteinDistance,
        //NGram,
    }

    //---

    public class PhoneticMatch
    {
        public String term;
        public float distance;
    }

    public class PhoneticSearch
    {
        static T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public const int DefaultDistance = 2;

        //takes the searchString, normalized and tokenizes it, makes 'terms' where it combines the words ... e.g. 1, 12, 123, 1234, 2, 23, 234 ... then it takes the possible terms and matches the tokens using Metaphone2/Soundex code and levenstein distance, longer terms are preferred
        //only returns the best match!

        public static PhoneticMatch FindTermInString(String searchString, String[] possibleTerms, PhoneticMatchAlgorithm matchAlgorithm, float levensteinThreshold)
        {
            String[] tokens = searchString.normalizedTokens();

            //build the phonetic searchTerms
            var searchTermsList = new List<Object[]>();
            for (int i = 0; i < tokens.Length; i++)
            {
                int left = tokens.Length - i + 1;

                for (int c = 1; c < left; c++)
                {
                    var subarray = SubArray(tokens, i, c);
                    String str = String.Join("", subarray);
                    String meta = applyAlgorithm(matchAlgorithm, str);
                    if (meta.Length>0)
                    {
                        Object[] term = { str, meta, subarray.Length };
                        searchTermsList.Add(term);
                    }
                }
            }
            Object[][] searchTerms = searchTermsList.ToArray();
                                                  
			//sort by length
            Array.Sort(searchTerms, (x, y) => {
            	int c1 = (int)x[2];
                int c2 = (int)y[2];
                return c2 - c1;
            });

            //make metaphone code of each possibility
            var matchTermsList = new List<Object[]>();
            foreach (String possibility in possibleTerms)
			{
                String meta = applyAlgorithm(matchAlgorithm, possibility);
				if (meta.Length>0)
				{
                    Object[] term = { possibility, meta };
                    matchTermsList.Add(term);
                }
            }
			Object[][] matchTerms = matchTermsList.ToArray();

			//find the best matching term (based on levenstein distance)
            int currentLevel = Int32.MaxValue;
            float minDistance = float.MaxValue;
			String possibleMatch = null;
            foreach (Object[] searchTerm in searchTerms)
			{
				int level = (int)searchTerm[2];
				if (level < currentLevel)
				{
					if (minDistance < levensteinThreshold)
					{
						break; //we dont wanna go down one level if we found a match
					}
				}
				currentLevel = level;
        
                foreach (Object[] matchTerm in matchTerms)
				{
					String toMatch = (String)matchTerm[1];
					String toSearch = (String)searchTerm[1];
                    float distance = toMatch.levensteinDistanceTo(toSearch);

					if (distance < minDistance)
					{
						minDistance = distance;
						possibleMatch = (String)matchTerm[0];
					}
				}
			}

			if (possibleMatch != null)
			{
                //return wrapped result
                PhoneticMatch match = new PhoneticMatch();
				match.term = possibleMatch;
				match.distance = minDistance;
				return match;
			}
            return null;
        }

        private static String applyAlgorithm(PhoneticMatchAlgorithm algorithm, String str)
        {
            switch (algorithm)
            {
                case PhoneticMatchAlgorithm.Metaphone2:
                    return str.metaphone2Code();

                case PhoneticMatchAlgorithm.Soundex:
                    return str.soundexCode();

                case PhoneticMatchAlgorithm.KeepOriginal:
                default:
                    return str;
            }
        }
    }
}