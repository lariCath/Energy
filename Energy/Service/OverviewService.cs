namespace Energy.Service;

public class OverviewService
{
    private readonly IAPI dataAPI;

    public OverviewService(IAPI dataAPI)
    {
        this.dataAPI = dataAPI;
    }

    public async Task<List<EnergyData2>> GetDataAsync()
    {
        try
        {
            DateTime startTime = new DateTime(2023, 05, 25, 17, 0, 0);
            DateTime endTime = new DateTime(2023, 05, 25, 18, 0, 0);

            var start = startTime.ToString("yyyy-MM-dd HH:mm");
            var end = endTime.ToString("yyyy-MM-dd HH:mm");

            var a = new QueryParams("de", start, end);
            var rawData = await dataAPI.GetData(a);

            var result = new List<EnergyData2>();
            var lists = rawData.Content;

            for (int i = 0; i < lists!.TimeStamp.Count; i++)
            {
                var d = new EnergyData2(lists.TimeStamp[i], lists.RenewableShare[i]);
                result.Add(d);
            }

            return result;
        }
        catch (Exception ex)
        {
            return new List<EnergyData2>();
        }
    }
}
