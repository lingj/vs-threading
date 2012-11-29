﻿//-----------------------------------------------------------------------
// <copyright file="JoinableTask`1.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Threading {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	public partial class JoinableTaskContext {
		/// <summary>
		/// Tracks asynchronous operations and provides the ability to Join those operations to avoid
		/// deadlocks while synchronously blocking the Main thread for the operation's completion.
		/// </summary>
		/// <typeparam name="T">The type of value returned by the asynchronous operation.</typeparam>
		public class JoinableTask<T> : JoinableTask {
			/// <summary>
			/// Initializes a new instance of the <see cref="JoinableTask"/> class.
			/// </summary>
			/// <param name="owner">The instance that began the async operation.</param>
			/// <param name="synchronouslyBlocking">A value indicating whether the launching thread will synchronously block for this job's completion.</param>
			public JoinableTask(JoinableTaskFactory owner, bool synchronouslyBlocking)
				: base(owner, synchronouslyBlocking) {
			}

			/// <summary>
			/// Gets the asynchronous task that completes when the async operation completes.
			/// </summary>
			public new Task<T> Task {
				get { return (Task<T>)base.Task; }
			}

			/// <summary>
			/// Joins any main thread affinity of the caller with the asynchronous operation to avoid deadlocks
			/// in the event that the main thread ultimately synchronously blocks waiting for the operation to complete.
			/// </summary>
			/// <param name="cancellationToken">A cancellation token that will exit this method before the task is completed.</param>
			/// <returns>A task that completes after the asynchronous operation completes and the join is reverted, with the result of the operation.</returns>
			public new async Task<T> JoinAsync(CancellationToken cancellationToken = default(CancellationToken)) {
				await base.JoinAsync(cancellationToken);
				return await this.Task;
			}

			/// <summary>
			/// Synchronously blocks the calling thread until the operation has completed.
			/// If the calling thread is the Main thread, deadlocks are mitigated.
			/// </summary>
			/// <param name="cancellationToken">A cancellation token that will exit this method before the task is completed.</param>
			/// <returns>The result of the asynchronous operation.</returns>
			public new T Join(CancellationToken cancellationToken = default(CancellationToken)) {
				base.Join(cancellationToken);
				Assumes.True(this.Task.IsCompleted);
				return this.Task.Result;
			}

			/// <summary>
			/// Gets an awaiter that is equivalent to calling <see cref="JoinAsync"/>.
			/// </summary>
			/// <returns>A task whose result is the result of the asynchronous operation.</returns>
			public new TaskAwaiter<T> GetAwaiter() {
				return this.JoinAsync().GetAwaiter();
			}

			internal new T CompleteOnCurrentThread() {
				base.CompleteOnCurrentThread();
				return this.Task.GetAwaiter().GetResult();
			}
		}
	}
}