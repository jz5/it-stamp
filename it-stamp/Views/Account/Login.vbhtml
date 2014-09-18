@ModelType LoginViewModel

@Code
    ViewBag.Title = "ログイン"
End Code


<div class="row">
    <div class="col-sm-12 col-md-8">
        <h1>@ViewBag.Title</h1>

        <section id="loginForm">
            @Using Html.BeginForm("Login", "Account", New With {.ReturnUrl = ViewBag.ReturnUrl}, FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
                @Html.AntiForgeryToken()
                @<text>
                    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                    <div class="form-group">
                        @Html.LabelFor(Function(m) m.EmailOrUserName, New With {.class = "control-label"})
                        <span class="text-muted">（英数字のユーザー名 または メールアドレス）</span>
                        <div class="form-inline">
                            @Html.TextBoxFor(Function(m) m.EmailOrUserName, New With {.class = "form-control"})
                            @Html.ValidationMessageFor(Function(m) m.EmailOrUserName, "", New With {.class = "text-danger"})
                        </div>
                    </div>
                    <div class="form-group">
                        @Html.LabelFor(Function(m) m.Password, New With {.class = "control-label"})
                        <div class="form-inline">
                            @Html.PasswordFor(Function(m) m.Password, New With {.class = "form-control"})
                            @Html.ValidationMessageFor(Function(m) m.Password, "", New With {.class = "text-danger"})
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="checkbox">
                            @Html.CheckBoxFor(Function(m) m.RememberMe)
                            @Html.LabelFor(Function(m) m.RememberMe)
                        </div>
                    </div>
                    <div class="form-group">
                        <input type="submit" value="ログイン" class="btn btn-primary" />
                    </div>
                    <p>
                        @Html.ActionLink("新規ユーザーとして登録", "Register")
                    </p>
                    <p>
                        @Html.ActionLink("パスワードを忘れた場合", "ForgotPassword")
                    </p>
                </text>
            End Using
        </section>
        <section id="socialLoginForm">
            @Html.Partial("_ExternalLoginsListPartial", New ExternalLoginListViewModel With {.Action = "ExternalLogin", .ReturnUrl = ViewBag.ReturnUrl})
        </section>

    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
