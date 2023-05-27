using System.Runtime.InteropServices;
using BootstrapBlazor.Components;
using Energy.Models;
using Energy.Service;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;


namespace Energy.Pages;

public partial class Data
{
    [Inject] public OverviewService? OverviewService { get; set; }
    [Inject] public Radzen.DialogService? dialogService { get; set; }

    private ShareData? shareData;

    private IEnumerable<EnergyData2>? data;
    private IEnumerable<EnergyData2>? traffic;
    private IEnumerable<WeatherData2>? weather;

    List<Dropdown>? dropdown;
    RadzenDataGrid<Appliance>? applianceGrid;

    private int househould;
    private DateTime timeHousehouldStart;
    private DateTime timeHousehouldEnd;

    private int battery;
    private DateTime timeBatteryStart;
    private DateTime timeBatteryEnd;

    private int car;
    private DateTime timeCarStart;
    private DateTime timeCarEnd;

    private int thermalStorage;
    private DateTime timeThermalStart;
    private DateTime timeThermalEnd;
    protected override async Task OnInitializedAsync()
    {
        data = await OverviewService!.GetDataAsync();
        weather = await OverviewService!.GetWeather();
        shareData = await OverviewService!.GetWindAndSolarData();
        traffic = await OverviewService!.GetTrafficLightData();



        dropdown = new()
        {
            new Dropdown(1, "Household Appliance"),
            new Dropdown(2, "Battery Storage"),
            new Dropdown(3, "E-Car"),
            new Dropdown(4, "Thermal Storage")
        };
    }


    void OnClick(object c, EventArgs e)
    {

        if (househould != 0)
        {
            System.Console.WriteLine(househould);
            System.Console.WriteLine(timeHousehouldStart);
            System.Console.WriteLine(timeHousehouldEnd);

            System.Console.WriteLine(timeHousehouldStart.ToString("HH:mm"));

            //Methode Haushalt
        }
        if (battery != 0)
        {
            //Methode Batterie

            System.Console.WriteLine(battery);
            System.Console.WriteLine(timeBatteryStart);
        }
        if (car != 0)
        {
            //Methode Auto
        }
        if (thermalStorage != 0)
        {
            //Methode Wärme
        }


        /*Console.WriteLine(househould);
        Console.WriteLine(battery);
        Console.WriteLine(car);
        Console.WriteLine(thermalStorage);*/
    }

    public class Appliance
{
    public Dropdown? DropDown
    {
        get; set;

    }
    public DateTime Time
    {
        get; set;
    }

}
public record Dropdown(int Id, string Name);


}
