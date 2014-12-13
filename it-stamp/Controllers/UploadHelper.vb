Imports System.Drawing

Public Class UploadHelper

    Private File As HttpPostedFileBase

    Sub New(file As HttpPostedFileBase, path As String)
        Me.File = file
        _ImageFormat = GetFormat(file.ContentType)
        _Path = path
    End Sub

    Private _ImageFormat As System.Drawing.Imaging.ImageFormat
    Private _Path As String

    ReadOnly Property IsSupportedImageFormat As Boolean
        Get
            Return Me.File.ContentType.StartsWith("image/") AndAlso _ImageFormat IsNot Nothing
        End Get
    End Property

    Function GetIconPath(group As String, id As String) As String

        ' Save
        Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")
        Dim y = now.Year.ToString
        Dim m = now.Month.ToString
        Dim groupFolder = IO.Path.Combine(_Path, group)
        Dim yearFolder = IO.Path.Combine(groupFolder, y)
        Dim monthFolder = IO.Path.Combine(yearFolder, m)

        If Not IO.Directory.Exists(groupFolder) Then
            IO.Directory.CreateDirectory(groupFolder)
        End If
        If Not IO.Directory.Exists(yearFolder) Then
            IO.Directory.CreateDirectory(yearFolder)
        End If
        If Not IO.Directory.Exists(monthFolder) Then
            IO.Directory.CreateDirectory(monthFolder)
        End If

        Return String.Format("{0}/{1}/{2}/{3}.{4}", group, y, m, id, If(_ImageFormat Is Drawing.Imaging.ImageFormat.Png, "png", "jpeg"))

    End Function

    Public Sub CloneFile(baseFilePath As String, newFilePath As String)
        If baseFilePath <> "" Then
            Dim baseFile = IO.Path.Combine(_Path, baseFilePath)
            If IO.File.Exists(baseFile) Then
                Dim newFile = IO.Path.Combine(_Path, newFilePath)
                IO.File.Copy(baseFile, newFile)
            End If
        End If
    End Sub

    Public Shared Sub DeleteFile(ByVal basePath As String, ByVal filePath As String)
        If filePath <> "" AndAlso Not filePath.StartsWith("Icons/") Then
            Dim deleteFile = IO.Path.Combine(basePath, filePath)
            If IO.File.Exists(deleteFile) Then
                IO.File.Delete(deleteFile)
            End If
        End If
    End Sub

    Public Sub RelpaceFile(oldFilePath As String, newFilePath As String, icon As Bitmap)
        If oldFilePath <> "" AndAlso Not oldFilePath.StartsWith("Icons/") Then
            Dim deleteFile = IO.Path.Combine(_Path, oldFilePath)
            If IO.File.Exists(deleteFile) Then
                IO.File.Delete(deleteFile)
            End If
        End If

        Dim upfile = IO.Path.Combine(_Path, newFilePath)
        icon.Save(upfile, _ImageFormat)
        icon.Dispose()

    End Sub

    Private Function GetFormat(contentType As String) As System.Drawing.Imaging.ImageFormat
        Dim format As System.Drawing.Imaging.ImageFormat = Nothing
        If contentType.Contains("jpeg") Then
            format = System.Drawing.Imaging.ImageFormat.Jpeg
        ElseIf contentType.Contains("png") Then
            format = System.Drawing.Imaging.ImageFormat.Png
        End If
        Return format
    End Function

End Class
