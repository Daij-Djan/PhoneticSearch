//
//  NSString+normalizedTokens.m
//  SiemonaDemo
//
//  Created by Dominik Pich on 27.04.17.
//  Copyright © 2017 Dominik Pich. All rights reserved.
//

#import "NSString+normalizedTokens.h"

@implementation NSString (normalizedTokens)

- (NSArray<NSString*>*)normalizedTokens {
    NSMutableString *s = [self mutableCopy];
    CFMutableStringRef str = (__bridge CFMutableStringRef)s;
    
    CFStringTransform(str, nil, kCFStringTransformToLatin, NO);
    CFStringTransform(str, nil, kCFStringTransformStripCombiningMarks, NO);
    
    CFStringTokenizerRef tokenizer = CFStringTokenizerCreate(nil, str, CFRangeMake(0, CFStringGetLength(str)), 0, CFLocaleCopyCurrent());
    
    NSMutableArray *mutableTokens = [NSMutableArray array];
    CFStringTokenizerTokenType type;
    do {
        type = CFStringTokenizerAdvanceToNextToken(tokenizer);
        CFRange range = CFStringTokenizerGetCurrentTokenRange(tokenizer);
        CFStringRef token = CFStringCreateWithSubstring(nil, str, range);
        [mutableTokens addObject:(__bridge NSString*)token];
    }
    while(type != kCFStringTokenizerTokenNone);
    
    return mutableTokens;
}

@end
