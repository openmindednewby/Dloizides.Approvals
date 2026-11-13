using Shouldly;

namespace Dloizides.Approvals.Tests;

public class MakerCheckerGuardTests
{
    private static readonly Guid Maker = Guid.NewGuid();
    private static readonly Guid Editor = Guid.NewGuid();
    private static readonly Guid Checker = Guid.NewGuid();

    [Fact]
    public void GuardCanApprove_WhenApproverIsIndependent_DoesNotThrow()
    {
        var subject = new ApprovalSubject("REC-1", [Maker, Editor], []);

        var error = Record.Exception(() => MakerCheckerPolicy.GuardCanApprove(subject, Checker));

        error.ShouldBeNull();
    }

    [Fact]
    public void GuardCanApprove_WhenApproverEditedButDidNotCreate_ThrowsViolationNamingApprover()
    {
        var subject = new ApprovalSubject("REC-1", [Maker, Editor], []);

        var error = Should.Throw<MakerCheckerViolationException>(
            () => MakerCheckerPolicy.GuardCanApprove(subject, Editor));

        error.UserId.ShouldBe(Editor);
    }

    [Fact]
    public void GuardCanApprove_WhenApproverAlreadyApproved_ThrowsViolationNamingApprover()
    {
        var subject = new ApprovalSubject("REC-1", [Maker], [Checker]);

        var error = Should.Throw<MakerCheckerViolationException>(
            () => MakerCheckerPolicy.GuardCanApprove(subject, Checker));

        error.UserId.ShouldBe(Checker);
    }

    [Fact]
    public void GuardCanApprove_WithNoContributors_ThrowsPlainInvalidOperationNamingReference()
    {
        var subject = new ApprovalSubject("REC-9", [], []);

        var error = Record.Exception(() => MakerCheckerPolicy.GuardCanApprove(subject, Checker));

        error.ShouldNotBeNull();
        error.GetType().ShouldBe(typeof(InvalidOperationException));
        error.Message.ShouldContain("REC-9");
    }

    [Fact]
    public void GuardCanApprove_WithEmptyApproverId_ThrowsArgumentException()
    {
        var subject = new ApprovalSubject("REC-1", [Maker], []);

        var error = Record.Exception(() => MakerCheckerPolicy.GuardCanApprove(subject, Guid.Empty));

        error.ShouldBeOfType<ArgumentException>();
    }

    [Fact]
    public void GuardCanApprove_WithNullSubject_ThrowsArgumentNullException()
    {
        ApprovalSubject? subject = null;

        var error = Record.Exception(() => MakerCheckerPolicy.GuardCanApprove(subject!, Checker));

        error.ShouldBeOfType<ArgumentNullException>();
    }
}
