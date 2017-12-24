///*
//	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
//	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
//*/

//using System;
//using System.Collections.Generic;
//using System.Reflection;

//using SyncPrem.Infrastructure.Tabular.Meta;

//using ZZZ = System.Func<object, object>;

//namespace SyncPrem.Infrastructure.Oxymoron.Strategies
//{
//	/// <summary>
//	/// Returns an alternate value that is a script function of the original value.
//	/// DATA TYPE: any
//	/// </summary>
//	public sealed class ScriptObfuscationStrategy : ObfuscationStrategy<ScriptObfuscationStrategyConfiguration>
//	{
//		#region Constructors/Destructors

//		public ScriptObfuscationStrategy()
//		{
//		}

//		#endregion

//		#region Fields/Constants

//		private static readonly IDictionary<string, ZZZ> csharpCodeCache = new Dictionary<string, ZZZ>();

//		#endregion

//		#region Properties/Indexers/Events

//		private static IDictionary<string, ZZZ> CSharpCodeCache
//		{
//			get
//			{
//				return csharpCodeCache;
//			}
//		}

//		#endregion

//		#region Methods/Operators

//		public static object CompileCSharpCode(object columnValue, string scriptSource)
//		{
//			CompilerResults cr;
//			CompilerParameters cp;
//			ZZZ callback;

//			if (!CSharpCodeCache.TryGetValue(scriptSource, out callback))
//			{
//				using (CSharpCodeProvider provider = new CSharpCodeProvider())
//				{
//					cp = new CompilerParameters();

//					cp.ReferencedAssemblies.Add("System.dll");
//					cp.GenerateExecutable = false;
//					//cp.OutputAssembly = null;
//					cp.GenerateInMemory = true;

//					cr = provider.CompileAssemblyFromSource(cp, scriptSource);

//					if (cr.Errors.Cast<CompilerError>().Any())
//						throw new InvalidOperationException(string.Format("{0}", string.Join(",", cr.Errors.Cast<CompilerError>().Select(e => e.ErrorText).ToArray())));

//					Type type = cr.CompiledAssembly.GetType("Script", false);
//					MethodInfo method = type.GetMethod("GetValue", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(object) }, null);

//					callback = (o) => method.Invoke(null, new object[] { o });

//					CSharpCodeCache.Add(scriptSource, callback);
//				}
//			}

//			if ((object)callback == null)
//				throw new InvalidOperationException(string.Format("callback"));

//			return callback(columnValue);
//		}

//		protected override object CoreGetObfuscatedValue(IOxymoronEngine oxymoronEngine, ColumnConfiguration<ScriptObfuscationStrategyConfiguration> columnSpec, IColumn column, object columnValue)
//		{
//if ((object)oxymoronEngine == null)
//throw new ArgumentNullException(nameof(oxymoronEngine));
//			if ((object)columnSpec == null)
//				throw new ArgumentNullException(nameof(columnSpec));

//			if ((object)column == null)
//				throw new ArgumentNullException(nameof(column));

//			if ((object)columnSpec.ObfuscationStrategySpecificConfiguration == null)
//				throw new InvalidOperationException(string.Format("Specification missing: '{0}'.", nameof(columnSpec.ObfuscationStrategySpecificConfiguration)));

//			return CompileCSharpCode(columnValue, columnSpec.ObfuscationStrategySpecificConfiguration.SourceCode);
//		}

//		#endregion
//	}
//}

