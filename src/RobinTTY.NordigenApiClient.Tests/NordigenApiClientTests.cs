﻿using System.Net;

namespace RobinTTY.NordigenApiClient.Tests;

public class NordigenApiClientTests
{
    [Test]
    public async Task ExecuteRequestWithDefaultBaseAddress()
    {
        var apiClient = TestExtensions.GetConfiguredClient();
        await ExecuteExampleRequest(apiClient);
    }
    
    [Test]
    public async Task ExecuteRequestWithCustomBaseAddress()
    {
        var apiClient = TestExtensions.GetConfiguredClient("https://ob.gocardless.com/api/v2/");
        await ExecuteExampleRequest(apiClient);
    }

    private async Task ExecuteExampleRequest(NordigenClient apiClient)
    {
        var response = await apiClient.TokenEndpoint.GetTokenPair();
        TestExtensions.AssertNordigenApiResponseIsSuccessful(response, HttpStatusCode.OK);
        var response2 = await apiClient.TokenEndpoint.RefreshAccessToken(response.Result!.RefreshToken);
        TestExtensions.AssertNordigenApiResponseIsSuccessful(response2, HttpStatusCode.OK);
    }
}