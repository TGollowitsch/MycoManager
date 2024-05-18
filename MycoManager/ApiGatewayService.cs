using Amazon.ApiGatewayV2.Model;
using Amazon.ApiGatewayV2;
using Amazon;

namespace MycoManager
{
    public static class ApiGatewayService
    {
        private static readonly AmazonApiGatewayV2Client apiGatewayClient = new AmazonApiGatewayV2Client(RegionEndpoint.APSoutheast2);
        public static readonly string apiId = "q7ooteyxqh";

        public static async Task<string> IntegrateFunctionToApi(string lambdaFunctionName)
        {
            try
            {
                // Get HTTP API
                var getApiRequest = new GetApiRequest
                {
                    ApiId = apiId
                };
                var getApiResponse = await apiGatewayClient.GetApiAsync(getApiRequest);

                // Create integration
                var createIntegrationRequest = new CreateIntegrationRequest
                {
                    ApiId = getApiResponse.ApiId,
                    IntegrationType = IntegrationType.AWS_PROXY,
                    IntegrationUri = await LambdaService.GetLambdaArn(lambdaFunctionName),
                    PayloadFormatVersion = "2.0"
                };
                var createIntegrationResponse = await apiGatewayClient.CreateIntegrationAsync(createIntegrationRequest);

                // Create route
                var createRouteRequest = new CreateRouteRequest
                {
                    ApiId = getApiResponse.ApiId,
                    RouteKey = $"ANY /{lambdaFunctionName}",
                    Target = $"integrations/{createIntegrationResponse.IntegrationId}",
                };
                var createRouteResponse = await apiGatewayClient.CreateRouteAsync(createRouteRequest);

                await LambdaService.AddTriggerToLambdaFunction(
                    lambdaFunctionName,
                    $"arn:aws:execute-api:{RegionEndpoint.APSoutheast2.SystemName}:{await AccountIdService.GetAwsAccountId()}:{apiId}/*");

                Console.WriteLine("HTTP API created and deployed successfully.");

                return $"{getApiResponse.ApiEndpoint}/{lambdaFunctionName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating and deploying HTTP API: {ex.Message}");
                return null;
            }
        }

        public static async Task RemoveFunctionFromApi(string functionName)
        {
            // Get API
            var getApiRequest = new GetApiRequest
            {
                ApiId = apiId
            };
            var getApiResponse = await apiGatewayClient.GetApiAsync(getApiRequest);

            // Delete Route
            var getRoutesRequest = new GetRoutesRequest
            {
                ApiId = getApiResponse.ApiId
            };

            var routes = await apiGatewayClient.GetRoutesAsync(getRoutesRequest);

            var route = routes.Items.FirstOrDefault(route => route.RouteKey == $"ANY /{functionName}");
            var routeId = route.RouteId;
            var integrationId = route.Target[(route.Target.IndexOf('/') + 1)..];

            var deleteRouteRequest = new DeleteRouteRequest
            {
                ApiId = apiId,
                RouteId = routeId
            };
            await apiGatewayClient.DeleteRouteAsync(deleteRouteRequest);

            // Delete Integration
            var deleteIntegreationRequest = new DeleteIntegrationRequest
            {
                ApiId = apiId,
                IntegrationId = integrationId
            };
            await apiGatewayClient.DeleteIntegrationAsync(deleteIntegreationRequest);
        }
    }
}
