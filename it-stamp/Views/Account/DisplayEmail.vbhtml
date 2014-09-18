@Code
    ViewData("Title") = "確認メールの送信"
End Code

<h1>@ViewBag.Title</h1>
<div>
    <p>
        確認メールを送信しました。メールのリンク先にアクセスしてください。
    </p>
    <p class="text-danger">
        For DEMO only: You can click this link to confirm the email: <a href="@ViewBag.Link">link</a>
        Please change this code to register an email service in IdentityConfig to send an email.
    </p>
</div>
