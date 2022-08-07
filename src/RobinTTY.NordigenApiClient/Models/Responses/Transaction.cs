﻿using System.Text.Json.Serialization;

namespace RobinTTY.NordigenApiClient.Models.Responses;

/// <summary>
/// A financial transaction.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Used as access-ID in the PSD2 interface, where more details on an transaction are offered. If this data
    /// attribute is provided this shows that the AIS (Account Information Service) can get access on more details
    /// about this transaction.
    /// </summary>
    [JsonPropertyName("transactionId")]
    public string TransactionId { get; set; }
    /// <summary>
    /// The name of the party which owes the money.
    /// </summary>
    [JsonPropertyName("debtorName")]
    public string DebtorName { get; }
    /// <summary>
    /// The account of the party which owes the money.
    /// </summary>
    [JsonPropertyName("debtorAccount")]
    public MinimalBankAccount DebtorAccount { get; }
    /// <summary>
    /// Ultimate party that owes an amount of money to the (ultimate) creditor.
    /// </summary>
    [JsonPropertyName("ultimateDebtor")]
    public string UltimateDebtor { get; }
    /// <summary>
    /// The name of the party which is owed the money.
    /// </summary>
    [JsonPropertyName("creditorName")]
    public string CreditorName { get; }
    /// <summary>
    /// The account of the party which is owed the money.
    /// </summary>
    [JsonPropertyName("creditorAccount")]
    public MinimalBankAccount CreditorAccount { get; }
    /// <summary>
    /// The transaction amount including details about the currency the amount is denominated in.
    /// </summary>
    [JsonPropertyName("transactionAmount")]
    public AmountCurrencyPair TransactionAmount { get; }
    /// <summary>
    /// Code consisting of "DomainCode"-"FamilyCode"-"SubFamilyCode".
    /// For reference see <see href="https://wikipedia.org/wiki/ISO_20022">ISO20022</see>.
    /// <para>Example: PMNT-RCDT-ESCT defining a transaction assigned to the PayMeNT domain(PMNT), belonging
    /// to the family of ReceivedCreDitTransfer(RCDT) that facilitated the EuropeanSEPACreditTransfer(ESCT).</para>
    /// </summary>
    [JsonPropertyName("bankTransactionCode")]
    public string BankTransactionCode { get; }
    /// <summary>
    /// The date when the transaction was posted to the account.
    /// </summary>
    [JsonPropertyName("bookingDate")]
    public DateTime BookingDate { get; }
    /// <summary>
    /// Date at which assets become available to the account owner in case of a credit entry, or cease to be
    /// available to the account owner in case of a debit entry.
    /// </summary>
    [JsonPropertyName("valueDate")]
    public DateTime ValueDate { get; }
    /// <summary>
    /// Unstructured reference issued by the seller used to establish a link between the payment of an invoice
    /// and the invoice instance. The reference helps the seller to assign an incoming payment to the invoice by
    /// using a reference such as the invoice number or a purchase order number.
    /// <para><b>Limited by the PSD2 specification to 140 characters, truncates additional information if necessary.</b></para>
    /// </summary>
    [JsonPropertyName("remittanceInformationUnstructured")]
    public string RemittanceInformationUnstructured { get; }
    /// <summary>
    /// Unstructured reference issued by the seller used to establish a link between the payment of an invoice
    /// and the invoice instance. The reference helps the seller to assign an incoming payment to the invoice by
    /// using a reference such as the invoice number or a purchase order number.
    /// <para><b>Might contain more information than <see cref="RemittanceInformationUnstructured"/> (if contents were truncated).</b></para>
    /// </summary>
    [JsonPropertyName("remittanceInformationUnstructuredArray")]
    public IEnumerable<string> RemittanceInformationUnstructuredArray { get; }
    /// <summary>
    /// Unique identification assigned by the initiating party to unambiguously identify the transaction. This 
    /// identification is passed on, unchanged, throughout the entire end-to-end chain.
    /// </summary>
    [JsonPropertyName("endToEndId")]
    public string EndToEndId { get; }
    /// <summary>
    /// Identification of mandates, e.g. a SEPA mandate id.
    /// </summary>
    [JsonPropertyName("mandateId")]
    public string MandateId { get; }
    /// <summary>
    /// Proprietary bank transaction code to identify the underlying transaction.
    /// </summary>
    [JsonPropertyName("proprietaryBankTransactionCode")]
    public string ProprietaryBankTransactionCode { get; }
    /// <summary>
    /// Underlying reason for the transaction, as published in an external purpose code list.
    /// <para>For reference see: <see href="https://www.iso20022.org/catalogue-messages/additional-content-messages/external-code-sets"/></para>
    /// </summary>
    [JsonPropertyName("purposeCode")]
    public string PurposeCode { get; }

    /// <summary>
    /// Creates a new instance of <see cref="Transaction"/>.
    /// </summary>
    /// <param name="transactionId">Used as access-ID in the PSD2 interface, where more details on an transaction are offered. If this data
    /// attribute is provided this shows that the AIS (Account Information Service) can get access on more details about this transaction.</param>
    /// <param name="debtorName">The name of the party which owes the money.</param>
    /// <param name="debtorAccount">The account of the party which owes the money.</param>
    /// <param name="ultimateDebtor">Ultimate party that owes an amount of money to the (ultimate) creditor.</param>
    /// <param name="creditorName">The name of the party which is owed the money.</param>
    /// <param name="creditorAccount">The account of the party which is owed the money.</param>
    /// <param name="transactionAmount">The transaction amount including details about the currency the amount is denominated in.</param>
    /// <param name="bankTransactionCode">Code consisting of "DomainCode"-"FamilyCode"-"SubFamilyCode".
    /// For reference see <see href="https://wikipedia.org/wiki/ISO_20022">ISO20022</see>.
    /// <para>Example: PMNT-RCDT-ESCT defining a transaction assigned to the PayMeNT domain(PMNT), belonging
    /// to the family of ReceivedCreDitTransfer(RCDT) that facilitated the EuropeanSEPACreditTransfer(ESCT).</para></param>
    /// <param name="bookingDate">The date when the transaction was posted to the account.</param>
    /// <param name="valueDate">Date at which assets become available to the account owner in case of a credit entry, or cease to be
    /// available to the account owner in case of a debit entry.</param>
    /// <param name="remittanceInformationUnstructured">Unstructured reference issued by the seller used to establish a link between the payment of an invoice
    /// and the invoice instance. The reference helps the seller to assign an incoming payment to the invoice by
    /// using a reference such as the invoice number or a purchase order number.
    /// <para><b>Limited by the PSD2 specification to 140 characters, truncates additional information if necessary.</b></para></param>
    /// <param name="remittanceInformationUnstructuredArray">Unstructured reference issued by the seller used to establish a link between the payment of an invoice
    /// and the invoice instance. The reference helps the seller to assign an incoming payment to the invoice by
    /// using a reference such as the invoice number or a purchase order number.
    /// <para><b>Might contain more information than <see cref="RemittanceInformationUnstructured"/> (if contents were truncated).</b></para></param>
    /// <param name="endToEndId">Unique identification assigned by the initiating party to unambiguously identify the transaction. This 
    /// identification is passed on, unchanged, throughout the entire end-to-end chain.</param>
    /// <param name="mandateId">Identification of mandates, e.g. a SEPA mandate id.</param>
    /// <param name="proprietaryBankTransactionCode">Proprietary bank transaction code to identify the underlying transaction.</param>
    /// <param name="purposeCode">Underlying reason for the transaction, as published in an external purpose code list.
    /// <para>For reference see: <see href="https://www.iso20022.org/catalogue-messages/additional-content-messages/external-code-sets"/></para></param>
    [JsonConstructor]
    public Transaction(string transactionId, string debtorName, MinimalBankAccount debtorAccount, string ultimateDebtor, string creditorName, MinimalBankAccount creditorAccount, AmountCurrencyPair transactionAmount, string bankTransactionCode, DateTime bookingDate, DateTime valueDate, string remittanceInformationUnstructured, IEnumerable<string> remittanceInformationUnstructuredArray, string endToEndId, string mandateId, string proprietaryBankTransactionCode, string purposeCode)
    {
        TransactionId = transactionId;
        DebtorName = debtorName;
        DebtorAccount = debtorAccount;
        UltimateDebtor = ultimateDebtor;
        CreditorName = creditorName;
        CreditorAccount = creditorAccount;
        TransactionAmount = transactionAmount;
        BankTransactionCode = bankTransactionCode;
        BookingDate = bookingDate;
        ValueDate = valueDate;
        RemittanceInformationUnstructured = remittanceInformationUnstructured;
        RemittanceInformationUnstructuredArray = remittanceInformationUnstructuredArray;
        EndToEndId = endToEndId;
        MandateId = mandateId;
        ProprietaryBankTransactionCode = proprietaryBankTransactionCode;
        PurposeCode = purposeCode;
    }
}
