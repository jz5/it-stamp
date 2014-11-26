@Imports Microsoft.AspNet.Identity
@If Request.IsAuthenticated Then
    @Using Html.BeginForm("LogOff", "Account", FormMethod.Post, New With {.id = "logoutForm", .class = "navbar-right"})
        @Html.AntiForgeryToken()
        @If Session("IconPath") <> "" Then
            @<img src="@Href("~/Uploads/" & Session("IconPath"))" class="navbar-brand" style="padding-right:0;" alt="" />
        End If
        @<ul class="nav navbar-nav navbar-right">
            <li class="dropdown">
                <a class="dropdown-toggle" data-toggle="dropdown" href="#">@Session("DisplayName") <span class="caret"></span></a>
                <ul class="dropdown-menu" role="menu">
                    <li><a href="@Href("~/Events/CheckIn")">チェックイン</a></li>
                    <li class="divider"></li>
                    <li><a href="@Href("~/Users/" & User.Identity.GetUserName & "/Edit")"><i class="glyphicon glyphicon-cog"></i> 設定</a></li>
                    <li><a href="@Href("~/Account/Manage")"><i class="glyphicon glyphicon-user"></i> アカウント</a></li>
                    <li class="divider"></li>
                    <li><a href="javascript:document.getElementById('logoutForm').submit()">ログオフ</a></li>
                </ul>
            </li>
        </ul>
    End Using
Else
    @<ul class="nav navbar-nav navbar-right">
        <li>@Html.ActionLink("ログイン", "Login", "Account", New With {.ReturnUrl = If(Request.RawUrl.ToLower.Contains("login"), "", Request.RawUrl)}, htmlAttributes:=New With {.id = "loginLink"})</li>
    </ul>
End If
