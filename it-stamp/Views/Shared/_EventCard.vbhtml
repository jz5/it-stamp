@ModelType [Event]
@code
    Dim siteName = Model.GetEventSiteName
    Dim eventSiteLogo = ""
    If siteName <> "" Then
        eventSiteLogo = Href("~/images/logos/" & siteName & ".png")
    End If

    Dim now = TokyoTime.Now

End Code
<div class="media">
    <div class="pull-left">
        <a href="@Href("/Events/")@Model.Id">
            <img class="media-object img-rounded" src="@Href(Model.GetIconPath)" alt="@Model.Name">
        </a>
        @If eventSiteLogo <> "" Then
            @<div><img src="@eventSiteLogo" class="img-responsive text-center" style="width:72px;margin:5px 12px;" /></div>
        End If
    </div>
    <div class="media-body">
        <h3>
            <a href="@Href("/Events/")@Model.Id">@Model.Name</a>
            @If Model.SpecialEvents IsNot Nothing AndAlso Model.SpecialEvents.Any Then
                @<span title="@(Model.SpecialEvents.First.SpecialEvent.Name & "対象")">⭐</span>
            End If
        </h3>
        <div>
            <div>

                @If Model.StartDateTime.Date <= now.Date AndAlso now.Date <= Model.EndDateTime.Date Then
                    @<span class="badge badge-primary">&nbsp;今日&nbsp;</span>
                ElseIf Model.StartDateTime.Date = now.Date.AddDays(1) OrElse Model.EndDateTime.Date = now.Date.AddDays(1) Then
                    @<span class="badge badge-default">&nbsp;明日&nbsp;</span>
                ElseIf Model.EndDateTime.Date.AddDays(1) = now.Date Then
                    @<span class="badge badge-default">&nbsp;昨日&nbsp;</span>
                End If
                <time class="small text-muted">@Model.FriendlyDateTime</time>
            </div>
            <div class="small">@(If(Model.Prefecture IsNot Nothing, Model.Prefecture.Name, "")) @Model.Place</div>
            <div class="small">@Model.Description.Excerpt(200)</div>
            <div class="clearfix">
                @If Model.Community IsNot Nothing Then
                    @<div class="pull-left small"><a href="@Href("/Communities/")@Model.Community.Id">@Model.Community.Name</a></div>
                End If
            </div>
        </div>
    </div>
</div>