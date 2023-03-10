using System;
namespace FunLang
{

	public class FFunction : Expression
	{
		public List<FSymbol> parameters;
		public Expression body;
		public Token tok { get; set; }

		public FFunction(List<FSymbol> _parameters, Expression _body, Token _tok)
		{
			parameters = _parameters;
			body = _body;
			tok = _tok;
		}

		public Expression eval(Env env)
		{
			return this;
		}

		public FCallable GetFCallable() { return new FunctionCall(parameters, body, tok, true); }

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
			return $"Func [{res}] => {body.ToString()}";
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

		public object Clone()
		{
			var clone_params = new List<FSymbol>();
			foreach (var param in parameters)
			{
				clone_params.Add((FSymbol)param.Clone());
			}
			return new FFunction(clone_params, (Expression)body.Clone(), (Token)tok.Clone());
		}
		public FType GetFType() { return FType.FFunction; }
	}
}
