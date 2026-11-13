# Dloizides.Approvals

Maker-checker (four-eyes) approval rule for any record a second person must approve.

```bash
dotnet add package Dloizides.Approvals
```

## Rule

`MakerCheckerPolicy.GuardCanApprove(subject, approverId)` refuses:

| Case | Throws |
|---|---|
| The record has no known contributors (fail closed) | `InvalidOperationException` |
| The approver created or edited the record | `MakerCheckerViolationException` |
| The approver already approved it | `MakerCheckerViolationException` |

"Contributed", not "created": if A creates a record and B edits it, B cannot approve it.

## Threshold

```csharp
var threshold = new ApprovalThreshold(limit: 10_000m, approvalsAtOrBelowLimit: 0, approvalsAboveLimit: 2);
var required = MakerCheckerPolicy.RequiredApprovals(amount, threshold);
MakerCheckerPolicy.GuardCanApprove(new ApprovalSubject(reference, contributors, approvers), approverId);
var done = MakerCheckerPolicy.IsFullyApproved(subjectAfterApproval, required);
```

The value is compared by magnitude, so a negative amount above the limit needs the higher count.
With no threshold, one approval is required.

Each aggregate keeps its own contributor and approver sets and hands them to the policy;
the rule is shared, the storage is not.
