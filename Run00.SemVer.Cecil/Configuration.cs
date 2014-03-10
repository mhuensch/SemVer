using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.SemVer.Cecil
{
	public class Configuration : IConfiguration
	{
		string IConfiguration.Get(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}

		void IConfiguration.Set(string key, string value)
		{
			ConfigurationManager.AppSettings[key] = value;
		}
	}
}
