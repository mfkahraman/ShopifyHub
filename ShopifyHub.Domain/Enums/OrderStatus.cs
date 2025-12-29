namespace ShopifyHub.Domain.Enums;

public enum OrderStatus
{
    Pending,
    Authorized,
    PartiallyPaid,
    Paid,
    PartiallyRefunded,
    Refunded,
    Voided
}

public enum FulfillmentStatus
{
    Fulfilled,
    Null,
    Partial,
    Restocked
}

public enum SyncStatus
{
    Pending,
    Running,
    Completed,
    Failed
}