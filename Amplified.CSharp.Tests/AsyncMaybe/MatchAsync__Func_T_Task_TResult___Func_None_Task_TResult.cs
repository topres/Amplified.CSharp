using System.Threading.Tasks;
using Xunit;

namespace Amplified.CSharp
{
    // ReSharper disable once InconsistentNaming
    public class AsyncMaybe__MatchAsync__Func_T_Task_TResult___Func_None_Task_TResult
    {
        [Fact]
        public async Task NoneValue_WithLambdas()
        {
            var invocations = 0;
            var source = AsyncMaybe<int>.None();
            var result = await source.MatchAsync(some => Task.FromResult(some + 1), none => { invocations++; return Task.FromResult(0); });
            Assert.Equal(0, result);
            Assert.Equal(1, invocations);
        }
        
        [Fact]
        public async Task WithLambdas()
        {
            var source = AsyncMaybe<int>.Some(1);
            var result = await source.MatchAsync(some => Task.FromResult(some + 1), none => Task.FromResult(0));
            Assert.Equal(2, result);
        }
        
        [Fact]
        public async Task WithSomeLambda_AndNoneReference()
        {
            Task<int> MatchNone(None none) => Task.FromResult(0);
            
            var source = AsyncMaybe<int>.Some(1);
            var result = await source.MatchAsync(some => Task.FromResult(some + 1), MatchNone);
            Assert.Equal(2, result);
        }
        
        [Fact]
        public async Task WithSomeReference_AndNoneLambda()
        {
            Task<int> MatchSome(int some) => Task.FromResult(some + 1);
            
            var source = AsyncMaybe<int>.Some(1);
            var result = await source.MatchAsync(MatchSome, none => Task.FromResult(0));
            Assert.Equal(2, result);
        }
        
        [Fact]
        public async Task WithReferences()
        {
            Task<int> MatchSome(int some) => Task.FromResult(some + 1);
            Task<int> MatchNone(None none) => Task.FromResult(0);
            
            var source = AsyncMaybe<int>.Some(1);
            var result = await source.MatchAsync(MatchSome, MatchNone);
            Assert.Equal(2, result);
        }
    }
}
