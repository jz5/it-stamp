@Imports Microsoft.AspNet.Identity

@If Request.IsAuthenticated
    @Using Html.BeginForm("LogOff", "Account", FormMethod.Post, New With { .id = "logoutForm", .class = "navbar-right" })
        @Html.AntiForgeryToken()
        @<ul class="nav navbar-nav navbar-right">
            @*<li>
                @Html.ActionLink(User.Identity.GetUserName(), "Manage", "Account", routeValues:=Nothing, htmlAttributes:=New With {.title = "管理"})
            </li>
            <li><a href="javascript:document.getElementById('logoutForm').submit()">ログオフ</a></li>*@

             <li class="dropdown">
                 <a class="dropdown-toggle" data-toggle="dropdown" href="#"><i class="glyphicon glyphicon-heart"></i> @User.Identity.GetUserName() <span class="caret"></span></a>
                 <ul class="dropdown-menu" role="menu">
                     <li><a href="@Href("~/Events/Today")">チェックイン</a></li>
                     <li class="divider"></li>
                     <li><a href=""><i class="glyphicon glyphicon-cog"></i> 設定</a></li>
                     <li><a href="@Href("~/Account/Manage")"><i class="glyphicon glyphicon-user"></i> アカウント</a></li>
                     <li class="divider"></li>
                     <li><a href="javascript:document.getElementById('logoutForm').submit()">ログオフ</a></li>
                 </ul>
             </li>
        </ul>
                     End Using
                 Else
    @<ul class="nav navbar-nav navbar-right">
        @*<li>@Html.ActionLink("登録", "Register", "Account", routeValues := Nothing, htmlAttributes := New With { .id = "registerLink" })</li>*@
        <li>@Html.ActionLink("ログイン", "Login", "Account", New With {.ReturnUrl = If(Request.RawUrl.ToLower.Contains("login"), "", Request.RawUrl)}, htmlAttributes:=New With {.id = "loginLink"})</li>
    </ul>
End If

