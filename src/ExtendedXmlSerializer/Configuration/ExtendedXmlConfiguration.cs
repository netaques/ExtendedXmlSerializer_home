﻿// MIT License
// 
// Copyright (c) 2016 Wojciech Nagórski
//                    Michael DeMond
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using ExtendedXmlSerialization.Conversion.Xml;
using ExtendedXmlSerialization.ElementModel;
using ExtendedXmlSerialization.TypeModel;

namespace ExtendedXmlSerialization.Configuration
{
	public class ExtendedXmlConfiguration : IExtendedXmlConfiguration, IInternalExtendedXmlConfiguration
	{
		readonly IXmlContextFactory _context;
		readonly IConfiguredRootConverterFactory _factory;

		public ExtendedXmlConfiguration() : this(new Namespaces(), new CollectionItemTypeLocator()) {}

		public ExtendedXmlConfiguration(INamespaces namespaces, ICollectionItemTypeLocator locator)
			: this(namespaces, locator, new AddDelegates(locator, new AddMethodLocator())) {}

		public ExtendedXmlConfiguration(INamespaces namespaces, ICollectionItemTypeLocator locator, IAddDelegates add)
			: this(
				new XmlContextFactory(new Elements(locator, add), namespaces, new Types(namespaces, new TypeContexts())),
				ConfiguredRootConverterFactory.Default) {}

		public ExtendedXmlConfiguration(IXmlContextFactory context, IConfiguredRootConverterFactory factory)
		{
			_context = context;
			_factory = factory;
		}

		public bool AutoProperties { get; set; }
		public bool Namespaces { get; set; }
		public IPropertyEncryption EncryptionAlgorithm { get; set; }

		IExtendedXmlTypeConfiguration IInternalExtendedXmlConfiguration.GetTypeConfiguration(Type type)
		{
			return _cache.ContainsKey(type) ? _cache[type] : null;
		}

		readonly Dictionary<Type, IExtendedXmlTypeConfiguration> _cache =
			new Dictionary<Type, IExtendedXmlTypeConfiguration>();

		public IExtendedXmlTypeConfiguration<T> ConfigureType<T>()
		{
			var configType = new ExtendedXmlTypeConfiguration<T>();

			_cache.Add(typeof(T), configType);
			return configType;
		}

		public IExtendedXmlConfiguration UseAutoProperties()
		{
			AutoProperties = true;
			return this;
		}

		public IExtendedXmlConfiguration UseNamespaces()
		{
			Namespaces = true;
			return this;
		}

		public IExtendedXmlConfiguration UseEncryptionAlgorithm(IPropertyEncryption propertyEncryption)
		{
			EncryptionAlgorithm = propertyEncryption;
			return this;
		}

		public IExtendedXmlSerializer Create() => new ExtendedXmlSerializer(_context, _factory.Get(this));
	}
}