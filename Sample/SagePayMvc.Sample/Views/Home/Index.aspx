<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SagePayMvc.Sample.Models.Product[]>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
		This site is a small example of using SagePayMvc within an ASP.NET MVC 2 application.
    </p>
	<p>
		Click a product to add it to your shopping basket.
	</p>
	<ul>
		<% foreach (var product in Model) { %>
			<li><%= Html.ActionLink(product.Name, "Add", "Basket", new{id=product.Id},null) %> - <%= product.Price.ToString("C") %></li>
		<% } %>
	</ul>
</asp:Content>
