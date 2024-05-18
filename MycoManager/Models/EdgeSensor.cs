namespace MycoManager.Models
{
    public class EdgeSensor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public double SetTemperature { get; set; }
        public double SetHumidity { get; set; }
        public int SetCO2PPM { get; set; }
    }
}
