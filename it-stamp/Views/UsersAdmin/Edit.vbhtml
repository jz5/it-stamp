@ModelType EditUserViewModel

@Code
    ViewData("Title") = "Edit"
End Code

<h1>@ViewBag.Title</h1>

@Using Html.BeginForm("Edit", "UsersAdmin", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @<text>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Email, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Email, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Email, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.Label("Roles", New With {.class = "control-label"})

            @For Each item In Model.RolesList
                @<div class="form-inline">
                    <div class="checkbox">
                        <input type="checkbox" name="SelectedRole" id="@item.Value" value="@item.Value" checked="@item.Selected" class="checkbox-inline" />
                        @Html.Label(item.Value, New With {.class = "control-label"})
                    </div>
                </div>
            Next
        </div>

        <div class="form-group">
            @Html.Label("Communities", New With {.class = "control-label"})

            @If Model.CommunitiesList.Count > 0 Then
                @For Each item In Model.CommunitiesList
                    @<div class="form-inline">
                        <div class="checkbox">
                            <input type="checkbox" name="SelectedCommunities" id="@("com" & item.Value)" value="@item.Value" checked="checked" class="checkbox-inline" />
                            <label class="control-label" for="@("com" & item.Value)">@item.Text</label>
                        </div>
                    </div>
                Next

            Else
                @<p>No communities</p>
            End If
        </div>

        <!-- コミュニティ -->
        <div class="form-group">
            <div class="form-inline">
                @Html.DropDownListFor(Function(m) m.CommunityId, Model.CommunitiesSelectList, "選択してください", New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.CommunityId, "", New With {.class = "text-danger"})
            </div>
        </div>


        <div class="form-group">
            @Html.Label("Owner Communities", New With {.class = "control-label"})

            @If Model.OwnerCommunitiesList.Count > 0 Then
                @For Each item In Model.OwnerCommunitiesList
                    @<div class="form-inline">
                        <div class="checkbox">
                            <input type="checkbox" name="SelectedOwnerCommunities" id="@("ocom" & item.Value)" value="@item.Value" checked="checked" class="checkbox-inline" />
                            <label class="control-label" for="@("ocom" & item.Value)">@item.Text</label>
                        </div>
                    </div>
                Next

            Else
                @<p>No owner communities</p>
            End If
        </div>

        <!-- コミュニティ -->
        <div class="form-group">
            <div class="form-inline">
                @Html.DropDownListFor(Function(m) m.OwnerCommunityId, Model.CommunitiesSelectList, "選択してください", New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.OwnerCommunityId, "", New With {.class = "text-danger"})
            </div>
        </div>


        <div class="form-group">
            <input type="submit" value="保存" class="btn btn-primary" />
        </div>
    </text>

End Using

<hr />
@Html.ActionLink("Back to List", "Index")

@section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section