#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThrottleDebounce;

/// <summary>
/// Run a delegate with retries if it throws an exception.
/// </summary>
public static class Retrier {

    // ExceptionAdjustment: M:System.TimeSpan.FromMilliseconds(System.Double) -T:System.OverflowException
    // These are the specific maximum values that don't overflow, tailored for the runtime version.
    private static readonly TimeSpan MaxDelay = TimeSpan.FromMilliseconds(Environment.Version.Major >= 6 ? uint.MaxValue - 1 : int.MaxValue);

    /// <summary>
    /// Run the given <paramref name="action"/> at most <paramref name="maxAttempts"/> times, until it returns without throwing an exception.
    /// </summary>
    /// <param name="action">An action which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="maxAttempts">The total number of times <paramref name="action"/> is allowed to run in this invocation, equal to <c>1</c> initial attempt plus up to <c>maxAttempts - 1</c> retries if it throws an exception. Must be at least 1, if you pass 0 it will clip to 1. Defaults to 2. For infinite retries, pass <see langword="null"/>.</param>
    /// <param name="delay">How long to wait between attempts. Defaults to <see langword="null"/>, which means no delay. This is a function of how many attempts have already failed (starting from <c>1</c>), to allow for strategies such as exponential back-off. Values outside the range <c>[0, int.MaxValue]</c> ms will be clipped (<c>[0, uint.MaxValue-1]</c> ms starting in .NET 6).</param>
    /// <param name="isRetryAllowed">Allows certain exceptions that indicate permanent failures to not trigger retries. For example, <see cref="ArgumentOutOfRangeException"/> will usually be thrown every time you call a function with the same arguments, so there is no reason to retry, and <paramref name="isRetryAllowed"/> could return <c>false</c> in that case. Defaults to retrying on every exception besides <see cref="OutOfMemoryException"/>.</param>
    /// <param name="beforeRetry">Action to run between attempts, possibly to clean up some state before the next retry. For example, you may want to disconnect a failed connection before reconnecting. Defaults to no action.</param>
    /// <param name="cancellationToken">Allows you to cancel remaining attempts and delays.</param>
    /// <exception cref="TaskCanceledException">If the <paramref name="cancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="action"/> on its final attempt.</exception>
    public static void Attempt(Action<int>            action,
                               uint?                  maxAttempts       = 2,
                               Func<int, TimeSpan>?   delay             = null,
                               Func<Exception, bool>? isRetryAllowed    = null,
                               Action?                beforeRetry       = null,
                               CancellationToken      cancellationToken = default) {
        int attempt;
        for (attempt = 0; (!maxAttempts.HasValue || attempt < maxAttempts - 1) && !cancellationToken.IsCancellationRequested; attempt++) {
            try {
                action.Invoke(attempt);
                return;
            } catch (Exception e) when (e is not OutOfMemoryException) {
                // This brace-less syntax convinces dotCover that the "throw;" statement is fully covered.
                if (!isRetryAllowed?.Invoke(e) ?? false) throw;

                if (GetDelay(delay, attempt) is { } duration) {
                    Task.Delay(duration, cancellationToken).GetAwaiter().GetResult();
                }

                beforeRetry?.Invoke();
            }
        }

        if (!cancellationToken.IsCancellationRequested) {
            action.Invoke(attempt);
        } else {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        }
    }

    /// <summary>
    /// Run the given <paramref name="func"/> at most <paramref name="maxAttempts"/> times, until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">An action which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="maxAttempts">The total number of times <paramref name="func"/> is allowed to run in this invocation, equal to <c>1</c> initial attempt plus up to <c>maxAttempts - 1</c> retries if it throws an exception. Must be at least 1, if you pass 0 it will clip to 1. Defaults to 2. For infinite retries, pass <see langword="null"/>.</param>
    /// <param name="delay">How long to wait between attempts. Defaults to <see langword="null"/>, which means no delay. This is a function of how many attempts have already failed (starting from <c>1</c>), to allow for strategies such as exponential back-off. Values outside the range <c>[0, int.MaxValue]</c> ms will be clipped (<c>[0, uint.MaxValue-1]</c> ms starting in .NET 6).</param>
    /// <param name="isRetryAllowed">Allows certain exceptions that indicate permanent failures to not trigger retries. For example, <see cref="ArgumentOutOfRangeException"/> will usually be thrown every time you call a function with the same arguments, so there is no reason to retry, and <paramref name="isRetryAllowed"/> could return <c>false</c> in that case. Defaults to retrying on every exception besides <see cref="OutOfMemoryException"/>.</param>
    /// <param name="beforeRetry">Action to run between attempts, possibly to clean up some state before the next retry. For example, you may want to disconnect a failed connection before reconnecting. Defaults to no action.</param>
    /// <param name="cancellationToken">Allows you to cancel remaining attempts and delays.</param>
    /// <exception cref="TaskCanceledException">If the <paramref name="cancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static T Attempt<T>(Func<int, T>           func,
                               uint?                  maxAttempts       = 2,
                               Func<int, TimeSpan>?   delay             = null,
                               Func<Exception, bool>? isRetryAllowed    = null,
                               Action?                beforeRetry       = null,
                               CancellationToken      cancellationToken = default) {
        int attempt;
        for (attempt = 0; (!maxAttempts.HasValue || attempt < maxAttempts - 1) && !cancellationToken.IsCancellationRequested; attempt++) {
            try {
                return func.Invoke(attempt);
            } catch (Exception e) when (e is not OutOfMemoryException) {
                if (!isRetryAllowed?.Invoke(e) ?? false) throw;

                if (GetDelay(delay, attempt) is { } duration) {
                    Task.Delay(duration, cancellationToken).GetAwaiter().GetResult();
                }

                beforeRetry?.Invoke();
            }
        }

        if (!cancellationToken.IsCancellationRequested) {
            return func.Invoke(attempt);
        } else {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        }
    }

    /// <summary>
    /// Run the given <paramref name="func"/> at most <paramref name="maxAttempts"/> times, until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">An action which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="maxAttempts">The total number of times <paramref name="func"/> is allowed to run in this invocation, equal to <c>1</c> initial attempt plus up to <c>maxAttempts - 1</c> retries if it throws an exception. Must be at least 1, if you pass 0 it will clip to 1. Defaults to 2. For infinite retries, pass <see langword="null"/>.</param>
    /// <param name="delay">How long to wait between attempts. Defaults to <see langword="null"/>, which means no delay. This is a function of how many attempts have already failed (starting from <c>1</c>), to allow for strategies such as exponential back-off. Values outside the range <c>[0, int.MaxValue]</c> ms will be clipped (<c>[0, uint.MaxValue-1]</c> ms starting in .NET 6).</param>
    /// <param name="isRetryAllowed">Allows certain exceptions that indicate permanent failures to not trigger retries. For example, <see cref="ArgumentOutOfRangeException"/> will usually be thrown every time you call a function with the same arguments, so there is no reason to retry, and <paramref name="isRetryAllowed"/> could return <c>false</c> in that case. Defaults to retrying on every exception besides <see cref="OutOfMemoryException"/>.</param>
    /// <param name="beforeRetry">Action to run between attempts, possibly to clean up some state before the next retry. For example, you may want to disconnect a failed connection before reconnecting. Defaults to no action.</param>
    /// <param name="cancellationToken">Allows you to cancel remaining attempts and delays.</param>
    /// <exception cref="TaskCanceledException">If the <paramref name="cancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static async Task Attempt(Func<int, Task>        func,
                                     uint?                  maxAttempts       = 2,
                                     Func<int, TimeSpan>?   delay             = null,
                                     Func<Exception, bool>? isRetryAllowed    = null,
                                     Func<Task>?            beforeRetry       = null,
                                     CancellationToken      cancellationToken = default) {
        int attempt;
        for (attempt = 0; (!maxAttempts.HasValue || attempt < maxAttempts - 1) && !cancellationToken.IsCancellationRequested; attempt++) {
            try {
                await func.Invoke(attempt).ConfigureAwait(false);
                return;
            } catch (Exception e) when (e is not OutOfMemoryException) {
                if (!isRetryAllowed?.Invoke(e) ?? false) throw;

                if (GetDelay(delay, attempt) is { } duration) {
                    await Task.Delay(duration, cancellationToken).ConfigureAwait(false);
                }

                beforeRetry?.Invoke();
            }
        }

        if (!cancellationToken.IsCancellationRequested) {
            await func.Invoke(attempt).ConfigureAwait(false);
        } else {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        }
    }

    /// <summary>
    /// Run the given <paramref name="func"/> at most <paramref name="maxAttempts"/> times, until it returns without throwing an exception.
    /// </summary>
    /// <param name="func">An action which is prone to sometimes throw exceptions. The <see cref="int"/> argument is the number of attempts, starting from <c>0</c> for the initial attempt.</param>
    /// <param name="maxAttempts">The total number of times <paramref name="func"/> is allowed to run in this invocation, equal to <c>1</c> initial attempt plus up to <c>maxAttempts - 1</c> retries if it throws an exception. Must be at least 1, if you pass 0 it will clip to 1. Defaults to 2. For infinite retries, pass <see langword="null"/>.</param>
    /// <param name="delay">How long to wait between attempts. Defaults to <see langword="null"/>, which means no delay. This is a function of how many attempts have already failed (starting from <c>1</c>), to allow for strategies such as exponential back-off. Values outside the range <c>[0, int.MaxValue]</c> ms will be clipped (<c>[0, uint.MaxValue-1]</c> ms starting in .NET 6).</param>
    /// <param name="isRetryAllowed">Allows certain exceptions that indicate permanent failures to not trigger retries. For example, <see cref="ArgumentOutOfRangeException"/> will usually be thrown every time you call a function with the same arguments, so there is no reason to retry, and <paramref name="isRetryAllowed"/> could return <c>false</c> in that case. Defaults to retrying on every exception besides <see cref="OutOfMemoryException"/>.</param>
    /// <param name="beforeRetry">Action to run between attempts, possibly to clean up some state before the next retry. For example, you may want to disconnect a failed connection before reconnecting. Defaults to no action.</param>
    /// <param name="cancellationToken">Allows you to cancel remaining attempts and delays.</param>
    /// <exception cref="TaskCanceledException">If the <paramref name="cancellationToken"/> is cancelled.</exception>
    /// <exception cref="Exception">Any exception thrown by <paramref name="func"/> on its final attempt.</exception>
    public static async Task<T> Attempt<T>(Func<int, Task<T>>     func,
                                           uint?                  maxAttempts       = 2,
                                           Func<int, TimeSpan>?   delay             = null,
                                           Func<Exception, bool>? isRetryAllowed    = null,
                                           Func<Task>?            beforeRetry       = null,
                                           CancellationToken      cancellationToken = default) {
        int attempt;
        for (attempt = 0; (!maxAttempts.HasValue || attempt < maxAttempts - 1) && !cancellationToken.IsCancellationRequested; attempt++) {
            try {
                return await func.Invoke(attempt).ConfigureAwait(false);
            } catch (Exception e) when (e is not OutOfMemoryException) {
                if (!isRetryAllowed?.Invoke(e) ?? false) throw;

                if (GetDelay(delay, attempt) is { } duration) {
                    await Task.Delay(duration, cancellationToken).ConfigureAwait(false);
                }

                beforeRetry?.Invoke();
            }
        }

        if (!cancellationToken.IsCancellationRequested) {
            return await func.Invoke(attempt).ConfigureAwait(false);
        } else {
            throw new TaskCanceledException("Remaining Retrier attempts were cancelled");
        }
    }

    private static TimeSpan? GetDelay(Func<int, TimeSpan>? delay, int attempt) => delay?.Invoke(attempt) switch {
        { } duration when duration <= TimeSpan.Zero => null,
        { } duration when duration > MaxDelay       => MaxDelay,
        { } duration                                => duration,
        null                                        => null
    };

}