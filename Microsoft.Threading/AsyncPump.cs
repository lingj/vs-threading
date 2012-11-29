﻿//-----------------------------------------------------------------------
// <copyright file="AsyncPump.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Threading {
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>Provides a pump that supports running asynchronous methods on the current thread.</summary>
	public class AsyncPump {
		private readonly JoinableTaskContext.JoinableTaskCollection collection;
		private readonly JoinableTaskContext.JoinableTaskFactory factory;

		public AsyncPump(Thread mainThread = null, SynchronizationContext syncContext = null) {
			var context = new JoinableTaskContext(mainThread, syncContext);
			this.collection = context.CreateCollection();
			this.factory = context.CreateFactory(this.collection);
		}

		public AsyncPump(JoinableTaskContext context) {
			Requires.NotNull(context, "context");

			this.collection = context.CreateCollection();
			this.factory = context.CreateFactory(this.collection);
		}

		public JoinableTaskContext.JoinableTaskFactory Factory {
			get { return this.factory; }
		}

		public TaskScheduler MainThreadTaskScheduler {
			get { return this.factory.MainThreadJobScheduler; }
		}

		public JoinableTaskContext.JoinRelease Join() {
			return this.collection.Join();
		}

		public void RunSynchronously(Func<Task> asyncMethod) {
			this.factory.Run(asyncMethod);
		}

		public T RunSynchronously<T>(Func<Task<T>> asyncMethod) {
			return this.factory.Run(asyncMethod);
		}

		public JoinableTaskContext.JoinableTask BeginAsynchronously(Func<Task> asyncMethod) {
			return this.factory.Start(asyncMethod);
		}

		public JoinableTaskContext.JoinableTask<T> BeginAsynchronously<T>(Func<Task<T>> asyncMethod) {
			return this.factory.Start(asyncMethod);
		}

		public void CompleteSynchronously(Task task) {
			this.RunSynchronously(async delegate {
				using (this.Join()) {
					await task;
				}
			});
		}

		public JoinableTaskContext.MainThreadAwaitable SwitchToMainThreadAsync(CancellationToken cancellationToken = default(CancellationToken)) {
			return this.factory.SwitchToMainThreadAsync(cancellationToken);
		}

		public JoinableTaskContext.RevertRelevance SuppressRelevance() {
			return this.factory.Context.SuppressRelevance();
		}
	}
}