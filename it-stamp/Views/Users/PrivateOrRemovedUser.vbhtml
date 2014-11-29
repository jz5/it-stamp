@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code

    ViewBag.Title = If(Model.IsRemoved, Model.UserName, Model.FriendlyName)

End Code
<div class="row">
    <div class="col-md-8">
        @If Model.IsRemoved Then
            @<div class="alert alert-info fade in" role="alert">
                🚫 退会したユーザーです。
            </div>

        ElseIf Model.IsPrivate Then
            @<h1>@(User.Identity.GetUserName)さん</h1>
            @<div class="alert alert-info fade in" role="alert">
                🔒 プライベートモードのユーザーです。
            </div>
        End If
    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
