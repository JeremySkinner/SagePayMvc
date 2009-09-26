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

namespace SagePayMvc {
	/// <summary>
	/// Item for a shopping basket
	/// </summary>
	public class BasketItem {
		public BasketItem(int quantity, string description, decimal itemPriceExcVat) : this(quantity, description, itemPriceExcVat, Configuration.Current.VatMultiplier) {
		}

		public BasketItem(int quantity, string description, decimal itemPriceExcVat, decimal vatMultiplier) {
			Quantity = quantity;
			Description = description ?? "";
			ItemPrice = itemPriceExcVat;

			ItemTotal = itemPriceExcVat*vatMultiplier;
			ItemTax = ItemTotal - itemPriceExcVat;
			LineTotal = Quantity*ItemTotal;
		}

		public string Description { get; private set; }
		public decimal ItemPrice { get; private set; }
		public decimal ItemTax { get; private set; }
		public decimal ItemTotal { get; private set; }
		public decimal LineTotal { get; private set; }
		public int Quantity { get; private set; }
	}
}