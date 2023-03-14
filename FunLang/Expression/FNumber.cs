using System;
namespace FunLang
{
	public class FNumber : Expression
	{
		public int? i;
		public float? f;
		public Token? tok { get; set; } = null;

		public FNumber(int _i, Token? _tok)
		{
			i = _i;
			f = null;
			tok = _tok;
		}

		public FNumber(float _f, Token? _tok)
		{
			f = _f;
			i = null;
			tok = _tok;
		}

		public Expression eval(Env env)
		{
			return this;
		}

		public override string ToString()
		{
			if (i.HasValue)
			{
				return $"Num {i}";
			}
			else if (f.HasValue)
			{
				return $"Num {f}";
			}
			return $"Num null";
		}
		public bool Equals(Expression exp)
		{
			if (exp.GetFType() != FType.FNumber) return false;
			var num = (FNumber)exp;
			if (i.HasValue && num.i.HasValue)
			{
				return (i == num.i);
			}
			else
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
		public FType GetFType()
		{
			return FType.FNumber;
		}
	}
}