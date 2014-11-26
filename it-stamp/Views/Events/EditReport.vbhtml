@ModelType [Event]
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "開催メモの編集"
End Code

<h1>@ViewBag.Title</h1>
<div class="jumbotron">
    <div class="jumbotron-contents">
        @Html.Partial("_EventCard", Model)
    </div>
</div>

<p>💡 開催メモは、コミュニティ管理者のみ閲覧できます。</p>

@Using Html.BeginForm("EditReport", "Events", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @<text>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        @Html.HiddenFor(Function(m) m.Id)
        @Html.HiddenFor(Function(m) m.Name)
        <div class="form-group">
            @Html.LabelFor(Function(m) m.ParticipantsOfflineCount, New With {.class = "control-label"}) <span class="text-primary">*</span>
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.ParticipantsOfflineCount, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.ParticipantsOfflineCount, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(m) m.ParticipantsOnlineCount, New With {.class = "control-label"}) <span class="text-primary">*</span>
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.ParticipantsOnlineCount, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.ParticipantsOnlineCount, "", New With {.class = "text-danger"})
            </div>
        </div>        <div class="form-group">
            @Html.LabelFor(Function(m) m.ReportMemo, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.EditorFor(Function(m) m.ReportMemo, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(m) m.ReportMemo, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            <input type="submit" value="保存" class="btn btn-primary" />
        </div>
    </text>
End Using

<hr />
@Html.ActionLink("戻る", "Edit", "Events", New With {.id = Model.Id}, Nothing)

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
