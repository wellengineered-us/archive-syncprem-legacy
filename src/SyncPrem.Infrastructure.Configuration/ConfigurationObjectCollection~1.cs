/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SyncPrem.Infrastructure.Configuration
{
	public class ConfigurationObjectCollection<TItemConfigurationObject> : Collection<TItemConfigurationObject>, IConfigurationObjectCollection<TItemConfigurationObject>, IConfigurationObjectCollection
		where TItemConfigurationObject : IConfigurationObject
	{
		#region Constructors/Destructors

		public ConfigurationObjectCollection(IConfigurationObject site)
		{
			if ((object)site == null)
				throw new ArgumentNullException(nameof(site));

			this.site = site;
		}

		#endregion

		#region Fields/Constants

		private readonly IConfigurationObject site;

		#endregion

		#region Properties/Indexers/Events

		[ConfigurationIgnore]
		public IConfigurationObject Site
		{
			get
			{
				return this.site;
			}
		}

		#endregion

		#region Methods/Operators

		public void AddRange(IEnumerable<TItemConfigurationObject> items)
		{
			if ((object)items == null)
				throw new ArgumentNullException(nameof(items));

			foreach (TItemConfigurationObject item in items)
			{
				this.Add(item);
			}
		}

		/// <summary>
		/// Removes all elements from the collection.
		/// </summary>
		protected override void ClearItems()
		{
			foreach (TItemConfigurationObject item in this.Items)
			{
				item.Surround = null;
				item.Parent = null;
			}

			base.ClearItems();
		}

		/// <summary>
		/// Inserts an element into the collection at the specified index.
		/// </summary>
		/// <param name="index"> The zero-based index at which item should be inserted. </param>
		/// <param name="item"> The object to insert. The value can be null for reference types. </param>
		protected override void InsertItem(int index, TItemConfigurationObject item)
		{
			if ((object)item == null)
				throw new ArgumentNullException(nameof(item));

			item.Surround = this;
			item.Parent = this.Site;

			base.InsertItem(index, item);
		}

		/// <summary>
		/// Removes the element at the specified index of the collection.
		/// </summary>
		/// <param name="index"> The zero-based index of the element to remove. </param>
		protected override void RemoveItem(int index)
		{
			TItemConfigurationObject item;

			item = base[index];

			if ((object)item != null)
			{
				item.Surround = null;
				item.Parent = null;
			}

			base.RemoveItem(index);
		}

		/// <summary>
		/// Replaces the element at the specified index.
		/// </summary>
		/// <param name="index"> The zero-based index of the element to replace. </param>
		/// <param name="item"> The new value for the element at the specified index. The value can be null for reference types. </param>
		protected override void SetItem(int index, TItemConfigurationObject item)
		{
			if ((object)item == null)
				throw new ArgumentNullException(nameof(item));

			item.Surround = this;
			item.Parent = this.Site;

			base.SetItem(index, item);
		}

		#endregion
	}
}