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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SagePayMvc {
	/// <summary>
	/// A shopping basket
	/// </summary>
	public class ShoppingBasket : IEnumerable<BasketItem> {
		readonly List<BasketItem> basket = new List<BasketItem>();
		string name;

		/// <summary>
		/// Creates a new instance of the ShoppingBasket class
		/// </summary>
		/// <param name="name">The name of the basket (eg 'Shopping Basket for John Smith')</param>
		public ShoppingBasket(string name) {
			this.name = name;
		}

		/// <summary>
		/// Name of the basket
		/// </summary>
		public string Name {
			get { return name; }
		}

		/// <summary>
		/// Total cost of the basket
		/// </summary>
		public decimal Total {
			get { return basket.Sum(x => x.LineTotal); }
		}

		/// <summary>
		/// Adds an item to the basket
		/// </summary>
		public void Add(BasketItem item) {
			basket.Add(item);
		}


		/// <summary>
		/// Removes an item from the basket
		/// </summary>
		/// <param name="item"></param>
		public void Remove(BasketItem item) {
			basket.Remove(item);
		}

		public IEnumerator<BasketItem> GetEnumerator() {
			return basket.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		/// <summary>
		/// Converts the basket to a string in a format that can be inspected by SagePay.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			if (basket.Count == 0) {
				return null;
			}

			var builder = new StringBuilder(basket.Count.ToString());

			foreach (var item in basket) {
				builder.Append(":");
				builder.Append(item.Description.Replace(":", "#"));
				builder.Append(":");
				builder.Append(item.Quantity);
				builder.Append(":");
				builder.AppendFormat("{0:F2}", item.ItemPrice);
				builder.Append(":");
				builder.AppendFormat("{0:F2}", item.ItemTax);
				builder.Append(":");
				builder.AppendFormat("{0:F2}", item.ItemTotal);
				builder.Append(":");
				builder.AppendFormat("{0:F2}", item.LineTotal);
			}

			return builder.ToString();
		}
	}
}