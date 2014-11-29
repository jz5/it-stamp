@Imports Microsoft.AspNet.Identity
@ModelType ManageUserViewModel
@Code
    ViewBag.Title = "アカウントの管理"
End Code

<h1>@ViewBag.Title</h1>

@If ViewBag.StatusMessage <> "" AndAlso Request.IsAuthenticated Then
    @<div class="alert alert-success fade in" role="alert">
        <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
        @ViewBag.StatusMessage
    </div>
End If
@If ViewBag.ErrorMessage <> "" AndAlso Request.IsAuthenticated Then
    @<div class="alert alert-danger fade in" role="alert">
        <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
        @ViewBag.ErrorMessage
    </div>
End If

<section>
    <h2>メールアドレスの変更</h2>
    @Using Html.BeginForm("Manage", "Account", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})

        @Html.AntiForgeryToken()

        @<text>
            @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
            <div class="form-group">
                @Html.LabelFor(Function(m) m.Email, New With {.class = "control-label"})
                <div class="form-inline">
                    @Html.TextBoxFor(Function(m) m.Email, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.Email, "", New With {.class = "text-danger"})
                </div>
            </div>
            <div class="form-group">
                <input type="submit" value="@(If(ViewBag.HasEmail, "メールの変更", "メールの設定"))" class="btn btn-default" />
            </div>
        </text>
    End Using
</section>

<section>
    <h2>パスワードの変更</h2>

    @If ViewBag.HasLocalPassword Then
        @Html.ActionLink("パスワードの変更", "ChangePassword")
    Else
        @Html.ActionLink("パスワードの設定", "ChangePassword")
    End If
</section>

<section id="externalLogins">
    @Html.Partial("_ExternalLoginsListPartial", New ExternalLoginListViewModel With {.Action = "LinkLogin", .ReturnUrl = ViewBag.ReturnUrl})
    @Html.Action("RemoveAccountList")
</section>

<section>
    <h2>アカウントの削除</h2>
    @Html.ActionLink("退会", "Close", Nothing, New With {.class = "btn btn-default"})
</section>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
