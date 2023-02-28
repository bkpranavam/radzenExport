using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Testgridexport.Pages
{
    public partial class EmplyeeTables
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public TestService TestService { get; set; }

        protected IEnumerable<Testgridexport.Models.Test.EmplyeeTable> emplyeeTables;

        protected RadzenDataGrid<Testgridexport.Models.Test.EmplyeeTable> grid0;
        protected bool isEdit = true;
        protected override async Task OnInitializedAsync()
        {
            emplyeeTables = await TestService.GetEmplyeeTables();
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await TestService.ExportEmplyeeTablesToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible()).Select(c => c.Property))
}, "EmplyeeTables");
            }

            if (args == null || args.Value == "xlsx")
            {
                await TestService.ExportEmplyeeTablesToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible()).Select(c => c.Property))
}, "EmplyeeTables");
            }
        }
        protected bool errorVisible;
        protected Testgridexport.Models.Test.EmplyeeTable emplyeeTable;

        protected async Task FormSubmit()
        {
            try
            {
                var result = isEdit ? await TestService.UpdateEmplyeeTable(emplyeeTable.Id, emplyeeTable) : await TestService.CreateEmplyeeTable(emplyeeTable);

            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {

        }
    }
}