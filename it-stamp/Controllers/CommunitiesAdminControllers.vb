Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports ItStamp

Public Class CommunitiesAdminController
    Inherits System.Web.Mvc.Controller

    Private db As New ApplicationDbContext

    ' GET: CommunitiesAdmin
    Async Function Index() As Task(Of ActionResult)
        Return View(Await db.Communities.ToListAsync())
    End Function

    ' GET: CommunitiesAdmin/Details/5
    Async Function Details(ByVal id As Integer?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim community As Community = Await db.Communities.FindAsync(id)
        If IsNothing(community) Then
            Return HttpNotFound()
        End If
        Return View(community)
    End Function

    ' GET: CommunitiesAdmin/Create
    Function Create() As ActionResult
        Return View()
    End Function

    ' POST: CommunitiesAdmin/Create
    '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
    '詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function Create(<Bind(Include:="Id,Name,Description,Url,IsHidden,IsLocked,CreationDateTime,LastUpdatedDateTime")> ByVal community As Community) As Task(Of ActionResult)
        If ModelState.IsValid Then
            db.Communities.Add(community)
            Await db.SaveChangesAsync()
            Return RedirectToAction("Index")
        End If
        Return View(community)
    End Function

    ' GET: CommunitiesAdmin/Edit/5
    Async Function Edit(ByVal id As Integer?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim community As Community = Await db.Communities.FindAsync(id)
        If IsNothing(community) Then
            Return HttpNotFound()
        End If
        Return View(community)
    End Function

    ' POST: CommunitiesAdmin/Edit/5
    '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
    '詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function Edit(<Bind(Include:="Id,Name,Description,Url,IsHidden,IsLocked,CreationDateTime,LastUpdatedDateTime")> ByVal community As Community) As Task(Of ActionResult)
        If ModelState.IsValid Then
            db.Entry(community).State = EntityState.Modified
            Await db.SaveChangesAsync()
            Return RedirectToAction("Index")
        End If
        Return View(community)
    End Function

    ' GET: CommunitiesAdmin/Delete/5
    Async Function Delete(ByVal id As Integer?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim community As Community = Await db.Communities.FindAsync(id)
        If IsNothing(community) Then
            Return HttpNotFound()
        End If
        Return View(community)
    End Function

    ' POST: CommunitiesAdmin/Delete/5
    <HttpPost()>
    <ActionName("Delete")>
    <ValidateAntiForgeryToken()>
    Async Function DeleteConfirmed(ByVal id As Integer) As Task(Of ActionResult)
        Dim community As Community = Await db.Communities.FindAsync(id)
        db.Communities.Remove(community)
        Await db.SaveChangesAsync()
        Return RedirectToAction("Index")
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing) Then
            db.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
