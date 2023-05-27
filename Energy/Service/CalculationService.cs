namespace Energy.Service;

public class CalculationService
{
    public List<EnergyData2> GetCarData(List<EnergyData2> values1, List<DateTime[]> timeframesCar)
    {
        return CarFunc(timeframesCar, values1);
    }

    public Data2 GetDeviceData(List<EnergyData2> values1, List<DateTime[]> timeframesCar, int duration)
    {
        DevicesFunc(timeframesCar, values1, duration, out double maxScore, out int bestTime);

        return new Data2(bestTime, maxScore);
    }

    public Data3 GetHeaterData(int heatinDuaration, List<double> values1, List<double> meanTemp)
    {
        WaermeFunc(heatinDuaration, values1, meanTemp, out List<double> waermeMaxValues, out List<int> waermeMaxHours);

        return new Data3(waermeMaxHours, waermeMaxValues);
    }

    public Data4 GetBatteryData(List<double> windShare, List<double> solarShare)
    {
        BatteryFunc(solarShare, windShare, out List<double> windValues, out List<double> windHours);

        return new Data4(windHours, windValues);
    }

    private List<EnergyData2> CarFunc(List<DateTime[]> timeframes, List<EnergyData2> hourlyValues)
    {
        var orderedData = hourlyValues.Where(h => TimeInTimeframes(h.Time, timeframes)).OrderByDescending(v => v.RenewableShare).ToList();

        return orderedData;
    }

    private bool TimeInTimeframes(DateTime time, List<DateTime[]> frames)
    {
        foreach (var frame in frames)
        {
            if (frame[0] < time && time < frame[1]) return true;
        }

        return false;
    }

    private List<double> MeanPerHour(List<double> values)
    {
        List<double> hourlyValues = new List<double>();

        for (int i = 0; i < values.Count; i += 4)
        {
            double sum = values.GetRange(i, 4).Sum();
            double mean = sum / 4;
            hourlyValues.Add(mean);
        }

        return hourlyValues;
    }

    private void NMaxElements(List<double> list1, List<int> list3, int N, out List<double> finalList, out List<int> indexList)
    {
        finalList = new List<double>();
        List<double> tempList = new List<double>(list1);
        List<double> tempList2 = new List<double>(list1);

        indexList = new List<int>();

        for (int i = 0; i < N; i++)
        {
            double max1 = 0;

            for (int j = 0; j < tempList2.Count; j++)
            {
                if (tempList2[j] > max1)
                {
                    max1 = tempList2[j];
                }
            }

            int index1 = tempList.IndexOf(max1);
            int indexValue = list3[index1];
            indexList.Add(indexValue);

            tempList2.Remove(max1);
            finalList.Add(max1);
        }
    }

    private void WaermeFunc(int T_waerme, List<double> hourlyValues, List<double> temp, out List<double> maxValues, out List<int> maxHours)
    {
        int count = hourlyValues.Count(x => x > 60);
        maxValues = new List<double>();
        maxHours = new List<int>();


        if (temp[0] > 15 && temp[1] > 15)
        {
            Console.WriteLine("Temperatur hoch - kein Aufheizen");
            maxValues.Add(0);
            maxHours.Add(0);
        }
        else if (count < 3)
        {
            Console.WriteLine("Kein Aufheizen da Wetter zu schlecht");
            maxValues.Add(200);
            maxHours.Add(200);
        }
        else
        {
            List<int> hourSet = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            //List<double> maxValues;
            //List<int> maxHours;
            NMaxElements(hourlyValues, hourSet, T_waerme, out maxValues, out maxHours);

        }
    }

    private void BatteryFunc(List<double> solarShare, List<double> windShare, out List<double> windValues, out List<double> windHours)
    {
        int count = solarShare.Count(x => x > 30);
        int count2 = windShare.Count(x => x > 30);
        windValues = new List<double>();
        windHours = new List<double>();

        if (count > 2)
        {
            Console.WriteLine("die sonne scheint und der batteriespeicher lädt sich selber");
            windValues.Add(0);
            windHours.Add(0);
        }
        else if (count2 < 3)
        {
            Console.WriteLine("wir laden nicht weil keine erneuerbare");
            windValues.Add(200);
            windHours.Add(200);
        }
        else
        {
            for (int i = 0; i < windShare.Count; i++)
            {
                if (windShare[i] > 30)
                {
                    //Console.WriteLine(windShare[i]);
                    windValues.Add(windShare[i]);
                    windHours.Add(i);
                }
            }
        }
    }

    private void DevicesFunc(List<DateTime[]> timeframes, List<EnergyData2> hourlyValues, int duration, out double maxScore, out int bestTime)
    {
        List<double> scoreList = new List<double>();
        List<int> startTimes = new List<int>();
        List<int> times = new();

        foreach(var time in timeframes)
        {
            times.AddRange(time.Select(t => t.Hour));
        }

        times = times.Distinct().ToList();

        var possible = hourlyValues.Where(h => TimeInTimeframes(h.Time, timeframes)).ToList();

        foreach (var el in times)
        {
            for (int i = el[0]; i < el[1] - (duration - 1); i++)
            {
                double score = 0;
                startTimes.Add(i);

                for (int j = 0; j < duration; j++)
                {
                    score += hourlyValues[i + j];
                }

                scoreList.Add(score);
            }
        }

        double maxScoreValue = scoreList.Max();
        int index1 = scoreList.IndexOf(maxScoreValue);
        int bestStartTime = startTimes[index1];

        maxScore = maxScoreValue;
        bestTime = bestStartTime;
    }
}


public record Data1(double relValue, double relHour);
public record Data2(int time, double value);
public record Data3(List<int> hours, List<double> values);
public record Data4(List<double> hours, List<double> values);

#region refcode
//public static void Do(List<double> values1, List<double> meanTemp, List<double> windShare, List<double> solarShare, List<int[]> timeframesCar, List<int[]> timeframesOtherDevice)
//{
//    //api viertelstündlich -> jeder vierte wert
//    //List<double> hourlyValues = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 82.0, 88.3, 92.4, 94.3, 93.2, 89.5, 82.9, 69.8, 54.4, 42.3, 38.1, 39.4, 42.4, 99.8 };
//    //List<double> hourlyValues = new List<double> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22.0, 18.3,12.4,14.3, 13.2, 19.5, 12.9, 19.8, 14.4, 12.3, 18.1, 39.4, 12.4, 99.8};
//    //List<double> hourlyValues = MeanPerHour(values);
//    //Console.WriteLine("Stundenmittelwerte: " + string.Join(", ", hourlyValues));

//    int T_waerme = 5;   //wärmeadedauer
//                        //List<double> temp = new List<double> { 13, 18 };    //vorrausage durchschnittstemp nchste 2 tage
//    List<double> waermeMaxValues;
//    List<int> waermeMaxHours;
//    WaermeFunc(T_waerme, values1, meanTemp, out waermeMaxValues, out waermeMaxHours);
//    //Console.WriteLine("Wärme: " + string.Join(", ", waermeMaxValues.ToArray()));
//    //Console.WriteLine("Wärme Stunden: " + string.Join(", ", waermeMaxHours.ToArray()));

//    //List<double> solarShare = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20.0, 10.3, 10.4, 14.3, 13.2, 10.5, 10.9, 10.8, 14.4, 12.3, 18.1, 39.4, 12.4, 43.8 };
//    //List<double> windShare = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20.0, 30.3, 40.4, 94.3, 93.2, 50.5, 40.9, 30.8, 54.4, 42.3, 38.1, 39.4, 42.4, 43.8 };

//    List<double> windValues, windHours;
//    BatteryFunc(solarShare, windShare, out windValues, out windHours);

//    //Console.WriteLine("Battery: " + string.Join(", ", windValues.ToArray()));
//    //Console.WriteLine("Battery Hours: " + string.Join(", ", windHours.ToArray()));

//    int T_car = 4;  //Ladeddauer Auto
//                    //List<int[]> timeframes = new List<int[]> { new int[] { 12, 17 }, new int[] { 18, 20 } };        //wann verfügabr von user

//    List<Data1> carMaxValues;
//    CarFunc(T_car, timeframesCar, values1, out carMaxValues);
//    //Console.WriteLine("E-Auto: " + string.Join(", ", carMaxValues.Select(c => c.relValue).ToArray()));
//    //Console.WriteLine("E-Auto Stunden: " + string.Join(", ", carMaxValues.Select(c => c.relHour).ToArray()));

//    int T_geraete = 2;  //ladedauer des geräts
//                        //List<int[]> timeframes2 = new List<int[]> { new int[] { 12, 17 }, new int[] { 18, 20 } };   //zeitslot
//    double maxScore;
//    int bestTime;
//    DevicesFunc(T_geraete, timeframesOtherDevice, values1, out maxScore, out bestTime);
//    //Console.WriteLine("Geräte: " + maxScore + bestTime);
//}
#endregion