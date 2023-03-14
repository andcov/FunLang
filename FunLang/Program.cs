namespace FunLang;

internal class Program
{
	private static void Main(string[] args)
	{
		var program = new FunLang(
"""
(
<<<<<<< HEAD
define map lambda (f l) => (
	if l then (
		(push f first l map $f rest l)
	) else (
		()
	)
)

define filter lambda (f l) => (
	if l then (
		if (f first l) then (
			(push first l filter $f rest l)
		) else (
			(filter $f rest l)
		)
	) else (
		()
	)
)

println map $lambda x => (+ 5 x) filter $lambda x => (== 0 % x 2) (1 2 5 3 4 5 1 2 3 4 5 6 7 8 5 5 24)
=======
define map lambda (f l) => (if l then ((push f first l map $f rest l)) else (())) define filter lambda (f l) => (if l then (if (f first l) then ((push first l filter $f rest l)) else ((filter $f rest l))) else (()))define sum lambda l => (if l then (+ first l sum rest l)else (0))

println map $lambda x => (* 8 x) filter $lambda x => (== 0 % x 2) (1 2 3 4 5 6.7)
>>>>>>> 898b71b (Rename functionator symbol)
)
""");

        Console.WriteLine("Parse: " + program.parse());
        //Console.WriteLine("Output: ");
        //var res = program.evaluate();
        //Console.WriteLine("Result:\n" + res);
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
            .Replace("$", " $ ")
			.Replace("/*", " /* ")
			.Replace("*/", " */ ")
            .Split(new char[0]).ToList();

		for (int i = 0; i < split_code.Count() - 1; ++i)
		{
			if (split_code[i] != "")
			{
				split_code.Insert(i + 1, "");

			}
			if (split_code[i] == "(" ||
				split_code[i] == ")" ||
				split_code[i] == "=>" ||
				split_code[i] == "$" ||
                split_code[i] == "/*" ||
                split_code[i] == "*/")
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
				var val = read_from_tokens(tokens);
				if (val != null)
				{
					list.Add(val);
				}
			}

			tokens.RemoveAt(0); // remove ')'

			return list;
		}
		else if (token.Value() == ")")
		{
			throw new InvalidOperationException("Unexpected ')'");
		}
		else if (token.Value() == "/*")
        {
            while (tokens[0].Value() != "*/")
            {
				read_from_tokens(tokens);
            }

            tokens.RemoveAt(0); // remove '*/'

            return null;
        }
        else if (token.Value() == "*/")
        {
            throw new InvalidOperationException("Unexpected '*/'");
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

			var then_token = tokens.First();
			tokens.RemoveAt(0);
			if(then_token.Value() != "then")
			{
				throw new InvalidOperationException("Unknown token; expected 'then'");
			}
			var then = read_from_tokens(tokens);

			var else_token = tokens.First();
			tokens.RemoveAt(0);
			if(else_token.Value() != "else")
			{
				throw new InvalidOperationException("Unknown token; expected 'else'");
			}
			var other = read_from_tokens(tokens);

			return new FIf(cond, then, other, curr_tok);
		}
		else if (token.Value() == "$")
		{
			return new FFunctionator(token);
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
