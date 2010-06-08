<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Upload
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<p>hi</p>
<% if (ViewData["file"] != null)
         {%>

         <p><b><%= ViewData["file"] %></b></p>
      <%
         }%>

</asp:Content>
