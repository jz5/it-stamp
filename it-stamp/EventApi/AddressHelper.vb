Imports System.Runtime.CompilerServices

Module AddressHelper

    Private Prefs As New List(Of Prefecture)

    Sub New()
        Dim ps = New String() {"北海道", "青森県", "岩手県", "宮城県", "秋田県", "山形県", "福島県", "茨城県", "栃木県", "群馬県", "埼玉県", "千葉県", "東京都", "神奈川県", "新潟県", "富山県", "石川県", "福井県", "山梨県", "長野県", "岐阜県", "静岡県", "愛知県", "三重県", "滋賀県", "京都府", "大阪府", "兵庫県", "奈良県", "和歌山県", "鳥取県", "島根県", "岡山県", "広島県", "山口県", "徳島県", "香川県", "愛媛県", "高知県", "福岡県", "佐賀県", "長崎県", "熊本県", "大分県", "宮崎県", "鹿児島県", "沖縄県"}
        For i = 0 To ps.Count - 1
            Prefs.Add(New Prefecture With {.Id = i + 1, .Name = ps(i)})
        Next
    End Sub

    <Extension>
    Function RemovePrefecture(address As String) As String
        Dim a = address.Trim
        For Each p In Prefs
            If a.StartsWith(p.Name) Then
                Return address.Substring(p.Name.Length)
            End If
        Next
        Return a
    End Function

    <Extension>
    Function GetPrefecture(address As String) As Prefecture
        Dim a = address.Trim
        For Each p In Prefs
            If a.StartsWith(p.Name) Then
                Return p
            End If
        Next
        Return Nothing
    End Function

    <Extension>
    Function GetPrefecture(id As Integer) As Prefecture
        Return Prefs.Where(Function(p) p.Id = id).First
    End Function
End Module