using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EnvironmentSensor;

public class Function
{

    public class Response
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public int CO2 { get; set; }
    }

    public Response FunctionHandler(ILambdaContext context)
    {
        Random rng = new Random();

        return new Response
        {
            Temperature = rng.NextDouble() + rng.Next(5, 35),
            Humidity = rng.NextDouble(),
            CO2 = rng.Next(250, 6000)
        };
    }
}
