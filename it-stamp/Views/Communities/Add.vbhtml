@Imports Microsoft.AspNet.Identity
@ModelType Community
@Code

End Code

<h1>コミュニティの登録</h1>

@Using Html.BeginForm("Add", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
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
            @Html.LabelFor(Function(m) m.Url, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Url, New With {.type = "url", .class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Url, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            <input type="submit" value="登録" class="btn btn-primary" />
        </div>
    </text>
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section

