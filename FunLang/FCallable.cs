using System;
namespace FunLang
{
	public abstract class FCallable : Expression
	{
        public List<FSymbol> parameters = new List<FSymbol>();
        public Env env = new Env();
        public Token tok;
        public bool isClosure;

        public abstract Expression eval(Env env);
        public abstract object Clone();

        public int ParamCount() { return parameters.Count(); }
        public FType GetFType() { return FType.FCallable; }
        public bool Equals(Expression exp) { return false; }
        public Token GetToken() { return tok; }
        public void SetToken(Token _tok) { tok = _tok; }
    }

    public class Adder : FCallable
    {
        public Adder()
        {
            var x = new FSymbol("__x__");
            var y = new FSymbol("__y__");
            isClosure = false;

            parameters.Add(x);
            parameters.Add(y);
        }

        public override object Clone()
        {
            return new Adder();
        }

        public override Expression eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            var arg2 = env[parameters[1].name];
            if (arg1.GetFType() == arg2.GetFType())
            {
                if (arg1.GetFType() == FType.FNumber)
                {
                    FNumber v1 = (FNumber)arg1;
                    FNumber v2 = (FNumber)arg2;
                    if (v1.i.HasValue && v2.i.HasValue)
                    {
                        return new FNumber(v1.i.Value + v2.i.Value);
                    }
                    else
                    {
                        float f1 = (v1.i.HasValue) ? (float)v1.i : v1.f.Value;
                        float f2 = (v2.i.HasValue) ? (float)v2.i : v2.f.Value;
                        return new FNumber(f1 + f2);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new InvalidOperationException($"Expected the same type, got {arg1.GetFType()} and {arg2.GetFType()}");
            }
        }
    }

    public class Substracter : FCallable
    {
        public Substracter()
        {
            var x = new FSymbol("__x__");
            var y = new FSymbol("__y__");
            isClosure = false;

            parameters.Add(x);
            parameters.Add(y);
        }

        public override object Clone()
        {
            return new Substracter();
        }

        public override Expression eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            var arg2 = env[parameters[1].name];
            if (arg1.GetFType() == arg2.GetFType())
            {
                if (arg1.GetFType() == FType.FNumber)
                {
                    FNumber v1 = (FNumber)arg1;
                    FNumber v2 = (FNumber)arg2;
                    if (v1.i.HasValue && v2.i.HasValue)
                    {
                        return new FNumber(v1.i.Value - v2.i.Value);
                    }
                    else
                    {
                        float f1 = (v1.i.HasValue) ? (float)v1.i : v1.f.Value;
                        float f2 = (v2.i.HasValue) ? (float)v2.i : v2.f.Value;
                        return new FNumber(f1 - f2);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new InvalidOperationException($"Expected the same type, got {arg1.GetFType()} and {arg2.GetFType()}");
            }
        }
    }

    public class Multiplier : FCallable
    {
        public Multiplier()
        {
            var x = new FSymbol("__x__");
            var y = new FSymbol("__y__");
            isClosure = false;

            parameters.Add(x);
            parameters.Add(y);
        }

        public override object Clone()
        {
            return new Multiplier();
        }

        public override Expression eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            var arg2 = env[parameters[1].name];
            if (arg1.GetFType() == arg2.GetFType())
            {
                if (arg1.GetFType() == FType.FNumber)
                {
                    FNumber v1 = (FNumber)arg1;
                    FNumber v2 = (FNumber)arg2;
                    if (v1.i.HasValue && v2.i.HasValue)
                    {
                        return new FNumber(v1.i.Value * v2.i.Value);
                    }
                    else
                    {
                        float f1 = (v1.i.HasValue) ? (float)v1.i : v1.f.Value;
                        float f2 = (v2.i.HasValue) ? (float)v2.i : v2.f.Value;
                        return new FNumber(f1 * f2);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new InvalidOperationException($"Expected the same type, got {arg1.GetFType()} and {arg2.GetFType()}");
            }
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

        public override object Clone()
        {
            return new Equal();
        }

        public override Expression eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            var arg2 = env[parameters[1].name];
            if (arg1.Equals(arg2))
                return new FNumber(1);
            else
                return new FNumber(0);
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

        public override object Clone()
        {
            return new Length();
        }

        public override Expression eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            if (arg1.GetFType() == FType.FList)
                return new FNumber(((FList)arg1).Count());
            else
                throw new InvalidOperationException("Can only measure length of list");
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

        public override object Clone()
        {
            return new First();
        }

        public override Expression eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            if (arg1.GetFType() == FType.FList) {
                var l = (FList)arg1;
                if (l.Count >= 1)
                    return l[0];
                else
                    throw new InvalidOperationException("List too short");
            } else
                throw new InvalidOperationException("Can only take first element of list");
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

        public override object Clone()
        {
            return new Second();
        }

        public override Expression eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            if (arg1.GetFType() == FType.FList)
            {
                var l = (FList)arg1;
                if (l.Count >= 2)
                    return l[1];
                else
                    throw new InvalidOperationException("List too short");
            }
            else
                throw new InvalidOperationException("Can only take first element of list");
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

        public override object Clone()
        {
            return new Rest();
        }

        public override Expression eval(Env env)
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
                    throw new InvalidOperationException("List too short");
            }
            else
                throw new InvalidOperationException("Can only take rest out of list");
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

        public override object Clone()
        {
            return new Push();
        }

        public override Expression eval(Env env)
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
                throw new InvalidOperationException($"Can only push element onto list. Got: {arg2.GetFType()}");
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

        public override object Clone()
        {
            return new Println();
        }

        public override Expression eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            Console.WriteLine(arg1);

            return new FNull(this.tok);
        }
    }

    public class FunctionCall : FCallable
    {
        public Expression body;

        public FunctionCall(List<FSymbol> _parameters, Expression _body, Token _tok, bool _isClosure)
        {
            parameters = _parameters;
            body = _body;
            tok = _tok;
            isClosure = _isClosure;
        }

        public override object Clone()
        {
            var clone_params = new List<FSymbol>();
            foreach (var param in parameters)
            {
                clone_params.Add((FSymbol)param.Clone());
            }
            return new FunctionCall(clone_params, (Expression)body.Clone(), (Token)tok.Clone(), isClosure);
        }

        public override Expression eval(Env env)
        {
            var res = body.eval(env);
            if (res.GetFType() == FType.FList && ((FList)res).Count() == 1)
                return ((FList)res)[0];
            else
                return res;
        }
    }
}

