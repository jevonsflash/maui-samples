using System;

namespace ThrottleDebounce; 

public static class Throttler {

    public static RateLimitedAction Throttle(Action action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(action, wait, leading,
            trailing, wait);
    }

    public static RateLimitedAction<T1> Throttle<T1>(Action<T1> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(action, wait, leading, trailing,
            wait);
    }

    public static RateLimitedAction<T1, T2> Throttle<T1, T2>(Action<T1, T2> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(action, wait, leading, trailing,
            wait);
    }

    public static RateLimitedAction<T1, T2, T3> Throttle<T1, T2, T3>(Action<T1, T2, T3> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4> Throttle<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, object, object, object, object, object, object, object, object, object, object, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5> Throttle<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, object, object, object, object, object, object, object, object, object, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5, T6> Throttle<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, object, object, object, object, object, object, object, object, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7> Throttle<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, TimeSpan wait, bool leading = true,
                                                                                                     bool                               trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, object, object, object, object, object, object, object, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8> Throttle<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, TimeSpan wait, bool leading = true,
                                                                                                             bool                                   trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, object, object, object, object, object, object, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, object, object, object, object, object, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object, object, object, object, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object, object, object, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object, object, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, object, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, object, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, object>(action, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<TResult> Throttle<TResult>(Func<TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, wait, leading,
            trailing, wait);
    }

    public static RateLimitedFunc<T1, TResult> Throttle<T1, TResult>(Func<T1, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, wait, leading,
            trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, TResult> Throttle<T1, T2, TResult>(Func<T1, T2, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, object, object, object, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, wait, leading, trailing,
            wait);
    }

    public static RateLimitedFunc<T1, T2, T3, TResult> Throttle<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, object, object, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, TResult> Throttle<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, object, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, TResult> Throttle<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, TimeSpan wait, bool leading = true,
                                                                                                     bool                              trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, object, object, object, object, object, object, object, object, object, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, TResult> Throttle<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func, TimeSpan wait, bool leading = true,
                                                                                                             bool                                  trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, object, object, object, object, object, object, object, object, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, TResult> Throttle<T1, T2, T3, T4, T5, T6, T7, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, object, object, object, object, object, object, object, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, object, object, object, object, object, object, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, object, object, object, object, object, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object, object, object, object, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object, object, object, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object, object, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, object, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, object, TResult>(func, wait, leading, trailing, wait);
    }

    public static RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>
        Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func,
                                                                                                 TimeSpan wait, bool leading = true, bool trailing = true) {
        return new RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(func, wait, leading, trailing, wait);
    }

}