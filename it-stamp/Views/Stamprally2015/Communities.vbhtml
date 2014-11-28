@ModelType SearchCommunitiesViewModel
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "参加コミュニティ一覧"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <a href="@Href("~/Stamprally/2015")"><img src="@Href("~/images/stamprally2015/stamprally-logo.png")" alt="" class="img-responsive" /></a>

        <h1>@ViewBag.Title</h1>
        <div>
            <span><small>@(Model.TotalCount)件@(If(Model.CurrentPage > 1, " " & Model.CurrentPage & "ページ目", ""))</small></span>
        </div>


        @For Each e In Model.Results
            @Html.Partial("_CommunityCard", e, ViewData)
        Next

        <ul class="pagination">
            <li class="@(If(Model.CurrentPage = 1, "disabled", ""))">@Html.ActionLink("«", "Index", "Communities")</li>
            @For i = Model.StartPage To Model.EndPage
                If i = Model.CurrentPage Then
                @<li class="active"><a href="@Request.RawUrl">@i<span class="sr-only">(current)</span></a></li>
                Else
                @<li>@Html.ActionLink(i, "Index", "Communities", New With {.page = i}, Nothing)</li>
                End If
            Next
            <li class="@(If(Model.CurrentPage = Model.TotalPages, "disabled", ""))">@Html.ActionLink("»", "Index", "Communities", New With {.page = Model.TotalPages}, Nothing)</li>
        </ul>


    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
