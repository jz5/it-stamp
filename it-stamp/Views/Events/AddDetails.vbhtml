@ModelType AddEventDetailsViewModel
@Imports Microsoft.AspNet.Identity
@code
    ViewBag.Title = "IT勉強会の登録"
End Code

<h1>@ViewBag.Title</h1>

@Using Html.BeginForm("AddDetails", "Events", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @<text>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Name, New With {.class = "control-label"})
            <span class="text-primary">*</span><span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Name, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Name, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.Description, New With {.class = "control-label"})
            <span class="text-muted">（簡潔な紹介）</span>
            <div class="form-inline">
                @Html.EditorFor(Function(m) m.Description, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(m) m.Description, "", New With {.class = "text-danger"})
            </div>
        </div>

        <!-- 開始時間 -->
        <div class="form-group">
            @Html.LabelFor(Function(m) m.EndDate, "開始日時", New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                <div class="input-group date">
                    @Html.TextBoxFor(Function(m) m.StartDate, "{0:yyyy/MM/dd}", New With {.class = "form-control"})<span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
                <div class="input-group bootstrap-timepicker">
                    @Html.TextBoxFor(Function(m) m.StartTime, New With {.class = "form-control"})<span class="input-group-addon"><i class="glyphicon glyphicon-time"></i></span>
                </div>
                @Html.ValidationMessageFor(Function(m) m.StartTime, "", New With {.class = "text-danger"})
            </div>
        </div>

        <!-- 終了日時 -->
        <div class="form-group">
            @Html.LabelFor(Function(m) m.EndDate, "終了日時", New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                <div class="input-group date">
                    @Html.TextBoxFor(Function(m) m.EndDate, "{0:yyyy/MM/dd}", New With {.class = "form-control"})<span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
                <div class="input-group bootstrap-timepicker">
                    @Html.TextBoxFor(Function(m) m.EndTime, New With {.class = "form-control"})<span class="input-group-addon"><i class="glyphicon glyphicon-time"></i></span>
                </div>

                @Html.ValidationMessageFor(Function(m) m.EndDate, "", New With {.class = "text-danger"})
                @Html.ValidationMessageFor(Function(m) m.EndTime, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(m) m.PrefectureId, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Prefecture, New With {.class = "form-control", .disabled = "disabled"})
                @Html.HiddenFor(Function(m) m.PrefectureId)
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Address, New With {.class = "control-label"})
            <span class="text-muted">（地図表示用・都道府県不要）</span>
            <div class="form-inline">

                @Html.TextBoxFor(Function(m) m.Address, If(Not Model.IsOnline, New With {.class = "form-control"}, New With {.class = "form-control", .disabled = "disabled"}))
                @Html.ValidationMessageFor(Function(m) m.Address, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Place, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Place, If(Not Model.IsOnline, New With {.class = "form-control"}, New With {.class = "form-control", .disabled = "disabled"}))
                @Html.ValidationMessageFor(Function(m) m.Place, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Url, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Url, New With {.class = "form-control", .placeholder = "http://example.jp"})
                @Html.ValidationMessageFor(Function(m) m.Url, "", New With {.class = "text-danger"})
            </div>
        </div>

        <!-- コミュニティ -->
        <div class="form-group">
            @Html.LabelFor(Function(m) m.CommunityId, New With {.class = "control-label"})
            <span class="text-muted">（指定すると後から変更できません。「未指定」の場合、後から選択できます。）</span>
            <div class="form-inline">
                <input id="com-search-box" type="search" value="" class="form-control" placeholder="絞り込み（例: ○○ユーザーグループ）" style="min-width:280px;" />
            </div>
            <div class="form-inline">
                @Html.DropDownListFor(Function(m) m.CommunityId, Model.CommunitiesSelectList, "(未指定)", New With {.class = "form-control", .size = "10", .id = "com-list", .style = "max-width:500px;min-width:280px;"})
                @Html.ValidationMessageFor(Function(m) m.CommunityId, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.CommunityName, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.CommunityName, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.CommunityName, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            <input type="submit" value="登録" class="btn btn-primary" />
        </div>
    </text>
End Using


@Section Styles
    @Styles.Render("~/Content/datepicker3.css")
    @Styles.Render("~/Content/bootstrap-timepicker.css")
End Section


@Section Scripts

    @Scripts.Render("~/bundles/jqueryval")
    @Scripts.Render("~/Scripts/bootstrap-datepicker.js")
    @Scripts.Render("~/Scripts/locales/bootstrap-datepicker.ja.js")
    @Scripts.Render("~/Scripts/bootstrap-timepicker.js")
    @Scripts.Render("~/Scripts/jquery.selectboxsearch.min.js")
    <script>
        (function ($) {
            $('.input-group.date').datepicker({
                startDate: "@Model.StartDate.ToString("yyyy/MM/dd")",
                endDate: "@Now.AddYears(1).ToString("yyyy/MM/dd")",
                todayBtn: "linked",
                language: "ja",
                autoclose: true,
                todayHighlight: true
            }).datepicker("update", "@Model.StartDate.ToString("yyyy/MM/dd")");

            $('#StartTime').timepicker({
                showInputs: false,
                defaultTime: false,
                showMeridian: false
            }).on("show.timepicker", function (e) {
                if ($('#StartTime').val() == "")
                    $('#StartTime').timepicker("setTime", "00:00");
            });
            $('#EndTime').timepicker({
                showInputs: false,
                defaultTime: false,
                showMeridian: false
            }).on("show.timepicker", function (e) {
                if ($('#EndTime').val() == "")
                    $('#EndTime').timepicker("setTime", "00:00");
            });

            $("#com-search-box").selectboxsearch("#com-list");

            $("#CommunityName").keyup(function () {
                if ($(this).val() != "") {
                    $("#com-search-box").attr("disabled", "disabled");
                    $("#com-list").attr("disabled", "disabled");
                } else {
                    $("#com-search-box").removeAttr("disabled");
                    $("#com-list").removeAttr("disabled");
                }
            });

            if ($("#CommunityName").val() != "") {
                $("#com-search-box").attr("disabled", "disabled");
                $("#com-list").attr("disabled", "disabled");
            }

        })(jQuery);
    </script>
End Section

