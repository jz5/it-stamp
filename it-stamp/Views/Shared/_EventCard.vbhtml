@ModelType [Event]
@code
    Dim icon = "http://placehold.it/96x96"
End Code
<div class="media">
    <a class="pull-left" href="@Href("/Events/")@Model.Id">
        <img class="media-object img-rounded" src="@(If(Model.Community isnot Nothing andalso Model.Community.IconPath<>"", "/Uploads/" & Model.Community.IconPath, icon))" alt="@Model.Name">
    </a>
    <div class="media-body">
        <h3><a href="@Href("/Events/")@Model.Id">@Model.Name</a></h3>
        <div>
            <div>
                @Model.FriendlyDateTime
                @If Model.StartDateTime.Date <= Now.Date AndAlso Now.Date <= Model.EndDateTime.Date Then
                    @<span class="text-primary small">（今日）</span>
                ElseIf Model.StartDateTime.Date = Now.Date.AddDays(1) OrElse Model.EndDateTime.Date = Now.Date.AddDays(1) Then
                    @<span class="text-muted small">（明日）</span>
                ElseIf Model.EndDateTime.Date.AddDays(1) = Now.Date Then
                    @<span class="text-muted small">（昨日）</span>
                End If
            </div>

            <div>@Model.Prefecture.Name @Model.Place</div>
            <div>@Model.Description.Excerpt</div>

            @If Model.Community IsNot Nothing Then
                @<div><a href="@Href("/Communities/")@Model.Community.Id">@Model.Community.Name</a></div>
            End If
        </div>
    </div>
</div>
