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

using NUnit.Framework;

namespace SagePayMvc.Tests {
	[TestFixture]
	public class BasketTester {
		[Test]
		public void Converts_basket_to_string() {
			//format is total items:description:quantity:ItemPrice:ItemTax:ItemTotal:LineTotal
			//So this is 2 items in the basket:
			//One called foo, with a price of £5, tax of £5 (total of £10)
			//...and another called bar with a price of £2, tax of £3, (total of £5)
			//Yes, the tax rates are silly...
			const string expected = "2:foo:1:5.00:5.00:10.00:10.00:bar:1:2.00:0.00:2.00:2.00";

			var basket = new ShoppingBasket("my basket") {
			                                             	new BasketItem(1, "foo", 5, 2),
			                                             	new BasketItem(1, "bar", 2, 1)
			                                             };

			basket.ToString().ShouldEqual(expected);
		}

		[Test]
		public void Calculates_total() {
			var basket = new ShoppingBasket("my basket") {
			                                             	new BasketItem(1, "foo", 5, 2),
			                                             	new BasketItem(1, "bar", 2, 1)
			                                             };

			basket.Total.ShouldEqual(12);
		}

		[Test]
		public void Basket_name() {
			var basket = new ShoppingBasket("my basket");
			basket.Name.ShouldEqual("my basket");
		}
	}
}