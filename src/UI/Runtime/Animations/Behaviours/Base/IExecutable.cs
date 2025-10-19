using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Nk7.UI.Animations
{
    public interface IExecutable
    {
        void Execute(Container animatedContainer);
    }

    public interface IAsyncExecutable
    {
        UniTask ExecuteAsync(Container animatedContainer, CancellationToken cancellationToken = default,
            Action onStartCallback = null, Action onFinishCallback = null);
    }

    public interface IInstantlyExecutable
    {
        void ExecuteInstantly(Container animatedContainer);
    }
}
