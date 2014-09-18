@Code
    ViewBag.Title = "パスワードのリセット"
End Code

<h1>@ViewBag.Title</h1>
<div>
    <p>
        メールを送信しました。メールを確認してパスワードをリセットしてください。
    </p>
    <p class="text-danger">
        For DEMO only: You can click this link to reset password: <a href="@ViewBag.Link">link</a>
        Please change this code to register an email service in IdentityConfig to send an email.
    </p>
</div>
