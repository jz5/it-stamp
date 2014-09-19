﻿@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code

    ViewBag.Title = Model.FriendlyName

    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96")
End Code

@*@Html.Partial("_TopBanner")*@

<div class="row">
    <div class="col-md-8">

        <h1>@ViewBag.Title</h1>

        @If Model.IsPrivate Then
            @<div class="alert alert-info fade in" role="alert">
                <i class="glyphicon glyphicon-ban-circle"></i> プライベートモードのユーザーです。
            </div>
        Else
            @<text>
                <div class="media">
                    @If Model.Url <> "" Then
                        @<a class="pull-left" href="@Model.Url" target="_blank">
                            <img class="media-object img-rounded" src="@icon" alt="@Model.FriendlyName">
                        </a>
                    Else
                        @<img class="pull-left media-object img-rounded" src="@icon" alt="@Model.FriendlyName">
                    End If
                    <div class="media-body">
                        <p>@Html.Raw(Html.Encode(Model.Description).Replace(vbCrLf, "<br />"))</p>
                        @If Model.Url <> "" Then
                            @<p><a href="@Model.Url" target="_blank">@Model.Url</a></p>
                        End If
                    </div>
                </div>

                <h2>参加したIT勉強会</h2>

                @If Model.Communities.Count = 0 Then
                    @<p class="text-muted">参加したIT勉強会はありません。</p>
                Else
                    @<p class="text-muted">（未実装）</p>
                End If

                <h2>参加しているコミュニティ</h2>

                @If Model.Communities.Count = 0 Then
                    @<p class="text-muted">参加しているコミュニティはありません。</p>
                Else
                    @<p class="text-muted">（未実装）</p>
                End If




                @Html.Partial("_SocialButtons")
            </text>

        End If


    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
