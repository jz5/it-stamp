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

<Authorize(Roles:="Admin")>
Public Class SpecialEventsAdminController
    Inherits System.Web.Mvc.Controller

    Private db As New ApplicationDbContext

    ' GET: SpecialEvents
    Async Function Index() As Task(Of ActionResult)
        Return View(Await db.SpecialEvents.ToListAsync())
    End Function

    ' GET: SpecialEvents/Details/5
    Async Function Details(ByVal id As Integer?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim specialEvent As SpecialEvent = Await db.SpecialEvents.FindAsync(id)
        If IsNothing(specialEvent) Then
            Return HttpNotFound()
        End If
        Return View(specialEvent)
    End Function

    ' GET: SpecialEvents/Create
    Function Create() As ActionResult
        Return View()
    End Function

    ' POST: SpecialEvents/Create
    '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
    '詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function Create(<Bind(Include:="Id,Name,StartDateTime,EndDateTime")> ByVal specialEvent As SpecialEvent) As Task(Of ActionResult)
        If ModelState.IsValid Then
            db.SpecialEvents.Add(specialEvent)
            Await db.SaveChangesAsync()
            Return RedirectToAction("Index")
        End If
        Return View(specialEvent)
    End Function

    ' GET: SpecialEvents/Edit/5
    Async Function Edit(ByVal id As Integer?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim specialEvent As SpecialEvent = Await db.SpecialEvents.FindAsync(id)
        If IsNothing(specialEvent) Then
            Return HttpNotFound()
        End If
        Return View(specialEvent)
    End Function

    ' POST: SpecialEvents/Edit/5
    '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
    '詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function Edit(<Bind(Include:="Id,Name,StartDateTime,EndDateTime")> ByVal specialEvent As SpecialEvent) As Task(Of ActionResult)
        If ModelState.IsValid Then
            db.Entry(specialEvent).State = EntityState.Modified
            Await db.SaveChangesAsync()
            Return RedirectToAction("Index")
        End If
        Return View(specialEvent)
    End Function

    ' GET: SpecialEvents/Delete/5
    Async Function Delete(ByVal id As Integer?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim specialEvent As SpecialEvent = Await db.SpecialEvents.FindAsync(id)
        If IsNothing(specialEvent) Then
            Return HttpNotFound()
        End If
        Return View(specialEvent)
    End Function

    ' POST: SpecialEvents/Delete/5
    <HttpPost()>
    <ActionName("Delete")>
    <ValidateAntiForgeryToken()>
    Async Function DeleteConfirmed(ByVal id As Integer) As Task(Of ActionResult)
        Dim specialEvent As SpecialEvent = Await db.SpecialEvents.FindAsync(id)
        db.SpecialEvents.Remove(specialEvent)
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
