Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations

' ApplicationUser クラスにプロパティを追加することでユーザーのプロファイル データを追加できます。詳細については、http://go.microsoft.com/fwlink/?LinkID=317594 を参照してください。
Public Class ApplicationUser
    Inherits IdentityUser

    <StringLength(50)>
    <Display(Name:="表示名")>
    Property DisplayName As String

    <Url>
    <DataType(DataType.Url)>
    <StringLength(256)>
    <Display(Name:="Webサイト")>
    Property Url As String

    <StringLength(1000)>
    <DataType(DataType.MultilineText)>
    <Display(Name:="紹介文")>
    Property Description As String

    <StringLength(100)>
    Property IconPath As String

    <Display(Name:="プライベートモード")>
    Property IsPrivate As Boolean

    <StringLength(256)>
    Property Twitter As String
    <StringLength(256)>
    Property Facebook As String
    <StringLength(256)>
    Property Other As String

    Property ShareTwitter As Boolean
    Property ShareFacebook As Boolean
    Property ShareOther As Boolean


    Overridable Property Favorites As ICollection(Of Favorite)
    Overridable Property CheckIns As ICollection(Of CheckIn)
    Overridable Property Communities As ICollection(Of Community)
    Overridable Property OwnerCommunities As ICollection(Of Community)

    Public Async Function GenerateUserIdentityAsync(manager As UserManager(Of ApplicationUser)) As Task(Of ClaimsIdentity)
        ' authenticationType が CookieAuthenticationOptions.AuthenticationType で定義されているものと一致している必要があります
        Dim userIdentity = Await manager.CreateIdentityAsync(Me, DefaultAuthenticationTypes.ApplicationCookie)
        ' ここにカスタム ユーザー クレームを追加します
        Return userIdentity
    End Function

    ReadOnly Property FriendlyName As String
        Get
            Return If(Me.IsPrivate, Me.UserName, String.Format("{0}（{1}）", Me.DisplayName, Me.UserName)) & "さん"
        End Get
    End Property

End Class

Public Class ApplicationDbContext
    Inherits IdentityDbContext(Of ApplicationUser)
    Public Sub New()
        MyBase.New("DefaultConnection", throwIfV1Schema:=False)
    End Sub

    Public Shared Function Create() As ApplicationDbContext
        Return New ApplicationDbContext()
    End Function

    Property Events As DbSet(Of [Event])
    Property Communities As DbSet(Of Community)
    Property Prefectures As DbSet(Of Prefecture)
    Property CheckIns As DbSet(Of CheckIn)
    Property Comments As DbSet(Of Comment)
    Property SpecialEvents As DbSet(Of SpecialEvent)
    Property Favorites As DbSet(Of Favorite)
    Property Stamps As DbSet(Of Stamp)


    Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
        MyBase.OnModelCreating(modelBuilder)

        modelBuilder.Entity(Of Community)().HasMany(Function(c) c.Members).WithMany(Function(u) u.Communities).Map(Sub(mc)
                                                                                                                       mc.ToTable("CommunityMemberRelations")
                                                                                                                       mc.MapLeftKey("Id")
                                                                                                                       mc.MapRightKey("UserId")
                                                                                                                   End Sub)
        modelBuilder.Entity(Of Community)().HasMany(Function(c) c.Owners).WithMany(Function(u) u.OwnerCommunities).Map(Sub(mc)
                                                                                                                           mc.ToTable("CommunityOwnerRelations")
                                                                                                                           mc.MapLeftKey("Id")
                                                                                                                           mc.MapRightKey("UserId")
                                                                                                                       End Sub)

    End Sub



End Class
