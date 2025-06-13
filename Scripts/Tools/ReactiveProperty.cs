using System;
using UnityEngine;

[Serializable]
public class ReactiveProperty<T>
{
    [SerializeField]
    private T _value;

    public event Action<T> OnValueChanged;

    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    public ReactiveProperty(T initialValue)
    {
        _value = initialValue;
    }
}