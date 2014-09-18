Imports System.ComponentModel.DataAnnotations

Public Class ExternalLoginConfirmationViewModel
    <Required>
    <RegularExpression("^[A-Za-z0-9_]+$", ErrorMessage:="英数字のみ使えます。")>
    <Remote("IsUserNameAvailable", "Account", ErrorMessage:="既に使われています。")>
    <Display(Name:="ユーザー名")>
    Public Property UserName As String

End Class

Public Class ExternalLoginListViewModel
    Public Property Action As String
    Public Property ReturnUrl As String
End Class

Public Class ManageUserViewModel
    <Required>
    <EmailAddress(ErrorMessage:="正しいメールアドレスの形式で入力してください。")>
    <Display(Name:="メールアドレス")>
    Public Property Email As String

End Class

Public Class ChangePasswordViewModel
    <Required>
    <DataType(DataType.Password)>
    <Display(Name:="現在のパスワード")>
    Public Property OldPassword As String

    <Required>
    <StringLength(100, ErrorMessage:="{2}文字以上で入力してください。", MinimumLength:=8)>
    <DataType(DataType.Password)>
    <Display(Name:="新しいパスワード")>
    Public Property NewPassword As String

    <DataType(DataType.Password)>
    <Display(Name:="新しいパスワードの確認")>
    <Compare("NewPassword", ErrorMessage:="パスワードと一致していません。")>
    Public Property ConfirmPassword As String
End Class

Public Class LoginViewModel
    <Required>
    <Display(Name:="ユーザー名")>
    Public Property EmailOrUserName As String

    <Required>
    <DataType(DataType.Password)>
    <Display(Name:="パスワード")>
    Public Property Password As String

    <Display(Name:="このアカウントを記憶する")>
    Public Property RememberMe As Boolean
End Class

Public Class RegisterViewModel
    <Required>
    <Display(Name:="ユーザー名")>
    <RegularExpression("^[A-Za-z0-9_]+$", ErrorMessage:="英数字のみ使えます。")>
    <Remote("IsUserNameAvailable", "Account", ErrorMessage:="既に使われています。")>
    Public Property UserName As String

    <Required>
    <EmailAddress(ErrorMessage:="正しいメールアドレスの形式で入力してください。")>
    <Display(Name:="メールアドレス")>
    Public Property Email As String

    <Required>
    <StringLength(100, ErrorMessage:="{2}文字以上で入力してください。", MinimumLength:=8)>
    <DataType(DataType.Password)>
    <Display(Name:="パスワード")>
    Public Property Password As String

    <DataType(DataType.Password)>
    <Display(Name:="パスワードの確認")>
    <Compare("Password", ErrorMessage:="パスワードと一致していません。")>
    Public Property ConfirmPassword As String

End Class

Public Class ResetPasswordViewModel
    <Required>
    <EmailAddress(ErrorMessage:="正しいメールアドレスの形式で入力してください。")>
    <Display(Name:="メールアドレス")>
    Public Property Email() As String

    <Required>
    <StringLength(100, ErrorMessage:="{2}文字以上で入力してください。", MinimumLength:=8)>
    <DataType(DataType.Password)>
    <Display(Name:="パスワード")>
    Public Property Password() As String

    <DataType(DataType.Password)>
    <Display(Name:="パスワードの確認")>
    <Compare("Password", ErrorMessage:="パスワードと一致していません。")>
    Public Property ConfirmPassword() As String

    Public Property Code() As String
End Class

Public Class ForgotPasswordViewModel
    <Required>
    <EmailAddress(ErrorMessage:="正しいメールアドレスの形式で入力してください。")>
    <Display(Name:="メールアドレス")>
    Public Property Email() As String
End Class

