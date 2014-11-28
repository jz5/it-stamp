@Code
    ViewBag.Title = "パスワードのリセットの確認"
End Code

<h1>@ViewBag.Title</h1>
<div>
    <p>
        パスワードがリセットされました。@Html.ActionLink("ここをクリックしてログイン", "Login", "Account", routeValues:=Nothing, htmlAttributes:=New With {Key .id = "loginLink"})してください
    </p>
    <div style="height:500px;"></div>
</div>
