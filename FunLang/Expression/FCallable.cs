﻿using System;

namespace FunLang
{
	public abstract class FCallable : IExpression
	{
        public List<FSymbol> parameters = new();
		public Env env = new();
		public bool isClosure;
		public Token? Tok { get; set; } = null;
		

		public abstract IExpression Eval(Env env);

		public int ParamCount() => parameters.Count;
		
		public override bool Equals(Object? obj)
        {
            if (obj == null || obj is not FCallable)
            {
                return false;
            }
            return this.GetType == obj.GetType;
        }
        public int Compare(IExpression exp) => (this.Equals(exp)) ? 0 : -1;
        public bool IsTrue() => true;

        public abstract object Clone();
		public FType GetFType() => FType.FCallable;
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

		public override object Clone() => new Add();
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

        public override object Clone() => new Substract();
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

        public override object Clone() => new Multiply();
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

        public override object Clone() => new Divide();
    }

    public class Modulo : FCallable
    {
        public Modulo()
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

        public override object Clone() => new Modulo();
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
            return (arg1.Equals(arg2)) ? new FNumber(1, Tok) : new FNumber(0, Tok);
        }

        public override object Clone() => new Equal();
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
            return (!arg1.Equals(arg2)) ? new FNumber(1, Tok) : new FNumber(0, Tok);
        }

        public override object Clone() => new Different();
    }

    public class Greater : FCallable
    {
        public Greater()
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
            return (arg1.Compare(arg2) > 0) ? new FNumber(1, Tok) : new FNumber(0, Tok);
        }

        public override object Clone() => new Greater();
    }

    public class GreaterEq : FCallable
    {
        public GreaterEq()
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
            return (arg1.Compare(arg2) >= 0) ? new FNumber(1, Tok) : new FNumber(0, Tok);
        }

        public override object Clone() => new GreaterEq();
    }

    public class Less : FCallable
    {
        public Less()
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
            return (arg1.Compare(arg2) < 0) ? new FNumber(1, Tok) : new FNumber(0, Tok);
        }

        public override object Clone() => new Less();
    }

    public class LessEq : FCallable
    {
        public LessEq()
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
            return (arg1.Compare(arg2) <= 0) ? new FNumber(1, Tok) : new FNumber(0, Tok);
        }

        public override object Clone() => new LessEq();
    }

    public class Not : FCallable
    {
        public Not()
        {
            var x = new FSymbol("__x__");
            isClosure = false;

            parameters.Add(x);
        }

        public override IExpression Eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            return (arg1.IsTrue()) ? new FNumber(0, Tok) : new FNumber(1, Tok);
        }

        public override object Clone() => new Not();
    }

    public class Or : FCallable
    {
        public Or()
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
            return (arg1.IsTrue() || arg2.IsTrue()) ? new FNumber(1, Tok) : new FNumber(0, Tok);
        }

        public override object Clone() => new Or();
    }

    public class And : FCallable
    {
        public And()
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
            return (arg1.IsTrue() && arg2.IsTrue()) ? new FNumber(1, Tok) : new FNumber(0, Tok);
        }

        public override object Clone() => new And();
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

        public override object Clone() => new Length();
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

        public override object Clone() => new First();
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

        public override object Clone() => new Second();
    }

    public class Third : FCallable
    {
        public Third()
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
                if (l.Count >= 3)
                    return l[2];
                else
                    throw new InvalidFunProgram("List too short", Tok);
            }
            else
                throw new InvalidFunProgram("Can only take third element of list", Tok);
        }

        public override object Clone() => new Third();
    }

    public class Last : FCallable
    {
        public Last()
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
                    return l[^1];
                else
                    throw new InvalidFunProgram("List too short to take last element", Tok);
            }
            else
                throw new InvalidFunProgram("Can only take last element out of list", Tok);
        }

        public override object Clone() => new Last();
    }

    public class Empty : FCallable
    {
        public Empty()
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
                return new FNumber((((FList)arg1).Count == 0) ? 1 : 0, Tok);
            }
            else
                throw new InvalidFunProgram($"Can only check if list is empty. Got {arg1.GetFType()}", Tok);
        }

        public override object Clone() => new Empty();
    }

    public class Tail : FCallable
	{
		public Tail()
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
					throw new InvalidFunProgram("List must have at least one element to take its tail", Tok);
			}
			else
				throw new InvalidFunProgram("Can only take the tail of a list", Tok);
		}

        public override object Clone() => new Tail();
    }

    public class Head : FCallable
    {
        public Head()
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
                    res.RemoveAt(res.Count - 1);
                    return res;
                }
                else
                    throw new InvalidFunProgram("List must have at least one element to take its head", Tok);
            }
            else
                throw new InvalidFunProgram("Can only take the head of a list", Tok);
        }

        public override object Clone() => new Head();
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

        public override object Clone() => new Push();
    }

    public class Append : FCallable
    {
        public Append()
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

                l.Add(arg1);
                return l;
            }
            else
                throw new InvalidFunProgram($"Can only append element onto list. Got: {arg2.GetFType()}", Tok);
        }

        public override object Clone() => new Append();
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

        public override object Clone() => new Println();
    }

    public class Error : FCallable
    {
        public Error()
        {
            var exp = new FSymbol("__exp__");
            isClosure = false;

            parameters.Add(exp);
        }

        public override IExpression Eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            Console.Error.WriteLine("error: " + arg1);
            throw new InvalidFunProgram(Tok);
        }

        public override object Clone() => new Error();
    }

    public class Readln : FCallable
    {
        public Readln()
        {
            isClosure = false;
        }

        public override IExpression Eval(Env env)
        {
            var s = Console.In.ReadLine();
            FList l = new(Tok);

            for (int i = 0; i < s.Length; ++i)
            {
                l.Add(new FChar(s[i], null));
            }

            return l;
        }

        public override object Clone() => new Readln();
    }

    public class Range : FCallable
    {
        public Range()
        {
            var n = new FSymbol("__n__");
            isClosure = false;

            parameters.Add(n);
        }

        public override IExpression Eval(Env env)
        {
            var arg1 = env[parameters[0].name];
            if (arg1.GetFType() != FType.FNumber)
            {
                throw new InvalidFunProgram($"Range takes a positive integer as an argument. Got: {arg1.GetFType()}", Tok);
            }

            var num = (FNumber)arg1;
            if (num.i == null || (num.i < 0))
            {
                throw new InvalidFunProgram($"Range takes a positive integer as an argument. Got: {arg1}", Tok);
            }

            FList l = new(Tok);
            for (int i = 0; i < num.i; ++i)
            {
                l.Add(new FNumber(i, null));
            }

            return l;  
        }

        public override object Clone() => new Range();
    }

    public class Num : FCallable {
        public Num()
        {
            var exp = new FSymbol("__exp__");
            isClosure = false;

            parameters.Add(exp);
        }

        public override IExpression Eval(Env env)
        {
            var arg1 = env[parameters[0].name];

            if (arg1.GetFType() == FType.FNumber)
            {
                return arg1;
            }

            if (arg1.GetFType() == FType.FChar)
            {
                var ch = (FChar)arg1;
                return new FNumber((int)ch.ch, Tok);
            }

                if (arg1.GetFType() == FType.FList)
            {
                var l = (FList)arg1;
                if (l.IsListOf(FType.FChar)) {
                    var res = "";
                    foreach (var el in l) {
                        var ch = (FChar)el;
                        res += ch.ch;
                    }
                    if (Int32.TryParse(res, out int inum))
                    {
                        return new FNumber(inum, Tok);
                    }
                    else if (float.TryParse(res, out float fnum))
                    {
                        return new FNumber(fnum, Tok);
                    }
                    else
                    {
                        throw new InvalidFunProgram($"Could not convert {arg1} to a number", Tok);
                    }
                }
            }

            throw new InvalidFunProgram($"Could not convert {arg1} to a number", Tok);
        }

        public override object Clone() => new Num();
    }

    public class FunctionCall : FCallable
	{
		public IExpression body;

		public FunctionCall(List<FSymbol> _parameters, IExpression _body, Token? _tok, bool _isClosure)
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

