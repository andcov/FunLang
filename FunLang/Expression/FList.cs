using System;
namespace FunLang
{

    public class FList : List<Expression>, Expression, ICloneable
    {
        public Token tok { get; set; }

        public FList(Token _tok)
        {
            tok = _tok;
        }

        public FList(Expression exp, Token _tok)
        {
            this.Add(exp);
            tok = _tok;
        }

        public Expression eval(Env env)
        {
            if (this.isListOf(FType.FNull)) return new FNull(tok);

            if (this.Count() == 1  // has one element
                    && this[0].eval(env).GetFType() == FType.FFunction) // that is a function
            {
                return this[0].eval(env);
            }

            var res = new FList(new Token());
            var unwrap = false;

            for (int i = 0; i < this.Count(); ++i)
            {
                if (this[i].GetFType() == FType.FList // we have a list
                    && ((FList)this[i]).Count() == 1  // with one element
                    && ((FList)this[i])[0].eval(env).GetFType() == FType.FFunction) // that is a function
                {
                    res.Add(((FList)this[i])[0].eval(env));
                }
                else
                {
                    var ev = this[i].eval(env);
                    if (ev.GetFType() == FType.FFunction)
                    {
                        var func = (FFunction)ev;
                        ev = func.GetFCallable();
                    }
                    if (ev.GetFType() == FType.FCallable)
                    {
                        unwrap = true;
                        var func = (FCallable)ev;
                        var args = new FList(new Token());
                        (args, var next_i) = evalNFrom(env, i + 1, func.ParamCount());

                        var func_env = new Env();
                        if (func.isClosure)
                            func_env = (Env)env.Clone();
                        else
                            func_env.AddStandard();

                        func_env.AddArguments(func.parameters, args);

                        ev = func.eval(func_env);
                        i = next_i;
                    }

                    if (ev.GetFType() != FType.FNull)
                    {
                        res.Add(ev);
                    }
                }
            }
            if (unwrap && res.Count() == 1)
            {
                return res[0];
            }

            return res;
        }

        public (FList, int) evalNFrom(Env env, int i, int n)
        {
            if (this.Count() - i < n)
            {
                throw new InvalidOperationException($"Not enough symbols to evaluate {n}, {i}, {this.Count()}: {this}");
            }

            Expression first;
            var last_i = i;

            if (this[i].GetFType() == FType.FList // we have a list
                    && ((FList)this[i]).Count() == 1  // with one element
                    && ((FList)this[i])[0].eval(env).GetFType() == FType.FFunction) // that is a function
            {
                first = ((FList)this[i])[0].eval(env);
            }
            else
            {
                first = this[i].eval(env);

                if (first.GetFType() == FType.FFunction)
                {
                    var func = (FFunction)first;
                    first = func.GetFCallable();
                }
                if (first.GetFType() == FType.FCallable)
                {
                    var func = (FCallable)first;
                    var args = new FList(new Token());
                    (args, last_i) = evalNFrom(env, i + 1, func.ParamCount());

                    var func_env = new Env();
                    if (func.isClosure)
                        func_env = (Env)env.Clone();
                    else
                        func_env.AddStandard();

                    func_env.AddArguments(func.parameters, args);

                    first = func.eval(func_env);
                }
            }
            if (n == 1)
            {
                return (new FList(first, new Token()), last_i);
            }

            var (rest, new_i) = evalNFrom(env, last_i + 1, n - 1);
            rest.Insert(0, first);

            return (rest, new_i);
        }

        public bool isListOf(FType type)
        {
            foreach (var e in this)
            {
                if (e.GetFType() != type)
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            var res = "";
            foreach (var e in this)
            {
                res += e.ToString() + ", ";
            }
            if (this.Count() > 0)
            {
                res = res.Remove(res.Length - 2);
            }
            return $"[{res}]";
        }
        public bool Equals(Expression exp)
        {
            if (exp.GetFType() != FType.FList) return false;
            var other = (FList)exp;
            if (other.Count() != this.Count()) return false;
            for (int i = 0; i < this.Count(); ++i)
            {
                if (!this[i].Equals(other[i])) return false;
            }
            return true;
        }

        public object Clone()
        {
            var res = new FList(tok);
            foreach (var el in this)
            {
                res.Add((Expression)(el.Clone()));
            }

            return res;
        }
        public FType GetFType()
        {
            return FType.FList;
        }
    }

}