using System;
namespace FunLang
{
	public class FDefine : IExpression
	{
		public Token? Tok { get; set; } = null;
		public FSymbol? sym;
		public FList? list;
		public IExpression vals;

		public FDefine(FSymbol _sym, IExpression _vals, Token? _tok)
		{
			sym = _sym;
			list = null;
			vals = _vals;
			Tok = _tok;
		}

		public FDefine(FList _list, IExpression _vals, Token? _tok)
		{
			sym = null;
			list = _list;
			vals = _vals;
			Tok = _tok;
		}

		public IExpression Eval(Env env)
		{
            var eval_vals = vals.Eval(env);
            if (sym != null)
			{
                env[sym.name] = eval_vals;
			}
			else if (list != null)
			{
				if (eval_vals.GetFType() == FType.FList && ((FList)eval_vals).Count == list.Count)
				{
                    var list_vals = (FList)eval_vals;
                    for (int i = 0; i < list.Count; ++i)
                    {
                        var sym = (FSymbol)list[i];
                        env[sym.name] = list_vals[i];
                    }
                } else
				{
                    throw new InvalidFunProgram("Could not create list out of right side of definition", vals.Tok);
                }
			}

			return new FNumber(-1000, null);
		}

		public override string ToString()
		{
			var res = "Def ";
			if (sym != null)
			{
				res += sym + " <- " + vals;
			}
			else
			{
				res += list + " <- " + vals;
			}
			return res;
		}
		public override bool Equals(Object? obj)
		{
            if (obj == null || obj is not IExpression)
            {
                return false;
            }
            var exp = (IExpression)obj;

            if (exp.GetFType() != FType.FDefine) return false;
            var otherDef = (FDefine)exp;
			if (sym != null)
			{
				return sym.Equals(otherDef.sym) && vals.Equals(otherDef.vals);
			}
			else if (list != null)
			{
                return list.Equals(otherDef.list) && vals.Equals(otherDef.vals);
            }
			return false;
        }
        public int Compare(IExpression exp) => (this.Equals(exp)) ? 0 : -1;
        public bool IsTrue() => throw new InvalidFunProgram("Cannot evaluate truth value of FDefine.", Tok);

        public object Clone()
		{
			if (sym != null)
			{
				return new FDefine((FSymbol)sym.Clone(), (IExpression)vals.Clone(), (Token)Tok.Clone());
			}
			else
			{
				return new FDefine((FList)list.Clone(), (IExpression)vals.Clone(), (Token)Tok.Clone());
			}
		}
		public FType GetFType() => FType.FDefine;
	}
}
