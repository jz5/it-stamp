@ModelType ItStamp.SpecialEvent
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>SpecialEvent</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Name)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Name)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.StartDateTime)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.StartDateTime)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.EndDateTime)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.EndDateTime)
        </dd>

    </dl>
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.Id }) |
    @Html.ActionLink("Back to List", "Index")
</p>
