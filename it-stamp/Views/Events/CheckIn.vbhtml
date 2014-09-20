@ModelType CheckInViewModel

@Code
    ViewBag.Title = "チェックイン"
End Code

@Html.Partial("_TopBanner")

<div class="row">
    <div class="col-sm-12 col-md-8">

        <h1><i class="glyphicon glyphicon-ok"></i> @ViewBag.Title</h1>

        @If ViewBag.StatusMessage <> "" Then
            @<div class="alert alert-success fade in" role="alert">
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                @ViewBag.StatusMessage
            </div>
        End If

        @If ViewBag.ErrorMessage <> "" Then
            @<div class="alert alert-danger fade in" role="alert">
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                @ViewBag.ErrorMessage
            </div>
        End If

        <div class="panel">
            <div class="panel-body">
                @Html.Partial("_EventResult", Model.Event)
            </div>
        </div>


        @Using Html.BeginForm("CheckIn", "Events", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
            @Html.AntiForgeryToken()
            @Html.HiddenFor(Function(m) m.Event.Id)
            @Html.HiddenFor(Function(m) m.Event.Name)

            @Html.ValidationSummary(False, "", New With {.class = "text-danger"})

            @If ViewBag.CheckIned Then
                @<p>チェックイン済み</p>

            ElseIf Model.Event.IsCanceled OrElse Model.Event.IsHidden Then
                @<p>チェックインできません。</p>

            ElseIf Model.Event.StartDateTime.AddHours(-1) <= Now Then
                If Not Request.IsAuthenticated Then
                @<p>@Html.ActionLink("ログイン", "Login", "Account", New With {.ReturnUrl = If(Request.RawUrl.ToLower.Contains("login"), "", Request.RawUrl)}, Nothing) してチェックイン！</p>
                Else
                @<div class="form-group">
                    <div class="form-inline">
                        <input type="submit" value="チェックイン" class="btn btn-primary" />
                    </div>
                </div>
                End If
            Else
                @<p>開始時間の1時間前からチェックインできるようになります。</p>
            End If
        End Using

    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
