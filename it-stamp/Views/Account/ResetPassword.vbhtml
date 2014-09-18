@ModelType ResetPasswordViewModel
@Code
    ViewBag.Title = "パスワードのリセット"
End Code

<h1>@ViewBag.Title</h1>

@Using Html.BeginForm("ResetPassword", "Account", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})

    @Html.AntiForgeryToken()

    @<text>
        <p>パスワードをリセットしてください。</p>

        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        @Html.HiddenFor(Function(m) m.Code)


        <div class="form-group">
            @Html.LabelFor(Function(m) m.Email, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Email, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Email, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.Password, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.PasswordFor(Function(m) m.Password, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Password, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.ConfirmPassword, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.PasswordFor(Function(m) m.ConfirmPassword, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.ConfirmPassword, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            <input type="submit" class="btn btn-primary" value="リセット" />
        </div>
    </text>
End Using

@section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
