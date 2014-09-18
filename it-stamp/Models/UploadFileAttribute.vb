Imports System.ComponentModel.DataAnnotations
Imports System.IO

<AttributeUsage(AttributeTargets.[Property], AllowMultiple:=False)>
Public NotInheritable Class UploadFileAttribute
    Inherits ValidationAttribute

    Public Sub New()
        MaxLength = Integer.MaxValue

        ' デフォルトのエラーメッセージを設定する
        ErrorMessage = "ファイル形式が不正です。"
    End Sub

    Public Property MaxLength As Integer

    Public Property Extensions As String

    Public Overrides Function IsValid(value As Object) As Boolean
        If value Is Nothing Then
            Return False
        End If

        Dim postedFile = TryCast(value, HttpPostedFileBase)
        If postedFile Is Nothing Then
            Return True
        End If

        If postedFile.ContentLength > MaxLength Then
            Return False
        End If

        Dim ext = Path.GetExtension(postedFile.FileName).Replace(".", "")
        If Not String.IsNullOrEmpty(Extensions) AndAlso Not Extensions.Split(";"c).Any(Function(p) p = ext) Then
            Return False
        End If

        Return True
    End Function
End Class

