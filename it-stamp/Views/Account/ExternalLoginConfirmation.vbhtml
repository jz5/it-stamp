@ModelType ExternalLoginConfirmationViewModel
@Code
    ViewBag.Title = "アカウント登録"

    If ViewBag.LoginProvider = "" Then
        ViewBag.LoginProvider = Request.Form("LoginProvider")
    End If
End Code

<h1>@ViewBag.Title</h1>


@Using Html.BeginForm("ExternalLoginConfirmation", "Account", New With {.ReturnUrl = ViewBag.ReturnUrl}, FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()

    @<text>
        <div class="alert alert-info fade in" role="alert">
            <strong>@ViewBag.LoginProvider</strong> によって正常に認証されました。<br />
            このサイトのユーザー名を登録してください。
        </div>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        <div class="form-group">
            @Html.LabelFor(Function(m) m.UserName, New With {.class = "control-label"})
            <span class="text-muted">（英数字のみ・後から変更できません）</span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.UserName, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.UserName, "", New With {.class = "text-danger"})
                @Html.Hidden("LoginProvider", ViewBag.LoginProvider)
            </div>
        </div>
        <div class="form-group">
            <input type="submit" class="btn btn-primary" value="登録" />
        </div>
    </text>
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
