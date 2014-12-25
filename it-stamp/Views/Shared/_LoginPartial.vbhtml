@Imports Microsoft.AspNet.Identity
@code
    Dim name = Session("DisplayName")
    Dim icon = Session("IconPath")

    If name = "" Then
        Dim db = New ApplicationDbContext
        Dim id = User.Identity.GetUserId
        Dim appUser = db.Users.Where(Function(u) u.Id = id).SingleOrDefault
        If appUser IsNot Nothing Then
            name = appUser.DisplayName
            icon = appUser.IconPath
            Session("DisplayName") = name
            Session("IconPath") = icon
        End If
    End If

End Code
@If Request.IsAuthenticated Then
    @Using Html.BeginForm("LogOff", "Account", FormMethod.Post, New With {.id = "logoutForm", .class = "navbar-right"})
        @Html.AntiForgeryToken()

        @<ul class="nav navbar-nav navbar-right hidden-xs visible-sm visible-md visible-lg">
            @*<li class="hidden-xs" style="margin-right:-10px;" data-toggle="tooltip"><a href=""><span style="font-size:15px;font-weight:normal;padding:0.3em 0.6em 0.2em;" class="label label-success">0</span></a></li>*@
            <li class="dropdown">
                <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                    <img src="@Href("~/Uploads/" & icon)" class="navbar-brand hidden-xs img-rounded" style="height:40px;width:40px;margin:-10px 10px -10px 0;padding:0;" alt="" />
                    @name <span class="caret"></span>
                </a>
                <ul class="dropdown-menu" role="menu">
                    <li><a href="@Href("~/Users/" & User.Identity.GetUserName & "/My")">マイページ</a></li>
                    <li class="divider"></li>
                    <li><a href="@Href("~/Users/" & User.Identity.GetUserName & "/Edit")">プロフィール編集</a></li>
                    <li><a href="@Href("~/Account/Manage")">アカウント管理</a></li>
                    <li class="divider"></li>
                    <li><a href="javascript:document.getElementById('logoutForm').submit()">ログオフ</a></li>
                </ul>
            </li>
        </ul>
        @<ul class="nav navbar-nav navbar-right visible-xs hidden-sm hidden-md hidden-lg">
            <li><a href="@Href("~/Users/" & User.Identity.GetUserName & "/My")">マイページ</a></li>
            <li class="divider"></li>
            <li><a href="@Href("~/Users/" & User.Identity.GetUserName & "/Edit")">プロフィール編集</a></li>
            <li><a href="@Href("~/Account/Manage")">アカウント管理</a></li>
            <li class="divider"></li>
            <li><a href="javascript:document.getElementById('logoutForm').submit()">ログオフ</a></li>
        </ul>
    End Using
Else
    @<ul class="nav navbar-nav navbar-right">
        <li>@Html.ActionLink("ログイン", "Login", "Account", New With {.ReturnUrl = If(Request.RawUrl.ToLower.Contains("login"), "", Request.RawUrl)}, htmlAttributes:=New With {.id = "loginLink"})</li>
    </ul>
End If
