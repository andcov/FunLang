namespace FunLang;

internal class Program
{
	private static void Main(string[] args)
	{
        FunLang program = new(
"""
(
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

println map $ lambda x => (* 5 x) filter $lambda x => (== 0 % x 2) (1 2 3 4 5 6)
)
""");

		try
		{
			program.Evaluate();
		}
		catch (InvalidFunProgram e)
		{
			Console.WriteLine(e.Message);
            Console.WriteLine(program.ReportErrorFromToken(e.tok));
		}
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

    public List<Token> Tokenise()
    {
        List<Token> t_list = new();

		var split_code = code.Replace("(", " ( ")
			.Replace(")", " ) ")
			.Replace("=>", " => ")
            .Replace("$", " $ ")
			.Replace("/*", " /* ")
			.Replace("*/", " */ ")
            .Split(new char[0]).ToList();

		for (int i = 0; i < split_code.Count - 1; ++i)
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

		for (int i = 0; i < split_code.Count; ++i)
		{
			if (split_code[i] != "" && split_code[i][0] == '"')
			{
                while (split_code[i][^1] != '"')
				{
					if(i + 1 >= split_code.Count)
					{
						throw new InvalidFunProgram("Cannot find the end of the string", null);
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

        return RemoveComments(t_list);
	}

    private static List<Token> RemoveComments(List<Token> toks)
    {
		int cnt = 0;
        List<Token> res = new();

        for (int i = 0; i < toks.Count; ++i)
		{
			if (toks[i].token == "/*")
			{
				cnt++;
			}

			if (cnt == 0)
			{
				res.Add((Token)toks[i].Clone());
			}

            if (toks[i].token == "*/")
            {
                cnt--;
                if (cnt < 0)
                {
                    throw new InvalidFunProgram("Unexpected end of comment", toks[i]);
                }
            }
        }

		if (cnt > 0)
		{
            throw new InvalidFunProgram("Comment block not closed", null);
        }

		return res;
	}

    public IExpression Parse() => ReadFromTokens(this.Tokenise());

    private IExpression ReadFromTokens(List<Token> tokens)
    {
        if (tokens.Count == 0)
		{
			throw new InvalidFunProgram("Unexpected EOF Token", null);
		}

		var token = tokens.First();
		tokens.RemoveAt(0);

		if (token.Value() == "(")
		{
            FList list = new(token);

			while (tokens[0].Value() != ")")
			{
				var val = ReadFromTokens(tokens);
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
			throw new InvalidFunProgram("Unexpected ')'", token);
		}
        else if (token.Value() == "lambda")
		{
			var parameters = new List<FSymbol>();

			var param = ReadFromTokens(tokens);
			if (param.GetFType() != FType.FSymbol)
			{
				if (param.GetFType() != FType.FList)
				{
					throw new InvalidFunProgram("Function parameters can only be symbols", token);
				}
				var l = (FList)param;
				if (!l.IsListOf(FType.FSymbol))
				{
					throw new InvalidFunProgram("Function parameters can only be symbols", token);
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
				throw new InvalidFunProgram("Unknown token; expected '=>'", arrow_token);
			}

			var body = ReadFromTokens(tokens);
			if (body.GetFType() != FType.FList)
			{
				throw new InvalidFunProgram("The body of a function should be surrounded by parentheses", body.Tok);
			}

			return new FFunction(parameters, body, token);
		}
		else if (token.Value() == "define")
		{
			var to_def = ReadFromTokens(tokens);
			var vals = ReadFromTokens(tokens);
			if (to_def.GetFType() == FType.FSymbol)
			{
				return new FDefine((FSymbol)to_def, vals, token);
			}
			else if (to_def.GetFType() == FType.FList && ((FList)to_def).IsListOf(FType.FSymbol))
			{
				return new FDefine((FList)to_def, vals, token);
			}
			else
			{
				throw new InvalidFunProgram("The value to be defined can only be a symbol or a list of symbols", vals.Tok);
			}
		}
		else if (token.Value() == "if")
		{
			var curr_tok = (Token)token.Clone();
			var cond = ReadFromTokens(tokens);

			var then_token = tokens.First();
			tokens.RemoveAt(0);
			if(then_token.Value() != "then")
			{
				throw new InvalidFunProgram("Unknown token; expected 'then'", then_token);
			}
			var then = ReadFromTokens(tokens);

			var else_token = tokens.First();
			tokens.RemoveAt(0);
			if(else_token.Value() != "else")
			{
				throw new InvalidFunProgram("Unknown token; expected 'else'", else_token);
			}
			var other = ReadFromTokens(tokens);

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

    public IExpression Evaluate()
    {
		var env = new Env();
		env.AddStandard();
		return Parse().Eval(env);
	}

	public string ReportErrorFromToken(Token tok) {
		var split_code = code.Split('\n').ToList();
		for (int i = 0; i < split_code.Count - 1; ++i)
		{
			if (split_code[i] != "")
			{
				split_code.Insert(i + 1, "");

			}
		}
		int next_pos = 0;
		int line_number = 0;
		foreach (var l in split_code)
		{
			int position = next_pos;
			if (l.Length == 0)
			{
				next_pos += 1;
			}
			else
			{
				next_pos += l.Length;
                line_number += 1;
                if (tok.position >= position && tok.position < next_pos)
				{
					var line_string = line_number.ToString() + ": ";
					return (line_string + l + "\n" + (new string(' ', tok.position - position + line_string.Length)) + "^");
				}
			}
		}
		return "";
	}
}
