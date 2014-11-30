@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code

    ViewBag.Title = Model.FriendlyName
    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96")

End Code


<div class="row">
    <div class="col-md-8">

        <h1>@ViewBag.Title</h1>

        @If ViewBag.StatusMessage <> "" Then
            @<div class="alert alert-success fade in" role="alert">
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                @ViewBag.StatusMessage
            </div>
        End If

        <h2>表示アイコン</h2>

        <img class="media-object img-rounded" src="@icon" alt="@Model.FriendlyName" style="margin-bottom: 30px;" />
        @Html.ActionLink("変更", "Upload", "Users", New With {.id = "Me"}, New With {.class = "btn btn-default"})

        <h2>プロフィール</h2>

        @Using Html.BeginForm("Edit", "Users", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
            @Html.AntiForgeryToken()
            @<text>
                @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

                <div class="form-group">
                    @Html.LabelFor(Function(m) m.DisplayName, New With {.class = "control-label"}) <span class="text-primary">*</span>
                    <span class="text-muted"></span>
                    <div class="form-inline">
                        @Html.TextBoxFor(Function(m) m.DisplayName, New With {.class = "form-control"})
                        @Html.ValidationMessageFor(Function(m) m.DisplayName, "", New With {.class = "text-danger"})
                    </div>
                </div>

                <div class="form-group">
                    @Html.LabelFor(Function(m) m.Description, New With {.class = "control-label"})
                    <span class="text-muted"></span>
                    <div class="form-inline">
                        @Html.EditorFor(Function(m) m.Description, New With {.htmlAttributes = New With {.class = "form-control"}})
                        @Html.ValidationMessageFor(Function(m) m.Description, "", New With {.class = "text-danger"})
                    </div>
                </div>

                <div class="form-group">
                    @Html.LabelFor(Function(m) m.Url, New With {.class = "control-label"})
                    <span class="text-muted"></span>
                    <div class="form-inline">
                        @Html.TextBoxFor(Function(m) m.Url, New With {.type = "url", .class = "form-control", .placeholder = "http://example.jp"})
                        @Html.ValidationMessageFor(Function(m) m.Url, "", New With {.class = "text-danger"})
                    </div>
                </div>

                <div class="form-group">
                    <div class="form-inline">
                        <div class="checkbox">
                            @Html.CheckBoxFor(Function(m) m.IsPrivate, New With {.class = "form-control"})
                            @Html.LabelFor(Function(m) m.IsPrivate, New With {.class = "control-label toggle"})
                        </div>

                        @Html.ValidationMessageFor(Function(m) m.IsPrivate, "", New With {.class = "text-danger"})
                    </div>
                    <p class="text-muted">
                        💡 プライベートモード「ON」の場合、ユーザー名（@Model.UserName）以外の情報は表示されません。
                    </p>
                </div>

                <div class="form-group">
                    <input type="submit" value="保存" class="btn btn-primary" />
                </div>
            </text>
        End Using



        <hr />
        <aside>
            <span>
                🔎 @Html.ActionLink(Model.FriendlyName & "のプロフィールページを確認", "Details", "Users", New With {.userName = Model.UserName}, Nothing)
            </span>
        </aside>
        
    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

@section styles
    @Styles.Render("~/Content/skins/square/blue.css")
End Section

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script>
        $('input').iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-blue'
        });
    </script>
End Section

