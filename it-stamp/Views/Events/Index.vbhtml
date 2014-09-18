@ModelType SearchEventsViewModel
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "IT勉強会一覧"
End Code

@Html.Partial("_TopBanner")

<div class="row">
    <div class="col-md-8">

        <h1>@ViewBag.Title</h1>

        <div class="btn-toolbar" role="toolbar">
            <div class="btn-group">
                <button class="btn btn-default active" type="button">昨日</button>
                <button class="btn btn-default" type="button">今日</button>
                <button class="btn btn-default" type="button">aa</button>
            </div>
        </div>

        @*<table class="table">
                <thead>
                    <tr>
                        <td>日</td>
                        <td>月</td>
                        <td>火</td>
                        <td>水</td>
                        <td>木</td>
                        <td>金</td>
                        <td>土</td>
                    </tr>
                </thead>
                <tbody>

                </tbody>
            </table>*@



        <div>
            <span><small>@(Model.TotalCount)件@(If(Model.CurrentPage > 1, " " & Model.CurrentPage & "ページ目", ""))</small></span>
        </div>


        @Html.Partial("_EventResults")


        @If Request.IsAuthenticated Then
            @<hr />
            @Html.ActionLink("登録", "Add", "Events", Nothing, New With {.class = "btn btn-default"})
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





