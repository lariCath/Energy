using Energy.Models;
using Energy.Service;
using Microsoft.AspNetCore.Components;

namespace Energy.Pages;

public partial class Data
{
    [Inject] public OverviewService OverviewService { get; set; }

    private IEnumerable<EnergyData2> data;
    private ShareData? shareData;
    private IEnumerable<WeatherData2>? weather;

    protected override async Task OnInitializedAsync()
    {
        data = await OverviewService!.GetDataAsync();
        weather = await OverviewService!.GetWeather();
        shareData = await OverviewService!.GetData2();
    }
}
