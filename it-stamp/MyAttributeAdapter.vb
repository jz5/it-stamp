Imports System.ComponentModel.DataAnnotations
Imports ItStamp.Resources

Public Class MyAttributeAdapter
    Inherits RequiredAttributeAdapter

    Sub New(metadata As ModelMetadata, context As ControllerContext, attribute As RequiredAttribute)
        MyBase.New(metadata, context, attribute)
        attribute.ErrorMessageResourceType = GetType(MyResource)
        attribute.ErrorMessageResourceName = "PropertyValueRequired"
    End Sub

End Class

Public Class MyStringLengthAttributeAdapter
    Inherits StringLengthAttributeAdapter

    Sub New(metadata As ModelMetadata, context As ControllerContext, attribute As StringLengthAttribute)
        MyBase.New(metadata, context, attribute)
        attribute.ErrorMessageResourceType = GetType(MyResource)
        attribute.ErrorMessageResourceName = "StringLengthMessage"
    End Sub

End Class
