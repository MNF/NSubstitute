using System;
using NSubstitute.Core;
using NSubstitute.Proxies;
using NSubstitute.Specs.Infrastructure;
using NSubstitute.Specs.SampleStructures;
using NUnit.Framework;

namespace NSubstitute.Specs.Proxies
{
    public class ProxyFactorySpecs
    {
        public class Concern : ConcernFor<ProxyFactory>
        {
            protected IProxyFactory _delegateFactory;
            protected IProxyFactory _dynamicProxyFactory;
            protected ICallRouter _callRouter;

            public override void Context()
            {                
                _delegateFactory = mock<IProxyFactory>();
                _dynamicProxyFactory = mock<IProxyFactory>();
                _callRouter = mock<ICallRouter>();
            }

            public override ProxyFactory CreateSubjectUnderTest()
            {
                return new ProxyFactory(_delegateFactory, _dynamicProxyFactory);
            }
        }

        public class When_proxying_a_delegate_type : Concern
        {
            private Func<int, string> _delegateProxy;
            private Func<int, string> _result;

            [Test]
            public void Should_get_delegate_proxy()
            {
                Assert.That(_result, Is.SameAs(_delegateProxy));
            }

            public override void Because()
            {
                _result = sut.GenerateProxy<Func<int, string>>(_callRouter);
            }

            public override void Context()
            {
                base.Context();
                _delegateProxy = x => x.ToString();
                _delegateFactory.stub(x => x.GenerateProxy<Func<int, string>>(_callRouter)).Return(_delegateProxy);
            }
        }
        
        public class When_proxying_a_non_delegate_type : Concern
        {
            private object _dynamicProxy;
            private object _result;

            [Test]
            public void Should_get_dynamic_proxy()
            {
                Assert.That(_result, Is.SameAs(_dynamicProxy));
            }

            public override void Because()
            {
                _result = sut.GenerateProxy<object>(_callRouter);
            }

            public override void Context()
            {
                base.Context();
                _dynamicProxy = new object();
                _dynamicProxyFactory.stub(x => x.GenerateProxy<object>(_callRouter)).Return(_dynamicProxy);
            }
        }
    }
}