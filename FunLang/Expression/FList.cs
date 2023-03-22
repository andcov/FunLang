using System;
namespace FunLang
{

    public class FList : List<IExpression>, IExpression, ICloneable
    {
        public Token? Tok { get; set; } = null;

        public FList(Token? _tok) => Tok = _tok;


        public FList(IExpression exp, Token? _tok)
        {
            this.Add(exp);
            Tok = _tok;
        }

        public IExpression Eval(Env env)
        {
            var res = new FList(null);
            var unwrap = false;
            var functionator = false;

            for (int i = 0; i < this.Count; ++i)
            {
                var ev = this[i].Eval(env);

                if (ev.GetFType() == FType.FFunctionator)
                {
                    functionator = true;
                    continue;
                }

                if (ev.GetFType() == FType.FFunction)
                {
                    if (functionator)
                    {
                        functionator = false;
                    }
                    else
                    {
                        var call = ((FFunction)ev).GetFCallable();
                        ev = call;
                    }
                }

                if (functionator == true)
                {
                    throw new InvalidFunProgram("Functionator can only be applied to a function", Tok);
                }

                if (ev.GetFType() == FType.FCallable)
                {
                    unwrap = true;
                    var func = (FCallable)ev;
                    (FList args, var next_i) = EvalNFrom(env, i + 1, func.ParamCount(), false);

                    var func_env = new Env();
                    if (func.isClosure)
                        func_env = (Env)env.Clone();
                    else
                        func_env.AddStandard();

                    func_env.AddArguments(func.parameters, args);

                    ev = func.Eval(func_env);
                    i = next_i;
                }
                    
                res.Add(ev);
            }
            if (unwrap && res.Count == 1)
            {
                return res[0];
            }

            return res;
        }

        public (FList, int) EvalNFrom(Env env, int i, int n, bool functionator)
        {
            if (n == 0) return (new FList(null), i - 1);

            if (this.Count - i < n)
            {
                throw new InvalidFunProgram($"Not enough symbols to evaluate {n}, {i}, {this.Count}: {this}", Tok);
            }

            IExpression first;
            var last_i = i;

            first = this[i].Eval(env);

            if (first.GetFType() == FType.FFunction && functionator == false)
            {
                var call = ((FFunction)first).GetFCallable();
                first = call;
            }

            if (first.GetFType() != FType.FFunction && functionator == true)
            {
                throw new InvalidFunProgram("Functionator can only be applied to a function", first.Tok);
            }

            if (first.GetFType() == FType.FCallable)
            {
                var func = (FCallable)first;
                (FList args, last_i) = EvalNFrom(env, i + 1, func.ParamCount(), false);

                var func_env = new Env();
                if (func.isClosure)
                    func_env = (Env)env.Clone();
                else
                    func_env.AddStandard();

                func_env.AddArguments(func.parameters, args);

                first = func.Eval(func_env);
            } else if (first.GetFType() == FType.FFunctionator)
            {
                return EvalNFrom(env, last_i + 1, n, true);
            }

            if (n == 1)
            {
                return (new FList(first, null), last_i);
            }

            var (rest, new_i) = EvalNFrom(env, last_i + 1, n - 1, false);
            rest.Insert(0, first);

            return (rest, new_i);
        }

        public bool IsListOf(FType type)
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
            if (this.Count > 0 && this.IsListOf(FType.FChar))
            {
                foreach (var ch in this)
                {
                    res += ((FChar)ch).ch;
                }
                return '"' + res + '"';
            }
            else
            {
                foreach (var e in this)
                {
                    res += e.ToString() + ", ";
                }
                if (this.Count > 0)
                {
                    res = res.Remove(res.Length - 2);
                }
                return $"[{res}]";
            }
        }
        public override bool Equals(Object? obj)
        {
            if (obj == null || obj is not IExpression)
            {
                return false;
            }
            var exp = (IExpression)obj;

            if (exp.GetFType() != FType.FList) return false;
            var other = (FList)exp;
            if (other.Count != this.Count) return false;
            for (int i = 0; i < this.Count; ++i)
            {
                if (!this[i].Equals(other[i])) return false;
            }
            return true;
        }
        public int Compare(IExpression exp)
        {
            if (this.Equals(exp)) return 0;
            else if (exp.GetFType() == FType.FChar || exp.GetFType() == FType.FNumber)
            {
                return -1 * exp.Compare(this);
            }
            else if (exp.GetFType() == FType.FList)
            {
                var other = (FList)exp;
                int i = 0;
                while(i < this.Count && i < other.Count)
                {
                    var cmp = this[i].Compare(other[i]);
                    if (cmp != 0) return cmp;
                    i++;
                }
                return (this.Count > other.Count) ? 1 : -1;
            }

            return 1;
        }

        public object Clone()
        {
            var res = new FList(Tok);
            foreach (var el in this)
            {
                res.Add((IExpression)(el.Clone()));
            }

            return res;
        }
        public FType GetFType() => FType.FList;
    }
}