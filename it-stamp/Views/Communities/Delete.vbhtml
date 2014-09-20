@ModelType Community

@Code
    ViewBag.Title = "コミュニティの削除"
    
End Code

@Html.Partial("_TopBanner")

<div class="row">
    <div class="col-sm-12 col-md-8">

        <h1>@ViewBag.Title</h1>

        @*@If ViewBag.StatusMessage <> "" Then
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
        End If*@

        <div class="panel">
            <div class="panel-body">
                @Html.Partial("_CommunityCard", Model)
            </div>
        </div>

        @If ViewBag.CanDelete Then
            @Using Html.BeginForm("Delete", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
                @Html.AntiForgeryToken()
                @Html.HiddenFor(Function(m) m.Id)
                @Html.HiddenFor(Function(m) m.Name)

                @Html.ValidationSummary(False, "", New With {.class = "text-danger"})

                @<div class="form-group">
                    <div class="form-inline">
                        <input id="follow-btn" type="submit" value="削除" class="btn btn-primary" />
                    </div>
                </div>

            End Using
        Else
            @<p>削除できません。</p>
        End If

        <hr />
        @Html.ActionLink("戻る", "Edit", "Communities", New With {.id = Model.Id}, Nothing)


    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
