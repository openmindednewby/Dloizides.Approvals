namespace Dloizides.Approvals;

/// <summary>
/// Approvals needed at or below a value limit and above it; the value is compared by magnitude.
/// </summary>
public sealed class ApprovalThreshold
{
    public ApprovalThreshold(decimal limit, int approvalsAtOrBelowLimit, int approvalsAboveLimit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);
        ArgumentOutOfRangeException.ThrowIfNegative(approvalsAtOrBelowLimit);
        ArgumentOutOfRangeException.ThrowIfLessThan(approvalsAboveLimit, approvalsAtOrBelowLimit);
        Limit = limit;
        ApprovalsAtOrBelowLimit = approvalsAtOrBelowLimit;
        ApprovalsAboveLimit = approvalsAboveLimit;
    }

    public decimal Limit { get; }

    public int ApprovalsAtOrBelowLimit { get; }

    public int ApprovalsAboveLimit { get; }

    /// <summary>Approvals required for a value of this size.</summary>
    public int RequiredApprovalsFor(decimal value) =>
        Math.Abs(value) > Limit ? ApprovalsAboveLimit : ApprovalsAtOrBelowLimit;
}
