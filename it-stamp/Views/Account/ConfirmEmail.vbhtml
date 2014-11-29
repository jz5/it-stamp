@Code
    ViewBag.Title = "アカウントの確認完了"
End Code

<h1>@ViewBag.Title</h1>
<div>
    @Html.ValidationSummary("", New With {.class = "text-danger"})
    
    <p>ありがとうございます。@Html.ActionLink("ログイン", "Login", "Account", routeValues:=Nothing, htmlAttributes:=New With {Key .id = "loginLink"})してください。</p>
    <div style="height:500px;"></div>
</div>
