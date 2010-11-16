namespace SagePayMvc.Sample.Models {
	using System;

	public class User {
		public string Forename { get; set; }
		public string Surname { get; set; }
		
		public string Name {
			get { return Forename + " " + Surname; }
		}
		
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Town { get; set; }
		public string County { get; set; }
		public string Postcode { get; set; }

		public string Telephone { get; set; }
	}
}