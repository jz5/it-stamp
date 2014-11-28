@Code
    ViewBag.Title = "IT勉強会一覧"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <a href="@Href("~/Stamprally/2015")"><img src="@Href("~/images/stamprally2015/stamprally-logo.png")" alt="" class="img-responsive" /></a>

        <h1>📢 @ViewBag.Title</h1>

        <div class="clearfix">
            <div class="pull-left">
                <span><small>@(Model.TotalCount)件@(If(Model.CurrentPage > 1, " " & Model.CurrentPage & "ページ目", ""))</small></span>
            </div>

            <div class="pull-right">
                <p>
                    <div class="dropdown">
                        <a data-toggle="dropdown" href="#">検索条件 <span class="caret"></span></a>
                        <ul class="dropdown-menu" role="menu">
                            <li>@Html.ActionLink("開催予定の勉強会", "Events", "Stamprally2015", Nothing, Nothing)</li>
                            <li>@Html.ActionLink("過去の勉強会", "Events", "Stamprally2015", New With {.past = True}, Nothing)</li>
                        </ul>
                    </div>
                </p>
            </div>
        </div>

        @For Each e In Model.Results
            @Html.Partial("_EventCard", e)
        Next

        <ul class="pagination">
            <li class="@(If(Model.CurrentPage = 1, "disabled", ""))">@Html.ActionLink("«", "Events", "Stamprally2015", New With {.past = Model.Past}, Nothing)</li>
            @For i = Model.StartPage To Model.EndPage
                If i = Model.CurrentPage Then
                @<li class="active"><a href="@Request.RawUrl">@i<span class="sr-only">(current)</span></a></li>
                Else
                @<li>@Html.ActionLink(i, "Events", "Stamprally2015", New With {.page = i, .past = Model.Past}, Nothing)</li>
                End If
            Next
            <li class="@(If(Model.CurrentPage = Model.TotalPages, "disabled", ""))">@Html.ActionLink("»", "Events", "Stamprally2015", New With {.page = Model.TotalPages, .past = Model.Past}, Nothing)</li>
        </ul>

    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
