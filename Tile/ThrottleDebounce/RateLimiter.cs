#nullable enable

using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace ThrottleDebounce;

internal partial class RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>
{

    private Delegate rateLimitedCallback;
    private readonly bool leading;
    private readonly bool trailing;
    private readonly Timer minTimer;
    private readonly Timer? maxTimer;

    private int queuedInvocations;
    private object[]? mostRecentInvocationParameters;
    private TResult? mostRecentResult;
    private bool disposed;

    /// <exception cref="ArgumentException">if <paramref name="leading"/> and <paramref name="trailing"/> were both <see langword="false"/>, or if <paramref name="maxWait"/> is non-positive</exception>
    internal RateLimiter(Delegate rateLimitedCallback, TimeSpan wait, bool leading, bool trailing, TimeSpan maxWait = default)
    {
        if (!leading && !trailing)
        {
            throw new ArgumentException("One or both of the leading and trailing arguments must be true, but both were false.");
        }
        else if (TimeSpan.Zero.Equals(wait))
        {
            throw new ArgumentException("The wait argument must have a positive duration.");
        }
        else if (maxWait < TimeSpan.Zero)
        {
            throw new ArgumentException("The maxWait argument must not have a negative duration.");
        }

        this.rateLimitedCallback = rateLimitedCallback;
        this.leading             = leading;
        this.trailing            = trailing;

        minTimer         =  new Timer { AutoReset = false, Interval = wait.TotalMilliseconds };
        minTimer.Elapsed += delegate { WaitTimeHasElapsed(); };

        if (maxWait != default)
        {
            maxTimer         =  new Timer { AutoReset = false, Interval = maxWait.TotalMilliseconds };
            maxTimer.Elapsed += delegate { WaitTimeHasElapsed(); };
        }
    }

    public void Update(Delegate rateLimitedCallback)
    {
        this.rateLimitedCallback=rateLimitedCallback;
    }

    private void WaitTimeHasElapsed()
    {
        if (Interlocked.Exchange(ref queuedInvocations, 0) > 0 && !disposed)
        {
            mostRecentResult = (TResult)rateLimitedCallback.DynamicInvoke(mostRecentInvocationParameters);

            resetTimers();
        }
    }

    private TResult? OnUserInvocation(object[] arguments)
    {
        if (!disposed)
        {
            mostRecentInvocationParameters = arguments;

            bool minTimerRunning = minTimer.Enabled;
            if (leading && !minTimerRunning)
            {
                mostRecentResult = (TResult)rateLimitedCallback.DynamicInvoke(arguments);
            }
            else if (trailing)
            {
                Interlocked.Add(ref queuedInvocations, 1);
            }

            resetTimers();
        }

        return mostRecentResult;
    }

    private void resetTimers()
    {
        try
        {
            minTimer.Stop();
            minTimer.Start();
            maxTimer?.Start();
        }
        catch (ObjectDisposedException)
        {
            //Do nothing. Don't try to start timers if they have been concurrently disposed of in another thread.
        }
    }

    public void Dispose()
    {
        disposed = true;
        minTimer.Dispose();
        maxTimer?.Dispose();
        queuedInvocations              = 0;
        mostRecentResult               = default;
        mostRecentInvocationParameters = null;
    }

}