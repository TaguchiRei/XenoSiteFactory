using System;

/// <summary>
/// ServiceLocatorに登録されるインターフェースにつける属性。
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public sealed class RegisterableServiceAttribute : System.Attribute
{
}