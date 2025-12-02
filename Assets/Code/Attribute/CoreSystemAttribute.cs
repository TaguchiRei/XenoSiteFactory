using System;

/// <summary>
/// ゲーム上に常駐し、SystemServiceに登録されるインターフェースにつける属性。
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public sealed class CoreSystemAttribute : System.Attribute
{
}