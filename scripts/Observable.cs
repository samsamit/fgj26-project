using System;
using System.Collections.Generic;

public class Observable<T>
{
	private List<Action<T>> AfterChangeObservers = [];
	private List<Action<T>> BeforeChangeObservers = [];

	private T value;

	public Observable(T value)
	{
		this.value = value;
	}

	public T Get()
	{
		return value;
	}

	public void Set(T value)
	{
		foreach (var observer in BeforeChangeObservers)
		{
			observer.Invoke(value);
		}
		this.value = value;
		foreach (var observer in AfterChangeObservers)
		{
			observer.Invoke(value);
		}
	}

	public void RegisterAfterChangeObserver(Action<T> observer)
	{
		AfterChangeObservers.Add(observer);
		observer.Invoke(value);  // Invoke immediately with current value
	}

	public void RegisterBeforeChangeObserver(Action<T> observer)
	{
		BeforeChangeObservers.Add(observer);
	}

	public void DeregisterAllObservers()
	{
		AfterChangeObservers.Clear();
		BeforeChangeObservers.Clear();
	}
}
