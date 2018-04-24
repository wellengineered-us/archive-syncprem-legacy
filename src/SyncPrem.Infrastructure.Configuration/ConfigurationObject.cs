/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Configuration
{
	/// <summary>
	/// Provides a base for all configuration objects.
	/// </summary>
	public abstract class ConfigurationObject : IConfigurationObject
	{
		#region Constructors/Destructors

		protected ConfigurationObject(IConfigurationObjectCollection<IConfigurationObject> items)
		{
			if ((object)items == null)
				throw new ArgumentNullException(nameof(items));

			this.items = items;
		}

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

		protected static Type GetTypeFromString(string aqtn, IList<Message> messages = null)
		{
			Type type = null;

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(aqtn))
				return null;

			if ((object)messages == null)
				type = Type.GetType(aqtn, false);
			else
			{
				try
				{
					type = Type.GetType(aqtn, true);
				}
				catch (Exception ex)
				{
					messages.Add(NewError(string.Format("Error loading type '{0}' from string: {1}.", aqtn, ex.Message)));
				}
			}

			return type;
		}

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

		public virtual IEnumerable<Message> Validate(object context)
		{
			return new Message[] { };
		}

		public Task<IEnumerable<Message>> ValidateAsync()
		{
			return this.ValidateAsync(null);
		}

		public virtual Task<IEnumerable<Message>> ValidateAsync(object context)
		{
			IEnumerable<Message> messages;
			messages = new Message[] { };
			return Task.FromResult(messages);
		}

		#endregion
	}
}