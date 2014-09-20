@ModelType Community
@Imports Microsoft.AspNet.Identity
@Code

    Dim userIcon = Href("/Uploads/Icons/anon.png")

    ViewData("Details") = True
    
End Code

@Html.Partial("_TopBanner")

<div class="row">
    <div class="col-md-8">

        <h1>@Model.Name</h1>

        @If ViewBag.StatusMessage <> "" AndAlso Request.IsAuthenticated Then
            @<div class="alert alert-success fade in" role="alert">
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                @ViewBag.StatusMessage
            </div>
        End If

        @Html.Partial("_CommunityCard", Model, ViewData)
        
        <h2>主催しているIT勉強会</h2>

        <p class="text-muted">（未実装）</p>
        @*<p class="text-muted">開催予定のIT勉強会はありません。</p>
            <div>
                @Html.ActionLink("過去のIT勉強会", "Events", "Search")
            </div>*@

        <h2>フォロワー <span class="badge badge-primary @(If(Model.Members.Count = 0, "hidden", ""))">@Model.Members.Count</span></h2>
        <div>
            @If Model.Members.Count = 0 Then
                @<p class="text-muted">フォロワーはいません。</p>
            Else
                @For Each m In Model.Members.Where(Function(u) Not u.IsPrivate)
                    @<a href="@Href("~/Users/" & m.UserName)"><img src="@(If(M.IconPath <> "", Href("/Uploads/" & m.IconPath), "http://placehold.it/16x16"))" class="img-rounded icon24" alt="" title="@m.FriendlyName" /></a>
                Next
                                                If Model.Members.Where(Function(u) u.IsPrivate).Count > 0 Then
                @<img src="@userIcon" class="img-rounded icon24" alt="" title="プライベートユーザー（ひとり以上）" />
                End If

            End If
        </div>



        @Html.Partial("_SocialButtons")

        @If ViewBag.CanEdit Then
            @<a href="@Url.Action("Edit", "Communities", New With {.id = Model.Id})"><i class="glyphicon glyphicon-pencil"></i> 編集</a>
        Else
            @<i class="glyphicon glyphicon-pencil" title="編集権限がありません"></i>
        End If




    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>


@section Scripts
    <script>
        (function ($) {
            var followed = @(If(ViewBag.Followd, "true", "false"));
            if (followed) {
                $("#follow-btn").hover(function () {
                    $(this).val("解除").removeClass("btn-default").addClass("btn-primary");
                },function () {
                    $(this).val("フォロー中").removeClass("btn-primary").addClass("btn-default");
                });
            } else {
                $("#follow-btn").hover(function () {
                    $(this).removeClass("btn-default").addClass("btn-primary");
                },function () {
                    $(this).removeClass("btn-primary").addClass("btn-default");
                });
            }
        })(jQuery);
    </script>
End Section