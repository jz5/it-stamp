@Imports Microsoft.AspNet.Identity
@ModelType ChangePasswordViewModel


<p class="text-info"><strong>@User.Identity.GetUserName()</strong> としてログインしています。</p>

@Using Html.BeginForm("ChangePassword", "Account", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})

    @Html.AntiForgeryToken()

    @<text>
        @Html.ValidationSummary("", New With {.class = "text-danger"})
        <div class="form-group">
            @Html.LabelFor(Function(m) m.OldPassword, New With {.class = "control-label"})
            @Html.PasswordFor(Function(m) m.OldPassword, New With {.class = "form-control"})
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.NewPassword, New With {.class = "control-label"})
            @Html.PasswordFor(Function(m) m.NewPassword, New With {.class = "form-control"})
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.ConfirmPassword, New With {.class = "control-label"})
            @Html.PasswordFor(Function(m) m.ConfirmPassword, New With {.class = "form-control"})
        </div>

        <div class="form-group">
            <input type="submit" value="パスワードの変更" class="btn btn-default" />
        </div>
    </text>
End Using
