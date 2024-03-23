#nullable enable

// ReSharper disable UseArrayEmptyMethod - not available in .NET Framework 4

namespace ThrottleDebounce;

internal partial class RateLimiter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>: RateLimitedFunc<TResult>, RateLimitedFunc<T1, TResult>,
    RateLimitedFunc<T1, T2, TResult>,
    RateLimitedFunc<T1, T2, T3, TResult>, RateLimitedFunc<T1, T2, T3, T4, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, TResult>,
    RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>,
    RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>,
    RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>,
    RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>,
    RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>,
    RateLimitedAction, RateLimitedAction<T1>, RateLimitedAction<T1, T2>,
    RateLimitedAction<T1, T2, T3>,
    RateLimitedAction<T1, T2, T3, T4>, RateLimitedAction<T1, T2, T3, T4, T5>, RateLimitedAction<T1, T2, T3, T4, T5, T6>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7>,
    RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
    RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
    RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
    RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {

    void RateLimitedAction.Invoke() => OnUserInvocation(new object[0]);

    void RateLimitedAction<T1>.Invoke(T1 arg1) => OnUserInvocation(new object[] { arg1! });

    void RateLimitedAction<T1, T2>.Invoke(T1 arg1, T2 arg2) => OnUserInvocation(new object[] { arg1!, arg2! });

    void RateLimitedAction<T1, T2, T3>.Invoke(T1 arg1, T2 arg2, T3 arg3) => OnUserInvocation(new object[] { arg1!, arg2!, arg3! });

    void RateLimitedAction<T1, T2, T3, T4>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4! });

    void RateLimitedAction<T1, T2, T3, T4, T5>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5! });

    void RateLimitedAction<T1, T2, T3, T4, T5, T6>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6! });

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7! });

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8! });

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9! });

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10! });

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11! });

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.
        Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11!, arg12! });

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.Invoke(T1  arg1,  T2  arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11,
                                                                                          T12 arg12, T13 arg13) => OnUserInvocation(new object[]
        { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11!, arg12!, arg13! });

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.Invoke(T1  arg1,  T2  arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11,
                                                                                               T12 arg12, T13 arg13, T14 arg14) => OnUserInvocation(new object[]
        { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11!, arg12!, arg13!, arg14! });

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.Invoke(T1  arg1,  T2  arg2,  T3  arg3,  T4  arg4,  T5  arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
                                                                                                    T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) => OnUserInvocation(new object[]
        { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11!, arg12!, arg13!, arg14!, arg15! });

    void RateLimitedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>.Invoke(T1  arg1,  T2  arg2,  T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
                                                                                                         T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11!, arg12!, arg13!, arg14!, arg15!, arg16! });

    TResult? RateLimitedFunc<TResult>.Invoke() => OnUserInvocation(new object[0]);

    TResult? RateLimitedFunc<T1, TResult>.Invoke(T1 arg1) => OnUserInvocation(new object[] { arg1! });

    TResult? RateLimitedFunc<T1, T2, TResult>.Invoke(T1 arg1, T2 arg2) => OnUserInvocation(new object[] { arg1!, arg2! });

    TResult? RateLimitedFunc<T1, T2, T3, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3) => OnUserInvocation(new object[] { arg1!, arg2!, arg3! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>.Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>.Invoke(T1  arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11,
                                                                                                T12 arg12) => OnUserInvocation(new object[]
        { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11!, arg12! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>.Invoke(T1  arg1,  T2  arg2,  T3  arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
                                                                                                     T11 arg11, T12 arg12, T13 arg13) => OnUserInvocation(new object[]
        { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11!, arg12!, arg13! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>.Invoke(T1  arg1,  T2  arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
                                                                                                          T11 arg11, T12 arg12, T13 arg13, T14 arg14) => OnUserInvocation(new object[]
        { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11!, arg12!, arg13!, arg14! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>.Invoke(T1  arg1,  T2  arg2,  T3  arg3,  T4  arg4,  T5  arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                                                                                                               T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11!, arg12!, arg13!, arg14!, arg15! });

    TResult? RateLimitedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>.Invoke(T1  arg1,  T2  arg2,  T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                                                                                                                    T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) =>
        OnUserInvocation(new object[] { arg1!, arg2!, arg3!, arg4!, arg5!, arg6!, arg7!, arg8!, arg9!, arg10!, arg11!, arg12!, arg13!, arg14!, arg15!, arg16! });

}