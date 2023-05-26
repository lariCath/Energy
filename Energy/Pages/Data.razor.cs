using Energy.Models;
using Energy.Service;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Energy.Pages;

public partial class Data
{
    [Inject] public OverviewService? OverviewService { get; set; }

    private ShareData? shareData;
    private IEnumerable<EnergyData2>? data;
    private IEnumerable<WeatherData2>? weather;
    List<Dropdown> dropdown;

    private int value;

    protected override async Task OnInitializedAsync()
    {
        data = await OverviewService!.GetDataAsync();
        weather = await OverviewService!.GetWeather();
        shareData = await OverviewService!.GetData2();
        dropdown = new()
        {
            new Dropdown(1, "Household"),
            new Dropdown(2, "Battery"),
            new Dropdown(3, "E-Car"),
            new Dropdown(4, "Thermal")
        };
    }


    public record Dropdown(int Id, string Name);
}
