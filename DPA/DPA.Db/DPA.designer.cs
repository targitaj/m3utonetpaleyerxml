﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DPA.Db
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="DPA")]
	public partial class DPADataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertLegalStatus(LegalStatus instance);
    partial void UpdateLegalStatus(LegalStatus instance);
    partial void DeleteLegalStatus(LegalStatus instance);
    partial void InsertPerson(Person instance);
    partial void UpdatePerson(Person instance);
    partial void DeletePerson(Person instance);
    partial void InsertDepartment(Department instance);
    partial void UpdateDepartment(Department instance);
    partial void DeleteDepartment(Department instance);
    #endregion
		
		public DPADataContext() : 
				base(global::DPA.Db.Properties.Settings.Default.DPAConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DPADataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DPADataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DPADataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DPADataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<LegalStatus> LegalStatus
		{
			get
			{
				return this.GetTable<LegalStatus>();
			}
		}
		
		public System.Data.Linq.Table<Person> Persons
		{
			get
			{
				return this.GetTable<Person>();
			}
		}
		
		public System.Data.Linq.Table<Department> Departments
		{
			get
			{
				return this.GetTable<Department>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.LegalStatus")]
	public partial class LegalStatus : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Description;
		
		private EntitySet<Person> _Persons;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    #endregion
		
		public LegalStatus()
		{
			this._Persons = new EntitySet<Person>(new Action<Person>(this.attach_Persons), new Action<Person>(this.detach_Persons));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(50)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="LegalStatus_Person", Storage="_Persons", ThisKey="Id", OtherKey="LegalStatusId")]
		public EntitySet<Person> Persons
		{
			get
			{
				return this._Persons;
			}
			set
			{
				this._Persons.Assign(value);
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
		
		private void attach_Persons(Person entity)
		{
			this.SendPropertyChanging();
			entity.LegalStatus = this;
		}
		
		private void detach_Persons(Person entity)
		{
			this.SendPropertyChanging();
			entity.LegalStatus = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Person")]
	public partial class Person : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _LegalStatusId;
		
		private string _PersonName;
		
		private bool _IsLRResident;
		
		private int _IncomeTaxRate;
		
		private bool _IsInsolvent;
		
		private string _Address;
		
		private bool _ReceiveNewsletter;
		
		private bool _IsIncompleteInformation;
		
		private string _PersonalCodeNmr;
		
		private EntityRef<LegalStatus> _LegalStatus;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnLegalStatusIdChanging(int value);
    partial void OnLegalStatusIdChanged();
    partial void OnPersonNameChanging(string value);
    partial void OnPersonNameChanged();
    partial void OnIsLRResidentChanging(bool value);
    partial void OnIsLRResidentChanged();
    partial void OnIncomeTaxRateChanging(int value);
    partial void OnIncomeTaxRateChanged();
    partial void OnIsInsolventChanging(bool value);
    partial void OnIsInsolventChanged();
    partial void OnAddressChanging(string value);
    partial void OnAddressChanged();
    partial void OnReceiveNewsletterChanging(bool value);
    partial void OnReceiveNewsletterChanged();
    partial void OnIsIncompleteInformationChanging(bool value);
    partial void OnIsIncompleteInformationChanged();
    partial void OnPersonalCodeNmrChanging(string value);
    partial void OnPersonalCodeNmrChanged();
    #endregion
		
		public Person()
		{
			this._LegalStatus = default(EntityRef<LegalStatus>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LegalStatusId", DbType="Int NOT NULL")]
		public int LegalStatusId
		{
			get
			{
				return this._LegalStatusId;
			}
			set
			{
				if ((this._LegalStatusId != value))
				{
					if (this._LegalStatus.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnLegalStatusIdChanging(value);
					this.SendPropertyChanging();
					this._LegalStatusId = value;
					this.SendPropertyChanged("LegalStatusId");
					this.OnLegalStatusIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PersonName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string PersonName
		{
			get
			{
				return this._PersonName;
			}
			set
			{
				if ((this._PersonName != value))
				{
					this.OnPersonNameChanging(value);
					this.SendPropertyChanging();
					this._PersonName = value;
					this.SendPropertyChanged("PersonName");
					this.OnPersonNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsLRResident", DbType="Bit NOT NULL")]
		public bool IsLRResident
		{
			get
			{
				return this._IsLRResident;
			}
			set
			{
				if ((this._IsLRResident != value))
				{
					this.OnIsLRResidentChanging(value);
					this.SendPropertyChanging();
					this._IsLRResident = value;
					this.SendPropertyChanged("IsLRResident");
					this.OnIsLRResidentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IncomeTaxRate", DbType="Int NOT NULL")]
		public int IncomeTaxRate
		{
			get
			{
				return this._IncomeTaxRate;
			}
			set
			{
				if ((this._IncomeTaxRate != value))
				{
					this.OnIncomeTaxRateChanging(value);
					this.SendPropertyChanging();
					this._IncomeTaxRate = value;
					this.SendPropertyChanged("IncomeTaxRate");
					this.OnIncomeTaxRateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsInsolvent", DbType="Bit NOT NULL")]
		public bool IsInsolvent
		{
			get
			{
				return this._IsInsolvent;
			}
			set
			{
				if ((this._IsInsolvent != value))
				{
					this.OnIsInsolventChanging(value);
					this.SendPropertyChanging();
					this._IsInsolvent = value;
					this.SendPropertyChanged("IsInsolvent");
					this.OnIsInsolventChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Address", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Address
		{
			get
			{
				return this._Address;
			}
			set
			{
				if ((this._Address != value))
				{
					this.OnAddressChanging(value);
					this.SendPropertyChanging();
					this._Address = value;
					this.SendPropertyChanged("Address");
					this.OnAddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReceiveNewsletter", DbType="Bit NOT NULL")]
		public bool ReceiveNewsletter
		{
			get
			{
				return this._ReceiveNewsletter;
			}
			set
			{
				if ((this._ReceiveNewsletter != value))
				{
					this.OnReceiveNewsletterChanging(value);
					this.SendPropertyChanging();
					this._ReceiveNewsletter = value;
					this.SendPropertyChanged("ReceiveNewsletter");
					this.OnReceiveNewsletterChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsIncompleteInformation", DbType="Bit NOT NULL")]
		public bool IsIncompleteInformation
		{
			get
			{
				return this._IsIncompleteInformation;
			}
			set
			{
				if ((this._IsIncompleteInformation != value))
				{
					this.OnIsIncompleteInformationChanging(value);
					this.SendPropertyChanging();
					this._IsIncompleteInformation = value;
					this.SendPropertyChanged("IsIncompleteInformation");
					this.OnIsIncompleteInformationChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PersonalCodeNmr", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string PersonalCodeNmr
		{
			get
			{
				return this._PersonalCodeNmr;
			}
			set
			{
				if ((this._PersonalCodeNmr != value))
				{
					this.OnPersonalCodeNmrChanging(value);
					this.SendPropertyChanging();
					this._PersonalCodeNmr = value;
					this.SendPropertyChanged("PersonalCodeNmr");
					this.OnPersonalCodeNmrChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="LegalStatus_Person", Storage="_LegalStatus", ThisKey="LegalStatusId", OtherKey="Id", IsForeignKey=true)]
		public LegalStatus LegalStatus
		{
			get
			{
				return this._LegalStatus.Entity;
			}
			set
			{
				LegalStatus previousValue = this._LegalStatus.Entity;
				if (((previousValue != value) 
							|| (this._LegalStatus.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._LegalStatus.Entity = null;
						previousValue.Persons.Remove(this);
					}
					this._LegalStatus.Entity = value;
					if ((value != null))
					{
						value.Persons.Add(this);
						this._LegalStatusId = value.Id;
					}
					else
					{
						this._LegalStatusId = default(int);
					}
					this.SendPropertyChanged("LegalStatus");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Department")]
	public partial class Department : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private System.Nullable<int> _ParentId;
		
		private string _Address;
		
		private string _Name;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnParentIdChanging(System.Nullable<int> value);
    partial void OnParentIdChanged();
    partial void OnAddressChanging(string value);
    partial void OnAddressChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    #endregion
		
		public Department()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ParentId", DbType="Int")]
		public System.Nullable<int> ParentId
		{
			get
			{
				return this._ParentId;
			}
			set
			{
				if ((this._ParentId != value))
				{
					this.OnParentIdChanging(value);
					this.SendPropertyChanging();
					this._ParentId = value;
					this.SendPropertyChanged("ParentId");
					this.OnParentIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Address", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Address
		{
			get
			{
				return this._Address;
			}
			set
			{
				if ((this._Address != value))
				{
					this.OnAddressChanging(value);
					this.SendPropertyChanging();
					this._Address = value;
					this.SendPropertyChanged("Address");
					this.OnAddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
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
}
#pragma warning restore 1591
