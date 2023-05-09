using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Flow.RPA.Desktop.Shared.ApplicationFlow;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{

	public class SynchronizationContextDispatcher : IUIThreadDispatcher
	{

		public SynchronizationContextDispatcher(SynchronizationContext context)
		{
			this._context = context;
		}


		public T Invoke<T>(Func<T> func)
		{
			T result = default(T);
			this._context.Send(delegate(object _)
			{
				result = func();
			}, null);
			return result;
		}


		public void Invoke(Action action)
		{
			this._context.Send(delegate(object _)
			{
				action();
			}, null);
		}


		public Task<T> InvokeAsync<T>(Func<T> func)
		{
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
			this._context.Post(delegate(object _)
			{
				try
				{
					tcs.SetResult(func());
				}
				catch (Exception ex)
				{
					tcs.SetException(ex);
					throw;
				}
			}, null);
			return tcs.Task;
		}


		public Task InvokeAsync(Action action)
		{
			this._context.Post(delegate(object o)
			{
				action();
			}, null);
			return Task.CompletedTask;
		}


		private readonly SynchronizationContext _context;
	}
}
