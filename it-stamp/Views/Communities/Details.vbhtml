@ModelType Community
@Imports Microsoft.AspNet.Identity
@Code

    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96")
End Code

@*<ol class="breadcrumb breadcrumb-arrow">
        <li><a href="#">Home</a></li>
        <li><a href="#">コミュニティ</a></li>
        <li class="active"><span>@Model.Name</span></li>
    </ol>*@

@Html.Partial("_TopBanner")

<div class="row">
    <div class="col-md-8">

        <h1>@Model.Name</h1>

        @If ViewBag.StatusMessage <> "" Then
            @<div class="alert alert-success fade in" role="alert">
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                @ViewBag.StatusMessage
            </div>
        End If

        <div class="media">
            @If Model.Url <> "" Then
                @<a class="pull-left" href="@Model.Url">
                    <img class="media-object img-rounded" src="@icon" alt="@Model.Name">
                </a>
            Else
                @<img class="pull-left media-object img-rounded" src="@icon" alt="@Model.Name">
            End If
            <div class="media-body">
                <p>@Html.Raw(Html.Encode(Model.Description).Replace(vbCrLf, "<br />"))</p>
            </div>
        </div>

        <h2>主催しているIT勉強会</h2>

        <p class="text-muted">開催予定のIT勉強会はありません。</p>

        <div>
            @Html.ActionLink("過去のIT勉強会", "Events", "Search")
        </div>
        @*<div>
                @Html.ActionLink("登録", "Add", "Communities", Nothing, New With {.class = "btn btn-default"})
            </div>*@

        <h2>メンバー</h2>

        @If Model.Members.Count = 0 Then
            @<p class="text-muted">メンバーはいません。</p>
        Else
            @For Each m In Model.Members.Where(Function(u) Not u.IsPrivate)
                @<p>@m.UserName</p>
            Next
        End If

        @If Model.Members.Where(Function(m) m.Id = User.Identity.GetUserId).FirstOrDefault Is Nothing Then
            @Html.ActionLink("参加", "Edit", "CommunitiesAdmin", Nothing, New With {.class = "btn btn-default"})
        Else
            @Html.ActionLink("脱退", "Edit", "CommunitiesAdmin", Nothing, New With {.class = "btn btn-default"})
        End If


        @If ViewBag.CanEdit Then
            @<hr />
            @Html.ActionLink("編集", "Edit", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
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



