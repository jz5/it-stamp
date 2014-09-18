@ModelType IEnumerable(Of ItStamp.SpecialEvent)
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
            @Html.DisplayNameFor(Function(model) model.StartDateTime)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.EndDateTime)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Name)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.StartDateTime)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.EndDateTime)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.Id }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.Id }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.Id })
        </td>
    </tr>
Next

</table>
