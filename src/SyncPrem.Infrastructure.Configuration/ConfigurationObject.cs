/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Configuration
{
	/// <summary>
	/// Provides a base for all configuration objects.
	/// </summary>
	public abstract class ConfigurationObject : IConfigurationObject
	{
		#region Constructors/Destructors

		protected ConfigurationObject()
		{
			this.items = new ConfigurationObjectCollection<IConfigurationObject>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly IConfigurationObjectCollection<IConfigurationObject> items;
		private IConfigurationObject content;
		private IConfigurationObject parent;
		private IConfigurationObjectCollection surround;

		#endregion

		#region Properties/Indexers/Events

		[ConfigurationIgnore]
		public virtual Type[] AllowedChildTypes
		{
			get
			{
				return new Type[] { typeof(IConfigurationObject) };
			}
		}

		[ConfigurationIgnore]
		public IConfigurationObjectCollection<IConfigurationObject> Items
		{
			get
			{
				return this.items;
			}
		}

		[ConfigurationIgnore]
		public IConfigurationObject Content
		{
			get
			{
				return this.content;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.content, value);
				this.content = value;
			}
		}

		[ConfigurationIgnore]
		public IConfigurationObject Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		[ConfigurationIgnore]
		public IConfigurationObjectCollection Surround
		{
			get
			{
				return this.surround;
			}
			set
			{
				this.surround = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected static Message NewError(string description)
		{
			return new Message(string.Empty, description, Severity.Error);
		}

		/// <summary>
		/// Ensures that for any configuration object property, the correct parent instance is set/unset.
		/// Should be called in the setter for all configuration object properties, before assigning the value.
		/// Example:
		/// set { this.EnsureParentOnPropertySet(this.content, value); this.content = value; }
		/// </summary>
		/// <param name="oldValueObj"> The old configuration object value (the backing field). </param>
		/// <param name="newValueObj"> The new configuration object value (value). </param>
		protected void EnsureParentOnPropertySet(IConfigurationObject oldValueObj, IConfigurationObject newValueObj)
		{
			if ((object)oldValueObj != null)
			{
				oldValueObj.Surround = null;
				oldValueObj.Parent = null;
			}

			if ((object)newValueObj != null)
			{
				newValueObj.Surround = null;
				newValueObj.Parent = this;
			}
		}

		public IEnumerable<Message> Validate()
		{
			return this.Validate(null);
		}

		public abstract IEnumerable<Message> Validate(object context);

		#endregion
	}
}