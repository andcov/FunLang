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

        Token tok { get; set; }
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
	}
}