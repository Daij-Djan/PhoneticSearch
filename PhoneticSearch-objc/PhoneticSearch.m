//
//  NSObject+StringUtils_m.m
//  SiemonaDemo
//
//  Created by Dominik Pich on 24.04.17.
//  Copyright © 2017 Dominik Pich. All rights reserved.
//

#import "PhoneticSearch.h"
#import "NSString+Metaphone2.h"
#import "NSString+Soundex.h"
#import "NSString+Levenstein.h"
#import "NSString+normalizedTokens.h"

@interface PhoneticMatch ()
@property(strong) NSString *term;
@property(assign) float distance;
@end
@implementation PhoneticMatch
@end

@implementation PhoneticSearch

+ (PhoneticMatch*)findTermInString:(NSString *)searchString possibleTerms:(NSArray<NSString*> *)possibleTerms algorithm:(PhoneticMatchAlgorithm)matchAlgorithm levensteinThreshold:(float)levensteinThreshold {
    NSArray<NSString*> *tokens = searchString.normalizedTokens;
    
    //build the phonetic searchTerms
    NSMutableArray *searchTerms = [NSMutableArray array];
    for(NSUInteger i = 0; i<tokens.count;i++) {
        NSUInteger left = tokens.count - i + 1;
        
        for(NSUInteger c=1;c<left;c++) {
            NSArray *subarray = [tokens subarrayWithRange:NSMakeRange(i, c)];
            NSString *str = [subarray componentsJoinedByString:@""];
            NSString *meta = [self applyAlgorithm:matchAlgorithm toString:str];
            if(meta.length) {
                [searchTerms addObject:@[str, meta, @(subarray.count)]];
            }
        }
    }
    
    //sort by length
    [searchTerms sortUsingComparator:^NSComparisonResult(NSArray*  _Nonnull obj1, NSArray * _Nonnull obj2) {
        NSNumber *c1 = obj1[2];
        NSNumber *c2 = obj2[2];
        return [c2 compare:c1];
    }];
    
    //make metaphone code of each possibility
    NSMutableArray *matchTerms = [NSMutableArray array];
    for (NSString *possibility in possibleTerms) {
        NSString *meta = [self applyAlgorithm:matchAlgorithm toString:possibility];
        if(meta.length) {
            [matchTerms addObject:@[possibility,meta]];
        }
    }
    
    //find the best matching term (based on levenstein distance)
    NSUInteger currentLevel = MAXFLOAT;
    float minDistance = MAXFLOAT;
    id possibleMatch = nil;
    for (NSArray *searchTerm in searchTerms) {
        NSUInteger level = [searchTerm[2] unsignedIntegerValue];
        if(level < currentLevel) {
            if(minDistance < levensteinThreshold) {
                break; //we dont wanna go down one level if we found a match
            }
        }
        currentLevel = level;
        
        for (NSArray *matchTerm in matchTerms) {
            NSString *toMatch = matchTerm[1];
            NSString *toSearch = searchTerm[1];
            float distance = [toMatch levensteinDistanceTo:toSearch];
            
            if(distance < minDistance) {
                minDistance = distance;
                possibleMatch = matchTerm[0];
            }
        }
    }

    if(possibleMatch) {
        //return wrapped result
        PhoneticMatch *match = [[PhoneticMatch alloc] init];
        match.term = possibleMatch;
        match.distance = minDistance;
        return match;
    }
    return nil;
}

+ (NSString*)applyAlgorithm:(PhoneticMatchAlgorithm)algorithm toString:(NSString*)str {
    switch (algorithm) {
        case PhoneticAlgorithmMatchMetaphone2:
            return str.metaphone2Code;
            
        case PhoneticAlgorithmMatchSoundex:
            return str.soundexCode;
            
        case PhoneticAlgorithmMatchKeepOriginal:
        default:
            return str;
    }
}

@end
