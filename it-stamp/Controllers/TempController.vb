Imports System.Web.Mvc
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin


<Authorize>
Public Class TempController
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

    ' GET: Temp
    Async Function Index() As Task(Of ActionResult)
        Dim atnd = New Atnd
        Dim evs = Await atnd.GetEvents(AddressHelper.GetPrefecture("東京都"), Now)

        ViewBag.Events = evs

        Return View()
    End Function

    '
    ' POST: /Temp/
    <HttpPost>
    <ValidateAntiForgeryToken>
    Public Async Function Index(model As TempViewModel) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View()
        End If

        'Dim userId = User.Identity.GetUserId
        'Await DownloadTwitterProfileImage(Await UserManager.GetClaimsAsync(userId), userId)


        Return View()
    End Function


    Private Async Function DownloadTwitterProfileImage(claims As IEnumerable(Of Claim), userId As String) As Task
        ' Retrieve the twitter access token and claim
        Dim accessTokenClaim = claims.FirstOrDefault(Function(x) x.Type = "urn:tokens:twitter:accesstoken")
        Dim accessTokenSecretClaim = claims.FirstOrDefault(Function(x) x.Type = "urn:tokens:twitter:accesstokensecret")

        If accessTokenClaim IsNot Nothing AndAlso accessTokenSecretClaim IsNot Nothing Then

            Dim tokens = CoreTweet.Tokens.Create(ConfigurationManager.AppSettings("TwitterConsumerKey"), ConfigurationManager.AppSettings("TwitterConsumerSecret"), accessTokenClaim.Value, accessTokenSecretClaim.Value)
            'Await tokens.Statuses.UpdateAsync(Function(status As String) "Hello")

            Await tokens.Statuses.UpdateAsync(New Dictionary(Of String, Object) From {{"status", "昨日のプロ生の PV は 1621 でした。"}})

            '' Initialize the Twitter client
            'Dim service = New TwitterService("your twitter consumer key", "your twitter consumer secret", accessTokenClaim.Value, accessTokenSecretClaim.Value)

            'Dim profile = service.GetUserProfile(New GetUserProfileOptions())
            'If profile IsNot Nothing AndAlso Not [String].IsNullOrWhiteSpace(profile.ProfileImageUrlHttps) Then
            '    Dim filename As String = Server.MapPath(String.Format("~/ProfileImages/{0}.jpeg", userId))

            '    Await DownloadProfileImage(New Uri(profile.ProfileImageUrlHttps), filename)
            'End If
        End If
    End Function

End Class
