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
using System.Xml;
using ExtendedXmlSerializer.Configuration;
using FluentAssertions;

namespace ExtendedXmlSerializer.Tests.Support
{
	sealed class SerializationSupport : ISerializationSupport
	{
		readonly IExtendedXmlSerializer _serializer;

		public SerializationSupport() : this(new ConfigurationContainer()) {}

		public SerializationSupport(IConfigurationContainer configuration) : this(configuration.Create()) {}

		public SerializationSupport(IExtendedXmlSerializer serializer)
		{
			_serializer = serializer;
		}

		public T Assert<T>(T instance, string expected)
		{
			var data = _serializer.Serialize(instance);
			data?.Replace("\r\n", "").Replace("\n", "").Should().Be(expected?.Replace("\r\n", "").Replace("\n", ""));
			var result = _serializer.Deserialize<T>(data);
			return result;
		}

		public void Serialize(XmlWriter writer, object instance) => _serializer.Serialize(writer, instance);

		public object Deserialize(XmlReader stream) => _serializer.Deserialize(stream);

		public void WriteLine(object instance)
		{
			// https://github.com/aspnet/Tooling/issues/324#issuecomment-275236780
			throw new InvalidOperationException(_serializer.Serialize(instance));
		}
	}
}