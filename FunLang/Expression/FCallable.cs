namespace FunLang
{
	public abstract class FCallable : IExpression
	{
        public List<FSymbol> parameters = new();
		public Env env = new();
		public bool isClosure;
		public Token? Tok { get; set; } = null;
		

		public abstract IExpression Eval(Env env);

		public int ParamCount() { return parameters.Count; }
		
		public bool Equals(IExpression exp) { return false; }

		public abstract object Clone();
		public FType GetFType() { return FType.FCallable; }
	}

	public class Add : FCallable
	{
		public Add()
		{
			var x = new FSymbol("__x__");
			var y = new FSymbol("__y__");
			isClosure = false;

			parameters.Add(x);
			parameters.Add(y);
		}

		public override IExpression Eval(Env env)
		{
			var arg1 = env[parameters[0].name];
			var arg2 = env[parameters[1].name];

			if (arg1.GetFType() == FType.FChar)
			{
				var num = new FNumber(((FChar)arg1).ch, null);
				arg1 = num;
			}
            if (arg2.GetFType() == FType.FChar)
            {
                var num = new FNumber(((FChar)arg2).ch, null);
                arg2 = num;
            }

            if (arg1.GetFType() == FType.FNumber && arg2.GetFType() == FType.FNumber)
            {
                FNumber v1 = (FNumber)arg1;
                FNumber v2 = (FNumber)arg2;
                if (v1.i.HasValue && v2.i.HasValue)
                {
                    return new FNumber(v1.i.Value + v2.i.Value, null);
                }
                else
                {
                    float f1 = (v1.i.HasValue) ? (float)v1.i : v1.f.Value;
                    float f2 = (v2.i.HasValue) ? (float)v2.i : v2.f.Value;
                    return new FNumber(f1 + f2, null);
                }
            }

            throw new InvalidFunProgram($"Cannot add {arg1.GetFType()} and {arg2.GetFType()}", Tok);
        }

		public override object Clone()
		{
			return new Add();
		}
	}

	public class Substract : FCallable
	{
		public Substract()
		{
			var x = new FSymbol("__x__");
			var y = new FSymbol("__y__");
			isClosure = false;

			parameters.Add(x);
			parameters.Add(y);
		}

		public override IExpression Eval(Env env)
		{
            var arg1 = env[parameters[0].name];
            var arg2 = env[parameters[1].name];

            if (arg1.GetFType() == FType.FChar)
            {
                var num = new FNumber(((FChar)arg1).ch, null);
                arg1 = num;
            }
            if (arg2.GetFType() == FType.FChar)
            {
                var num = new FNumber(((FChar)arg2).ch, null);
                arg2 = num;
            }

            if (arg1.GetFType() == FType.FNumber && arg2.GetFType() == FType.FNumber)
            {
                FNumber v1 = (FNumber)arg1;
                FNumber v2 = (FNumber)arg2;
                if (v1.i.HasValue && v2.i.HasValue)
                {
                    return new FNumber(v1.i.Value - v2.i.Value, null);
                }
                else
                {
                    float f1 = (v1.i.HasValue) ? (float)v1.i : v1.f.Value;
                    float f2 = (v2.i.HasValue) ? (float)v2.i : v2.f.Value;
                    return new FNumber(f1 - f2, null);
                }
            }

            throw new InvalidFunProgram($"Cannot subtract {arg1.GetFType()} from {arg2.GetFType()}", Tok);

        }

        public override object Clone()
		{
			return new Substract();
		}
	}

	public class Multiply : FCallable
	{
		public Multiply()
		{
			var x = new FSymbol("__x__");
			var y = new FSymbol("__y__");
			isClosure = false;

			parameters.Add(x);
			parameters.Add(y);
		}

		public override IExpression Eval(Env env)
		{
            var arg1 = env[parameters[0].name];
            var arg2 = env[parameters[1].name];

            if (arg1.GetFType() == FType.FChar)
            {
                var num = new FNumber(((FChar)arg1).ch, null);
                arg1 = num;
            }
            if (arg2.GetFType() == FType.FChar)
            {
                var num = new FNumber(((FChar)arg2).ch, null);
                arg2 = num;
            }

            if (arg1.GetFType() == FType.FNumber && arg2.GetFType() == FType.FNumber)
            {
                FNumber v1 = (FNumber)arg1;
                FNumber v2 = (FNumber)arg2;
                if (v1.i.HasValue && v2.i.HasValue)
                {
                    return new FNumber(v1.i.Value * v2.i.Value, null);
                }
                else
                {
                    float f1 = (v1.i.HasValue) ? (float)v1.i : v1.f.Value;
                    float f2 = (v2.i.HasValue) ? (float)v2.i : v2.f.Value;
                    return new FNumber(f1 * f2, null);
                }
            }

            throw new InvalidFunProgram($"Cannot multiply {arg1.GetFType()} and {arg2.GetFType()}", Tok);

        }

        public override object Clone()
		{
			return new Multiply();
		}
	}

    public class Divide : FCallable
    {
        public Divide()
        {
            var x = new FSymbol("__x__");
            var y = new FSymbol("__y__");
            isClosure = false;

            parameters.Add(x);
            parameters.Add(y);
        }

        public override IExpression Eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            var arg2 = env[parameters[1].name];

            if (arg1.GetFType() == FType.FChar)
            {
                var num = new FNumber(((FChar)arg1).ch, null);
                arg1 = num;
            }
            if (arg2.GetFType() == FType.FChar)
            {
                var num = new FNumber(((FChar)arg2).ch, null);
                arg2 = num;
            }

            if (arg1.GetFType() == FType.FNumber && arg2.GetFType() == FType.FNumber)
            {
                FNumber v1 = (FNumber)arg1;
                FNumber v2 = (FNumber)arg2;
                
                float f1 = (v1.i.HasValue) ? (float)v1.i : v1.f.Value;
                float f2 = (v2.i.HasValue) ? (float)v2.i : v2.f.Value;
                return new FNumber(f1 / f2, null);
            }

            throw new InvalidFunProgram($"Cannot divide {arg1.GetFType()} by {arg2.GetFType()}", Tok);

        }

        public override object Clone()
        {
            return new Divide();
        }
    }

    public class Mod : FCallable
    {
        public Mod()
        {
            var x = new FSymbol("__x__");
            var y = new FSymbol("__y__");
            isClosure = false;

            parameters.Add(x);
            parameters.Add(y);
        }

        public override IExpression Eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            var arg2 = env[parameters[1].name];
			if(arg1.GetFType() != FType.FNumber || arg2.GetFType() != FType.FNumber)
			{
                throw new InvalidFunProgram("Can only take modulo out of integers", Tok);
            }
			var num1 = (FNumber)arg1;
            var num2 = (FNumber)arg2;

            if (num1.i == null || num2.i == null)
            {
                throw new InvalidFunProgram("Can only take modulo out of integers", Tok);
            }

            return new FNumber(num1.i.Value % num2.i.Value, null);
        }

        public override object Clone()
        {
            return new Mod();
        }
    }

    public class Equal : FCallable
    {
        public Equal()
        {
            var x = new FSymbol("__x__");
            var y = new FSymbol("__y__");
            isClosure = false;

            parameters.Add(x);
            parameters.Add(y);
        }

        public override IExpression Eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            var arg2 = env[parameters[1].name];
            if (arg1.Equals(arg2))
                return new FNumber(1, null);
            else
                return new FNumber(0, null);
        }

        public override object Clone()
        {
            return new Equal();
        }
    }

    public class Different : FCallable
    {
        public Different()
        {
            var x = new FSymbol("__x__");
            var y = new FSymbol("__y__");
            isClosure = false;

            parameters.Add(x);
            parameters.Add(y);
        }

        public override IExpression Eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            var arg2 = env[parameters[1].name];
            if (!arg1.Equals(arg2))
                return new FNumber(1, null);
            else
                return new FNumber(0, null);
        }

        public override object Clone()
        {
            return new Different();
        }
    }


    public class Length : FCallable
	{
		public Length()
		{
			var l = new FSymbol("__l__");
			isClosure = false;

			parameters.Add(l);
		}

		public override IExpression Eval(Env env)
		{
			var arg1 = env[parameters[0].name];
			if (arg1.GetFType() == FType.FList)
				return new FNumber(((FList)arg1).Count(), null);
			else
				throw new InvalidFunProgram("Can only measure length of list", Tok);
		}

		public override object Clone()
		{
			return new Length();
		}
	}

	public class First : FCallable
	{
		public First()
		{
			var l = new FSymbol("__l__");
			isClosure = false;

			parameters.Add(l);
		}

		public override IExpression Eval(Env env)
		{
			var arg1 = env[parameters[0].name];
			if (arg1.GetFType() == FType.FList) {
				var l = (FList)arg1;
				if (l.Count >= 1)
					return l[0];
				else
					throw new InvalidFunProgram("List too short", Tok);
			} else
				throw new InvalidFunProgram("Can only take first element of list", Tok);
		}

		public override object Clone()
		{
			return new First();
		}
	}

	public class Second : FCallable
	{
		public Second()
		{
			var l = new FSymbol("__l__");
			isClosure = false;

			parameters.Add(l);
		}

		public override IExpression Eval(Env env)
		{
			var arg1 = env[parameters[0].name];
			if (arg1.GetFType() == FType.FList)
			{
				var l = (FList)arg1;
				if (l.Count >= 2)
					return l[1];
				else
					throw new InvalidFunProgram("List too short", Tok);
			}
			else
				throw new InvalidFunProgram("Can only take second element of list", Tok);
		}

		public override object Clone()
		{
			return new Second();
		}
	}

	public class Rest : FCallable
	{
		public Rest()
		{
			var l = new FSymbol("__l__");
			isClosure = false;

			parameters.Add(l);
		}

		public override IExpression Eval(Env env)
		{
			var arg1 = env[parameters[0].name];
			if (arg1.GetFType() == FType.FList)
			{
				var l = (FList)arg1;
				if (l.Count >= 1)
				{
					var res = (FList)l.Clone();
					res.RemoveAt(0);
					return res;
				}
				else
					throw new InvalidFunProgram("List too short", Tok);
			}
			else
				throw new InvalidFunProgram("Can only take rest out of list", Tok);
		}

		public override object Clone()
		{
			return new Rest();
		}
	}

	public class Push : FCallable
	{
		public Push()
		{
			var e = new FSymbol("__e__");
			var l = new FSymbol("__l__");
			isClosure = false;

			parameters.Add(e);
			parameters.Add(l);
		}

		public override IExpression Eval(Env env)
		{
			var arg1 = env[parameters[0].name];
			var arg2 = env[parameters[1].name];
			if (arg2.GetFType() == FType.FList)
			{
				var l = (FList)((FList)arg2).Clone();

				l.Insert(0, arg1);
				return l;
			}
			else
				throw new InvalidFunProgram($"Can only push element onto list. Got: {arg2.GetFType()}", Tok);
		}

		public override object Clone()
		{
			return new Push();
		}
	}

	public class Println : FCallable
	{
		public Println()
		{
			var exp = new FSymbol("__exp__");
			isClosure = false;

			parameters.Add(exp);
		}

		public override IExpression Eval(Env env)
		{
			var arg1 = env[parameters[0].name];
			Console.WriteLine(arg1);

            return new FNumber(-1000, null);
        }

		public override object Clone()
		{
			return new Println();
		}
	}

	public class FunctionCall : FCallable
	{
		public IExpression body;

		public FunctionCall(List<FSymbol> _parameters, IExpression _body, Token _tok, bool _isClosure)
		{
			parameters = _parameters;
			body = _body;
			Tok = _tok;
			isClosure = _isClosure;
		}

		public override IExpression Eval(Env env)
		{
			var res = body.Eval(env);
			if (res.GetFType() == FType.FList && ((FList)res).Count >= 1)
                return ((FList)res)[((FList)res).Count - 1]; // return last element
            else
				return res;
		}

		public override object Clone()
		{
			var clone_params = new List<FSymbol>();
			foreach (var param in parameters)
			{
				clone_params.Add((FSymbol)param.Clone());
			}
			return new FunctionCall(clone_params, (IExpression)body.Clone(), (Token)Tok.Clone(), isClosure);
		}
	}
}

