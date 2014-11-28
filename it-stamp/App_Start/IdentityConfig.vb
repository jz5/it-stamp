Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports System.Data.Entity
Imports System.Net

' このアプリケーションで使用されるアプリケーション ユーザー マネージャーを設定します。UserManager は ASP.NET Identity の中で定義されており、このアプリケーションで使用されます。
Public Class ApplicationUserManager
    Inherits UserManager(Of ApplicationUser)

    Public Sub New(store As IUserStore(Of ApplicationUser))
        MyBase.New(store)
    End Sub

    Public Shared Function Create(options As IdentityFactoryOptions(Of ApplicationUserManager), context As IOwinContext) As ApplicationUserManager
        Dim manager = New ApplicationUserManager(New UserStore(Of ApplicationUser)(context.Get(Of ApplicationDbContext)()))

        ' ユーザー名の検証ロジックを設定します
        manager.UserValidator = New UserValidator(Of ApplicationUser)(manager) With {
            .AllowOnlyAlphanumericUserNames = True,
            .RequireUniqueEmail = False
        }

        ' パスワードの検証ロジックを設定します
        manager.PasswordValidator = New PasswordValidator With {
            .RequiredLength = 8,
            .RequireNonLetterOrDigit = False,
            .RequireDigit = False,
            .RequireLowercase = False,
            .RequireUppercase = False
        }

        ' アカウントロック
        manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(30)
        manager.MaxFailedAccessAttemptsBeforeLockout = 5

        ' 2 要素認証プロバイダーを登録します。このアプリケーションでは、Phone and Emails をユーザー検証用コード受け取りのステップとして使用します。
        ' 独自のプロバイダーを作成して、ここにプラグインすることができます。
        manager.RegisterTwoFactorProvider("PhoneCode", New PhoneNumberTokenProvider(Of ApplicationUser) With {
                                          .MessageFormat = "Your security code is: {0}"
                                      })
        manager.RegisterTwoFactorProvider("EmailCode", New EmailTokenProvider(Of ApplicationUser) With {
                                          .Subject = "セキュリティ コード",
                                          .BodyFormat = "Your security code is: {0}"
                                          })
        manager.EmailService = New EmailService()
        manager.SmsService = New SmsService()
        Dim dataProtectionProvider = options.DataProtectionProvider
        If (dataProtectionProvider IsNot Nothing) Then
            manager.UserTokenProvider = New DataProtectorTokenProvider(Of ApplicationUser)(dataProtectionProvider.Create("ASP.NET Identity"))
        End If

        Return manager
    End Function

End Class


' Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
Public Class ApplicationRoleManager
    Inherits RoleManager(Of IdentityRole)
    Public Sub New(roleStore As IRoleStore(Of IdentityRole, String))
        MyBase.New(roleStore)
    End Sub

    Public Shared Function Create(options As IdentityFactoryOptions(Of ApplicationRoleManager), context As IOwinContext) As ApplicationRoleManager
        Return New ApplicationRoleManager(New RoleStore(Of IdentityRole)(context.Get(Of ApplicationDbContext)))
    End Function
End Class

Public Class EmailService
    Implements IIdentityMessageService

    Public Function SendAsync(message As IdentityMessage) As Task Implements IIdentityMessageService.SendAsync
        ' 電子メールを送信するには、電子メール サービスをここにプラグインします。
        Return ConfigSendGridasync(message)
    End Function

    Private Function ConfigSendGridasync(message As IdentityMessage) As Task
        Dim myMessage = New SendGrid.SendGridMessage()
        myMessage.AddTo(message.Destination)
        myMessage.From = New System.Net.Mail.MailAddress("no-reply@it-stamp.jp", "IT勉強会スタンプ")
        myMessage.Subject = "【IT勉強会スタンプ】" & message.Subject
        Dim footer = "<br><br>IT勉強会スタンプ http://itstamp.azurewebsites.net/"
        myMessage.Text = message.Body & footer
        myMessage.Html = message.Body & footer

        Dim credentials = New NetworkCredential(ConfigurationManager.AppSettings("MailAccount"), ConfigurationManager.AppSettings("MailPassword"))

        ' Create a Web transport for sending email.
        Dim transportWeb = New SendGrid.Web(credentials)

        ' Send the email.
        If transportWeb IsNot Nothing Then
            Return transportWeb.DeliverAsync(myMessage)
        Else
            Return Task.FromResult(0)
        End If
    End Function
End Class

Public Class SmsService
    Implements IIdentityMessageService

    Public Function SendAsync(message As IdentityMessage) As Task Implements IIdentityMessageService.SendAsync
        ' テキスト メッセージを送信するには、SMS サービスをここにプラグインします。
        Return Task.FromResult(0)
    End Function
End Class
