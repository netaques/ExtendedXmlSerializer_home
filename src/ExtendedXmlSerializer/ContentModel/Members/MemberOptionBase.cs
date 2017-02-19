// MIT License
// 
// Copyright (c) 2016 Wojciech Nag�rski
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
using System.Reflection;
using ExtendedXmlSerialization.ContentModel.Content;
using ExtendedXmlSerialization.Core.Sources;
using ExtendedXmlSerialization.Core.Specifications;
using ExtendedXmlSerialization.TypeModel;

namespace ExtendedXmlSerialization.ContentModel.Members
{
	public abstract class MemberOptionBase : OptionBase<MemberInformation, IMember>, IMemberOption
	{
		readonly ISerializers _serializers;
		readonly IAliases<MemberInfo> _alias;
		readonly IGetterFactory _getter;

		protected MemberOptionBase(IMemberSpecification specification, ISerializers serializers)
			: this(specification, serializers, MemberAliases.Default, GetterFactory.Default) {}

		protected MemberOptionBase(IMemberSpecification specification, ISerializers serializers, IAliases<MemberInfo> alias,
		                           IGetterFactory getter) : base(specification)
		{
			_serializers = serializers;
			_alias = alias;
			_getter = getter;
		}

		public override IMember Get(MemberInformation parameter)
		{
			var getter = _getter.Get(parameter.Metadata);
			var body = _serializers.Content(parameter.MemberType);
			var result = Create(AssignedSpecification.Default, _alias.Get(parameter.Metadata) ?? parameter.Metadata.Name,
			                    parameter.MemberType, getter, body, parameter.Metadata);
			return result;
		}

		protected abstract IMember Create(ISpecification<object> emit, string displayName, TypeInfo classification,
		                                  Func<object, object> getter,
		                                  ISerializer body, MemberInfo metadata);
	}
}