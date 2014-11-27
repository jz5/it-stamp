@ModelType ExternalLoginListViewModel
@Imports Microsoft.Owin.Security
@Code
    Dim loginProviders = Context.GetOwinContext().Authentication.GetExternalAuthenticationTypes()
End Code
<h2>別のサービスを使用してログイン</h2>
@If loginProviders.Count() = 0 Then
    @<div>
        <p>
            設定されている外部認証サービスはありません。
        </p>
    </div>
Else
    @<p><a href="@Href("~/Home/TOS")">利用規約</a>に同意してログイン</p>
    @Using Html.BeginForm(Model.Action, "Account", New With {.ReturnUrl = Model.ReturnUrl}, FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
        @Html.AntiForgeryToken()
        @<div id="socialLoginList">
           <p>
               @For Each p As AuthenticationDescription In loginProviders
                   @<button type="submit" class="btn btn-3rdparty" id="@p.AuthenticationType" name="provider" value="@p.AuthenticationType" title="@p.Caption アカウントを使用してログイン">@p.AuthenticationType</button>
               Next
           </p>
        </div>
        @<div><p><small>※ 許可なく外部サービスに投稿しません。</small></p></div>
        
    End Using
End If
