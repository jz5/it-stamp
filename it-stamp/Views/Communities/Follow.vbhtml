@ModelType Community

@Code
    ViewBag.Title = "フォロー"
    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96")

End Code

@Html.Partial("_TopBanner")

<div class="row">
    <div class="col-sm-12 col-md-8">

        <h1>@ViewBag.Title</h1>

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
                @Html.Partial("_CommunityCard", Model)
            </div>
        </div>


        @Using Html.BeginForm("Follow", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
            @Html.AntiForgeryToken()
            @Html.HiddenFor(Function(m) m.Id)
            @Html.HiddenFor(Function(m) m.Name)

            @Html.ValidationSummary(False, "", New With {.class = "text-danger"})

            @<div class="form-group">
                <div class="form-inline">
                    <input id="follow-btn" type="submit" value="@(if(ViewBag.Followd,"フォロー中","フォロー"))" class="btn btn-primary" />
                </div>
            </div>
        End Using

    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

@section Scripts
    <script>
        (function ($) {
            var followed = @(If(ViewBag.Followd, "true", "false"));
            if (followed) {
                $("#follow-btn").hover(function () {
                    $(this).val("解除");
                },function () {
                    $(this).val("フォロー中");
                });
            }
        })(jQuery);
    </script>
End Section