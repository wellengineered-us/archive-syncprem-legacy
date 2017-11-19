namespace NMock.Syntax
{
	internal interface IValueSyntaxBuilder : IValueSyntax
	{
		void Will(params IAction[] action);
	}
}