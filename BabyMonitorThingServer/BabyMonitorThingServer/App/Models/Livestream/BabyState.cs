namespace BabyMonitorThingServer.App.Models.Livestream;

public class BabyState
{
    public double? AtSecond {  get; set; }
    public string? Emotion { get; set; }
    public bool Awake { get; set; }   
}