Imports System.Linq.Expressions
Imports System.Web.Mvc

Public Module HtmlHelpers
    <System.Runtime.CompilerServices.Extension> _
    Public Function DescriptionFor(Of TModel, TValue)(self As HtmlHelper(Of TModel), expression As Expression(Of Func(Of TModel, TValue))) As MvcHtmlString
        Dim metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData)
        Dim description = metadata.Description

        Return MvcHtmlString.Create(String.Format("<span>{0}</span>", description))
    End Function
End Module
