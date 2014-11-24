@ModelType Community
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "コミュニティの編集"
    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96")

End Code

<h1>@ViewBag.Title</h1>

@Using Html.BeginForm("Edit", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @<text>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        @Html.HiddenFor(Function(m) m.IconPath)

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Name, New With {.class = "control-label"}) <span class="text-primary">*</span>
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Name, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Name, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Description, New With {.class = "control-label"})
            <span class="text-muted">（簡潔な紹介）</span>
            <div class="form-inline">
                @Html.EditorFor(Function(m) m.Description, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(m) m.Description, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Url, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Url, New With {.type = "url", .class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Url, "", New With {.class = "text-danger"})
            </div>
        </div>

        @If ViewBag.IsOwner Then
        @<div class="form-group">
            @Html.LabelFor(Function(m) m.IsLocked, New With {.class = "control-label"})
            <span class="text-muted"></span>
            @Html.CheckBoxFor(Function(m) m.IsLocked)
            @Html.ValidationMessageFor(Function(m) m.IsLocked, "", New With {.class = "text-danger"})
        </div>

        @<div class="form-group">
            @Html.LabelFor(Function(m) m.IsHidden, New With {.class = "control-label"})
            <span class="text-muted"></span>
            @Html.CheckBoxFor(Function(m) m.IsHidden)
            @Html.ValidationMessageFor(Function(m) m.IsHidden, "", New With {.class = "text-danger"})
        </div>
            
        End If

        <div class="form-group">
            <input type="submit" value="編集" class="btn btn-primary" />
        </div>
    </text>
        End Using


<hr />
<img class="media-object img-rounded" src="@icon" alt="@Model.Name" style="margin-bottom: 30px;" />
@Html.ActionLink("変更", "Upload", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})

<hr />

@If ViewBag.IsOwner Then
    @<h2>コミュニティスタンプ</h2>
    @Html.ActionLink("スタンプを追加", "UploadStamp", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
    @If Model.Stamps IsNot Nothing AndAlso Model.Stamps.Count > 0 Then
        @Using Html.BeginForm("EditDefaultStamp", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
            @Html.AntiForgeryToken()
            @<text>
                @Html.Hidden("id", Model.Id)

                <table class="table">
                    <tr>
                        <th>IsDefault</th>
                        <th>Name</th>
                        <th>Image</th>
                        <th>Expression</th>
                        <th></th>
                    </tr>
                    @For Each item In Model.Stamps
                        @<tr>
                            <td>@Html.RadioButton("defaultStamp", item.Id, (Model.DefaultStamp.Id = item.Id), New With {.id = Nothing})</td>
                            <td>@Html.Label(item.Name)</td>
                            <td><p><img class="img-rounded" src="@Href("/Uploads/" & item.Path)" /></p></td>
                            <td>@Html.Label(item.Expression)</td>
                            <td>
                                @Html.Label("Edit")
                                @Html.ActionLink("Delete", "DeleteStamp", New With {.id = Model.Id, .stampId = item.Id})
                            </td>
                        </tr>
                    Next
                </table>
                <div class="form-group">
                    <input type="submit" value="設定" class="btn btn-primary" />
                </div>
            </text>
        End Using
    Else
        @<p>コミュニティスタンプがまだ登録されていません。</p>
    End If
    @<hr />
@<h2>コミュニティ管理者</h2>
@Html.ActionLink("管理者を追加", "AddOwner", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
@<table class="table">
    <tr>
        <th>Image</th>
        <th>Name</th>
        <th></th>
    </tr>
    <tr>
        <td><p><img class="img-rounded" src="@(If(ViewBag.UserIcon <> "", Href("/Uploads/" & ViewBag.UserIcon), "http://placehold.it/96x96"))" /></p></td>
        <td>@ViewBag.UserFriendlyName</td>
        <td></td>
    </tr>
    @For Each item In Model.Owners
        @If item.Id = ViewBag.SessionUserId Then
                Continue For
            End If
        @<tr>
            <td><p><img class="img-rounded" src="@(If(item.IconPath <> "", Href("/Uploads/" & item.IconPath), "http://placehold.it/96x96"))" /></p></td>
            <td>@Html.Label(item.FriendlyName)</td>
            <td>
                @Html.ActionLink("Delete", "DeleteOwner", New With {.id = Model.Id, .targetId = item.Id})
            </td>
        </tr>
    Next
</table>
End If

<hr />
<h2>コミュニティの削除</h2>
@Html.ActionLink("削除", "Delete", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section

<script>

    $(".stamp-item").click(function () {
        // 探索してSelectedクラスをRemove
        r.find(".providedGroupwareLogo").removeClass("selected");
        $(this).addClass("selected");

        // Listと同期

        var e = $(this).attr("id").substring(n.length);
        $("#cba_groupwareLogoName").val(e)
    })

</script>
