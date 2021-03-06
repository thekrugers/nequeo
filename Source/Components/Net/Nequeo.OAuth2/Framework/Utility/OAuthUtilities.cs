﻿/*  Company :       Nequeo Pty Ltd, http://www.nequeo.com.au/
 *  Copyright :     Copyright © Nequeo Pty Ltd 2012 http://www.nequeo.com.au/
 * 
 *  File :          
 *  Purpose :       
 * 
 */

#region Nequeo Pty Ltd License
/*
    Permission is hereby granted, free of charge, to any person
    obtaining a copy of this software and associated documentation
    files (the "Software"), to deal in the Software without
    restriction, including without limitation the rights to use,
    copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the
    Software is furnished to do so, subject to the following
    conditions:

    The above copyright notice and this permission notice shall be
    included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
    OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
    HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
    WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
    FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
    OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

namespace Nequeo.Net.OAuth2.Framework.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Globalization;
	using System.Linq;
	using System.Net;
	using System.Text;

    using Nequeo.Net.Core.Messaging;
    using Nequeo.Net.Core.Messaging.Bindings;
    using Nequeo.Net.Core.Messaging.Reflection;
    using Nequeo.Net.OAuth2.Framework.Messages;
    using Nequeo.Net.OAuth2.Framework.ChannelElements;

	/// <summary>
	/// Some common utility methods for OAuth 2.0.
	/// </summary>
	public static class OAuthUtilities {
		/// <summary>
		/// The <see cref="StringComparer"/> instance to use when comparing scope equivalence.
		/// </summary>
		public static readonly StringComparer ScopeStringComparer = StringComparer.Ordinal;

		/// <summary>
		/// The string "Basic ".
		/// </summary>
		private const string HttpBasicAuthScheme = "Basic ";

		/// <summary>
		/// The delimiter between scope elements.
		/// </summary>
		private static readonly char[] scopeDelimiter = new char[] { ' ' };

		/// <summary>
		/// A colon, in a 1-length character array.
		/// </summary>
		private static readonly char[] ColonSeparator = new char[] { ':' };

		/// <summary>
		/// The encoding to use when preparing credentials for transit in HTTP Basic base64 encoding form.
		/// </summary>
		private static readonly Encoding HttpBasicEncoding = Encoding.UTF8;

		/// <summary>
		/// The characters that may appear in an access token that is included in an HTTP Authorization header.
		/// </summary>
		/// <remarks>
		/// This is defined in OAuth 2.0 DRAFT 10, section 5.1.1. (http://tools.ietf.org/id/draft-ietf-oauth-v2-10.html#authz-header)
		/// </remarks>
		private static string accessTokenAuthorizationHeaderAllowedCharacters = MessagingUtilities.UppercaseLetters +
																				MessagingUtilities.LowercaseLetters +
																				MessagingUtilities.Digits +
																				@"!#$%&'()*+-./:<=>?@[]^_`{|}~\,;";

		/// <summary>
		/// Identifies individual scope elements
		/// </summary>
		/// <param name="scope">The space-delimited list of scopes.</param>
		/// <returns>A set of individual scopes, with any duplicates removed.</returns>
		public static HashSet<string> SplitScopes(string scope) {
			if (string.IsNullOrEmpty(scope)) {
				return new HashSet<string>();
			}

			var set = new HashSet<string>(scope.Split(scopeDelimiter, StringSplitOptions.RemoveEmptyEntries), ScopeStringComparer);
			VerifyValidScopeTokens(set);
			return set;
		}

		/// <summary>
		/// Serializes a set of scopes as a space-delimited list.
		/// </summary>
		/// <param name="scopes">The scopes to serialize.</param>
		/// <returns>A space-delimited list.</returns>
		public static string JoinScopes(HashSet<string> scopes) {
			
			VerifyValidScopeTokens(scopes);
			return string.Join(" ", scopes.ToArray());
		}

		/// <summary>
		/// Parses a space-delimited list of scopes into a set.
		/// </summary>
		/// <param name="scopes">The space-delimited string.</param>
		/// <returns>A set.</returns>
		internal static HashSet<string> ParseScopeSet(string scopes) {
			
			return ParseScopeSet(scopes.Split(scopeDelimiter, StringSplitOptions.RemoveEmptyEntries));
		}

		/// <summary>
		/// Creates a set out of an array of strings.
		/// </summary>
		/// <param name="scopes">The array of strings.</param>
		/// <returns>A set.</returns>
		internal static HashSet<string> ParseScopeSet(string[] scopes) {
			
			return new HashSet<string>(scopes, StringComparer.Ordinal);
		}

		/// <summary>
		/// Verifies that a sequence of scope tokens are all valid.
		/// </summary>
		/// <param name="scopes">The scopes.</param>
		internal static void VerifyValidScopeTokens(IEnumerable<string> scopes) {
			
			foreach (string scope in scopes) {
				VerifyValidScopeToken(scope);
			}
		}

		/// <summary>
		/// Verifies that a given scope token (not a space-delimited set, but a single token) is valid.
		/// </summary>
		/// <param name="scopeToken">The scope token.</param>
		internal static void VerifyValidScopeToken(string scopeToken) {
			ErrorUtilities.VerifyProtocol(!string.IsNullOrEmpty(scopeToken), OAuthStrings.InvalidScopeToken, scopeToken);
			for (int i = 0; i < scopeToken.Length; i++) {
				// The allowed set of characters comes from OAuth 2.0 section 3.3 (draft 23)
				char ch = scopeToken[i];
				if (!(ch == '\x21' || (ch >= '\x23' && ch <= '\x5B') || (ch >= '\x5D' && ch <= '\x7E'))) {
					ErrorUtilities.ThrowProtocol(OAuthStrings.InvalidScopeToken, scopeToken);
				}
			}
		}

		/// <summary>
		/// Authorizes an HTTP request using an OAuth 2.0 access token in an HTTP Authorization header.
		/// </summary>
		/// <param name="request">The request to authorize.</param>
		/// <param name="accessToken">The access token previously obtained from the Authorization Server.</param>
		internal static void AuthorizeWithBearerToken(this HttpWebRequest request, string accessToken) {
			
			ErrorUtilities.VerifyProtocol(accessToken.All(ch => accessTokenAuthorizationHeaderAllowedCharacters.IndexOf(ch) >= 0), OAuthStrings.AccessTokenInvalidForHttpAuthorizationHeader);

			request.Headers[HttpRequestHeader.Authorization] = string.Format(
				CultureInfo.InvariantCulture,
				Protocol.BearerHttpAuthorizationHeaderFormat,
				accessToken);
		}

		/// <summary>
		/// Applies the HTTP Authorization header for HTTP Basic authentication.
		/// </summary>
		/// <param name="headers">The headers collection to set the authorization header to.</param>
		/// <param name="userName">The username.  Cannot be empty.</param>
		/// <param name="password">The password.  Cannot be null.</param>
		internal static void ApplyHttpBasicAuth(WebHeaderCollection headers, string userName, string password) {
			

			string concat = userName + ":" + password;
			byte[] bits = HttpBasicEncoding.GetBytes(concat);
			string base64 = Convert.ToBase64String(bits);
			string header = HttpBasicAuthScheme + base64;
			headers[HttpRequestHeader.Authorization] = header;
		}

		/// <summary>
		/// Extracts the username and password from an HTTP Basic authorized web header.
		/// </summary>
		/// <param name="headers">The incoming web headers.</param>
		/// <returns>The network credentials; or <c>null</c> if none could be discovered in the request.</returns>
		internal static NetworkCredential ParseHttpBasicAuth(WebHeaderCollection headers) {

			string authorizationHeader = headers[HttpRequestHeaders.Authorization];
			if (authorizationHeader != null && authorizationHeader.StartsWith(HttpBasicAuthScheme, StringComparison.Ordinal)) {
				string base64 = authorizationHeader.Substring(HttpBasicAuthScheme.Length);
				byte[] bits = Convert.FromBase64String(base64);
				string usernameColonPassword = HttpBasicEncoding.GetString(bits);
				string[] usernameAndPassword = usernameColonPassword.Split(ColonSeparator, 2);
				if (usernameAndPassword.Length == 2) {
					return new NetworkCredential(usernameAndPassword[0], usernameAndPassword[1]);
				}
			}

			return null;
		}
	}
}
