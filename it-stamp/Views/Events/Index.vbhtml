@ModelType SearchEventsViewModel
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "IT勉強会一覧"
End Code

<div class="row">
    <div class="col-md-8">

        <h1>@ViewBag.Title</h1>
        @If ViewBag.StatusMessage <> "" AndAlso Request.IsAuthenticated Then
            @<div class="alert alert-success fade in" role="alert">
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                @ViewBag.StatusMessage
            </div>
        End If
        <div class="clearfix">
            <div class="pull-left">
                <span><small>@(Model.TotalCount)件@(If(Model.CurrentPage > 1, " " & Model.CurrentPage & "ページ目", ""))</small></span>
            </div>

            <div class="pull-right">
                <div class="dropdown">
                    <a data-toggle="dropdown" href="#">検索条件 <span class="caret"></span></a>
                    <ul class="dropdown-menu" role="menu">
                        <li>@Html.ActionLink("開催予定の勉強会", "Index", "Events", Nothing, Nothing)</li>
                        <li>@Html.ActionLink("開催予定の勉強会（スタンプラリー対象）", "Index", "Events", New With {.SpecialEvent = 1}, Nothing)</li>
                        <li>@Html.ActionLink("過去の勉強会", "Index", "Events", New With {.past = True}, Nothing)</li>
                    </ul>
                </div>
            </div>
        </div>

        @Html.Partial("_EventResults")

        @If Request.IsAuthenticated Then
            @<div>
                <a href="@Url.Action("Add", "Events")"><i class="glyphicon glyphicon-plus"></i> IT勉強会の登録</a>
            </div>
        End If


    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

@Section Styles
    @Styles.Render("~/Content/datepicker3.css")
End Section

@Section Scripts

    @Scripts.Render("~/bundles/jqueryval")
    @Scripts.Render("~/Scripts/bootstrap-datepicker.js")
    @Scripts.Render("~/Scripts/locales/bootstrap-datepicker.ja.js")

    <script>
        $('.input-group.date').datepicker({
            startDate: "@Now.AddYears(-1).ToString("yyyy/M/d")",
            endDate: "@Now.AddYears(1).ToString("yyyy/M/d")",
            todayBtn: "linked",
            language: "ja",
            autoclose: true,
            todayHighlight: true
        }).datepicker("update", "@Now.ToString("yyyy/MM/dd")");

    </script>
End Section





