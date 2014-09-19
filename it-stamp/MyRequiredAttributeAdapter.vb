Imports System.ComponentModel.DataAnnotations
Imports ItStamp.Resources

Public Class MyRequiredAttributeAdapter
    Inherits RequiredAttributeAdapter

    Sub New(metadata As ModelMetadata, context As ControllerContext, attribute As RequiredAttribute)
        MyBase.New(metadata, context, attribute)
        attribute.ErrorMessageResourceType = GetType(MyResource)
        attribute.ErrorMessageResourceName = "PropertyValueRequired"
    End Sub

End Class
