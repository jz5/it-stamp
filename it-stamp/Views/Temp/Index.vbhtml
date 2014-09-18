@Imports Microsoft.AspNet.Identity
@ModelType TempViewModel
@Code
    ViewBag.Title = "Temp"
End Code

<h1>@ViewBag.Title</h1>

<p class="text-success">@ViewBag.StatusMessage</p>

<section>
    @Using Html.BeginForm("Index", "Temp", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})

        @Html.AntiForgeryToken()

        @<text>
            @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
            <div class="form-group">
                <input type="submit" value="Tweet" class="btn btn-primary" />
            </div>
        </text>
    End Using
</section>


@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
