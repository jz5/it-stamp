@ModelType IEnumerable(Of ItStamp.Community)
@Code
ViewData("Title") = "Index"
End Code

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.Name)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Description)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Url)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.IsHidden)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.IsLocked)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CreationDateTime)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.LastUpdatedDateTime)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Name)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Description)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Url)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.IsHidden)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.IsLocked)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CreationDateTime)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.LastUpdatedDateTime)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.Id }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.Id }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.Id })
        </td>
    </tr>
Next

</table>
