@ModelType Community
@Imports Microsoft.AspNet.Identity
@Code

End Code

@*<h1>コミュニティ管理者の登録</h1>

@If Model.Url = "" Then
    @<div class="alert alert-info fade in" role="alert">
        <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
        管理者を登録するには、Webサイトが登録されている必要があります。<br />
        
    </div>

    @Html.ActionLink("編集", "Edit", "Communities", Nothing, New With {.class = "btn btn-default"})
Else

    @<text>
        <h2>ファイルをアップロードして登録</h2>

        <p>Webサイトに下記のファイルをアップロード後、認証してください。</p>

        @Model.Url/<a href="">@User.Identity.GetUserId&amp;</a>


    </text>

End If

@Using Html.BeginForm("AuthorizeOwner", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @<text>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Name, New With {.class = "control-label"}) <span class="text-primary">*</span>
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Name, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Name, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Url, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Url, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Url, "", New With {.class = "text-danger"})
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
            <input type="submit" value="編集" class="btn btn-default" />
        </div>
    </text>
End Using

<hr />

@Using Html.BeginForm("Delete", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @<text>
        <div class="form-group">
            <input type="submit" value="削除" class="btn btn-primary" />
        </div>
    </text>
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section*@

