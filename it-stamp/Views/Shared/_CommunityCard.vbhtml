@ModelType Community
@Imports Microsoft.AspNet.Identity
@Code

    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96")

End Code

<div class="media">
    <div class="pull-left">
        @If ViewData("InternalLink") = True Then
            @<a href="@Href("/Communities/")@Model.Id">
                <img class="media-object img-rounded" src="@icon" alt="@Model.Name">
            </a>
        Else
            @If Model.Url <> "" Then
                @<a href="@Model.Url" target="_blank">
                    <img class="media-object img-rounded" src="@icon" alt="@Model.Name">
                </a>
            Else
                @<img class="media-object img-rounded" src="@icon" alt="@Model.Name">
            End If
        End If

        @If Request.IsAuthenticated AndAlso ViewData("Details") AndAlso Not Model.IsHidden Then
            @<div class="text-center">
                @Using Ajax.BeginForm("Follow", "Communities", New With {.id = Model.Id}, New AjaxOptions With {.HttpMethod = "POST", .OnSuccess = "onSuccess", .OnBegin = "onBegin"}, New With {.class = "form-horizontal", .role = "form"})
                    @Html.AntiForgeryToken()
                    @Html.HiddenFor(Function(m) m.Id)
                    @Html.HiddenFor(Function(m) m.Name)

                    @Html.ValidationSummary(False, "", New With {.class = "text-danger"})

                    @<div class="form-group">
                        <div class="form-inline">
                            <input id="follow-btn" type="submit" value="@(if(ViewBag.Followd,"フォロー中","フォロー"))" class="btn btn-default" style="max-width:96px;" />
                        </div>
                    </div>
                End Using
            </div>
        End If
    </div>
    <div class="media-body">
        @If ViewData("Details") Then
            @<p>@Html.Raw(Html.Encode(Model.Description).Replace(vbCrLf, "<br />"))</p>
        Else
            @<h3><a href="@Href("/Communities/")@Model.Id">@Model.Name</a></h3>
            @<p>@Model.Description.Excerpt</p>
        End If
        @If Model.Url <> "" Then
            @<a href="@Model.Url" target="_blank">@Model.Url</a>
        End If
    </div>
</div>
