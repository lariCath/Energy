using System.ComponentModel.Design;
using Energy.Service;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.ComponentModel.DataAnnotations;
using Radzen.Blazor;

namespace Energy.Pages;

public partial class Data
{
    [Inject] public OverviewService? OverviewService { get; set; }

    private IEnumerable<EnergyData2>? data;
    private IEnumerable<WeatherData2>? weather;
    Hashtable selection = new Hashtable();
    Appliance? appliance;

    int? value;
    protected override async Task OnInitializedAsync()
    {
        data = await OverviewService!.GetDataAsync();
        weather = await OverviewService!.GetWeather();
    }
    public enum Appliance
    {
        [Display(Description = "Household Appliance")]
        householdAppliance,
        [Display(Description = "Battery Storage")]
        batteryStorage,
        [Display(Description = "E-Car ")]
        eCar,
        [Display(Description = "Thermal Storage")]
        thermalStorage
    }
}
