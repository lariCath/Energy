using Energy.Models;
using Energy.Service;
using Microsoft.AspNetCore.Components;

namespace Energy.Pages;

public partial class Data
{
    [Inject] public OverviewService? OverviewService { get; set; }

    private ShareData? shareData;
    private IEnumerable<EnergyData2>? data;
    private IEnumerable<EnergyData2>? traffic;
    private IEnumerable<WeatherData2>? weather;
    List<Dropdown> dropdown;

    private int value;

    protected override async Task OnInitializedAsync()
    {
        data = await OverviewService!.GetDataAsync();
        weather = await OverviewService!.GetWeather();
        shareData = await OverviewService!.GetData2();
        traffic = await OverviewService!.GetTrafficLight();

        dropdown = new()
        {
            new Dropdown(1, "Household Appliance"),
            new Dropdown(2, "Battery Storage"),
            new Dropdown(3, "E-Car"),
            new Dropdown(4, "Thermal Storage")
        };

       
    }

    void OnChange(object value)
    {
        if (value.Equals("Household Appliance"))
        {

        }
        if (value.Equals("Battery Storage"))
        {

        }
        if (value.Equals("E-Car"))
        {

        }
        if (value.Equals("Thermal Storage"))
        {

        }

        Console.WriteLine($"Value changed to {str}");
    }



    public record Dropdown(int Id, string Name);
}
