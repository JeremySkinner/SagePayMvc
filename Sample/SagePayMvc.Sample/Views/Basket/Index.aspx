<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<SagePayMvc.Sample.Models.ShoppingBasketItem[]>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
	<h2>Shopping Basket Contents</h2>
	<table>
		<tr>
			<th>Product</th>
			<th>Price</th>
			<th>Quantity</th>
			<th>Subtotal</th>
			<th>&nbsp;</th>
		</tr>
		<% foreach (var basketItem in Model) { %>
		<tr>
			<td><%: basketItem.Product.Name %></td>
			<td><%: basketItem.Product.Price.ToString("C") %></td>
			<td><%: basketItem.Quantity %></td>
			<td><%: basketItem.Price.ToString("C") %></td>
			<td><%: Html.ActionLink("Remove", "Remove", new{id=basketItem.Id}) %></td>
		</tr>
		<% } %>
		<tr>
			<th colspan="4">Total</th>
			<td><%: Model.Sum(x => x.Price).ToString("C") %></td>
		</tr>
	</table>

	<form method="post" action="<%: Url.Action("Checkout") %>">
		<input type="submit" value="Go to Checkout" />
	</form>

</asp:Content>
