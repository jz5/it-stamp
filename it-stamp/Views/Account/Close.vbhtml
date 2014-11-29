@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "退会"
End Code

<h1>@ViewBag.Title</h1>

<div class="jumbotron">
    <div class="jumbotron-contents">
        <h4>次の情報が、削除されます。</h4>
        <ul>
            <li>アカウント情報（メールアドレス・外部サービスのログイン設定など）</li>
            <li>プロフィール</li>
            <li>フォロー</li>
            <li>チェックイン</li>
            <li>コメント</li>
        </ul>

        <h4>次の情報は、削除されません。</h4>
        <ul>
            <li>あなたが登録や編集したIT勉強会やコミュニティの情報</li>
            <li>ユーザー名（@User.Identity.GetUserName）</li>
        </ul>
    </div>
</div>

@Using Html.BeginForm("Close", "Account", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()

    @<p>退会すると元に戻せません。同じユーザー名のアカウントは作れません。</p>

    @<div class="form-group">
        <input type="submit" value="退会する" class="btn btn-primary" />
    </div>
End Using

<hr />
@Html.ActionLink("戻る", "Manage", "Account", New With {.id = User.Identity.GetUserId()}, Nothing)

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
