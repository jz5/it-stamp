Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Migrations
Imports System.Linq
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin

Namespace Migrations

    Friend NotInheritable Class Configuration
        Inherits DbMigrationsConfiguration(Of ApplicationDbContext)

        Public Sub New()
            AutomaticMigrationsEnabled = True
            ContextKey = "it_stamp.ApplicationDbContext"
        End Sub

        Protected Overrides Sub Seed(context As ApplicationDbContext)
            '  This method will be called after migrating to the latest version.

            '  You can use the DbSet(Of T).AddOrUpdate() helper extension method 
            '  to avoid creating duplicate seed data. E.g.
            '
            '    context.People.AddOrUpdate(
            '       Function(c) c.FullName,
            '       New Customer() With {.FullName = "Andrew Peters"},
            '       New Customer() With {.FullName = "Brice Lambson"},
            '       New Customer() With {.FullName = "Rowan Miller"})
            
            Dim userManager = New UserManager(Of ApplicationUser)(New UserStore(Of ApplicationUser)(context))
            Dim roleManager = New RoleManager(Of IdentityRole)(New RoleStore(Of IdentityRole)(context))

            ' Roles
            Dim roleNames() = New String() {"Admin"}

            For Each n In roleNames
                Dim role = roleManager.FindByName(n)
                If role Is Nothing Then
                    role = New IdentityRole(n)
                    Dim roleresult = roleManager.Create(role)
                End If
            Next


            Dim user = userManager.FindByName("admin")
            If user Is Nothing Then
                user = New ApplicationUser() With {
                    .UserName = "admin",
                    .Email = "admin@example.com",
                    .DisplayName = "Admin"
                }
                Dim result = userManager.Create(user, "Admin@123456")
                result = userManager.SetLockoutEnabled(user.Id, False)
            End If

            ' Add user admin to Role Admin if not already added
            Dim rolesForUser = userManager.GetRoles(user.Id)
            If Not rolesForUser.Contains("Admin") Then
                Dim result = userManager.AddToRole(user.Id, "Admin")
            End If

            ' Users
            For i = 1 To 20
                Dim name = "user" & i.ToString
                user = userManager.FindByName(name)
                If user Is Nothing Then
                    user = New ApplicationUser With {
                        .UserName = name,
                        .Email = name & "@example.com",
                        .DisplayName = "ユーザー" & i.ToString,
                        .IsPrivate = False}

                    Dim result = userManager.Create(user, name & "123456")
                    result = userManager.SetLockoutEnabled(user.Id, False)

                End If
            Next

            ' Add prefectures
            Dim prefs = New String() {"北海道", "青森県", "岩手県", "宮城県", "秋田県", "山形県", "福島県", "茨城県", "栃木県", "群馬県", "埼玉県", "千葉県", "東京都", "神奈川県", "新潟県", "富山県", "石川県", "福井県", "山梨県", "長野県", "岐阜県", "静岡県", "愛知県", "三重県", "滋賀県", "京都府", "大阪府", "兵庫県", "奈良県", "和歌山県", "鳥取県", "島根県", "岡山県", "広島県", "山口県", "徳島県", "香川県", "愛媛県", "高知県", "福岡県", "佐賀県", "長崎県", "熊本県", "大分県", "宮崎県", "鹿児島県", "沖縄県", "オンライン", "海外"}
            For i = 0 To prefs.Count - 1
                Dim p = New Prefecture() With {.Id = i + 1, .Name = prefs(i)}
                context.Prefectures.AddOrUpdate(p)
            Next

            ' Communities
            Dim r = New Random
            For i = 1 To 20
                Dim c = New Community With {
                    .Id = i,
                    .Name = "コミュニティ " & i.ToString,
                    .Description = "説明 " & i.ToString,
                    .Url = "http://google.com/?" & i.ToString,
                    .IconPath = "Icons/icon" & (r.Next(47) + 1).ToString("00") & ".png",
                    .CreationDateTime = Now,
                    .LastUpdatedDateTime = Now,
                    .CreatedBy = user,
                    .LastUpdatedBy = user
                    }
                context.Communities.AddOrUpdate(c)
            Next

            context.SaveChanges()

            context.Communities.First.Members = New List(Of ApplicationUser)
            context.Communities.First.Members.Add(user)

            For i = 1 To 20
                Dim id = i
                Dim e = New [Event] With {
                    .Id = i,
                    .Name = "勉強会" & i.ToString,
                    .Description = "説明1" & vbCrLf & "説明2",
                    .Url = "http://google.com/?" & i.ToString,
                    .CreationDateTime = Now,
                    .LastUpdatedDateTime = Now,
                    .CreatedBy = user,
                    .LastUpdatedBy = user,
                    .StartDateTime = Now.AddDays(i),
                    .EndDateTime = .StartDateTime.AddHours(8),
                    .Prefecture = context.Prefectures.Where(Function(p) p.Id = id).FirstOrDefault,
                    .Address = "千代田区千代田１−１",
                    .Place = "○○会場",
                    .Community = If(i Mod 5 <> 0, context.Communities.Where(Function(c) c.Id = id).FirstOrDefault, Nothing)
                    }
                context.Events.AddOrUpdate(e)
            Next

            context.SaveChanges()

        End Sub

    End Class

End Namespace
