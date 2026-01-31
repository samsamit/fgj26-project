using System;
using System.Collections.Generic;

public class Observable<T>
{
	private List<Action<T>> Observers = [];

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
		this.value = value;
		foreach (var observer in Observers)
		{
			observer.Invoke(value);
		}
	}

	public void RegisterObserver(Action<T> observer)
	{
		Observers.Add(observer);
	}
}
