//
//  Metaphone2.h
//  Created by sam on 2/03/10.
//
//  This is a direct port of the metaphone algorithm
//  found in the advas package for Python
//  http://advas.sourceforge.net
//
//  It's pretty much line-for-line, so I'm hoping the
//  testing done on the advas project is thorough
//  (or at least existent :p).
//
//  I take no responsibility for the correctness of the
//  underlying algorithm, and have made no attempt to
//  check the validity of the advas code. I only hope the
//  authors of advas knew what they were doing.
//
// Updated and packaged by Dominik Pich on 24.04.17
//
#import <Foundation/Foundation.h>

@interface NSString (Metaphone2)

@property(readonly) NSString *metaphone2Code;

@end
