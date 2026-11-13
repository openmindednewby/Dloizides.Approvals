using Shouldly;

namespace Dloizides.Approvals.Tests;

public class MakerCheckerGuardTests
{
    private static readonly Guid Maker = new Guid("11111111-1111-1111-1111-111111111111");
    private static readonly Guid Editor = new Guid("22222222-2222-2222-2222-222222222222");
    private static readonly Guid Checker = new Guid("33333333-3333-3333-3333-333333333333");

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
