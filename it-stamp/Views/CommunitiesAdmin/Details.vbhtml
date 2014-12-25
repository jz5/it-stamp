@ModelType ItStamp.Community
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

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

        <dt>
            CreatedBy
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.CreatedBy.FriendlyName)
        </dd>

        <dt>
            LastUpdatedBy
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.LastUpdatedBy.FriendlyName)
        </dd>
</dl>
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.Id }) |
    @Html.ActionLink("Back to List", "Index")
</p>
