using System;
using System.Reflection;

namespace Apliu.Standard.Tools
{
    public class ReflectionHelper
    {
        /// <summary>
        /// 动态调用Dll文件中类的方法
        /// </summary>
        /// <param name="dllFilePath">文件路径，需要绝对路径</param>
        /// <param name="NamespaceClassName">命名空间.类名</param>
        /// <param name="MethodName">方法名称</param>
        /// <param name="IsStatic">是否静态方法</param>
        /// <param name="ParamsArgs">方法参数</param>
        /// <returns></returns>
        public static object MethodInvokeDLL(string dllFilePath, string NamespaceClassName, string MethodName, bool IsStatic, object[] ParamsArgs)
        {
            try
            {
                //加载程序集(dll文件地址)，使用Assembly类   
                Assembly assembly = Assembly.LoadFile(dllFilePath);
                //创建类的实例  
                object obj = null;
                if (!IsStatic) obj = assembly.CreateInstance(NamespaceClassName);

                object value = assembly.GetType(NamespaceClassName).GetMethod(MethodName).Invoke(obj, ParamsArgs);

                return value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 动态调用类中的方法
        /// </summary>
        /// <param name="NamespaceClassName">命名空间.类名</param>
        /// <param name="MethodName">方法名称</param>
        /// <param name="IsStatic">是否静态方法</param>
        /// <param name="ParamsArgs">方法参数</param>
        /// <returns></returns>
        public static object MethodInvokeClass(string NamespaceClassName, string MethodName, bool IsStatic, object[] ParamsArgs)
        {
            try
            {
                string Namespace = NamespaceClassName.Substring(0, NamespaceClassName.LastIndexOf("."));

                Type type = Assembly.Load(Namespace).GetType(NamespaceClassName);
                MethodInfo method = type.GetMethod(MethodName);

                //创建类的实例  
                object obj = null;
                if (!IsStatic) obj = Activator.CreateInstance(type);

                //静态方法,Invoke的第一个参数为null  
                return method.Invoke(obj, ParamsArgs);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 动态创建dll文件中类的实例
        /// </summary>
        /// <param name="dllFilePath">文件路径，需要绝对路径</param>
        /// <param name="NamespaceClassName">命名空间.类名</param>
        /// <returns></returns>
        public static object CreateDllInstance(string dllFilePath, string NamespaceClassName)
        {
            try
            {
                //加载程序集(dll文件地址)，使用Assembly类   
                Assembly assembly = Assembly.LoadFile(dllFilePath);
                object obj = assembly.CreateInstance(NamespaceClassName);
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 动态创建类的实例
        /// </summary>
        /// <param name="NamespaceClassName">命名空间.类名</param>
        /// <returns></returns>
        public static object CreateObjInstance(string NamespaceClassName)
        {
            try
            {
                string Namespace = NamespaceClassName.Substring(0, NamespaceClassName.LastIndexOf("."));
                Type type = Assembly.Load(Namespace).GetType(NamespaceClassName);
                object obj = Activator.CreateInstance(type);
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 动态给指定对象的属性进行赋值，成功则返回True，属性不存在或失败则返回false
        /// </summary>
        /// <param name="objInstance">对象实例</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="Propertyvalue"></param>
        /// <returns></returns>
        public static bool SetPropertyValue(object objInstance, string PropertyName, object Propertyvalue)
        {
            try
            {
                Type objType = objInstance.GetType();
                PropertyInfo proInfo = objType.GetProperty(PropertyName);
                //判断对象是否有该属性
                if (proInfo != null)
                {
                    //为对象属性赋值
                    proInfo.SetValue(objInstance, Propertyvalue, null);
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 动态获取指定对象属性的值，属性不存在或失败则返回null
        /// </summary>
        /// <param name="objInstance">对象实例</param>
        /// <param name="PropertyName">属性名</param>
        /// <returns></returns>
        public static object GetPropertyValue(object objInstance, string PropertyName)
        {
            try
            {
                Type objType = objInstance.GetType();
                PropertyInfo proInfo = objType.GetProperty(PropertyName);
                //判断对象是否有该属性
                if (proInfo != null)
                {
                    //为对象属性赋值
                    return proInfo.GetValue(objInstance, null);
                }
                else return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
