#region License

// Copyright 2009 The Sixth Form College Farnborough (http://www.farnborough.ac.uk)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/SagePayMvc

#endregion

using System;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace SagePayMvc {
	/// <summary>
	/// Represents a collection of fields that make up an address
	/// </summary>
	public class Address {
		public string Surname { get; set; }
		public string Firstnames { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string PostCode { get; set; }
		public string Country { get; set; }
		public string State { get; set; }
		public string Phone { get; set; }

		/// <summary>
		/// Converts the address to a string using the specified address type. The address type will become the prefix,
		/// eg using a brefix of AddressType.Billing will generate strings containing BillingSurname, BillingFirstnames etc
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public string ToString(AddressType type) {
			string prefix = type.ToString();
			var builder = new StringBuilder();
			builder.Append(BuildPropertyString(prefix, x => x.Surname, Surname));
			builder.Append(BuildPropertyString(prefix, x => x.Firstnames, Firstnames));
			builder.Append(BuildPropertyString(prefix, x => x.Address1, Address1));
			builder.Append(BuildPropertyString(prefix, x => x.Address2, Address2, true));
			builder.Append(BuildPropertyString(prefix, x => x.City, City));
			builder.Append(BuildPropertyString(prefix, x => x.PostCode, PostCode));
			builder.Append(BuildPropertyString(prefix, x => x.Country, Country));
			builder.Append(BuildPropertyString(prefix, x => x.State, State, true));
			builder.Append(BuildPropertyString(prefix, x => x.Phone, Phone, true));

			return builder.ToString();
		}

		string BuildPropertyString(string prefix, Expression<Func<Address, object>> expression, string value, bool optional) {
			if (optional && string.IsNullOrEmpty(value)) return null;

			string name = PropertyToName(expression);
			return string.Format("&{0}{1}={2}", prefix, name, HttpUtility.UrlEncode(value));
		}


		string BuildPropertyString(string prefix, Expression<Func<Address, object>> expression, string value) {
			return BuildPropertyString(prefix, expression, value, false);
		}

		static string PropertyToName(Expression<Func<Address, object>> expression) {
			return (expression.Body as MemberExpression).Member.Name;
		}
	}
}