using Energy.Service;

namespace Energy.Models;

public class ShareData
{
    public List<EnergyData2> SolarShare { get; set; }
    public List<EnergyData2> WindOnshoreShare { get; set; }
    public List<EnergyData2> WindOffshoreShare { get; set; }
}


