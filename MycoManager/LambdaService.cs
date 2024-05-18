using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using MycoManager;

public class LambdaService
{
    private static readonly AmazonLambdaClient lambdaClient = new AmazonLambdaClient(RegionEndpoint.APSoutheast2);
    private const string runtime = "dotnet8";
    private const string bucketName = "myco-manager-bucket";

    public static async Task PublishLambdaFunctionFromS3(string functionName)
    {
        try
        {
            // Create a request to publish the Lambda function
            var request = new CreateFunctionRequest()
            {
                FunctionName = functionName,
                Handler = "EnvironmentSensor::EnvironmentSensor.Function::FunctionHandler",
                Runtime = runtime,
                Role = $"arn:aws:iam::{await AccountIdService.GetAwsAccountId()}:role/env-sensor-role",
                Code = new FunctionCode { S3Bucket = bucketName, S3Key = "EnvironmentSensor.zip" }
            };

            // Publish the Lambda function
            var response = await lambdaClient.CreateFunctionAsync(request);

            // Optionally, you can process the response or handle errors here
            Console.WriteLine("Lambda function published successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error publishing Lambda function: {ex.Message}");
        }
    }

    public static async Task DeleteLambdaFunction(string functionName)
    {
        try
        {
            var request = new DeleteFunctionRequest()
            {
                FunctionName = functionName
            };
            await lambdaClient.DeleteFunctionAsync(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting Lambda function: {ex.Message}");
        }
    }

    public static async Task<string> GetLambdaArn(string functionName)
    {
        var request = new GetFunctionRequest
        {
            FunctionName = functionName
        };

        var response = await lambdaClient.GetFunctionAsync(request);

        return response.Configuration.FunctionArn;
    }

    public static async Task AddTriggerToLambdaFunction(string functionName, string sourceArn)
    {
        try
        {
            var addPermissionRequest = new AddPermissionRequest
            {
                FunctionName = functionName,
                StatementId = "apigateway-any",
                Action = "lambda:InvokeFunction",
                Principal = "apigateway.amazonaws.com",
                SourceArn = sourceArn
            };

            var response = await lambdaClient.AddPermissionAsync(addPermissionRequest);

            Console.WriteLine("Trigger added successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding trigger: {ex.Message}");
        }
    }
}