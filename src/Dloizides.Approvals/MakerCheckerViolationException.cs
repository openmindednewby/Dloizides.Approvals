namespace Dloizides.Approvals;

/// <summary>
/// Thrown when the approver contributed to the record or has already approved it.
/// </summary>
public class MakerCheckerViolationException : InvalidOperationException
{
    public MakerCheckerViolationException(Guid userId)
        : base($"Maker-checker violation: user {userId} contributed to or already approved this record. A different user must approve.")
    {
        UserId = userId;
    }

    public MakerCheckerViolationException()
    {
    }

    public MakerCheckerViolationException(string message)
        : base(message)
    {
    }

    public MakerCheckerViolationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public Guid UserId { get; }
}
