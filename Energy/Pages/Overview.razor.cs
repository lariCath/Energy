using Energy.Service;
using Microsoft.AspNetCore.Components;

namespace Energy.Pages;

public partial class Overview
{
    [Inject] public OverviewService OverviewService { get; set; }

    private IEnumerable<EnergyData2> data;

    protected override async Task OnInitializedAsync()
    {
        data = await OverviewService!.GetDataAsync();
    }
}
