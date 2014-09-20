﻿@ModelType UploadCommunityIconViewModel
@Imports Microsoft.AspNet.Identity
@Code
    @ViewBag.Title = "アイコンの変更"
    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96")
End Code

<h1>@ViewBag.Title</h1>

@Using Html.BeginForm("Upload", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form", .enctype = "multipart/form-data"})
    @Html.AntiForgeryToken()
    @<text>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        @Html.HiddenFor(Function(m) m.IconPath)

        <div class="form-group">
            <img class="media-object img-rounded" src="@icon" alt="@Model.Name" />

            <div style="margin: 30px 0 15px;">
                <div class="form-inline">
                    @Html.TextBoxFor(Function(m) m.File, New With {.class = "form-control", .type = "file"})
                    @Html.ValidationMessageFor(Function(m) m.File, "", New With {.class = "text-danger"})
                </div>
            </div>
        </div>

        <ul class="text-info">
            <li>PNG/JPEG形式のみ</li>
            <li>96x96ピクセル推奨</li>
            <li>他者の権利を侵害する画像をアップロードしないでください。</li>
        </ul>

    </text>
End Using

@*@Using Html.BeginForm("Upload", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
        @Html.AntiForgeryToken()
        @<text>
            @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

            <div class="form-group">
                <input type="submit" value="削除" class="btn btn-default" />
            </div>

        </text>
    End Using*@

<hr />
@Html.ActionLink("戻る", "Edit", "Communities", New With {.id = Model.Id}, Nothing)

@Section Styles
    @Styles.Render("~/Content/fileinput.css")

End Section


@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    @Scripts.Render("~/Scripts/fileinput.js")

    <script>
        $("#File").fileinput({
            browseLabel: "選択...",
            uploadLabel: "アップロード",
            uploadClass: "btn btn-primary",
            removeLabel: "解除",
            browseClass: "btn btn-default",
            maxFileSize: 1024,
            maxFileCount: 1,
            msgSizeTooLarge: '<b>{maxSize} KB</b> 以下のファイル選択してください。"{name}" (<b>{size} KB</b>) '
        });
    </script>

End Section

