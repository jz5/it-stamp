@ModelType SearchCommunitiesViewModel
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "コミュニティ一覧"
    Dim icon = "http://placehold.it/96x96"
End Code

@Html.Partial("_TopBanner")

<div class="row">
    <div class="col-md-8">

        <h1>@ViewBag.Title</h1>

        <div>
            <span><small>@(Model.TotalCount)件@(If(Model.CurrentPage > 1, " " & Model.CurrentPage & "ページ目", ""))</small></span>
        </div>


        @For Each e In Model.Results
            @Html.Partial("_CommunityCard", e)
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


        @If Request.IsAuthenticated Then
            @<hr />
            @Html.ActionLink("登録", "Add", "Communities", Nothing, New With {.class = "btn btn-default"})
        End If

        @Html.Partial("_SocialButtons")


    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>


@Section Scripts

    @*<script>
            $(".alert").alert();
        </script>*@
End Section



