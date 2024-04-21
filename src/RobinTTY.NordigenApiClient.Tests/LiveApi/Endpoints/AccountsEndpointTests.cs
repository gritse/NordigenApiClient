﻿using System.Net;
using RobinTTY.NordigenApiClient.Models.Responses;
using RobinTTY.NordigenApiClient.Tests.Shared;

namespace RobinTTY.NordigenApiClient.Tests.LiveApi.Endpoints;

public class AccountsEndpointTests
{
    private readonly string[] _secrets = File.ReadAllLines("secrets.txt");
    private Guid _accountId;
    private NordigenClient _apiClient = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _accountId = Guid.Parse(_secrets[9]);
        _apiClient = TestHelpers.GetConfiguredClient();
    }

    #region RequestsWithSuccessfulResponse

    /// <summary>
    /// Tests the retrieval of an account.
    /// </summary>
    [Test]
    public async Task GetAccount()
    {
        var accountResponse = await _apiClient.AccountsEndpoint.GetAccount(_accountId);
        AssertionHelpers.AssertNordigenApiResponseIsSuccessful(accountResponse, HttpStatusCode.OK);
        var account = accountResponse.Result!;
        Assert.Multiple(() =>
        {
            Assert.That(account.InstitutionId, Is.EqualTo("SANDBOXFINANCE_SFIN0000"));
            Assert.That(account.Iban, Is.EqualTo("GL2010440000010445"));
            Assert.That(account.Status, Is.EqualTo(BankAccountStatus.Ready));
        });
    }

    /// <summary>
    /// Tests the retrieval of account balances.
    /// </summary>
    [Test]
    public async Task GetBalances()
    {
        var balancesResponse = await _apiClient.AccountsEndpoint.GetBalances(_accountId);
        AssertionHelpers.AssertNordigenApiResponseIsSuccessful(balancesResponse, HttpStatusCode.OK);
        var balances = balancesResponse.Result!;
        Assert.Multiple(() =>
        {
            Assert.That(balances, Has.Count.EqualTo(2));
            Assert.That(balances.Any(balance => balance.BalanceAmount.Amount == (decimal) 1913.12), Is.True);
            Assert.That(balances.Any(balance => balance.BalanceAmount.Currency == "EUR"), Is.True);
            Assert.That(balances.All(balance => balance.BalanceType != BalanceType.Undefined));
        });
    }

    /// <summary>
    /// Tests the retrieval of account details.
    /// </summary>
    [Test]
    public async Task GetAccountDetails()
    {
        var detailsResponse = await _apiClient.AccountsEndpoint.GetAccountDetails(_accountId);
        AssertionHelpers.AssertNordigenApiResponseIsSuccessful(detailsResponse, HttpStatusCode.OK);
        var details = detailsResponse.Result!;
        Assert.Multiple(() =>
        {
            Assert.That(details.Iban, Is.EqualTo("GL2010440000010445"));
            Assert.That(details.Name, Is.EqualTo("Main Account"));
            Assert.That(details.OwnerName, Is.EqualTo("Jane Doe"));
            Assert.That(details.CashAccountType, Is.EqualTo(CashAccountType.Current));
        });
    }

    /// <summary>
    /// Tests the retrieval of transactions.
    /// </summary>
    [Test]
    public async Task GetTransactions()
    {
        var transactionsResponse = await _apiClient.AccountsEndpoint.GetTransactions(_accountId);
        AssertionHelpers.AssertNordigenApiResponseIsSuccessful(transactionsResponse, HttpStatusCode.OK);
        var transactions = transactionsResponse.Result!;
        Assert.Multiple(() =>
        {
            Assert.That(transactions.BookedTransactions.Any(t =>
            {
                var matchesAll = true;
                matchesAll &= t.BankTransactionCode == "PMNT";
                matchesAll &= t.DebtorAccount?.Iban == "GL2010440000010445";
                matchesAll &= t.DebtorName == "MON MOTHMA";
                matchesAll &= t.RemittanceInformationUnstructured ==
                              "For the support of Restoration of the Republic foundation";
                matchesAll &= t.TransactionAmount.Amount == (decimal) 45.00;
                matchesAll &= t.TransactionAmount.Currency == "EUR";
                return matchesAll;
            }));
            Assert.That(transactions.PendingTransactions, Has.Count.GreaterThanOrEqualTo(1));
        });
    }

    /// <summary>
    /// Tests the retrieval of transactions within a specific time frame.
    /// </summary>
    [Test]
    public async Task GetTransactionRange()
    {
#if NET6_0_OR_GREATER
        var startDate = new DateOnly(2022, 08, 04);
        var balancesResponse =
            await _apiClient.AccountsEndpoint.GetTransactions(_accountId, startDate,
                DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromHours(24))));
#else
        var startDate = new DateTime(2022, 08, 04);
        var balancesResponse = await _apiClient.AccountsEndpoint.GetTransactions(_accountId, startDate,
            DateTime.Now.Subtract(TimeSpan.FromDays(1)));
#endif

        AssertionHelpers.AssertNordigenApiResponseIsSuccessful(balancesResponse, HttpStatusCode.OK);
        Assert.That(balancesResponse.Result!.BookedTransactions, Has.Count.AtLeast(6));
    }

    #endregion

    #region RequestsWithErrors

    /// <summary>
    /// Tests the retrieval of transactions within a specific time frame in the future. This should return an error.
    /// </summary>
    [Test]
    public async Task GetTransactionRangeInFuture()
    {
        var startDate = DateTime.Today.AddDays(1);
        var endDate = DateTime.Today.AddMonths(1).AddDays(1);

        // Returns AccountsError
#if NET6_0_OR_GREATER
        var transactionsResponse = await _apiClient.AccountsEndpoint.GetTransactions(_secrets[9],
            DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate));
#else
        var transactionsResponse = await _apiClient.AccountsEndpoint.GetTransactions(_secrets[9],
            startDate, endDate);
#endif

        Assert.Multiple(() =>
        {
            AssertionHelpers.AssertNordigenApiResponseIsUnsuccessful(transactionsResponse, HttpStatusCode.BadRequest);
            Assert.That(transactionsResponse.Error?.StartDateError, Is.Not.Null);
            Assert.That(transactionsResponse.Error?.EndDateError, Is.Not.Null);
            AssertionHelpers.AssertBasicResponseMatchesExpectations(transactionsResponse.Error?.StartDateError,
                "Date can't be in future",
                $"'{startDate:yyyy-MM-dd}' can't be greater than {DateTime.Today:yyyy-MM-dd}. Specify correct date range");
            AssertionHelpers.AssertBasicResponseMatchesExpectations(transactionsResponse.Error?.EndDateError,
                "Date can't be in future",
                $"'{endDate:yyyy-MM-dd}' can't be greater than {DateTime.Today:yyyy-MM-dd}. Specify correct date range");
        });
    }

    /// <summary>
    /// Tests the retrieval of transactions within a specific time frame where the date range is incorrect, since the endDate is before the startDate. This should throw an exception.
    /// </summary>
    [Test]
    public void GetTransactionRangeWithIncorrectRange()
    {
        var startDate = DateTime.Now.AddMonths(-1);
        var endDateBeforeStartDate = startDate.AddDays(-1);

#if NET6_0_OR_GREATER
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _apiClient.AccountsEndpoint.GetTransactions(_accountId, DateOnly.FromDateTime(startDate),
                DateOnly.FromDateTime(endDateBeforeStartDate)));

        Assert.That(exception.Message,
            Is.EqualTo(
                $"Starting date '{DateOnly.FromDateTime(startDate)}' is greater than end date '{DateOnly.FromDateTime(endDateBeforeStartDate)}'. When specifying date range, starting date must precede the end date."));
#else
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _apiClient.AccountsEndpoint.GetTransactions(_accountId, startDate, endDateBeforeStartDate));
        
        Assert.That(exception.Message,
            Is.EqualTo(
                $"Starting date '{startDate}' is greater than end date '{endDateBeforeStartDate}'. When specifying date range, starting date must precede the end date."));
#endif
    }

    #endregion
}
