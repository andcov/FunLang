namespace FunLang
{

	public class FFunction : IExpression
	{
		public List<FSymbol> parameters;
		public IExpression body;
		public Token? Tok { get; set; } = null;

		public FFunction(List<FSymbol> _parameters, IExpression _body, Token? _tok)
		{
			parameters = _parameters;
			body = _body;
			Tok = _tok;
		}

		public IExpression Eval(Env env)
		{
			return this;
		}

		public FCallable GetFCallable() { return new FunctionCall(parameters, body, Tok, true); }

		public override string ToString()
		{
			var res = "";
			foreach (var p in parameters)
			{
				res += p.ToString() + ", ";
			}
			if (parameters.Count > 0)
			{
				res = res.Remove(res.Length - 2);
			}
			return $"Func [{res}] => {body}";
		}
		public bool Equals(IExpression exp)
		{
			if (exp.GetFType() != FType.FFunction) return false;
			var other = (FFunction)exp;
			if (parameters.Count != other.parameters.Count) return false;
			for (int i = 0; i < parameters.Count; ++i)
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
			return new FFunction(clone_params, (IExpression)body.Clone(), (Token)Tok.Clone());
		}
		public FType GetFType() { return FType.FFunction; }
	}
}
