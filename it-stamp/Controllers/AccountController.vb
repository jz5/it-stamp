Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin

<RequireHttps>
<Authorize>
Public Class AccountController
    Inherits Controller

    Private _userManager As ApplicationUserManager

    Public Sub New()
    End Sub

    Public Sub New(manager As ApplicationUserManager)
        UserManager = manager
    End Sub

    Public Property UserManager() As ApplicationUserManager
        Get
            Return If(_userManager, HttpContext.GetOwinContext().GetUserManager(Of ApplicationUserManager)())
        End Get
        Private Set(value As ApplicationUserManager)
            _userManager = value
        End Set
    End Property

    '
    ' GET: /Account/Login
    <AllowAnonymous>
    Public Function Login(returnUrl As String) As ActionResult
        If Request.IsAuthenticated Then
            Return RedirectToAction("Index", "Home")
        End If
        ViewBag.ReturnUrl = returnUrl
        Return View()
    End Function

    '
    ' POST: /Account/Login
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function Login(model As LoginViewModel, returnUrl As String) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If

        ' パスワードの検証
        Dim user = Await UserManager.FindByNameOrEmailAsync(model.EmailOrUserName, model.Password)
        If user IsNot Nothing Then
            ' アカウントロック
            If Await UserManager.IsLockedOutAsync(user.Id) Then
                ModelState.AddModelError("", "アカウントをロックしました。しばらくしてからアクセスしてください。")
                Return View(model)
            End If

            ' パスワードが正しい場合はリトライをクリア
            Await UserManager.ResetAccessFailedCountAsync(user.Id)

            Await SignInAsync(user, model.RememberMe)
            Return RedirectToLocal(returnUrl)
        Else
            user = Await UserManager.FindByNameOrEmailAsync(model.EmailOrUserName) ' Email/UserName のみで取得しなおし
            If user IsNot Nothing Then
                Await UserManager.SetLockoutEnabledAsync(user.Id, True)
                Await UserManager.AccessFailedAsync(user.Id)

                ' アカウントロック
                If Await UserManager.IsLockedOutAsync(user.Id) Then
                    ModelState.AddModelError("", "アカウントをロックしました。しばらくしてからアクセスしてください。")
                    Return View(model)
                End If
            End If
            ModelState.AddModelError("", "ユーザー名またはパスワードが無効です。")
        End If

        ' ここで問題が発生した場合はフォームを再表示します
        Return View(model)
    End Function

    '
    ' GET: /Account/Register
    <AllowAnonymous>
    Public Function Register() As ActionResult
        If Request.IsAuthenticated Then
            Return RedirectToAction("Index", "Home")
        End If
        Return View()
    End Function

    '
    ' POST: /Account/Register
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function Register(model As RegisterViewModel) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If

        ' 登録済みメールアドレスかチェック
        If Await UserManager.FindByEmailAsync(model.Email) IsNot Nothing Then
            ModelState.AddModelError("", "登録済みのメールアドレスです。")
            Return View(model)
        End If

        ' ユーザーにサインインする前にローカル ログインを作成します。
        Dim user = New ApplicationUser() With {.UserName = model.UserName, .Email = model.Email, .DisplayName = model.UserName, .IsPrivate = True}
        Dim result = Await UserManager.CreateAsync(user, model.Password)
        If result.Succeeded Then
            ' アカウント確認とパスワード リセットを有効にする方法の詳細については、http://go.microsoft.com/fwlink/?LinkID=320771 を参照してください
            ' このリンクを含む電子メールを送信します
            Dim code = Await UserManager.GenerateEmailConfirmationTokenAsync(user.Id)
            Dim callbackUrl = Url.Action("ConfirmEmail", "Account", New With {.code = code, .userId = user.Id}, protocol:=Request.Url.Scheme)
            Await UserManager.SendEmailAsync(user.Id, "アカウントの確認", "このリンクをクリックすることによってアカウントを確認してください <a href=""" & callbackUrl & """>こちら</a>")
            ViewBag.Link = callbackUrl
            Return View("DisplayEmail")
        Else
            AddErrors(result)
        End If

        ' ここで問題が発生した場合はフォームを再表示します
        Return View(model)
    End Function

    '
    ' GET: /Account/ConfirmEmail
    <AllowAnonymous>
    Public Async Function ConfirmEmail(userId As String, code As String) As Task(Of ActionResult)
        If userId Is Nothing OrElse code Is Nothing Then
            Return View("Error")
        End If

        Dim result = Await UserManager.ConfirmEmailAsync(userId, code)
        If result.Succeeded Then
            Return View("ConfirmEmail")
        Else
            AddErrors(result)
            Return View()
        End If
    End Function

    '
    ' GET: /Account/ForgotPassword
    <AllowAnonymous>
    Public Function ForgotPassword() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/ForgotPassword
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function ForgotPassword(model As ForgotPasswordViewModel) As Task(Of ActionResult)
        If ModelState.IsValid Then
            Dim user = Await UserManager.FindByEmailAsync(model.Email)
            If user Is Nothing OrElse Not (Await UserManager.IsEmailConfirmedAsync(user.Id)) Then
                ModelState.AddModelError("", "ユーザーが存在しないか、メールアドレスが認証されていません。")
                Return View()
            End If

            ' アカウント確認とパスワード リセットを有効にする方法の詳細については、http://go.microsoft.com/fwlink/?LinkID=320771 を参照してください
            ' このリンクを含む電子メールを送信します
            Dim code As String = Await UserManager.GeneratePasswordResetTokenAsync(user.Id)
            Dim callbackUrl = Url.Action("ResetPassword", "Account", New With {.code = code, .userId = user.Id}, protocol:=Request.Url.Scheme)
            Await UserManager.SendEmailAsync(user.Id, "パスワードのリセット", "パスワードをリセットするには、<a href=""" & callbackUrl & """>ここ</a>をクリックしてください。")
            ViewBag.Link = callbackUrl
            Return View("ForgotPasswordConfirmation")
        End If

        ' ここで問題が発生した場合はフォームを再表示します
        Return View(model)
    End Function

    '
    ' GET: /Account/ForgotPasswordConfirmation
    <AllowAnonymous>
    Public Function ForgotPasswordConfirmation() As ActionResult
        Return View()
    End Function

    '
    ' GET: /Account/ResetPassword
    <AllowAnonymous>
    Public Function ResetPassword(code As String) As ActionResult
        If code Is Nothing Then
            Return View("Error")
        End If
        Return View()
    End Function

    '
    ' POST: /Account/ResetPassword
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function ResetPassword(model As ResetPasswordViewModel) As Task(Of ActionResult)
        If ModelState.IsValid Then
            Dim user = Await UserManager.FindByEmailAsync(model.Email)
            If user Is Nothing Then
                ModelState.AddModelError("", "ユーザーが見つかりません。")
                Return View()
            End If
            Dim result = Await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password)
            If result.Succeeded Then
                Return RedirectToAction("ResetPasswordConfirmation", "Account")
            Else
                AddErrors(result)
                Return View()
            End If
        End If

        ' ここで問題が発生した場合はフォームを再表示します
        Return View(model)
    End Function

    '
    ' GET: /Account/ResetPasswordConfirmation
    <AllowAnonymous>
    Public Function ResetPasswordConfirmation() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/Disassociate
    <HttpPost>
    <ValidateAntiForgeryToken>
    Public Async Function Disassociate(loginProvider As String, providerKey As String) As Task(Of ActionResult)
        Dim message As ManageMessageId? = Nothing
        Dim result As IdentityResult = Await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), New UserLoginInfo(loginProvider, providerKey))
        If result.Succeeded Then
            Dim userInfo = Await UserManager.FindByIdAsync(User.Identity.GetUserId())

            Select Case loginProvider
                Case "Twitter"
                    userInfo.Twitter = Nothing
                    userInfo.ShareTwitter = False
                    Await UserManager.UpdateAsync(userInfo)
                Case "Facebook"
                    userInfo.Facebook = Nothing
                    userInfo.ShareFacebook = False
                    Await UserManager.UpdateAsync(userInfo)
            End Select

            Await SignInAsync(userInfo, isPersistent:=False)
            message = ManageMessageId.RemoveLoginSuccess
        Else
            message = ManageMessageId.Error
        End If

        Return RedirectToAction("Manage", New With {
            .Message = message
        })
    End Function

    '
    ' GET: /Account/Manage
    Public Function Manage(ByVal message As ManageMessageId?) As ActionResult
        Dim appUser = UserManager.FindById(User.Identity.GetUserId)
        If appUser Is Nothing Then
            Return View()
        End If

        Dim msg As String
        Select Case message
            Case ManageMessageId.ChangePasswordSuccess
                msg = "パスワードを変更しました。"
            Case ManageMessageId.SetPasswordSuccess
                msg = "パスワードを設定しました。"
            Case ManageMessageId.RemoveLoginSuccess
                msg = "別サービスの関連付けを削除しました。"
            Case ManageMessageId.Error
                msg = "エラーが発生しました。"
            Case Else
                msg = ""
        End Select
        ViewBag.StatusMessage = msg

        ViewBag.HasLocalPassword = appUser.PasswordHash IsNot Nothing
        ViewBag.HasEmail = appUser.Email IsNot Nothing
        ViewBag.EmailConfirmed = appUser.EmailConfirmed
        ViewBag.ReturnUrl = Url.Action("Manage")

        Return View("Manage", New ManageUserViewModel() With {.Email = appUser.Email})
    End Function

    '
    ' POST: /Account/Manage
    <HttpPost>
    <ValidateAntiForgeryToken>
    Public Async Function Manage(model As ManageUserViewModel) As Task(Of ActionResult)
        Dim appUser = UserManager.FindById(User.Identity.GetUserId)
        If appUser Is Nothing Then
            Return View()
        End If

        ViewBag.HasLocalPassword = appUser.PasswordHash IsNot Nothing
        ViewBag.HasEmail = appUser.Email IsNot Nothing
        ViewBag.EmailConfirmed = appUser.EmailConfirmed
        ViewBag.ReturnUrl = Url.Action("Manage")

        If Not ModelState.IsValid Then
            Return View(model)
        End If

        appUser.Email = model.Email
        appUser.EmailConfirmed = False

        Dim result = Await UserManager.UpdateAsync(appUser)
        If result.Succeeded Then
            Dim code = Await UserManager.GenerateEmailConfirmationTokenAsync(appUser.Id)
            Dim callbackUrl = Url.Action("ConfirmEmail", "Account", New With {.code = code, .userId = appUser.Id}, protocol:=Request.Url.Scheme)
            Await UserManager.SendEmailAsync(appUser.Id, "アカウントの確認", "このリンクをクリックすることによってアカウントを確認してください <a href=""" & callbackUrl & """>こちら</a>")
            ViewBag.Link = callbackUrl
            Return View("DisplayEmail")
        Else
            AddErrors(result)
        End If

        ' ここで問題が発生した場合はフォームを再表示します
        Return View(model)
    End Function

    '
    ' GET: /Account/ChangePassword
    Public Function ChangePassword() As ActionResult
        ViewBag.HasLocalPassword = HasPassword()
        Return View()
    End Function

    '
    ' POST: /Account/ChangePassword
    <HttpPost>
    <ValidateAntiForgeryToken>
    Public Async Function ChangePassword(model As ChangePasswordViewModel) As Task(Of ActionResult)
        If HasPassword() Then
            If ModelState.IsValid Then
                Dim result As IdentityResult = Await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword)
                If result.Succeeded Then
                    Dim userInfo = Await UserManager.FindByIdAsync(User.Identity.GetUserId())
                    Await SignInAsync(userInfo, isPersistent:=False)
                    Return RedirectToAction("Manage", New With {
                        .Message = ManageMessageId.ChangePasswordSuccess
                    })
                Else
                    AddErrors(result)
                End If
            End If
        Else
            ' ユーザーにはローカル パスワードがないので、OldPassword フィールドに値がないために発生した検証エラーを削除します
            Dim state As ModelState = ModelState("OldPassword")
            If state IsNot Nothing Then
                state.Errors.Clear()
            End If

            If ModelState.IsValid Then
                Dim result As IdentityResult = Await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword)
                If result.Succeeded Then
                    Return RedirectToAction("Manage", New With {
                        .Message = ManageMessageId.SetPasswordSuccess
                    })
                Else
                    AddErrors(result)
                End If
            End If
        End If

        ' ここで問題が発生した場合はフォームを再表示します
        Return View(model)
    End Function

    '
    ' POST: /Account/ExternalLogin
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Function ExternalLogin(provider As String, returnUrl As String) As ActionResult
        ' 外部ログイン プロバイダーへのリダイレクトを要求します
        Return New ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", New With {.ReturnUrl = returnUrl}))
    End Function

    '
    ' GET: /Account/ExternalLoginCallback
    <AllowAnonymous>
    Public Async Function ExternalLoginCallback(returnUrl As String) As Task(Of ActionResult)
        Dim loginInfo = Await AuthenticationManager.GetExternalLoginInfoAsync()
        If loginInfo Is Nothing Then
            Return RedirectToAction("Login")
        End If

        ' ユーザーが既にログインを持っている場合、この外部ログイン プロバイダーを使用してユーザーをサインインします
        Dim user = Await UserManager.FindAsync(loginInfo.Login)
        If user IsNot Nothing Then

            Await StoreAuthTokenClaims(user)

            Await SignInAsync(user, isPersistent:=False)
            Return RedirectToLocal(returnUrl)
        Else
            ' ユーザーがアカウントを持っていない場合、ユーザーにアカウントを作成するよう求めます
            ViewBag.ReturnUrl = returnUrl
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider
            Return View("ExternalLoginConfirmation", New ExternalLoginConfirmationViewModel() With {.UserName = loginInfo.DefaultUserName})
        End If
        Return View("ExternalLoginFailure")
    End Function

    '
    ' POST: /Account/LinkLogin
    <HttpPost>
    <ValidateAntiForgeryToken>
    Public Function LinkLogin(provider As String) As ActionResult
        ' 現在のユーザーのログインをリンクするために、外部ログイン プロバイダーへのリダイレクトを要求します
        Return New ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId())
    End Function

    '
    ' GET: /Account/LinkLoginCallback
    Public Async Function LinkLoginCallback() As Task(Of ActionResult)
        Dim loginInfo = Await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId())
        If loginInfo Is Nothing Then
            Return RedirectToAction("Manage", New With {
                .Message = ManageMessageId.Error
            })
        End If
        Dim result = Await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login)
        If result.Succeeded Then
            Return RedirectToAction("Manage")
        End If
        Return RedirectToAction("Manage", New With {
            .Message = ManageMessageId.Error
        })
    End Function

    '
    ' POST: /Account/ExternalLoginConfirmation
    <HttpPost>
    <AllowAnonymous>
    <ValidateAntiForgeryToken>
    Public Async Function ExternalLoginConfirmation(model As ExternalLoginConfirmationViewModel, returnUrl As String) As Task(Of ActionResult)
        If User.Identity.IsAuthenticated Then
            Return RedirectToAction("Manage")
        End If

        If ModelState.IsValid Then
            ' 外部ログイン プロバイダーからユーザーに関する情報を取得します
            Dim info = Await AuthenticationManager.GetExternalLoginInfoAsync()
            If info Is Nothing Then
                Return View("ExternalLoginFailure")
            End If
            Dim user = New ApplicationUser() With {.UserName = model.UserName, .DisplayName = model.UserName, .IsPrivate = True}
            Dim result = Await UserManager.CreateAsync(user)
            If result.Succeeded Then
                result = Await UserManager.AddLoginAsync(user.Id, info.Login)
                If result.Succeeded Then
                    Await StoreAuthTokenClaims(user)
                    Await SignInAsync(user, isPersistent:=False)

                    ' アカウント確認とパスワード リセットを有効にする方法の詳細については、http://go.microsoft.com/fwlink/?LinkID=320771 を参照してください
                    ' このリンクを含む電子メールを送信します
                    ' Dim code = Await UserManager.GenerateEmailConfirmationTokenAsync(user.Id)
                    ' Dim callbackUrl = Url.Action("ConfirmEmail", "Account", New With { .code = code, .userId = user.Id }, protocol := Request.Url.Scheme)
                    ' SendEmail(user.Email, callbackUrl, "アカウントの確認", "このリンクをクリックすることによってアカウントを確認してください")

                    Return RedirectToLocal(returnUrl)
                End If
            End If
            AddErrors(result)
        End If

        ViewBag.ReturnUrl = returnUrl
        Return View(model)
    End Function

    Private Async Function StoreAuthTokenClaims(user As ApplicationUser) As Task
        ' Get the claims identity
        Dim claimsIdentity As ClaimsIdentity = Await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie)

        If claimsIdentity IsNot Nothing Then
            ' Retrieve the existing claims
            Dim currentClaims = Await UserManager.GetClaimsAsync(user.Id)

            ' Get the list of access token related claims from the identity
            Dim tokenClaims = claimsIdentity.Claims.Where(Function(c) c.Type.StartsWith("urn:"))

            ' Save the access token related claims
            For Each tokenClaim In tokenClaims
                Dim oldClaim = currentClaims.Where(Function(c) c.Type = tokenClaim.Type).FirstOrDefault
                If oldClaim IsNot Nothing AndAlso oldClaim.Value <> tokenClaim.Value Then
                    ' Replace
                    Await UserManager.RemoveClaimAsync(user.Id, oldClaim)
                    Await UserManager.AddClaimAsync(user.Id, tokenClaim)
                ElseIf oldClaim Is Nothing Then
                    ' Add
                    Await UserManager.AddClaimAsync(user.Id, tokenClaim)
                End If
            Next

            Await UpdateTwitterScreenName(user, tokenClaims)
            'Await UpdateFacebookId(user, tokenClaims)
        End If
    End Function

    Private Async Function UpdateTwitterScreenName(user As ApplicationUser, claims As IEnumerable(Of Claim)) As Task

        ' Update ScreenName
        Dim claim = claims.Where(Function(x) x.Type = "urn:twitter:screenname").SingleOrDefault
        If claim IsNot Nothing AndAlso user.Twitter <> claim.Value Then
            user.Twitter = claim.Value
            Await UserManager.UpdateAsync(user)
        End If

    End Function

    Private Async Function UpdateFacebookId(user As ApplicationUser, claims As IEnumerable(Of Claim)) As Task

        Dim claim = claims.Where(Function(x) x.Type = "urn:facebook:access_token").SingleOrDefault

        If claim Is Nothing Then
            Exit Function
        End If

        Dim fb = New Facebook.FacebookClient(claim.Value)
        Dim post = New With {
            .name = "Facebook SDK for .NET",
            .caption = "Build great social apps and get more installs.",
            .description = "The Facebook SDK for .NET makes it easier and faster to develop Facebook integrated .NET apps.",
            .link = "http://facebooksdk.net/",
            .picture = "http://facebooksdk.net/assets/img/logo75x75.png"
        }

        Dim fbPostTaskResult = Await fb.PostTaskAsync("/me/feed", post)
        Dim reslut = DirectCast(fbPostTaskResult, IDictionary(Of String, Object))


    End Function

    '
    ' POST: /Account/LogOff
    <HttpPost>
    <ValidateAntiForgeryToken>
    Public Function LogOff() As ActionResult
        AuthenticationManager.SignOut()
        Session.RemoveAll()
        Return RedirectToAction("Index", "Home")
    End Function

    '
    ' GET: /Account/ExternalLoginFailure
    <AllowAnonymous>
    Public Function ExternalLoginFailure() As ActionResult
        Return View()
    End Function

    <ChildActionOnly>
    Public Function RemoveAccountList() As ActionResult
        Dim linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId())
        ViewBag.ShowRemoveButton = linkedAccounts.Count > 1 Or HasPassword()
        Return DirectCast(PartialView("_RemoveAccountPartial", linkedAccounts), ActionResult)
    End Function

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso UserManager IsNot Nothing Then
            UserManager.Dispose()
            UserManager = Nothing
        End If
        MyBase.Dispose(disposing)
    End Sub

    <AllowAnonymous>
    Public Async Function IsUserNameAvailable(userName As String) As Task(Of ActionResult)
        Dim user = Await UserManager.FindByNameAsync(userName)
        Return Json(user Is Nothing, JsonRequestBehavior.AllowGet)
    End Function

#Region "ヘルパー"
    ' 外部ログインの追加時に XSRF の防止に使用します
    Private Const XsrfKey As String = "XsrfId"

    Private Function AuthenticationManager() As IAuthenticationManager
        Return HttpContext.GetOwinContext().Authentication
    End Function

    Private Async Function SignInAsync(user As ApplicationUser, isPersistent As Boolean) As Task
        AuthenticationManager.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie)
        AuthenticationManager.SignIn(New AuthenticationProperties() With {.IsPersistent = isPersistent}, Await user.GenerateUserIdentityAsync(UserManager))


        Session("DisplayName") = user.DisplayName
        Session("IconPath") = user.IconPath
    End Function

    Private Sub AddErrors(result As IdentityResult)
        For Each [error] As String In result.Errors
            ModelState.AddModelError("", [error])
        Next
    End Sub

    Private Function HasPassword() As Boolean
        Dim appUser = UserManager.FindById(User.Identity.GetUserId())
        If (appUser IsNot Nothing) Then
            Return appUser.PasswordHash IsNot Nothing
        End If
        Return False
    End Function

    Private Sub SendEmail(email As String, callbackUrl As String, subject As String, message As String)
        ' 電子メールの送信については、http://go.microsoft.com/fwlink/?LinkID=320771 を参照してください
    End Sub

    Private Function RedirectToLocal(returnUrl As String) As ActionResult
        If Url.IsLocalUrl(returnUrl) Then
            Return Redirect(returnUrl)
        Else
            Return RedirectToAction("Index", "Home")
        End If
    End Function

    Public Enum ManageMessageId
        ChangePasswordSuccess
        SetPasswordSuccess
        RemoveLoginSuccess
        [Error]
    End Enum

    Private Class ChallengeResult
        Inherits HttpUnauthorizedResult
        Public Sub New(provider As String, redirectUri As String)
            Me.New(provider, redirectUri, Nothing)
        End Sub
        Public Sub New(provider As String, redirectUri As String, userId As String)
            Me.LoginProvider = provider
            Me.RedirectUri = redirectUri
            Me.UserId = userId
        End Sub

        Public Property LoginProvider As String
        Public Property RedirectUri As String
        Public Property UserId As String

        Public Overrides Sub ExecuteResult(context As ControllerContext)
            Dim properties = New AuthenticationProperties() With {.RedirectUri = RedirectUri}
            If UserId IsNot Nothing Then
                properties.Dictionary(XsrfKey) = UserId
            End If
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider)
        End Sub
    End Class
#End Region

End Class
