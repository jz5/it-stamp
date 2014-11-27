@ModelType RegisterViewModel
@Code
    ViewBag.Title = "アカウント登録"
End Code

<h1>@ViewBag.Title</h1>

<section>
    @Using Html.BeginForm("Register", "Account", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})

        @Html.AntiForgeryToken()

        @<text>
            <p>新しいアカウントを作成します。</p>
            @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
            <div class="form-group">
                @Html.LabelFor(Function(m) m.UserName, New With {.class = "control-label"})
                <span class="text-muted">（英数字のみ・後から変更できません）</span>
                <div class="form-inline">
                    @Html.TextBoxFor(Function(m) m.UserName, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.UserName, "", New With {.class = "text-danger"})
                </div>
            </div>
            <div class="form-group">
                @Html.LabelFor(Function(m) m.Email, New With {.class = "control-label"})
                <div class="form-inline">
                    @Html.TextBoxFor(Function(m) m.Email, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.Email, "", New With {.class = "text-danger"})
                </div>
            </div>
            <div class="form-group">
                @Html.LabelFor(Function(m) m.Password, New With {.class = "control-label"})
                <span class="text-muted">（8文字以上）</span>
                <div class="form-inline">
                    @Html.PasswordFor(Function(m) m.Password, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.Password, "", New With {.class = "text-danger"})
                </div>
            </div>
            <div class="form-group">
                @Html.LabelFor(Function(m) m.ConfirmPassword, New With {.class = "control-label"})
                <div class="form-inline">
                    @Html.PasswordFor(Function(m) m.ConfirmPassword, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.ConfirmPassword, "", New With {.class = "text-danger"})
                </div>
            </div>
            <div class="form-group">
                <a href="@Href("~/Home/TOS")">利用規約</a>に同意して <input type="submit" class="btn btn-primary" value="登録" />
            </div>
        </text>
    End Using
</section>
<section id="socialLoginForm">
    @Html.Partial("_ExternalLoginsListPartial", New ExternalLoginListViewModel With {.Action = "ExternalLogin", .ReturnUrl = ViewBag.ReturnUrl})
</section>

@section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
