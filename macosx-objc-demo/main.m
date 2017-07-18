//
//  main.m
//  macosx-objc-demo
//
//  Created by Dominik Pich on 18.07.17.
//  Copyright © 2017 Dominik Pich. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PhoneticSearch.h"

#import "NSString+Metaphone2.h"

int main(int argc, const char * argv[]) {
    @autoreleasepool {
        id fuzzyInput = @"The house is maintained by aHAausritters while we are on vacation"; //input is something that is close to/sounds like Housesitter
        id knownWords = @[@"house sitter", @"baby sitter", @"gardener"];
        
        PhoneticMatch *match = [PhoneticSearch findTermInString:fuzzyInput possibleTerms:knownWords algorithm:PhoneticAlgorithmMatchMetaphone2 levensteinThreshold:PhoneticSearchDefaultDistance];
        
        if(match) {
            NSLog(@"We found %@ for the input %@. It has a levensteindistance of %f", match.term, fuzzyInput, match.distance);
        }
        else {
            NSLog(@"nothing found for input %@", fuzzyInput);
        }
    }
    return 0;
}
