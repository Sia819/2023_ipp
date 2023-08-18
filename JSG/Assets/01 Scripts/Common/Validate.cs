using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class Validate
{
    /// <summary>
    /// 인스펙터에서 요소가 선택되지 않았는지 검색합니다. <para/>
    /// 일반적인 사용방법은 <c>Validate.NullCheck(this, nameof(UnityEngine.Object))</c> 처럼 사용합니다.
    /// </summary>
    /// <param name="sender">경고를 보낼 컴포넌트 입니다. 일반적으로 this를 사용합니다.</param>
    /// <param name="variableName">누락되었는지 확인 할 변수명 입니다. 필드 또는 프로퍼티일 수 있습니다.</param>
    public static void NullCheck(MonoBehaviour sender, string variableName)
    {
        var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        // 멤버 체크 Reflection
        MemberInfo info = sender.GetType().GetField(variableName, bindingFlags);
        info ??= sender.GetType().GetProperty(variableName, bindingFlags);
        if (info != null)
        {
            UnityEngine.Object value = null;
            if (info is FieldInfo fieldInfo)
                value = fieldInfo.GetValue(sender) as UnityEngine.Object;
            else if (info is PropertyInfo propertyInfo)
                value = propertyInfo.GetValue(sender) as UnityEngine.Object;

            if (value == null)
            {
                Debug.LogWarning($"{sender.GetType().Name} 컴포넌트의 {variableName} 요소는 필수이므로 비어있을 수 없습니다.", sender);
            }
            return;
        }

        Debug.LogError($"{variableName}은(는) {sender.GetType().Name} 컴포넌트에서 찾을 수 없습니다.", sender);
    }
}