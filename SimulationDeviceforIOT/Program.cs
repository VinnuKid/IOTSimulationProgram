using System;
using Microsoft.Azure.Devices.Client;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SimulationDeviceforIOT
{
    class Program
    {
        private static DeviceClient deviceClient;
        private readonly static string connectionString = "<<------Connection String------>>";
        static void Main(string[] args)
        {
            Console.WriteLine("Connecting Device to IOT Hub...");
            deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
            SendDeviceToCloudMessagesAsync(deviceClient);
            Console.ReadLine();

        }

        private static async void SendDeviceToCloudMessagesAsync(DeviceClient deviceClient)
        {
            try
            {
                double minTemperature = 20;
                double minHumidity = 60;
                double minPressure = 50;
                Random rand = new Random();

                while (true)
                {
                    double currentTemperature = minTemperature + rand.NextDouble() * 15;
                    double currentHumidity = minHumidity + rand.NextDouble() * 20;
                    double currentPressure = minPressure + rand.NextDouble() * 20;


                    // Create JSON message  

                    var telemetryDataPoint = new
                    {

                        temperature = currentTemperature,
                        humidity = currentHumidity,
                        pressure = currentPressure
                    };

                    var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                    var message = new Message(Encoding.ASCII.GetBytes(messageString));
                    await deviceClient.SendEventAsync(message);
                    Console.WriteLine("message sent : {1}", messageString);
                    await Task.Delay(1000 * 10);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
