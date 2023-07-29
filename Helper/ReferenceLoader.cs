using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Server.Helper;

public class ReferenceLoader : MarshalByRefObject
{
	public string[] LoadReferences(string assemblyPath)
	{
		try
		{
			return (from x in Assembly.ReflectionOnlyLoadFrom(assemblyPath).GetReferencedAssemblies()
				select x.FullName).ToArray();
		}
		catch
		{
			return null;
		}
	}

	public void AppDomainSetup(string assemblyPath)
	{
		try
		{
			AppDomainSetup info = new AppDomainSetup
			{
				ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
			};
			AppDomain domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, info);
			((ReferenceLoader)Activator.CreateInstance(domain, typeof(ReferenceLoader).Assembly.FullName, typeof(ReferenceLoader).FullName, ignoreCase: false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, null, CultureInfo.CurrentCulture, new object[0]).Unwrap()).LoadReferences(assemblyPath);
			AppDomain.Unload(domain);
		}
		catch
		{
		}
	}
}
