//
//  Levenstein.h
//  SiemonaDemo
//
//  Created by Dominik Pich on 24.04.17.
//  Copyright © 2017 Dominik Pich. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface NSString (Levenstein)

-(float)levensteinDistanceTo:(NSString *)comparisonString;

@end
