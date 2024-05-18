using Amazon;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;

namespace MycoManager
{
    public static class AccountIdService
    {
        private static readonly IAmazonSecurityTokenService stsClient = new AmazonSecurityTokenServiceClient(RegionEndpoint.APSoutheast2);

        public static async Task<string> GetAwsAccountId()
        {
            try
            {
                var request = new GetCallerIdentityRequest();

                var response = await stsClient.GetCallerIdentityAsync(request);

                return response.Account;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving AWS account ID: {ex.Message}");
                return null;
            }
        }
    }
}
