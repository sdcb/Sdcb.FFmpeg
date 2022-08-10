<Query Kind="Statements">
  <NuGetReference>FParsec.CSharp</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>FParsec.CSharp; // extension functions (combinators &amp; helpers)</Namespace>
  <Namespace>Microsoft.FSharp.Core</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>static FParsec.CharParsers</Namespace>
  <Namespace>static FParsec.CSharp.CharParsersCS; // pre-defined parsers</Namespace>
  <Namespace>static FParsec.CSharp.PrimitivesCS; // combinator functions</Namespace>
</Query>

var parser = MakeParser();
var data = JsonConvert.DeserializeObject<MacroResult[]>(File.ReadAllText(@"C:\Users\sdfly\source\repos\FFmpeg.AutoGen\src\Sdcb.FFmpeg.AutoGen\bin\Debug\net6\macro.json"))
	.Where(x => x.Valid)
	.Take(50)
	.Select(x => new
	{
		Name = x.Name,
		x.Expression,
		MyParsed = Parse(parser, x.Raw),
		Raw = x.Raw,
	})
	.ToArray();

Console.WriteLine($"Total={data.Length}, Parsed={data.Count(x => x.MyParsed != null)}");
data.Dump();

FSharpFunc<FParsec.CharStream<Unit>, FParsec.Reply<Expression>> MakeParser()
{
	var id = Many1Chars(Choice(Letter, CharP('_')), Choice(Letter, Digit, CharP('_'))).And(WS);
	return new OPPBuilder<Unit, Expression, Unit>()
		.WithOperators(ops => ops
			.AddInfix(">", 10, WS, (x, y) => new BinaryExpression(x, new Operator(">"), y))
			.AddInfix("<", 10, WS, (x, y) => new BinaryExpression(x, new Operator("<"), y))
			.AddInfix("<<", 20, WS, (x, y) => new BinaryExpression(x, new Operator("<<"), y))
			.AddInfix(">>", 20, WS, (x, y) => new BinaryExpression(x, new Operator(">>"), y))
			.AddInfix("+", 30, WS, (x, y) => new BinaryExpression(x, new Operator("+"), y))
			.AddInfix("-", 30, WS, (x, y) => new BinaryExpression(x, new Operator("-"), y))
			.AddInfix("*", 40, WS, (x, y) => new BinaryExpression(x, new Operator("*"), y))
			.AddInfix("/", 40, WS, (x, y) => new BinaryExpression(x, new Operator("/"), y))
			.AddPrefix("-", 40, WS, (x) => new NegativeExpression(x)))
		.WithTerms(term => PrimitivesCS.Choice(
			Between('\'', Letter, '\'').And(WS).Map(x => (Expression)new ConstCharExpression(x)), 
			Between('"', ManyChars(NoneOf("\"")), '"').And(WS).Map(x => (Expression)new ConstStringExpression(x)), 
			NumberLiteral(NumberLiteralOptions.AllowSuffix, "Number").And(WS).Map(x => (Expression)new ConstNumberExpression(x)),
			Between(CharP('(').And(WS), term, CharP(')').And(WS)).Map(x => (Expression)new ParentheseExpression(x)), 
			Between(id.And(CharP('(')).And(WS), term.And(Many(CharP(',').And(term))), CharP(')').And(WS)).Map(x => (Expression)new ParentheseExpression(x)), 
			//id.And(Between(CharP('(').And(WS), term, CharP(')'))),
			id.Map(x => (Expression)new IdentifierExpression(x)) 
		))
		.Build()
		.ExpressionParser;
}

string Parse(FSharpFunc<FParsec.CharStream<Unit>, FParsec.Reply<Expression>> parser, string expression)
{
	return parser.ParseString(expression) switch
	{
		var x when x.Status == FParsec.ReplyStatus.Ok => x.Result.Serialize(), 
		_ => null
	};
}

abstract record Token
{
	public abstract string Serialize();
}
abstract record Expression : Token
{
}

record ConstNumberExpression(NumberLiteral number) : Expression
{
	public override string Serialize()
	{
		return number.String;
	}
}

record ConstStringExpression(string c) : Expression
{
	public override string Serialize()
	{
		return $"\"{c}\"";
	}
}

record ConstCharExpression(char c) : Expression
{
	public override string Serialize()
	{
		return $"'{c}'";
	}
}

record Operator(string op) : Token
{
	public override string Serialize() => op;
}

record NegativeExpression(Expression val) : Expression
{
	public override string Serialize()
	{
		return $"-{val.Serialize()}";
	}
}

record IdentifierExpression(string identifier) : Expression
{
	public override string Serialize()
	{
		return identifier;
	}
}

record BinaryExpression(Expression left, Operator op, Expression right) : Expression
{
	public override string Serialize()
	{
		return $"{left.Serialize()} {op.Serialize()} {right.Serialize()}";
	}
}

record ParentheseExpression(Expression content) : Expression
{
	public override string Serialize() => $"({content.Serialize()})";
}

record MacroResult
{
	public string Name { get; init; }
	public bool Valid { get; init; }
	public string Expression { get; init; }
	public string Raw { get; init; }
	public string Exception { get; init; }
}