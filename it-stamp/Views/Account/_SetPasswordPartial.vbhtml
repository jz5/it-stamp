@ModelType ChangePasswordViewModel


<div class="alert alert-info fade in" role="alert">
    このサイトのユーザー名とパスワードがありません。<br />
    パスワードを設定すると、外部サービスのログインなしでログインできるようになります。
</div>

@Using Html.BeginForm("ChangePassword", "Account", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()

    @<text>

        @Html.ValidationSummary("", New With {.class = "text-danger"})
        <div class="form-group">
            @Html.LabelFor(Function(m) m.NewPassword, New With {.class = "control-label"})
            @Html.PasswordFor(Function(m) m.NewPassword, New With {.class = "form-control"})
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.ConfirmPassword, New With {.class = "control-label"})
            @Html.PasswordFor(Function(m) m.ConfirmPassword, New With {.class = "form-control"})
        </div>
        <div class="form-group">
            <input type="submit" value="登録" class="btn btn-primary" />
        </div>
    </text>
End Using
