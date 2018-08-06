using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace ApliuTools.Web
{
    public class WebServiceHelper
    {
        /// <summary>
        /// 加载指定WebService的dll服务并存放到指定路径，如果是当前程序目录则直接文件名就可以。
        /// 如果存在且创建时间1天以内，则直接加载；
        /// 否则重新创建服务的dll；
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="dllFilePath">路径与dll名称 c:\\xxx.dll</param>
        /// <param name="TimeOutMinutes">dll服务过期时间，单位分钟</param>
        /// <returns></returns>
        public static bool LoadWebService(string Url, string dllFilePath, int TimeOutMinutes)
        {
            //string LocalPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string dllPath = dllFilePath;

            if (File.Exists(dllPath))
            {
                FileInfo dllinfo = new FileInfo(dllPath);
                if ((DateTime.Now - dllinfo.LastWriteTime).Minutes >= TimeOutMinutes)
                {
                    return CreateWebServiceDLL(Url, dllPath);
                }
                else return true;
            }
            else
            {
                return CreateWebServiceDLL(Url, dllPath);
            }
        }

        /// <summary>
        /// 将指定URL的WebService创建成Dll并存放到指定路径，如果是当前程序目录则直接文件名就可以
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="dllFilePath">路径与dll名称 c:\\xxx.dll</param>
        /// <returns></returns>
        public static bool CreateWebServiceDLL(string Url, string dllFilePath)
        {
            // 1. 使用 WebClient 下载 WSDL 信息。  
            WebClient web = new WebClient();
            Stream stream = web.OpenRead(Url);
            // 2. 创建和格式化 WSDL 文档。  
            ServiceDescription description = ServiceDescription.Read(stream);
            // 3. 创建客户端代理代理类。  
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
            importer.ProtocolName = "Soap"; // 指定访问协议。  
            importer.Style = ServiceDescriptionImportStyle.Client; // 生成客户端代理。  
            importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
            importer.AddServiceDescription(description, null, null); // 添加 WSDL 文档。  
            // 4. 使用 CodeDom 编译客户端代理类。  
            CodeNamespace nmspace = new CodeNamespace();        // 为代理类添加命名空间，缺省为全局空间。  
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(nmspace);
            ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameter = new CompilerParameters();
            parameter.GenerateExecutable = false;
            parameter.OutputAssembly = dllFilePath;  // 可以指定你所需的任何文件名。  
            parameter.ReferencedAssemblies.Add("System.dll");
            parameter.ReferencedAssemblies.Add("System.XML.dll");
            parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
            parameter.ReferencedAssemblies.Add("System.Data.dll");
            CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
            if (result.Errors.HasErrors)
            {
                // 显示编译错误信息  
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (CompilerError ce in result.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                return false;
                //throw new Exception(sb.ToString());
            }
            return true;
        }
    }
}
