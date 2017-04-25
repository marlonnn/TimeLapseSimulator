using System;
using Microsoft.Win32;

namespace Summer.System.Util
{
	public class RegistryHelper
	{
		public string productionName = "someProduction";
		public string RootPath
		{
			get { return @"SOFTWARE\casco\" + productionName; }
		}

		public bool IsAvalable
		{
			get { return getRootKey ( true ) != null; }
		}

		private RegistryKey getRootKey ( bool writable = true )
		{
			RegistryKey tcmsKey;
			try
			{
				RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
				tcmsKey = hklm.OpenSubKey ( RootPath, writable ) ??
									 hklm.CreateSubKey ( RootPath );
			} catch ( Exception e)
			{
				tcmsKey = null;
                throw e;
			}
			return tcmsKey;
		}


		public Int32 GetValueInt32 ( string name )
		{
			var vv = GetValue ( name );

			Int32 rr;
			Int32.TryParse ( vv, out rr );
			return rr;
		}
		public UInt16 GetValueUInt16 ( string name )
		{
			var vv = GetValue ( name );

			UInt16 rr;
			UInt16.TryParse ( vv, out rr );
			return rr;
		}
		public string GetValue ( string name )
		{
			using ( var tcmsKey = getRootKey ( ) )
			{
				object value = "";
				if ( tcmsKey != null )
				{
					try
					{
						value = tcmsKey.GetValue ( name );
					} catch ( Exception)
					{
						value = "";
					}
				}
				return value == null ? "" : value.ToString ( );

			}
		}

		public bool SetValue ( string name, string value )
		{
			var rst = true;
			using ( var tcmsKey = getRootKey ( ) )
			{
				try
				{
					if ( tcmsKey != null )
						tcmsKey.SetValue ( name, value );
				} catch ( Exception )
				{
					rst = false;
				}
			}
			return rst;
		}
	}
}
