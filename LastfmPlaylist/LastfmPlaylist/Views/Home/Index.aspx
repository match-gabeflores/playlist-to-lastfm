<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Last.fm Playlist Converter
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

<style type="text/css">
    #output 
    {
        padding: 1em;
        border-style: dashed;
        margin-top: 2em;
        background-color: #aaaaff;
        width: inherit;
    }
</style>

<script type="text/javascript">
    $(document).ready(function (event) {
        inputCSS(event);


        $("a").click(function () {
            $.get($(this).attr("href"), function (response) {
                $("#output").html($("p", response));
            });
            return false;
        });
        /*

        $("form[action$='Home/Upload']").click(function () {
            $.post($(this).attr("action"), function (response) {  // no action attribute on submit
                $("#output").html($("#time", response));
            });
            return false;
        });
        
          */
});
  </script>
<script type="text/javascript">
    function selectAll() {
        var content = eval("document.myform.lastfmArea");
        content.focus();
        content.select();
    }
</script>
<div id="content">
    <fieldset id="main-fieldset"><legend>Last.fm Playlist</legend>
          <div id="about-content">
          <fieldset><legend>About</legend>
          <p>This tool wil convert a music playlist to text that can be used on the 
          <a href="http://www.last.fm">Last.fm website</a>.<br /> Take a look at the <span style="font-weight:bold;"><a href="http://www.last.fm/user/w3stfa11/journal/2010/04/20/3ktefc_start_me_up_playlist">end result</a></span>.</p>
          
          <p><strong>Instructions:</strong> <ol>
          <li>Create a save a music playlist (using Winamp or iTunes). This will usually have a .m3u extension.</li>
          <li>Upload the playlist here.</li>
          <li>Copy the outputted text into a new Last.fm post. By default, it will include links to Wikipedia, MySpace, and YouTube.</li></ol></p>
          
          </fieldset>
</div>

<style>
#about-content { display: block; float:inherit; width: 600px; padding-right: 20px;}
#content { width: 800px; }

</style>

    <form name="myform" method="post" enctype="multipart/form-data" action="<%=Url.Action("Upload", "Home")%>">
	  <div id="selection">
      Open your playlist file: <br />
	  <input type="file" name="fileUpload" />  &nbsp;&nbsp;<input type="submit" value="Upload" />

          <div id="boxes">
          
            <%= Html.CheckBox("youtube",true) %>
            <%= Html.Label("YouTube") %>
            <input type="checkbox" value="true" name="wikipedia" checked="checked" /> <label for="wikipedia">Wikipedia</label>
            <input name="wikipedia" type="hidden" value="false" /> 
            <input type="checkbox" id="myspace123" value="true" name="myspace" checked="checked" /> <label for="myspace">MySpace</label>
            <input name="myspace" type="hidden" value="false" /> 
          </div>
	   </div>
      <br />
	  

      <!-- todo: onclick - select all and copy to clipboard | or add button/link to copy to clipboard -->
      <div id="textarea">
        <textarea cols="50" rows="18" name="lastfmArea" onclick="selectAll();"><%=TempData["output"]%></textarea>
       
	  </div>

      
	</form>
   
    </fieldset>
    </div>
    
    


      
</asp:Content>

