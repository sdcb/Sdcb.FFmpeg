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
	.Dump()
	.ToArray();

Console.WriteLine($"Total={data.Length}, Parsed={data.Count(x => x.MyParsed != null)}");
Parse(parser, "FFERRTAG( 'P','A','W','E')").Dump();


FSharpFunc<FParsec.CharStream<Unit>, FParsec.Reply<Expression>> MakeParser()
{
	HashSet<string> typeLiterals = "int64_t,UINT64_C".Split(',').OrderByDescending(x => x.Length).ToHashSet();
	FSharpFunc<FParsec.CharStream<Unit>, FParsec.Reply<string>> notReserved(string id) => typeLiterals.Contains(id) ? Zero<string>() : Return(id);
	var identifier1 = Choice(Letter, CharP('_'));
	var identifierRest = Choice(Letter, CharP('_'), Digit);
	var identifier = Purify(Many1Chars(identifier1, identifierRest)).AndTry(notReserved).Lbl("identifier");
	
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
		.WithImplicitOperator(50, (e1, e2) => ImplicitExpression.FromBinary(e1, e2))
		.WithTerms((FSharpFunc<FParsec.CharStream<Unit>, FParsec.Reply<Expression>> term) =>
		{
			var typeSyntax = Choice(typeLiterals.Select(t => StringP(t)).ToArray()).And(WS);
			var parenthese1 = Between(CharP('(').And(WS), term, CharP(')').And(WS));
			var parentheseN = Between(CharP('(').And(WS), Many(term, CharP(',').And(WS)), CharP(')').And(WS));
			return PrimitivesCS.Choice(
				Try(Between(CharP('(').And(WS), typeSyntax, CharP(')'))).And(term).And(WS).Map((id, val) => (Expression)new TypeConvertExpression(id, val)), 
				Between('\'', Letter, '\'').And(WS).Map(x => (Expression)new CharLiteralExpression(x)),
				Between('"', ManyChars(NoneOf("\"")), '"').And(WS).Map(x => (Expression)new StringLiteralExpression(x)),
				NumberLiteral(NumberLiteralOptions.AllowSuffix | NumberLiteralOptions.AllowHexadecimal | NumberLiteralOptions.DefaultFloat, "Number").And(WS).Map(x => (Expression)new NumberLiteralExpression(x)),
				parentheseN.Map(x => (Expression)new ParentheseExpression(x.ToArray())),
				typeSyntax.And(parenthese1).Map((id, val) => (Expression)new TypeConvertExpression(id, val)), 
				//identifier.And(parentheseN).Map((id, args) => (Expression)new FunctionCallExpression(id, args.ToArray())),
				identifier.Map(x => (Expression)new IdentifierExpression(x)) 
			).Label("expression");
		})
		.Build()
		.ExpressionParser;
}

string Parse(FSharpFunc<FParsec.CharStream<Unit>, FParsec.Reply<Expression>> parser, string raw)
{
	string inline = raw.Replace("\\\n", "");
	return parser.ParseString(inline) switch
	{
		{ Status: FParsec.ReplyStatus.Ok } x => x.Result.Serialize(), 
		//var x => throw new Exception(string.Join(",", FParsec.ErrorMessageList.ToHashSet(x.Error).Select(x => x.Type.ToString())) + $"\nraw: {inline}"), 
		var x => string.Join(",", FParsec.ErrorMessageList.ToHashSet(x.Error).Select(x => x.Type.ToString())) + $", raw: {inline}", 
		//_ => null, 
	};
}

abstract record Token
{
	public abstract string Serialize();
}
abstract record Expression : Token
{
}

record NumberLiteralExpression(NumberLiteral number) : Expression
{
	public override string Serialize()
	{
		return number.String;
	}
}

record StringLiteralExpression(string c) : Expression
{
	public override string Serialize()
	{
		return $"\"{c}\"";
	}
}

record CharLiteralExpression(char c) : Expression
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

record ParentheseExpression(Expression[] contents) : Expression
{
	public override string Serialize() => $"({string.Join(", ", contents.Select(x => x.Serialize()))})";
}

static class ImplicitExpression
{
	public static Expression FromBinary(Expression left, Expression right)
	{
		// UINT64_C(0x8000000000000000)
		return (left, right) switch
		{
			//(IdentifierExpression { IsType: true } id, ParentheseExpression { contents.Length: 1 } parenthese) => new TypeConvertExpression(id.identifier, parenthese.contents[0]),
			(IdentifierExpression id, ParentheseExpression parenthese) => new FunctionCallExpression(id.identifier, parenthese.contents),
			//(ParentheseExpression { contents: [IdentifierExpression { IsType: true } id] }, ParentheseExpression { contents: [Expression val] }) => new TypeConvertExpression(id.identifier, val),
			//(ParentheseExpression { contents: [IdentifierExpression { IsType: true } id] }, Expression val) => new TypeConvertExpression(id.identifier, val),
			_ => left, 
			//_ => throw new NotSupportedException($"left {left.GetType().Name}({left.Serialize()}), right: {right.GetType().Name}({left.Serialize()}) is not supported"), 
		};
	}
}

record FunctionCallExpression(string identifier, Expression[] arguments) : Expression
{
	public override string Serialize() => $"{identifier}({string.Join(", ", arguments.Select(x => x.Serialize()))})";
}

record TypeConvertExpression(string destType, Expression value) : Expression
{
	public override string Serialize() => $"({destType}){value.Serialize()}";
}

record MacroResult
{
	public string Name { get; init; }
	public bool Valid { get; init; }
	public string Expression { get; init; }
	public string Raw { get; init; }
	public string Exception { get; init; }
}