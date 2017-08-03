//
//  NSString+normalizedTokens.m
//  SiemonaDemo
//
//  Created by Dominik Pich on 27.04.17.
//  Copyright © 2017 Dominik Pich. All rights reserved.
//
using System.Text;
using System.Collections.Generic;

namespace PhoneticSearch
{
	public static class NormalizedTokenizer
	{
		public static string[] normalizedTokens(this string data)
		{
			var tokens = new List<string>();
			foreach (var str in data.Split(' '))
			{
				tokens.Add(RemoveSpecialCharacters(str));
			}
            return tokens.ToArray();
		}

		private static string RemoveSpecialCharacters(this string str)
		{
			StringBuilder sb = new StringBuilder();
			foreach (char c in str)
			{
				if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}
	}
}