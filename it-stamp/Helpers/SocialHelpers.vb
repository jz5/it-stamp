Imports System.Web.Mvc
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin

Public Class SocialHelpers

    Shared Async Function Tweet(claims As IEnumerable(Of Claim), status As String) As Task
        Dim accessTokenClaim = claims.FirstOrDefault(Function(x) x.Type = "urn:tokens:twitter:accesstoken")
        Dim accessTokenSecretClaim = claims.FirstOrDefault(Function(x) x.Type = "urn:tokens:twitter:accesstokensecret")

        If accessTokenClaim IsNot Nothing AndAlso accessTokenSecretClaim IsNot Nothing Then

            Dim tokens = CoreTweet.Tokens.Create(ConfigurationManager.AppSettings("TwitterConsumerKey"), ConfigurationManager.AppSettings("TwitterConsumerSecret"), accessTokenClaim.Value, accessTokenSecretClaim.Value)
            Await tokens.Statuses.UpdateAsync(New Dictionary(Of String, Object) From {{"status", status}})

        End If
    End Function

    Shared Async Function GetProfileImageUrl(claims As IEnumerable(Of Claim), screenName As String) As Task(Of Uri)
        Dim accessTokenClaim = claims.FirstOrDefault(Function(x) x.Type = "urn:tokens:twitter:accesstoken")
        Dim accessTokenSecretClaim = claims.FirstOrDefault(Function(x) x.Type = "urn:tokens:twitter:accesstokensecret")

        If accessTokenClaim IsNot Nothing AndAlso accessTokenSecretClaim IsNot Nothing Then

            Dim tokens = CoreTweet.Tokens.Create(ConfigurationManager.AppSettings("TwitterConsumerKey"), ConfigurationManager.AppSettings("TwitterConsumerSecret"), accessTokenClaim.Value, accessTokenSecretClaim.Value)
            Dim result = Await tokens.Users.ShowAsync(New Dictionary(Of String, Object) From {{"screen_name", screenName}})

            Dim url = result.ProfileImageUrlHttps.ToString
            Return New Uri(url.Replace("_normal.", "."))
        End If

        Return Nothing
    End Function

End Class
