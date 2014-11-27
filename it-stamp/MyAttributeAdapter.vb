Imports System.ComponentModel.DataAnnotations
Imports ItStamp.Resources

Public Class MyAttributeAdapter
    Inherits RequiredAttributeAdapter

    Sub New(metadata As ModelMetadata, context As ControllerContext, attribute As RequiredAttribute)
        MyBase.New(metadata, context, attribute)
        If attribute.ErrorMessage Is Nothing Then
            If attribute.ErrorMessageResourceType Is Nothing Then
                attribute.ErrorMessageResourceType = GetType(MyResource)
            End If
            If attribute.ErrorMessageResourceName Is Nothing Then
                attribute.ErrorMessageResourceName = "PropertyValueRequired"
            End If
        End If
    End Sub

End Class

Public Class MyStringLengthAttributeAdapter
    Inherits StringLengthAttributeAdapter

    Sub New(metadata As ModelMetadata, context As ControllerContext, attribute As StringLengthAttribute)
        MyBase.New(metadata, context, attribute)
        If attribute.ErrorMessage Is Nothing Then
            If attribute.ErrorMessageResourceType Is Nothing Then
                attribute.ErrorMessageResourceType = GetType(MyResource)
            End If
            If attribute.ErrorMessageResourceName Is Nothing Then
                attribute.ErrorMessageResourceName = "StringLengthMessage"
            End If
        End If
    End Sub

End Class
