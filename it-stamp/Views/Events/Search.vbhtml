@ModelType AddEventViewModel
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "IT勉強会の検索・登録"
    Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")

End Code
<h1>@ViewBag.Title</h1>

@If ViewBag.ErrorMessage <> "" AndAlso Request.IsAuthenticated Then
    @<div class="alert alert-danger fade in" role="alert">
        <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
        @ViewBag.ErrorMessage
    </div>
End If

@Using Html.BeginForm("Search", "Events", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
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
            <input type="submit" value="新規登録" class="btn btn-default" />
        </div>
    </text>
End Using

<div>
    <h2>イベント登録サイトから検索</h2>
    <p id="SearchKeyword" class="small"></p>
    <div class="jumbotron">
        <div class="jumbotron-contents" id="SearchResults">
            <h3 style="margin-top:10px;">開始日と都道府県を選択するとここにイベントが表示されます。</h3>
        </div>
    </div>
    <div>
        <p>下記のサイトに対応しています。</p>
        <ul class="list-inline">
            <li><img src="@Href("~/images/logos/" & "atnd.png")" /></li>
            <li><img src="@Href("~/images/logos/" & "connpass.png")" /></li>
        </ul>
        <p>未対応のサイトや、見つからない場合は、新規登録してください。</p>
    </div>
</div>

@Using Ajax.BeginForm("GetEvents", "Events", Nothing, New AjaxOptions With {.HttpMethod = "POST", .OnSuccess = "onSuccess", .OnBegin = "onBegin"}, New With {.id = "SearchForm"})
    @Html.AntiForgeryToken()
    @Html.Hidden("StartDate", Model.StartDate.ToString("yyyy/MM/dd"), New With {.id = "SearchStartDate"})
    @Html.Hidden("PrefectureId", Model.PrefectureId, New With {.id = "SearchPrefectureId"})

End Using

@Using Html.BeginForm("Select", "Events", FormMethod.Post, New With {.id = "SelectForm"})
    @Html.AntiForgeryToken()
End Using

@*<div class="jumbotron">
        <div class="jumbotron-contents">
            <p>💡 現在、新規登録のみ可能です。今後、登録済みのIT勉強会と、イベント登録サイトから検索できるようになる予定です。</p>
            @Html.ActionLink("開催予定のIT勉強会の表示", "Index", "Events", Nothing, New With {.class = "btn btn-default"})
        </div>
    </div>*@

@Section Styles
    @Styles.Render("~/Content/datepicker3.css")
End Section
@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    @Scripts.Render("~/Scripts/bootstrap-datepicker.js")
    @Scripts.Render("~/Scripts/locales/bootstrap-datepicker.ja.js")
    <script>
        (function ($) {
            $('.input-group.date').datepicker({
                startDate: "@now.AddYears(-1).ToString("yyyy/M/d")",
                endDate: "@now.AddYears(1).ToString("yyyy/M/d")",
                todayBtn: "linked",
                language: "ja",
                autoclose: true,
                todayHighlight: true
            }).datepicker("update", "@now.ToString("yyyy/MM/dd")");

            var startDate = "";
            var prefectureId = "";
            $("#StartDate").change(function () {
                if (startDate != $(this).val()) {
                    startDate = $(this).val();
                    if (startDate != "") {
                        search();
                    }
                }
            });

            var prefSelect = $("#PrefectureId");
            prefSelect.change(function () {
                search();
            });

            if (prefSelect.val()) {
                search();
            }

            function search() {
                $("#SearchStartDate").val($("#StartDate").val());
                $("#SearchPrefectureId").val($("#PrefectureId").val());
                $("#SearchForm").submit();
            }

        })(jQuery);

        function onSuccess(result) {
            if (result && $) {
                $("#SearchKeyword").text("キーワード: " + result.Keyword);

                if (result.ApiResults.length == 0) {
                    $("#SearchResults").html('<p style="margin-bottom:0;">該当するイベントは見つかりませんでした。</p>');
                    return;
                }

                var html = "";
                for (var i = 0; i < result.ApiResults.length; i++) {
                    var r = result.ApiResults[i];

                    html += '<li><img class="event-site-logo" src="@Href("~/images/logos/")' + r.Name + '.png" /> <a href="#" data-action="@Href("~/Events/Select/")?site=' + r.Name + '&eventId=' + r.EventId + '">' + r.Event.Name + '</a></li>';
                }
                $("#SearchResults").html('<ul class="list-unstyled" style="margin-bottom:0;">' + html + '</ul>');

                $("#SearchResults a").click(function () {
                    $("#SelectForm").attr("action", $(this).attr("data-action"));
                    $("#SelectForm").submit();
                    return false;
                });
            }
        }

        function onBegin() {
            $("#SearchKeyword").text("キーワード: " + $("#PrefectureId option:selected").text());
            $("#SearchResults").html("取得中...");
        }

    </script>
End Section
