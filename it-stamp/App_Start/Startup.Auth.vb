Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin.Security.DataProtection
Imports Microsoft.Owin.Security.Google
Imports Owin
Imports System
Imports Microsoft.Owin.Security.Twitter
Imports System.Security.Claims
Imports Microsoft.Owin.Security.Facebook

Partial Public Class Startup
    ' 認証設定の詳細については、http://go.microsoft.com/fwlink/?LinkId=301864 を参照してください
    Public Sub ConfigureAuth(app As IAppBuilder)
        ' リクエストあたり 1 インスタンスのみを使用するように DB コンテキストとユーザー マネージャーを設定します
        app.CreatePerOwinContext(AddressOf ApplicationDbContext.Create)
        app.CreatePerOwinContext(Of ApplicationUserManager)(AddressOf ApplicationUserManager.Create)
        app.CreatePerOwinContext(Of ApplicationRoleManager)(AddressOf ApplicationRoleManager.Create)

        ' アプリケーションが Cookie を使用して、サインインしたユーザーの情報を格納できるようにします
        ' また、サードパーティのログイン プロバイダーを使用してログインするユーザーに関する情報を、Cookie を使用して一時的に保存できるようにします
        ' サインイン Cookie の設定
        app.UseCookieAuthentication(New CookieAuthenticationOptions() With {
            .AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            .Provider = New CookieAuthenticationProvider() With {
                .OnValidateIdentity = SecurityStampValidator.OnValidateIdentity(Of ApplicationUserManager, ApplicationUser)(
                    validateInterval:=TimeSpan.FromMinutes(30),
                    regenerateIdentity:=Function(manager, user) user.GenerateUserIdentityAsync(manager))},
            .LoginPath = New PathString("/Account/Login")})

        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie)

        ' 次の行のコメントを解除して、サード パーティのログイン プロバイダーを使用したログインを有効にします
        'app.UseMicrosoftAccountAuthentication(
        '    clientId:="",
        '    clientSecret:="")

        app.UseTwitterAuthentication(
            New TwitterAuthenticationOptions With {
                .ConsumerKey = ConfigurationManager.AppSettings("TwitterConsumerKey"),
                .ConsumerSecret = ConfigurationManager.AppSettings("TwitterConsumerSecret"),
                .Provider = New TwitterAuthenticationProvider With {
                    .OnAuthenticated = Async Function(context)
                                           context.Identity.AddClaim(New Claim("urn:tokens:twitter:accesstoken", context.AccessToken))
                                           context.Identity.AddClaim(New Claim("urn:tokens:twitter:accesstokensecret", context.AccessTokenSecret))
                                           ' context.Identity.AddClaim(New Claim("urn:twitter:screenname", context.ScreenName))
                                       End Function
                    }
                })

        'app.UseFacebookAuthentication(
        '   appId:=ConfigurationManager.AppSettings("FacebookAppId"),
        '   appSecret:=ConfigurationManager.AppSettings("FacebookAppSecret"))


        app.UseFacebookAuthentication(
            New FacebookAuthenticationOptions With {
                .AppId = ConfigurationManager.AppSettings("FacebookAppId"),
                .AppSecret = ConfigurationManager.AppSettings("FacebookAppSecret"),
                .Provider = New FacebookAuthenticationProvider With {
                    .OnAuthenticated = Async Function(context)
                                           context.Identity.AddClaim(New Claim("urn:facebook:access_token", context.AccessToken))
                                           'context.Identity.AddClaim(New Claim("urn:facebook:name", context.Name))
                                           'context.Identity.AddClaim(New Claim("urn:facebook:id", context.Id))
                                           '' Get all claims
                                           'For Each c In context.User
                                           '    Dim claimType = String.Format("urn:facebook:{0}", c.Key)
                                           '    Dim claimValue = c.Value.ToString
                                           '    If Not context.Identity.HasClaim(claimType, claimValue) Then
                                           '        context.Identity.AddClaim(New Claim(claimType, claimValue, "XmlSchemaString", "Facebook"))
                                           '    End If
                                           'Next
                                       End Function
                    }
                })

        app.UseGoogleAuthentication(New GoogleOAuth2AuthenticationOptions() With {
           .ClientId = ConfigurationManager.AppSettings("GoogleClientId"),
           .ClientSecret = ConfigurationManager.AppSettings("GoogleClientSecret")})
    End Sub
End Class

