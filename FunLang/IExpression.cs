namespace FunLang
{
    public interface IExpression : ICloneable
	{
		public FType GetFType();
        public IExpression Eval(Env env);
		public bool Equals(IExpression exp);

        Token? Tok { get; set; }
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
		FFunctionator,
	}
}