@Imports Microsoft.AspNet.Identity
@Code

    @If ViewBag.HasLocalPassword Then
        ViewBag.Title = "パスワードの変更"
    Else
        ViewBag.Title = "パスワードの設定"
    End If
    
End Code

<h1>@ViewBag.Title</h1>

<p class="text-success">@ViewBag.StatusMessage</p>

@If ViewBag.HasLocalPassword Then
    @Html.Partial("_ChangePasswordPartial")
Else
    @Html.Partial("_SetPasswordPartial")
End If

@Html.ActionLink("アカウントの管理", "Manage")


@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
