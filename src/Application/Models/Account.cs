using SourceKit.Generators.Builder.Annotations;

namespace Models;

[GenerateBuilder]
public partial record Account(long Id, string Password, decimal TotalMoney);