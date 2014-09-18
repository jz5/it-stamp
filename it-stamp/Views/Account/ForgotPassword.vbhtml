@ModelType ForgotPasswordViewModel
@Code
    ViewBag.Title = "パスワードのリセット"
End Code

<h1>@ViewBag.Title</h1>

@Using Html.BeginForm("ForgotPassword", "Account", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @<text>
        <p>メールアドレスを入力してください。</p>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

        <div class="form-group">
            <div class="form-group">
                @Html.LabelFor(Function(m) m.Email, New With {.class = "control-label"})
                <div class="form-inline">
                    @Html.TextBoxFor(Function(m) m.Email, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.Email, "", New With {.class = "text-danger"})
                </div>
            </div>
        </div>
        <div class="form-group">
            <input type="submit" class="btn btn-primary" value="パスワードのリセット" />
        </div>
    </text>
End Using

@section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
