Imports System.Runtime.CompilerServices

Module IdentityExtensions

    <Extension>
    Async Function FindByNameOrEmailAsync(userManager As ApplicationUserManager, userNameOrEmail As String, password As String) As Threading.Tasks.Task(Of ApplicationUser)
        Dim userName = userNameOrEmail
        If userNameOrEmail.Contains("@") Then
            Dim user = Await userManager.FindByEmailAsync(userNameOrEmail)
            If user IsNot Nothing AndAlso user.EmailConfirmed Then
                userName = user.UserName
            End If
        End If

        Return Await userManager.FindAsync(userName, password)
    End Function

    <Extension>
    Async Function FindByNameOrEmailAsync(userManager As ApplicationUserManager, userNameOrEmail As String) As Threading.Tasks.Task(Of ApplicationUser)
        Dim userName = userNameOrEmail
        If userNameOrEmail.Contains("@") Then
            Dim user = Await userManager.FindByEmailAsync(userNameOrEmail)
            If user IsNot Nothing AndAlso user.EmailConfirmed Then
                userName = user.UserName
            End If
        End If

        Return Await userManager.FindByNameAsync(userName)
    End Function

    <Extension>
    Sub AddInternalError(modelState As ModelStateDictionary, user As System.Security.Principal.IPrincipal, ex As Exception)
        modelState.AddModelError("", If(user.IsInRole("Admin"), ex.Message, "内部エラー"))
    End Sub

End Module
