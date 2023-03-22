using System;
namespace FunLang
{
	public class FNumber : IExpression
	{
		public int? i;
		public float? f;
		public Token? Tok { get; set; } = null;

		public FNumber(int _i, Token? _tok)
		{
			i = _i;
			f = null;
			Tok = _tok;
		}

		public FNumber(float _f, Token? _tok)
		{
			f = _f;
			i = null;
			Tok = _tok;
		}

		public IExpression Eval(Env env) => this;

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
		public override bool Equals(Object? obj)
		{
            if (obj == null || obj is not IExpression)
            {
                return false;
            }
			var exp = (IExpression) obj;

			if (exp.GetFType() == FType.FNumber)
			{
				var num = (FNumber)exp;
				if (i.HasValue && num.i.HasValue)
				{
					return (i == num.i);
				}
				else
				{
					float f1 = (i.HasValue) ? (float)i : f.Value;
					float f2 = (num.i.HasValue) ? (float)num.i : num.f.Value;
					return Math.Abs((double)(f1 - f2)) < 0.000001;
				}
			}
			else if (exp.GetFType() == FType.FChar)
			{
                var ch = (FChar)exp;
				if (i.HasValue) return ch.ch == i;
				return false;
            }
			return false;
        }
		public int Compare(IExpression exp)
		{
			if (this.Equals(exp)) return 0;
			else if (exp.GetFType() == FType.FNumber)
			{
				var num = (FNumber)exp;
				if (i.HasValue && num.i.HasValue)
				{
					return (i > num.i) ? 1 : -1;
				}
				float f1 = (i.HasValue) ? (float)i : f.Value;
				float f2 = (num.i.HasValue) ? (float)num.i : num.f.Value;
				return (f1 > f2) ? 1 : -1;
			}
			else if (exp.GetFType() == FType.FChar)
			{
				var ch = (FChar)exp;
				var val = (i.HasValue) ? (float)i : f;
				return (val > ch.ch) ? 1 : -1;
			}

			return 1;
		}

		public object Clone()
		{
			if (i.HasValue)
			{
				return new FNumber(i.Value, Tok);
			}
			else
			{
				return new FNumber(f.Value, Tok);
			}
		}
		public FType GetFType() => FType.FNumber;
	}
}