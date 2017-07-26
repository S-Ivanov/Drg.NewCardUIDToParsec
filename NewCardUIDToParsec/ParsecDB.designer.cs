﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewCardUIDToParsec
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Parsec3")]
	public partial class ParsecDBDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Определения метода расширяемости
    partial void OnCreated();
    partial void InsertDRG_CARD_UID(DRG_CARD_UID instance);
    partial void UpdateDRG_CARD_UID(DRG_CARD_UID instance);
    partial void DeleteDRG_CARD_UID(DRG_CARD_UID instance);
    #endregion
		
		public ParsecDBDataContext() : 
				base(global::NewCardUIDToParsec.Properties.Settings.Default.Parsec3ConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public ParsecDBDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ParsecDBDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ParsecDBDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ParsecDBDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<v_DRG_CARD_IDENTIFIERS> v_DRG_CARD_IDENTIFIERS
		{
			get
			{
				return this.GetTable<v_DRG_CARD_IDENTIFIERS>();
			}
		}
		
		public System.Data.Linq.Table<DRG_CARD_UID> DRG_CARD_UID
		{
			get
			{
				return this.GetTable<DRG_CARD_UID>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.Person_GetPhoto")]
		public ISingleResult<Person_GetPhotoResult> Person_GetPhoto([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="UniqueIdentifier")] System.Nullable<System.Guid> pers_id)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), pers_id);
			return ((ISingleResult<Person_GetPhotoResult>)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.v_DRG_CARD_IDENTIFIERS")]
	public partial class v_DRG_CARD_IDENTIFIERS
	{
		
		private System.Guid _PERS_ID;
		
		private string _TAB;
		
		private string _LAST_NAME;
		
		private string _FIRST_NAME;
		
		private string _MIDDLE_NAME;
		
		private string _ORG_NAME;
		
		private object _POST;
		
		private string _CARD_UID;
		
		public v_DRG_CARD_IDENTIFIERS()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PERS_ID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid PERS_ID
		{
			get
			{
				return this._PERS_ID;
			}
			set
			{
				if ((this._PERS_ID != value))
				{
					this._PERS_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TAB", DbType="VarChar(50)")]
		public string TAB
		{
			get
			{
				return this._TAB;
			}
			set
			{
				if ((this._TAB != value))
				{
					this._TAB = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LAST_NAME", DbType="NVarChar(64) NOT NULL", CanBeNull=false)]
		public string LAST_NAME
		{
			get
			{
				return this._LAST_NAME;
			}
			set
			{
				if ((this._LAST_NAME != value))
				{
					this._LAST_NAME = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FIRST_NAME", DbType="NVarChar(64)")]
		public string FIRST_NAME
		{
			get
			{
				return this._FIRST_NAME;
			}
			set
			{
				if ((this._FIRST_NAME != value))
				{
					this._FIRST_NAME = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MIDDLE_NAME", DbType="NVarChar(64)")]
		public string MIDDLE_NAME
		{
			get
			{
				return this._MIDDLE_NAME;
			}
			set
			{
				if ((this._MIDDLE_NAME != value))
				{
					this._MIDDLE_NAME = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ORG_NAME", DbType="NVarChar(128)")]
		public string ORG_NAME
		{
			get
			{
				return this._ORG_NAME;
			}
			set
			{
				if ((this._ORG_NAME != value))
				{
					this._ORG_NAME = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_POST", DbType="Variant", UpdateCheck=UpdateCheck.Never)]
		public object POST
		{
			get
			{
				return this._POST;
			}
			set
			{
				if ((this._POST != value))
				{
					this._POST = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CARD_UID", DbType="Char(8)")]
		public string CARD_UID
		{
			get
			{
				return this._CARD_UID;
			}
			set
			{
				if ((this._CARD_UID != value))
				{
					this._CARD_UID = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.DRG_CARD_UID")]
	public partial class DRG_CARD_UID : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _PERS_ID;
		
		private string _UID;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPERS_IDChanging(System.Guid value);
    partial void OnPERS_IDChanged();
    partial void OnUIDChanging(string value);
    partial void OnUIDChanged();
    #endregion
		
		public DRG_CARD_UID()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PERS_ID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid PERS_ID
		{
			get
			{
				return this._PERS_ID;
			}
			set
			{
				if ((this._PERS_ID != value))
				{
					this.OnPERS_IDChanging(value);
					this.SendPropertyChanging();
					this._PERS_ID = value;
					this.SendPropertyChanged("PERS_ID");
					this.OnPERS_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UID", DbType="Char(8) NOT NULL", CanBeNull=false)]
		public string UID
		{
			get
			{
				return this._UID;
			}
			set
			{
				if ((this._UID != value))
				{
					this.OnUIDChanging(value);
					this.SendPropertyChanging();
					this._UID = value;
					this.SendPropertyChanged("UID");
					this.OnUIDChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	public partial class Person_GetPhotoResult
	{
		
		private System.Data.Linq.Binary _PHOTO;
		
		public Person_GetPhotoResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PHOTO", DbType="VarBinary(MAX)")]
		public System.Data.Linq.Binary PHOTO
		{
			get
			{
				return this._PHOTO;
			}
			set
			{
				if ((this._PHOTO != value))
				{
					this._PHOTO = value;
				}
			}
		}
	}
}
#pragma warning restore 1591