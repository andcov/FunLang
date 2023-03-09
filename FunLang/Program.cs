using System.Xml.Linq;
/*using static System.Runtime.InteropServices.JavaScript.JSType;*/

namespace FunLang;

/*

define map lambda (f l) => (
if l
(
	define r (map (f) (rest l))
	push (f first l) r
)
(
	()
)
)

map (lambda x => (+ 5 x)) (1 2 3)

*/

internal class Program
{
	private static void Main(string[] args)
	{
		var program = new FunLang(
"""
(

define map lambda (f l) => (
	if l
	(
		(push f first l map (f) rest l)
	)
	(
		()
	)
)

define filter lambda (f l) => (
	if l
	(
		if (f first l)
		(
			(push first l filter (f) rest l)
		)
		(
			(filter (f) rest l)
		)
	)
	(
		()
	)
)

println map (lambda x => (* x 2)) filter (lambda x => (== x 5)) (1 2 5 3 4 5 1 2 3 4 5 6 7 8 5 5)

)
""");

        Console.WriteLine("Par: " + program.parse());
        Console.WriteLine("Output: ");
        var res = program.evaluate();
        Console.WriteLine("Result:\n" + res);
    }
}

public class FunLang {
	public string code;

	public FunLang(string _code)
	{
		code = _code;
	}

	public List<Token> tokenise() {
		var t_list = new List<Token>();

		var split_code = code.Replace("(", " ( ")
			.Replace(")", " ) ")
			.Replace("=>", " => ")
			.Split(new char[0]).ToList();

		for (int i = 0; i < split_code.Count() - 1; ++i)
		{
			if (split_code[i] != "")
			{
				split_code.Insert(i + 1, "");

			}
			if (split_code[i] == "(" || split_code[i] == ")" || split_code[i] == "=>")
			{
				if (split_code[i - 1] == "")
				{
					split_code.RemoveAt(i - 1);
					--i;
				}
				if (split_code[i + 1] == "")
					split_code.RemoveAt(i + 1);

			}
		}

		for (int i = 0; i < split_code.Count(); ++i)
		{
			if (split_code[i] != "" && split_code[i][0] == '"')
			{
				while(split_code[i][split_code[i].Length - 1] != '"')
				{
					if(i + 1 >= split_code.Count())
					{
						throw new InvalidOperationException("Cannot find the end of the string");
					}
					if (split_code[i + 1] == "")
					{
						split_code[i] += " ";
					} else
					{
						split_code[i] += split_code[i + 1];
					}
					split_code.RemoveAt(i + 1);
				}
			}
		}

			int position = 0;
		foreach(var l in split_code)
		{
			if(l.Length == 0)
			{
				position += 1;
			} else
			{
				t_list = t_list.Append(new Token(l, position)).ToList();
				position += l.Length;
			}
		}

		return t_list;
	}

	public Expression parse() {
		return read_from_tokens(tokenise());
	}

	private Expression read_from_tokens(List<Token> tokens) {
		if(tokens.Count() == 0)
		{
			throw new InvalidOperationException("Unexpected EOF Token");
		}

		var token = tokens.First();
		tokens.RemoveAt(0);

		if (token.Value() == "(")
		{
			FList list = new FList(token);

			while (tokens[0].Value() != ")")
			{
				list.Add(read_from_tokens(tokens));
			}

			tokens.RemoveAt(0); // remove last ')'

			return list;
		}
		else if (token.Value() == ")")
		{
			throw new InvalidOperationException("Unexpected ')'");
		}
		else if (token.Value() == "lambda")
		{
			var parameters = new List<FSymbol>();

			var param = read_from_tokens(tokens);
			if (param.GetFType() != FType.FSymbol)
			{
				if (param.GetFType() != FType.FList)
				{
					throw new InvalidOperationException("Function parameters can only be symbols");
				}
				var l = (FList)param;
				if (!l.isListOf(FType.FSymbol))
				{
					throw new InvalidOperationException("Function parameters can only be symbols");
				}
				var sym_param = new List<FSymbol>();
				foreach (var sym in l)
				{
					sym_param.Add((FSymbol)sym);
				}
				parameters = sym_param;
			}
			else
			{
				parameters.Add((FSymbol)param);
			}

			var arrow_token = tokens.First();
			tokens.RemoveAt(0);
			if(arrow_token.Value() != "=>")
			{
				throw new InvalidOperationException("Unknown token; expected '=>'");
			}

			var body = read_from_tokens(tokens);
			if (body.GetFType() != FType.FList)
			{
				throw new InvalidOperationException("The body of a function should be surrounded by parentheses");
			}

			return new FFunction(parameters, body, token);
		}
		else if (token.Value() == "define")
		{
			var to_def = read_from_tokens(tokens);
			var vals = read_from_tokens(tokens);
			if (to_def.GetFType() == FType.FSymbol)
			{
				return new FDefine((FSymbol)to_def, vals, token);
			}
			else if (to_def.GetFType() == FType.FList && ((FList)to_def).isListOf(FType.FSymbol))
			{
				return new FDefine((FList)to_def, vals, token);
			}
			else
			{
				throw new InvalidOperationException("The value to be defined can only be a symbol or a list of symbols");
			}
		}
		else if (token.Value() == "if")
		{
			var curr_tok = (Token)token.Clone();
			var cond = read_from_tokens(tokens);
			var then = read_from_tokens(tokens);
			var other = read_from_tokens(tokens);
			return new FIf(cond, then, other, curr_tok);
		}
		else // a number, a char, a string or a symbol
		{

			var isInt = int.TryParse(token.Value(), out int i);
			var isFloat = float.TryParse(token.Value(), out float f);

			if (isInt)
			{
				return new FNumber(i, token);
			}
			else if (isFloat)
			{
				return new FNumber(f, token);
			}
			else if (token.Value()[0] == '\'')
			{
				//TODO: add support for chars such as \n
				return new FChar(token.Value()[1], token);
			}
			else if (token.Value()[0] == '"')
			{
				var str_list = new FList(token);
				for(int j = 1; j < token.Value().Length - 1; ++j)
				{
					str_list.Add(new FChar(token.Value()[j], token));
				}
				
				return str_list;
			}
			else
			{
				return new FSymbol(token.Value(), token);
			}
		}
	}

	public Expression evaluate() {
		var env = new Env();
		env.AddStandard();
		return parse().eval(env);
	}

	public string ReportErrorFromToken(Token tok) {
		var split_code = code.Split('\n').ToList();
		for (int i = 0; i < split_code.Count() - 1; ++i)
		{
			if (split_code[i] != "")
			{
				split_code.Insert(i + 1, "");

			}
		}
		int position = 0, next_pos = 0;
		foreach (var l in split_code)
		{
			position = next_pos;
			if (l.Length == 0)
			{
				next_pos += 1;
			}
			else
			{
				next_pos += l.Length;
				if(tok.position >= position && tok.position < next_pos)
				{
					if(tok.position == null)
					{
						throw new InvalidOperationException("Tried to use null token");
					}
					return (l + "\n" + (new string(' ', tok.position.Value - position)) + "^");
				}
			}
		}
		return "";
	}
}

public class Token : ICloneable
{
	public string? token = null;
	public int? position = null;

	public Token()
	{
		token = null;
		position = null;
	}

	public Token(string? _token, int? _position) {
		token = _token;
		position = _position;
	}

	public string Value() {
		if(token != null)
		{
			return token;
		}
		return "";
	}

	public override string ToString()
	{
		return $"<{token}, {position}>";
	}

	public object Clone()
	{
		return new Token(token, position);
	}
}
