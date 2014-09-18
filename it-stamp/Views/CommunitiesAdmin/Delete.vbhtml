@ModelType ItStamp.Community
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>Community</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Name)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Name)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Description)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Description)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Url)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Url)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.IsHidden)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.IsHidden)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.IsLocked)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.IsLocked)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.CreationDateTime)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.CreationDateTime)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.LastUpdatedDateTime)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.LastUpdatedDateTime)
        </dd>

    </dl>
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>
