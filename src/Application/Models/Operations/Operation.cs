using SourceKit.Generators.Builder.Annotations;

namespace Models.Operations;

[GenerateBuilder]
public partial record Operation(DateTime Date,
    long AccountId,
    decimal? AmountBefore,
    decimal? AmountAfter,
    OperationType Type);