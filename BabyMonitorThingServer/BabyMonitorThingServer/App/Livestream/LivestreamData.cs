using System.Collections.Concurrent;
using System.Globalization;
using BabyMonitorThingServer.App.Models.Livestream;

namespace BabyMonitorThingServer.App.Livestream;

public class LivestreamData
{
    private LivestreamData() { }
    private static LivestreamData? _instance = null;
    private static readonly object LockObject = new object();

    public static LivestreamData Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (LockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new LivestreamData();
                    }
                }
            }
            return _instance;
        }
    }

    public ConcurrentDictionary<string, SaveLivestreamData> DeviceIdLivestreamDataKeyValuePair { get; set; } =
        new ConcurrentDictionary<string, SaveLivestreamData>();

    public void StartRecordingLivestreamData(string deviceId)
    {
        Console.WriteLine($"Started recording livestream data for device {deviceId}");
        try
        {
            ClearLivestreamData(deviceId);
            Console.WriteLine($"Device id: {deviceId}");
            var saveLivestreamData = new SaveLivestreamData()
            {
                DeviceId = Guid.Parse(deviceId),
                DateStarted = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ServerKey = "85159608637075381844",
                BabyStates = new List<BabyState>()
            };

            DeviceIdLivestreamDataKeyValuePair.TryAdd(deviceId, saveLivestreamData);
            
            System.Timers.Timer aTimer = new(1000);
            aTimer.Elapsed += saveLivestreamData.OnElapsedInterval;
            aTimer.Start();
            saveLivestreamData.Timer = aTimer;
            
            System.Timers.Timer aTimer2 = new(3000);
            aTimer2.Elapsed += saveLivestreamData.OnElapsedInterval2;
            aTimer2.Start();
            saveLivestreamData.Timer2 = aTimer2;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in start recording livestream data {ex.Message}");
        }
    }

    public void ClearLivestreamData(string deviceId)
    {
        try
        {
            DeviceIdLivestreamDataKeyValuePair.TryRemove(deviceId, out _);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in clear livestream data {ex.Message}");
        }
    }
}