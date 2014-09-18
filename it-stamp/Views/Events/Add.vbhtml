@Imports Microsoft.AspNet.Identity
@ModelType AddEventViewModel
@Code

End Code

<h1>IT勉強会の登録</h1>

@Using Html.BeginForm("Add", "Events", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @<text>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        <div class="form-group">
            @Html.LabelFor(Function(m) m.StartDate, New With {.class = "control-label"})
            <span class="text-muted">（開始日）</span>
            <div class="form-inline">
                <div class="input-group date">
                    @Html.TextBoxFor(Function(m) m.StartDate, New With {.class = "form-control"})<span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
                @Html.ValidationMessageFor(Function(m) m.StartDate, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.PrefectureId, New With {.class = "control-label"})
            <span class="text-muted">（都道府県・オンライン）</span>
            <div class="form-inline">
                @Html.DropDownListFor(Function(m) m.PrefectureId, Model.PrefectureSelectList, "選択してください", New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.PrefectureId, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            <input type="submit" value="選択" class="btn btn-default" />
        </div>
    </text>
End Using


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

