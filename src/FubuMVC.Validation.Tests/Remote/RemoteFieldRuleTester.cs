using System;
using System.Linq.Expressions;
using FubuCore.Reflection;
using FubuMVC.Validation.Remote;
using FubuTestingSupport;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.Remote
{
    [TestFixture]
    public class RemoteFieldRuleTester
    {
        private Accessor accessorFor(Expression<Func<RemoteFieldModel, object>> expression)
        {
            return expression.ToAccessor();
        }

        [Test]
        public void equality_check()
        {
            var accessor = accessorFor(x => x.Name);

	        var r1 = RemoteFieldRule.For<RequiredFieldRule>(accessor);
			var r2 = RemoteFieldRule.For<RequiredFieldRule>(accessor);

            r1.ShouldEqual(r2);
        }

        [Test]
        public void equality_check_negative_accessor()
        {
            var r1 = RemoteFieldRule.For<RequiredFieldRule>(accessorFor(x => x.Name));
            var r2 = RemoteFieldRule.For<RequiredFieldRule>(accessorFor(x => x.Test));

            r1.ShouldNotEqual(r2);
        }

        [Test]
        public void equality_check_negative_type()
        {
            var accessor = accessorFor(x => x.Name);

			var r1 = RemoteFieldRule.For<RequiredFieldRule>(accessor);
			var r2 = RemoteFieldRule.For<EmailFieldRule>(accessor);

            r1.ShouldNotEqual(r2);
        }

        [Test]
        public void hash_is_repeatable()
        {
			var r1 = RemoteFieldRule.For<RequiredFieldRule>(accessorFor(x => x.Name));
			var r2 = RemoteFieldRule.For<RequiredFieldRule>(accessorFor(x => x.Name));

            r1.ToHash().ShouldEqual(r2.ToHash());
        }

        [Test]
        public void hash_is_unique_by_rule_model_and_accessor()
        {
            var r1 = RemoteFieldRule.For<RequiredFieldRule>(SingleProperty.Build<SomeNamespace.Model>(e => e.Property));
			var r2 = RemoteFieldRule.For<RequiredFieldRule>(SingleProperty.Build<OtherNamespace.Model>(e => e.Property));

            r1.ToHash().ShouldNotEqual(r2.ToHash());
        }

        public class RemoteFieldModel
        {
            public string Name { get; set; }
            public string Test { get; set; }
        }
    }

    namespace SomeNamespace
    {
        public class Model
        {
            public string Property { get; set; }
        }
    }

    namespace OtherNamespace
    {
        public class Model
        {
            public string Property { get; set; }
        }
    }

}
