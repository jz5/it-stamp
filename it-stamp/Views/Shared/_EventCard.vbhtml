@ModelType [Event]
@code
    Dim icon = "http://placehold.it/96x96"
End Code
<div class="media">
    <a class="pull-left" href="@Href("/Events/")@Model.Id">
        <img class="media-object img-rounded" src="@(If(Model.Community isnot Nothing andalso Model.Community.IconPath<>"", "/Uploads/" & Model.Community.IconPath, icon))" alt="@Model.Name">
    </a>
    <div class="media-body">
        <h3>
            <a href="@Href("/Events/")@Model.Id">@Model.Name</a>
            @If Model.SpecialEvents IsNot Nothing Then
                @<span title="@(Model.SpecialEvents.Name & "対象")">⭐</span>
            End If
        </h3>
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
            <div class="clearfix">
                @If Model.Community IsNot Nothing Then
                    @<div class="pull-left"><a href="@Href("/Communities/")@Model.Community.Id">@Model.Community.Name</a></div>
                End If
                @*<div style="" class="pull-right small">
                        <div class="pull-left">
                            <a href="#"><i class="glyphicon glyphicon-ok"></i> チェックイン 0</a>
                        </div>
                        <div class="pull-left" style="margin-left: 16px;">
                            <a href="#"><i class="glyphicon glyphicon-star"></i> お気に入り 0</a>
                        </div>
                        <div class="pull-left" style="margin-left: 16px;">
                            <a href="#"><i class="glyphicon glyphicon-comment"></i> コメント 0</a>
                        </div>
                    </div>*@
            </div>
        </div>
    </div>
</div>