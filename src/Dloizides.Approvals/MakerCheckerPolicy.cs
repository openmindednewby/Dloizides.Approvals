namespace Dloizides.Approvals;

/// <summary>
/// Segregation of duties: one person never both makes and approves a record.
/// </summary>
public static class MakerCheckerPolicy
{
    /// <summary>
    /// Throws unless the approver is a second pair of eyes: no contributors, a contributor or a repeat approver are refused.
    /// </summary>
    public static void GuardCanApprove(ApprovalSubject subject, Guid approverUserId)
    {
        ArgumentNullException.ThrowIfNull(subject);
        if (approverUserId == Guid.Empty)
            throw new ArgumentException("Approver id must not be empty.", nameof(approverUserId));

        if (subject.Contributors.Count == 0)
            throw new InvalidOperationException(
                $"{subject.Reference} records no contributors, so maker-checker cannot be enforced. Refusing to approve.");

        var approverContributed = subject.Contributors.Contains(approverUserId);
        var approverAlreadyApproved = subject.PriorApprovers.Contains(approverUserId);
        if (approverContributed || approverAlreadyApproved)
            throw new MakerCheckerViolationException(approverUserId);
    }

    /// <summary>Approvals required for a value; one when no threshold is configured.</summary>
    public static int RequiredApprovals(decimal value, ApprovalThreshold? threshold) =>
        threshold?.RequiredApprovalsFor(value) ?? 1;

    /// <summary>True when the record holds at least the required number of approvals.</summary>
    public static bool IsFullyApproved(ApprovalSubject subject, int requiredApprovals)
    {
        ArgumentNullException.ThrowIfNull(subject);
        ArgumentOutOfRangeException.ThrowIfNegative(requiredApprovals);
        return subject.PriorApprovers.Count >= requiredApprovals;
    }
}
