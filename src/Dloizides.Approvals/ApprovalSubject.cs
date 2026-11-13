namespace Dloizides.Approvals;

/// <summary>
/// The record being approved: who made or changed it and who has already approved it.
/// </summary>
public sealed record ApprovalSubject(
    string Reference,
    IReadOnlyCollection<Guid> Contributors,
    IReadOnlyCollection<Guid> PriorApprovers);
