@ModelType ICollection(Of UserLoginInfo)
@Imports Microsoft.AspNet.Identity

@If Model.Count > 0 Then
    @<h3>関連付けられているサービス</h3>
    @<table class="table">
        <tbody>
            @For Each account As UserLoginInfo In Model
                @<tr>
                    <td>@account.LoginProvider</td>
                    <td>
                        @If ViewBag.ShowRemoveButton Then
                                Using Html.BeginForm("Disassociate", "Account")
                            @Html.AntiForgeryToken()
                            @<div>
                                @Html.Hidden("loginProvider", account.LoginProvider)
                                @Html.Hidden("providerKey", account.ProviderKey)
                                <input type="submit" class="btn btn-default" value="削除" title="この @account.LoginProvider ログインをアカウントから削除します" />
                            </div>
                        End Using
                    Else
                            @: &nbsp;
                        End If
                    </td>
                </tr>
            Next
        </tbody>
    </table>
End If
