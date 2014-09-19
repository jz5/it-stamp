@ModelType SearchEventsViewModel

@Code
    Dim icon = "http://placehold.it/96x96"

End Code
@For Each e In Model.Results
    @<div class="media">
        <a class="pull-left" href="@Href("/Events/")@e.Id">
            <img class="media-object img-rounded" src="@(If(e.Community isnot Nothing andalso e.Community.IconPath<>"", "/Uploads/" & e.Community.IconPath, icon))" alt="@e.Name">
        </a>
        <div class="media-body">
            <h3><a href="@Href("/Events/")@e.Id">@e.Name</a></h3>
            <div>
                <div>
                    @e.FriendlyDateTime
                @If e.StartDateTime.Date <= Now.Date AndAlso Now.Date <= e.EndDateTime.Date Then
                    @<span class="text-primary small">（今日）</span>
                ElseIf e.StartDateTime.Date = Now.Date.AddDays(1) OrElse e.EndDateTime.Date = Now.Date.AddDays(1) Then
                    @<span class="text-muted small">（明日）</span>
                ElseIf e.EndDateTime.Date.AddDays(1) = Now.Date Then
                    @<span class="text-muted small">（昨日）</span>
                End If
            </div>

            <div><a href="">@e.Prefecture.Name</a> @e.Place</div>
            <div>@e.Description.Excerpt</div>
            @If e.Community IsNot Nothing Then
                @<div><a href="@Href("/Communities/")@e.Community.Id">@e.Community.Name</a></div>
            End If
        </div>
    </div>
</div>
Next

<ul class="pagination">
    <li class="@(If(Model.CurrentPage = 1, "disabled", ""))">@Html.ActionLink("«", "Index", "Events")</li>
    @For i = Model.StartPage To Model.EndPage
        If i = Model.CurrentPage Then
        @<li class="active"><a href="@Request.RawUrl">@i<span class="sr-only">(current)</span></a></li>
        Else
        @<li>@Html.ActionLink(i, "Index", "Events", New With {.page = i}, Nothing)</li>
        End If
    Next
    <li class="@(If(Model.CurrentPage = Model.TotalPages, "disabled", ""))">@Html.ActionLink("»", "Index", "Events", New With {.page = Model.TotalPages}, Nothing)</li>
</ul>