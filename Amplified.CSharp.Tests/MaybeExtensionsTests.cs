﻿using Amplified.CSharp.Extensions;
using Amplified.CSharp.Util;
using Xunit;
using static Amplified.CSharp.Maybe;

namespace Amplified.CSharp
{
    public class MaybeExtensionsTests
    {
        [Fact]
        public void Map_OnSome_ReturnsMappedResult()
        {
            var expected = Some(2);
            var result = Some(1).Map(v => v + 1);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Map_OnNone()
        {
            var result = None().AsMaybe<int>().Map(_ => Fail.With<int>());
            result.AssertIsNone();
        }

        [Fact]
        public void FlatMap_OnSome()
        {
            var expected = Some(2);
            var result = Some(1).FlatMap(v => expected);
            Assert.Equal(expected, result);
            Assert.IsType<Maybe<int>>(result);
        }

        [Fact]
        public void FlatMap_OnNone()
        {
            var result = None().AsMaybe<int>().FlatMap(v => Fail.With<Maybe<double>>());
            result.AssertIsNone();
        }

        [Fact]
        public void Filter_OnSome_ReturningFalse_IsNone()
        {
            var result = Some(1).Filter(v => false);
            result.AssertIsNone();
        }

        [Fact]
        public void Filter_OnSome_ReturningTrue_HasSameValue()
        {
            var maybe = Some(new object());
            var filtered = maybe.Filter(v => true);
            Assert.Same(maybe.OrFail(), filtered.OrFail());
        }

        [Fact]
        public void Filter_OnNone_ReturningFalse_IsNone()
        {
            var filtered = None().AsMaybe<object>().Filter(v => false);
            filtered.AssertIsNone();
        }

        [Fact]
        public void Filter_OnNone_ReturningTrue_IsNone()
        {
            var filtered = None().AsMaybe<object>().Filter(v => true);
            filtered.AssertIsNone();
        }

        [Fact]
        public void Zip_OnSome_AndSome_ReturnsSome()
        {
            var first = Some(1);
            var second = Some(true);
            var result = first.Zip(second, (f, s) => new { f, s });
            result.AssertIsSome();
        }

        [Fact]
        public void Zip_OnSome_AndSome_ReturnsSomeOfBothValues()
        {
            var first = Some(new object());
            var second = Some(new object());
            var result = first.Zip(second, (f, s) => new { f, s });
            Assert.Equal(first, result.Map(t => t.f));
            Assert.Equal(second, result.Map(t => t.s));
        }

        [Fact]
        public void Zip_OnNone_AndNone_DoesNotInvokeZipper()
        {
            var first = Maybe<int>.None;
            var second = Maybe<object>.None;

            var zipped = false;
            first.Zip(
                second,
                (f, s) =>
                {
                    zipped = true;
                    return new { f, s };
                }
            );

            Assert.False(zipped);
        }

        [Fact]
        public void Zip_OnNone_AndNone_ReturnsNone()
        {
            var first = Maybe<int>.None;
            var second = Maybe<object>.None;
            var result = first.Zip(second, (f, s) => new { f, s });
            result.AssertIsNone();
        }

        [Fact]
        public void Zip_OnNone_AndSome_ReturnsNone()
        {
            var first = Some(100);
            var second = Maybe<object>.None;
            var result = first.Zip(second, (f, s) => new { f, s });
            result.AssertIsNone();
        }

        [Fact]
        public void Zip_OnSome_AndNone_ReturnsNone()
        {
            var first = Maybe<object>.None;
            var second = Some(100);
            var result = first.Zip(second, (f, s) => new { f, s });
            result.AssertIsNone();
        }

        [Fact]
        public void OrReturn_OnSome_ReturnsValueOfSome()
        {
            const int value = 106879;
            var some = Some(value);
            var result = some.OrReturn(0);
            Assert.Equal(value, result);
        }

        [Fact]
        public void OrReturn_OnNone_ReturnsArgumentValue()
        {
            const int alternativeValue = 3780;
            var none = Maybe<int>.None;
            var result = none.OrReturn(alternativeValue);
            Assert.Equal(alternativeValue, result);
        }

        [Fact]
        public void OrDefault_OnSome_ReturnsValueOfSome()
        {
            var value = new object();
            var some = Some(value);
            var result = some.OrDefault();
            Assert.Same(value, result);
        }

        [Fact]
        public void OrDefault_OnNone_ReturnsDefaultValue()
        {
            var none = Maybe<object>.None;
            var result = none.OrDefault();
            Assert.Equal(default(object), result);
        }

        [Fact]
        public void OrGet_OnSome_ReturnsValueOfSome()
        {
            var value = new object();
            var some = Some(value);
            var result = some.OrGet(() => null);
            Assert.Same(value, result);
        }

        [Fact]
        public void OrGet_OnNone_ReturnsValueOfLambda()
        {
            var expected = new object();
            var none = Maybe<object>.None;
            var result = none.OrGet(() => expected);
            Assert.Same(expected, result);
        }

        [Fact]
        public void OrThrow_OnSome_ReturnsValueOfSome()
        {
            var value = new object();
            var some = Some(value);
            var result = some.OrThrow(new DummyException());
            Assert.Same(value, result);
        }

        [Fact]
        public void OrThrow_OnNone_ThrowsException()
        {
            var none = Maybe<object>.None;
            Assert.Throws<DummyException>(() => none.OrThrow(new DummyException()));
        }

        [Fact]
        public void OrThrow_Lambda_OnSome_ReturnsValueOfSome()
        {
            var value = new object();
            var some = Some(value);
            var result = some.OrThrow(() => new DummyException());
            Assert.Same(value, result);
        }

        [Fact]
        public void OrThrow_Lambda_OnNone_ThrowsException()
        {
            var none = Maybe<object>.None;
            Assert.Throws<DummyException>(() => none.OrThrow(() => new DummyException()));
        }

    }
}