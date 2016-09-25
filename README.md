Microsoft.VisualStudio.Threading
=================================

[![NuGet package](https://img.shields.io/nuget/v/Microsoft.VisualStudio.Threading.svg)](https://nuget.org/packages/Microsoft.VisualStudio.Threading)

## Features

* Async versions of many threading synchronization primitives
  * `AsyncAutoResetEvent`
  * `AsyncManualResetEvent`
  * `AsyncBarrier`
  * `AsyncCountdownEvent`
  * `AsyncSemaphore`
  * `AsyncReaderWriterLock`
* Async versions of very common types
  * `AsyncLazy<T>`
  * `AsyncLocal<T>`
  * `AsyncQueue<T>`
  * `AsyncEventHandler`
* Await extension methods
  * Await on a `TaskScheduler` to switch to it.
    Switch to a background thread with `await TaskScheduler.Default;`
  * Await on a `Task` with a timeout
  * Await on a `Task` with cancellation  
* `JoinableTaskFactory` that allows you to schedule asynchronous or synchronous work
  that does not deadlock with the UI thread even when the UI thread needs to
  synchronously block on the result.

## Supported platforms

* .NET 4.5
* Windows 8
* Windows Phone 8.1
* .NET Portable (Profile111, or .NET Standard 1.1)

[1]: http://nuget.org/packages/Microsoft.VisualStudio.Threading "Microsoft.VisualStudio.Threading NuGet package"
[2]: src\Microsoft.VisualStudio.Threading.NuGet\tools "Code Snippets"