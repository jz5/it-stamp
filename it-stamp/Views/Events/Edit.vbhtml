@ModelType EventDetailsViewModel
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "IT勉強会の編集"

    'Dim icon = If(Model.Community IsNot Nothing AndAlso Model.Community.IconPath <> "", "/Uploads/" & Model.Community.IconPath, "http://placehold.it/96x96")
End Code
<h1>@ViewBag.Title</h1>
@Using Html.BeginForm("Edit", "Events", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @<text>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        <div class="form-group">
            @Html.LabelFor(Function(m) m.Name, New With {.class = "control-label"}) <span class="text-primary">*</span>
            <span class="text-muted"></span>
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
            @Html.LabelFor(Function(m) m.EndDate, "開始日時", New With {.class = "control-label"}) <span class="text-primary">*</span>
            <span class="text-muted"></span>
            <div class="form-inline">
                <div class="input-group date">
                    @Html.TextBoxFor(Function(m) m.StartDate, "{0:yyyy/MM/dd}", New With {.class = "form-control"})<span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
                <div class="input-group bootstrap-timepicker">
                    @Html.TextBox("StartTime", If(Model.StartTime.HasValue, Model.StartTime.Value.ToString("HH:mm"), ""), New With {.class = "form-control"})<span class="input-group-addon"><i class="glyphicon glyphicon-time"></i></span>
                </div>
                @Html.ValidationMessageFor(Function(m) m.StartDate, "", New With {.class = "text-danger"})
                @Html.ValidationMessageFor(Function(m) m.StartTime, "", New With {.class = "text-danger"})
            </div>
        </div>
        <!-- 終了日時 -->
        <div class="form-group">
            @Html.LabelFor(Function(m) m.EndDate, "終了日時", New With {.class = "control-label"}) <span class="text-primary">*</span>
            <span class="text-muted"></span>
            <div class="form-inline">
                <div class="input-group date">
                    @Html.TextBoxFor(Function(m) m.EndDate, "{0:yyyy/MM/dd}", New With {.class = "form-control"})<span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
                <div class="input-group bootstrap-timepicker">
                    @Html.TextBox("EndTime", If(Model.EndTime.HasValue, Model.EndTime.Value.ToString("HH:mm"), ""), New With {.class = "form-control"})<span class="input-group-addon"><i class="glyphicon glyphicon-time"></i></span>
                </div>
                @Html.ValidationMessageFor(Function(m) m.EndDate, "", New With {.class = "text-danger"})
                @Html.ValidationMessageFor(Function(m) m.EndTime, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.PrefectureId, New With {.class = "control-label"}) <span class="text-primary">*</span>
            <span class="text-muted">（都道府県・オンライン）</span>
            <div class="form-inline">
                @Html.DropDownListFor(Function(m) m.PrefectureId, Model.PrefectureSelectList, "選択してください", New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.PrefectureId, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.Address, New With {.class = "control-label"})
            <span class="text-muted">（地図表示用・都道府県不要）</span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Address, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Address, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.Place, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Place, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Place, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.Url, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Url, New With {.type = "url", .class = "form-control", .placeholder = "http://example.jp"})
                @Html.ValidationMessageFor(Function(m) m.Url, "", New With {.class = "text-danger"})
            </div>
        </div>

        @If Model.Community Is Nothing OrElse User.IsInRole("Admin") Then
            @<div class="form-group">
                @Html.LabelFor(Function(m) m.CommunityId, New With {.class = "control-label"})
                <span class="text-muted"></span>
                <div class="form-inline">
                    @Html.DropDownListFor(Function(m) m.CommunityId, Model.CommunitiesSelectList, "選択してください", New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.CommunityId, "", New With {.class = "text-danger"})
                </div>
            </div>
        Else
            @<div class="form-group">
                @Html.LabelFor(Function(m) m.CommunityId, New With {.class = "control-label"})
                <span class="text-muted">（変更できません）</span>
                <div class="form-inline">
                    @Html.TextBox("DummyName", Model.Community.Name, New With {.class = "form-control", .disabled = "disabled"})
                </div>
            </div>
        End If

        @If ViewBag.CanEditDetails Then
            @<div class="form-group">
                @Html.LabelFor(Function(m) m.CheckInCode, New With {.class = "control-label"})
                <span class="text-muted"></span>
                <div class="form-inline" data-toggle="tooltip" data-placement="right" title="aaa">
                    @Html.TextBoxFor(Function(m) m.CheckInCode, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.CheckInCode, "", New With {.class = "text-danger"})
                </div>
            </div>

            @<div class="form-group">
                @Html.LabelFor(Function(m) m.SpecialEventId, New With {.class = "control-label"})
                <span class="text-muted"></span>
                <div class="form-inline">
                    @Html.DropDownListFor(Function(m) m.SpecialEventId, Model.SpecialEventsSelectList, "選択してください", New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.SpecialEventId, "", New With {.class = "text-danger"})
                </div>
            </div>

            @<div class="form-group">
                <div class="checkbox">
                    @Html.CheckBoxFor(Function(m) m.IsLocked)
                    @Html.LabelFor(Function(m) m.IsLocked, New With {.class = "control-label"})
                    <span class="text-muted"></span>
                    @Html.ValidationMessageFor(Function(m) m.IsLocked, "", New With {.class = "text-danger"})
                </div>
            </div>

            @<div class="form-group">
                <div class="checkbox">
                    @Html.CheckBoxFor(Function(m) m.IsHidden)
                    @Html.LabelFor(Function(m) m.IsHidden, New With {.class = "control-label"})
                    <span class="text-muted"></span>
                    @Html.ValidationMessageFor(Function(m) m.IsHidden, "", New With {.class = "text-danger"})
                </div>
            </div>

            @<div class="form-group">
                <div class="checkbox">
                    @Html.CheckBoxFor(Function(m) m.IsCanceled)
                    @Html.LabelFor(Function(m) m.IsCanceled, New With {.class = "control-label"})
                    <span class="text-muted"></span>
                    @Html.ValidationMessageFor(Function(m) m.IsCanceled, "", New With {.class = "text-danger"})
                </div>
            </div>
        End If

        <div class="form-group">
            <input type="submit" value="保存" class="btn btn-primary" />
        </div>
    </text>
End Using

@If Not ViewBag.CanEditDetails Then
    @<div>
        @If Model.Community Is Nothing Then
            @<span>💡 主催コミュニティの方は、<a href="@Href("~/Home/Contact/")">申請</a>すると詳細情報を編集できます。</span>
        Else
            @<span>💡 @(Model.Community.Name)の方ですか？　<a href="@Href("~/Home/Contact/")">申請</a>すると詳細情報を編集できます。</span>
        End If
    </div>
Else
    @<h2>開催メモ</h2>
    @<table class="table table-responsive">
        <tbody>
            <tr>
                <td style="border-top-width:0;">参加人数（オフライン）</td>
                <td style="border-top-width:0;">@(If(Model.IsReported, Model.ParticipantsOfflineCount.ToString & "名","未登録"))</td>
            </tr>
            <tr>
                <td>参加人数（オンライン）</td>
                <td>@(If(Model.IsReported, Model.ParticipantsOnlineCount.ToString & "名", "未登録"))</td>
            </tr>
            <tr>
                <td>備考</td>
                <td>@Html.Raw(Html.Encode(Model.ReportMemo).Replace(vbCrLf, "<br />"))</td>
            </tr>
        </tbody>
    </table>

    @Html.ActionLink("編集", "EditReport", "Events", New With {.id = Model.Id}, New With {.class = "btn btn-default"})

End If

@If ViewBag.CanDelete Then
    @<h2>IT勉強会の削除</h2>
    @Html.ActionLink("削除", "Delete", "Events", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
End If

@Section Styles
    @Styles.Render("~/Content/skins/square/blue.css")
    @Styles.Render("~/Content/datepicker3.css")
    @Styles.Render("~/Content/bootstrap-timepicker.css")
End Section

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    @Scripts.Render("~/Scripts/bootstrap-datepicker.js")
    @Scripts.Render("~/Scripts/locales/bootstrap-datepicker.ja.js")
    @Scripts.Render("~/Scripts/bootstrap-timepicker.js")
    <script>
        (function ($) {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue'
            });

            $('.input-group.date').datepicker({
                todayBtn: "linked",
                language: "ja",
                autoclose: true,
                todayHighlight: true
            });

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
        }(jQuery));

    </script>
End Section
