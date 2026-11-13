using Shouldly;

namespace Dloizides.Approvals.Tests;

public class RequiredApprovalsTests
{
    private static readonly Guid Maker = Guid.NewGuid();
    private static readonly Guid FirstChecker = Guid.NewGuid();
    private static readonly Guid SecondChecker = Guid.NewGuid();

    [Fact]
    public void RequiredApprovals_WithoutThreshold_ReturnsOne()
    {
        const decimal value = 1_000_000m;

        var required = MakerCheckerPolicy.RequiredApprovals(value, threshold: null);

        required.ShouldBe(1);
    }

    [Fact]
    public void RequiredApprovals_WhenValueAboveLimit_ReturnsApprovalsAboveLimit()
    {
        var threshold = new ApprovalThreshold(limit: 10_000m, approvalsAtOrBelowLimit: 0, approvalsAboveLimit: 2);

        var required = MakerCheckerPolicy.RequiredApprovals(10_000.01m, threshold);

        required.ShouldBe(2);
    }

    [Fact]
    public void RequiredApprovals_WhenValueEqualsLimit_ReturnsApprovalsAtOrBelowLimit()
    {
        var threshold = new ApprovalThreshold(limit: 10_000m, approvalsAtOrBelowLimit: 0, approvalsAboveLimit: 2);

        var required = MakerCheckerPolicy.RequiredApprovals(10_000m, threshold);

        required.ShouldBe(0);
    }

    [Fact]
    public void RequiredApprovals_WhenNegativeValueMagnitudeAboveLimit_ReturnsApprovalsAboveLimit()
    {
        var threshold = new ApprovalThreshold(limit: 10_000m, approvalsAtOrBelowLimit: 0, approvalsAboveLimit: 2);

        var required = MakerCheckerPolicy.RequiredApprovals(-50_000m, threshold);

        required.ShouldBe(2);
    }

    [Fact]
    public void ApprovalThreshold_WithNegativeLimit_ThrowsArgumentOutOfRange()
    {
        const decimal limit = -1m;

        var error = Record.Exception(() => new ApprovalThreshold(limit, approvalsAtOrBelowLimit: 0, approvalsAboveLimit: 2));

        error.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ApprovalThreshold_WithNegativeApprovalsAtOrBelowLimit_ThrowsArgumentOutOfRange()
    {
        const int approvalsAtOrBelowLimit = -1;

        var error = Record.Exception(() => new ApprovalThreshold(10_000m, approvalsAtOrBelowLimit, approvalsAboveLimit: 2));

        error.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ApprovalThreshold_WhenAboveLimitNeedsFewerApprovalsThanBelow_ThrowsArgumentOutOfRange()
    {
        const int approvalsAtOrBelowLimit = 2;
        const int approvalsAboveLimit = 1;

        var error = Record.Exception(() => new ApprovalThreshold(10_000m, approvalsAtOrBelowLimit, approvalsAboveLimit));

        error.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void IsFullyApproved_WhenApprovalsReachRequired_ReturnsTrue()
    {
        var subject = new ApprovalSubject("REC-1", [Maker], [FirstChecker, SecondChecker]);

        var approved = MakerCheckerPolicy.IsFullyApproved(subject, requiredApprovals: 2);

        approved.ShouldBeTrue();
    }

    [Fact]
    public void IsFullyApproved_WhenApprovalsBelowRequired_ReturnsFalse()
    {
        var subject = new ApprovalSubject("REC-1", [Maker], [FirstChecker]);

        var approved = MakerCheckerPolicy.IsFullyApproved(subject, requiredApprovals: 2);

        approved.ShouldBeFalse();
    }

    [Fact]
    public void IsFullyApproved_WithNegativeRequired_ThrowsArgumentOutOfRange()
    {
        var subject = new ApprovalSubject("REC-1", [Maker], []);

        var error = Record.Exception(() => MakerCheckerPolicy.IsFullyApproved(subject, requiredApprovals: -1));

        error.ShouldBeOfType<ArgumentOutOfRangeException>();
    }
}
