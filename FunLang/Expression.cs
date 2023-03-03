using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FunLang
{
	public interface Expression : ICloneable
	{
		public FType GetFType();
        public Expression eval(Env env);
        public bool Equals(Expression exp);
        public Token GetToken();
        public void SetToken(Token tok);
    }

	public enum FType {
		FNumber,
        FChar,
        FSymbol,
		FList,
		FFunction,
        FDefine,
        FCallable,
        FIf,
        FNull,
	}

    public class FNull : Expression
    {
        private Token tok;

        public FNull(Token _tok)
        {
            tok = _tok;
        }

        public FType GetFType() { return FType.FNull; }

        public override string ToString()
        {
            return "NULL";
        }

        public bool Equals(Expression exp)
        {
            return false;
        }

        public Expression eval(Env env)
        {
            return this;
        }

        public Token GetToken() { return tok; }
        public void SetToken(Token _tok) { tok = _tok; }

        public object Clone()
        {
            return new FNull((Token)tok.Clone());
        }
    }

    public class FNumber : Expression
	{
        public int? i;
        public float? f;
        private Token tok;

        public FNumber(int _i)
        {
            i = _i;
            f = null;
        }

        public FNumber(float _f)
        {
            f = _f;
            i = null;
        }

        public FNumber(int _i, Token _tok)
        {
            i = _i;
            f = null;
            tok = _tok;
        }

        public FNumber(float _f, Token _tok)
        {
            f = _f;
            i = null;
            tok = _tok;
        }

        public FType GetFType()
		{
			return FType.FNumber;
		}

        public override string ToString()
        {
            if(i.HasValue)
            {
                return $"Num {i}";
            } else if (f.HasValue)
            {
                return $"Num {f}";
            }
            return $"Num null";
        }

        public Expression eval(Env env)
        {
            return this;
        }

        public bool Equals(Expression exp)
        {
            if (exp.GetFType() != FType.FNumber) return false;
            var num = (FNumber)exp;
            if (i.HasValue && num.i.HasValue)
            {
                return (i == num.i);
            } else
            {
                float f1 = (i.HasValue) ? (float)i : f.Value;
                float f2 = (num.i.HasValue) ? (float)num.i : num.f.Value;
                return Math.Abs((double)(f1 - f2)) < 0.0001;
            }
        }

        public object Clone()
        {
            if (i.HasValue)
            {
                return new FNumber(i.Value, tok);
            }
            else
            {
                return new FNumber(f.Value, tok);
            }
        }

        public Token GetToken() { return tok; }
        public void SetToken(Token _tok) { tok = _tok; }
    }

    public class FChar : Expression
    {
        public char ch;
        private Token tok;

        public FChar(char _ch)
        {
            ch = _ch;
        }

        public FChar(char _ch, Token _tok)
        {
            ch = _ch;
            tok = _tok;
        }

        public FType GetFType()
        {
            return FType.FChar;
        }

        public override string ToString()
        {
            return $"Char {ch}";
        }

        public bool Equals(Expression exp)
        {
            if (exp.GetFType() != FType.FChar) return false;
            var other = (FChar)exp;
            return ch == other.ch;
        }

        public Expression eval(Env env)
        {
            return this;
        }

        public object Clone()
        {
            return new FChar(ch, tok);
        }

        public Token GetToken() { return tok; }
        public void SetToken(Token _tok) { tok = _tok; }
    }

    public class FSymbol : Expression
    {
        public string name;
        private Token tok;

        public FSymbol(string _name)
        {
            name = _name;
        }

        public FSymbol(string _name, Token _tok)
        {
            name = _name;
            tok = _tok;
        }

        public FType GetFType()
        {
            return FType.FSymbol;
        }

        public override string ToString()
        {
            return $"Sym {name}";
        }

        public bool Equals(Expression exp)
        {
            if (exp.GetFType() != FType.FSymbol) return false;
            return this.name == ((FSymbol)exp).name;
        }

        public Expression eval(Env env)
        {
            if(env.TryGetValue(name, out Expression e))
            {
                return e;
            }
            throw new InvalidOperationException("Cannot find the symbol " + name);
        }

        public object Clone()
        {
            return new FSymbol(name, tok);
        }

        public Token GetToken() { return tok; }
        public void SetToken(Token _tok) { tok = _tok; }
    }

    public class FList : List<Expression>, Expression, ICloneable
    {
        private Token tok;

        public FType GetFType()
        {
            return FType.FList;
        }

        public FList()
        {
        }

        public FList(Expression exp)
        {
            this.Add(exp);
        }

        public object Clone()
        {
            var res = new FList();
            foreach(var el in this)
            {
                res.Add((Expression)(el.Clone()));
            }

            return res;
        }

        public override string ToString()
        {
            var res = "";
            foreach(var e in this)
            {
                res += e.ToString() + ", ";
            }
            if(this.Count() > 0)
            {
                res = res.Remove(res.Length - 2);
            }
            return $"[{res}]";
        }

        public bool isListOf(FType type)
        {
            foreach(var e in this)
            {
                if (e.GetFType() != type)
                    return false;
            }
            return true;
        }

        public bool Equals(Expression exp)
        {
            if (exp.GetFType() != FType.FList) return false;
            var other = (FList)exp;
            if (other.Count() != this.Count()) return false;
            for(int i = 0; i < this.Count(); ++i)
            {
                if (!this[i].Equals(other[i])) return false;
            }
            return true;
        }

        public Expression eval(Env env)
        {
            if (this.isListOf(FType.FNull)) return new FNull(tok);

            if (this.Count() == 1  // has one element
                    && this[0].eval(env).GetFType() == FType.FFunction) // that is a function
            {
                return this[0].eval(env);
            }

            var res = new FList();
            var unwrap = false;

            for(int i = 0; i < this.Count(); ++i)
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
                        var args = new FList();
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

                    if(ev.GetFType() != FType.FNull)
                    {
                        res.Add(ev);
                    }
                }
            }
            if(unwrap && res.Count() == 1)
            {
                return res[0];
            }

            return res;
        }

        public (FList, int) evalNFrom(Env env, int i, int n)
        {
            if(this.Count() - i < n)
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
                    var args = new FList();
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
            if(n == 1)
            {
                return (new FList(first), last_i);
            }

            var (rest, new_i) = evalNFrom(env, last_i + 1, n - 1);
            rest.Insert(0, first);

            return (rest, new_i);
        }

        public bool isListWithOneFunc()
        {
            return (this.Count() == 1  // has one element
                    && this[0].GetFType() == FType.FFunction); // that is a function
        }

        public Token GetToken() { return tok; }
        public void SetToken(Token _tok) { tok = _tok; }
    }

    public class FDefine : Expression
    {
        private Token tok;
        public FSymbol? sym;
        public FList? list;
        public Expression vals;

        public FDefine(FSymbol _sym, Expression _vals, Token _tok)
        {
            sym = _sym;
            list = null;
            vals = _vals;
            tok = _tok;
        }

        public FDefine(FList _list, Expression _vals, Token _tok)
        {
            sym = null;
            list = _list;
            vals = _vals;
            tok = _tok;
        }

        public object Clone()
        {
            if (sym != null)
            {
                return new FDefine((FSymbol)sym.Clone(), (Expression)vals.Clone(), (Token)tok.Clone());
            }
            else
            {
                return new FDefine((FList)list.Clone(), (Expression)vals.Clone(), (Token)tok.Clone());
            }
        }

        public Expression eval(Env env)
        {
            if (sym != null)
            {
                env[sym.name] = vals.eval(env);
            }
            else if (list != null && vals.GetFType() == FType.FList)
            {
                var lvals = (FList)vals;
                for (int i = 0; i < list.Count(); ++i)
                {
                    var sym = (FSymbol)list[i];
                    env[sym.name] = lvals[i].eval(env);
                }
            }

            return new FNull(tok);
        }

        public bool Equals(Expression exp)
        {
            return false;
        }

        public override string ToString()
        {
            var res = "Def ";
            if(sym != null)
            {
                res += sym + " <- " + vals;
            } else
            {
                res += list + " <- " + vals;
            }
            return res;
        }
        public FType GetFType() { return FType.FDefine; }
        public Token GetToken() { return tok; }
        public void SetToken(Token _tok) { tok = _tok; }
    }

    public class FFunction : Expression
    {
        public List<FSymbol> parameters;
        public Expression body;
        private Token tok;

        public FFunction(List<FSymbol> _parameters, Expression _body, Token _tok)
        {
            parameters = _parameters;
            body = _body;
            tok = _tok;
        }

        public FFunction(Token _tok)
        {
            tok = _tok;
        }

        public FType GetFType() { return FType.FFunction; }

        public object Clone()
        {
            var clone_params = new List<FSymbol>();
            foreach(var param in parameters)
            {
                clone_params.Add((FSymbol) param.Clone());
            }
            return new FFunction(clone_params, (Expression)body.Clone(), (Token)tok.Clone());
        }

        public override string ToString()
        {
            var res = "";
            foreach (var p in parameters)
            {
                res += p.ToString() + ", ";
            }
            if (parameters.Count() > 0)
            {
                res = res.Remove(res.Length - 2);
            }
            return $"Func [{res}] => [{body.ToString()}]";
        }

        public bool Equals(Expression exp)
        {
            if (exp.GetFType() != FType.FFunction) return false;
            var other = (FFunction)exp;
            if (this.parameters.Count() != other.parameters.Count()) return false;
            for (int i = 0; i < this.parameters.Count(); ++i)
            {
                if (!this.parameters[i].Equals(other.parameters[i])) return false;
            }
            return this.body == other.body;
        }

        public Expression eval(Env env)
        {
            return this;
        }

        public FCallable GetFCallable() { return new FunctionCall(parameters, body, tok, true); }

        public Token GetToken() { return tok; }
        public void SetToken(Token _tok) { tok = _tok; }
    }

    public class FIf : Expression
    {
        public Expression condition;
        public Expression then;
        public Expression other;
        private Token tok;


        public FIf(Expression _condition, Expression _then, Expression _other)
        {
            condition = _condition;
            then = _then;
            other = _other;
        }

        public override string ToString()
        {
            return $"If [{condition}] then [{then}] else [{other}]";
        }

        public bool Equals(Expression exp)
        {
            return false;
        }

        public object Clone()
        {
            return new FIf((Expression)condition.Clone(), (Expression)then.Clone(), (Expression)other.Clone());
        }

        public Expression eval(Env env)
        {
            var cond = condition.eval(env);

            if (cond.GetFType() == FType.FList)
            {
                Expression res;

                if (((FList)cond).Count() != 0)
                {
                    res = then.eval(env);
                }
                else
                {
                    res = other.eval(env);
                }
                
                if (res.GetFType() == FType.FList && ((FList)res).Count() == 1)
                    return ((FList)res)[0];
                else
                    return res;

            }
            else if (cond.GetFType() == FType.FNumber)
            {
                var num = (FNumber)cond;

                Expression res;

                if (!num.Equals(new FNumber(0)))
                {
                    res = then.eval(env);
                }
                else
                {
                    res = other.eval(env);
                }

                if (res.GetFType() == FType.FList && ((FList)res).Count() == 1)
                    return ((FList)res)[0];
                else
                    return res;
            }
            throw new InvalidOperationException("Cannot evaluate truth value of " + cond);
        }

        public FType GetFType() { return FType.FIf; }
        public Token GetToken() { return tok; }
        public void SetToken(Token _tok) { tok = _tok; }
    }
}

