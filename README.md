# Sprint.Filter

## What is this?

Sprint.Filter is a library that allows you to easily filter the IEnumerable/IQueryable. Sprint.Filter allow you to create both simple filter with one condition, and filter with multiple conditions, so there is the ability to filter using internal class collection.

## How do I use it?

1. Install ["Sprint.Filter"](http://nuget.org/packages/Sprint.Filter/) via [NuGet](http://nuget.org).
2. Create your collection of filters and call ** ApplyFilters ** filtering.

## Example

**/Filters/UserFilter.cs**

```csharp
public class UserFilter : FilterCollection
{
	public UserFilter(string gridKey, IDirectionActivityService directionActivityService, IMvcPrincipal user)
    {
        Add("DirectionActivity",new ValueTypeFilter<DirectionActivity, int>()).For(c => c.Id)
            .Conditions(conditions => conditions["isin"] = new ValueTypeIsInCondition<int>())
            .SetTitle("Direction:")
            .SetTemplate("IsInFastFilter")
            .SetDictionary(() => directionActivityService.GetTreeSource(user))
            .ConditionBuilder((expression, conditionKey) => 
                expression != null
				?Linq.Expr<User, bool>(u => u.DirectionActivities.AsQueryable().Any(expression))
                : null);

        Action = "Grid";

        UpdateTargetId = gridKey;
    }
}
```

**/Controllers/DealerController.cs**

```csharp
//ActionGridOptions implements IFilterOptions interface, you can use FilterOptions class.
public ActionResult Grid(ActionGridOptions options, IMvcPrincipal user)
{
    ViewBag.SearchString = options.SearchString;            

    var filter = new UserFilter(DealerGridKey, _directionActivityService,user);

    var query = filter.Init(options).ApplyFilters(_dealerService.GetAll(user))
        .Where(UserSpecification.Search(options.SearchString).Predicate);

    var model = new DealerGridModel(DealerGridKey);

    return View(new ActionGridView<User>(model, query).Init(options));
}

public ActionResult Filter(ActionGridOptions options, IMvcPrincipal user)
{
    var filter = new UserFilter(DealerGridKey, _directionActivityService, user);

    filter.Init(options);

    return View("FastFilterCollection", filter);
}
```

**/Views/Shared/FastFilterCollection.cshtml**

```html
@model IFilterCollectionView
@{
    Layout = null;
    var options = (Model.FilterOptions as ActionGridOptions);
    var searchString = options != null ? options.SearchString : null;
}
<div class="fast-filter-wrap">
    <input type="text" class="search" placeholder="Поиск" data-update-grid="@Model.UpdateTargetId" value="@searchString" />
    <div class="filter-panel">
        <table>
            <tr>
                <td>
                    <span class="filter-panel-title">My Filters</span>
                </td>
                <td>
                    <a class="filter-background filter-background-white" data-grid-setting="@Model.UpdateTargetId" href="@Url.Action("GridSetting",new{gridKey=Model.UpdateTargetId})">
                        <i class="filter-icon f-column"></i>
                    </a>
                </td>
                <td>
                    <a class="filter-background filter-background-white" data-grid-save="@Model.UpdateTargetId" href="@Url.Action("CreateGridState","GridState",new{gridKey=Model.UpdateTargetId})">
                        <i class="filter-icon f-save"></i>
                    </a>
                </td>
                <td>
                    <a class="filter-background filter-background-white" href="@Url.Action("Excel")">
                        <i class="filter-icon f-excel"></i>
                    </a>
                </td>
            </tr>
        </table>
    </div>

    @Html.Action("SavedFilterList", "GridState", new { gridKey=Model.UpdateTargetId,stateId=Model.FilterOptions.LoadFilterId })

	<!--Create html form for post filter data, you can use jquery ajax form plugin (http://malsup.com/jquery/form/) -->
    @using (Html.BeginForm(Model.Action, Model.Controller, Model.RouteValues, FormMethod.Post, new { @class = "ajax-filter fast-filter-collection", data_update_grid = @Model.UpdateTargetId }))
    {    
        <dl>
            @foreach (var key in Model.Filters.Keys)
            {
                var template = Model.Filters[key].TemplateName;
				<!--Render filter template -->
                @Html.EditorFor(f=>Model.Filters[key],template)
            }
        </dl>    
        <div class="clear"></div>
        <div class="filter-collection-bottom">
            <button type="submit" class="highlight-btn"><span>Apply</span></button>
            <p class="standard-btn">            
                @Html.ActionLink("Clear","ClearFilter")
            </p>
        </div>
    }
</div>
```

**/Views/Products/Shared/EditorTemplates/IsInFastFilter.cshtml**

```html
@model IFilterView
@{
    Layout = null;
    var source = Model.Dictionary as IEnumerable<ActionSelectListItem>;
	<!--Selected values -->
    ViewData["Values"] = Model.Values;
}
<dt>
    <label>@Model.Title</label>
</dt>
<dd> 
	<!--Custom listbox, you can use @Html.ListBoxFor(x=>x.Values,Model.Dictionary,new{@class="ui-multi-select",style="width:118px;"})-->   
    @Html.ActionListBoxFor(x=>x.Values,source,new{@class="ui-multi-select",style="width:118px;"})
</dd>
@Html.HiddenFor(m=>m.ConditionKey)
@Html.HiddenFor(m=>m.TypeName)
```
