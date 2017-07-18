//
//  NSObject+StringUtils_m.h
//  SiemonaDemo
//
//  Created by Dominik Pich on 24.04.17.
//  Copyright © 2017 Dominik Pich. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSUInteger, PhoneticMatchAlgorithm) {
    PhoneticAlgorithmMatchKeepOriginal = 0,
    PhoneticAlgorithmMatchMetaphone2,
    PhoneticAlgorithmMatchSoundex,
};

typedef NS_ENUM(NSUInteger, PhoneticSearchAlgorithm) {
//    PhoneticSearchAlgorithmDirectComparison = 0,
    PhoneticSearchAlgorithmLevensteinDistance,
//    PhoneticSearchAlgorithmNGram,
};

//---

#define PhoneticSearchDefaultDistance 2

@interface PhoneticMatch : NSObject

@property(readonly) NSString *term;
@property(readonly) float distance;

@end

//---

@interface PhoneticSearch : NSObject

//takes the searchString, normalized and tokenizes it, makes 'terms' where it combines the words ... e.g. 1, 12, 123, 1234, 2, 23, 234 ... then it takes the possible terms and matches the tokens using Metaphone2/Soundex code and levenstein distance, longer terms are preferred
//only returns the best match!

+ (PhoneticMatch*)findTermInString:(NSString *)searchString
                     possibleTerms:(NSArray<NSString*> *)possibleTerms algorithm:(PhoneticMatchAlgorithm)matchAlgorithm levensteinThreshold:(float)levensteinThreshold;

@end
